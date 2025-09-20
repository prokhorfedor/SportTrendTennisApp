import React, { useState } from "react";
import { Box, TextField, Button, Typography, Card, CardContent } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { apiRequest } from "../api";

export default function Login({ onLogin }) {
    const [identifier, setIdentifier] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [showSignupPrompt, setShowSignupPrompt] = useState(false);
    const navigate = useNavigate();

    async function hashPasswordSHA256(password) {
        const encoder = new TextEncoder();
        const data = encoder.encode(password);
        const hashBuffer = await window.crypto.subtle.digest('SHA-256', data);
        return Array.from(new Uint8Array(hashBuffer)).map(b => b.toString(16).padStart(2, '0')).join('');
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!identifier || !password) {
            setError("Please enter email or phone and password.");
            return;
        }
        setError("");
        setShowSignupPrompt(false);
        try {
            // Hash password before sending (SHA-256)
            const hashedPassword = await hashPasswordSHA256(password);

            // Send to API
            const response = await apiRequest("User/Login", {
                method: "POST",
                body: JSON.stringify({ identifier, passwordHash: hashedPassword })
            });

            switch (response.result) {
                case 0: // Успішний вхід
                    navigate("/main");
                    break;
                case 1: // Неправильний пароль
                    setError("Incorrect password. Please try again.");
                    break;
                case 2: // Користувача не існує
                    setShowSignupPrompt(true);
                    break;
                default:
                    setError("Login failed. Please try again.");
            }
            if (onLogin) onLogin({ identifier });
        } catch (err) {

            setError("Login failed: " + err.message);
        }
    };

    return (
        <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "60vh" }}>
            <Card sx={{ minWidth: 350 }}>
                <CardContent>
                    <Typography variant="h5" gutterBottom>Login</Typography>
                    <form onSubmit={handleSubmit}>
                        <TextField
                            label="Email or Phone Number"
                            variant="outlined"
                            fullWidth
                            margin="normal"
                            value={identifier}
                            onChange={e => setIdentifier(e.target.value)}
                        />
                        <TextField
                            label="Password"
                            type="password"
                            variant="outlined"
                            fullWidth
                            margin="normal"
                            value={password}
                            onChange={e => setPassword(e.target.value)}
                        />
                        {error && <Typography color="error" variant="body2">{error}</Typography>}
                        <Button type="submit" variant="contained" color="primary" fullWidth sx={{ mt: 2 }}>
                            Login
                        </Button>
                    </form>
                    {showSignupPrompt && (
                        <Box sx={{ mt: 2 }}>
                            <Typography color="warning.main" variant="body2" gutterBottom>
                                User not found. Would you like to create a new account?
                            </Typography>
                            <Button variant="outlined" color="primary" fullWidth onClick={() => navigate("/signup")}>Create Account</Button>
                        </Box>
                    )}
                </CardContent>
            </Card>
        </Box>
    );
}
