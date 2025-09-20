import React, { useState } from "react";
import { Box, TextField, Button, Typography, Card, CardContent } from "@mui/material";
import { apiRequest } from "../api";
import { useNavigate } from "react-router-dom";

export default function Signup({ onSignup }) {
    const [form, setForm] = useState({
        firstName: "",
        lastName: "",
        email: "",
        phone: "",
        password: "",
        confirmPassword: ""
    });
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    async function hashPasswordSHA256(password) {
        const encoder = new TextEncoder();
        const data = encoder.encode(password);
        const hashBuffer = await window.crypto.subtle.digest('SHA-256', data);
        return Array.from(new Uint8Array(hashBuffer)).map(b => b.toString(16).padStart(2, '0')).join('');
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!form.firstName || !form.lastName || !form.email || !form.phone || !form.password || !form.confirmPassword) {
            setError("Please fill in all fields.");
            return;
        }
        if (form.password !== form.confirmPassword) {
            setError("Passwords do not match.");
            return;
        }
        setError("");
        try {
            const passwordHash = await hashPasswordSHA256(form.password);
            await apiRequest("User/CreateUser", {
                method: "POST",
                body: JSON.stringify({
                    firstName: form.firstName,
                    lastName: form.lastName,
                    email: form.email,
                    phone: form.phone,
                    passwordHash
                })
            });
            if (onSignup) onSignup(form);
            navigate("/main");
        } catch (err) {
            setError("Signup failed: " + err.message);
        }
    };

    return (
        <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "60vh" }}>
            <Card sx={{ minWidth: 350 }}>
                <CardContent>
                    <Typography variant="h5" gutterBottom>Sign Up</Typography>
                    <form onSubmit={handleSubmit}>
                        <TextField
                            label="First Name"
                            name="firstName"
                            variant="outlined"
                            fullWidth
                            margin="normal"
                            value={form.firstName}
                            onChange={handleChange}
                        />
                        <TextField
                            label="Last Name"
                            name="lastName"
                            variant="outlined"
                            fullWidth
                            margin="normal"
                            value={form.lastName}
                            onChange={handleChange}
                        />
                        <TextField
                            label="Email"
                            name="email"
                            type="email"
                            variant="outlined"
                            fullWidth
                            margin="normal"
                            value={form.email}
                            onChange={handleChange}
                        />
                        <TextField
                            label="Phone"
                            name="phone"
                            variant="outlined"
                            fullWidth
                            margin="normal"
                            value={form.phone}
                            onChange={handleChange}
                        />
                        <TextField
                            label="Password"
                            name="password"
                            type="password"
                            variant="outlined"
                            fullWidth
                            margin="normal"
                            value={form.password}
                            onChange={handleChange}
                        />
                        <TextField
                            label="Confirm Password"
                            name="confirmPassword"
                            type="password"
                            variant="outlined"
                            fullWidth
                            margin="normal"
                            value={form.confirmPassword}
                            onChange={handleChange}
                        />
                        {error && <Typography color="error" variant="body2">{error}</Typography>}
                        <Button type="submit" variant="contained" color="primary" fullWidth sx={{ mt: 2 }}>
                            Sign Up
                        </Button>
                    </form>
                </CardContent>
            </Card>
        </Box>
    );
}
