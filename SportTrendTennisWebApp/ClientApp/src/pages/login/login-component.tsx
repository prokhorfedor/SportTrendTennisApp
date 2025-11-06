import React, { useState } from "react";
import { Box, TextField, Button, Typography, Card, CardContent } from "@mui/material";
import { ShowSignUpComponent } from "./show-signup-component";

export interface LoginComponentProps {
    handleSubmit: (e: React.FormEvent) => void;
    error: string | null;
    showSignupPrompt: boolean;
    goToSignup: () => void;
    goToResetPassword: () => void;
}

function LoginComponent(props: LoginComponentProps) {
    const [identifier, setIdentifier] = useState("");
    const [password, setPassword] = useState("");
    const textFieldProps = {
        variant: "outlined" as const,
        fullWidth: true,
        margin: "normal" as const,
    };
    return (
        <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "60vh" }}>
            <Card sx={{ minWidth: 350 }}>
                <CardContent>
                    <Typography variant="h5" gutterBottom>Login</Typography>
                    <form onSubmit={props.handleSubmit}>
                        <TextField
                            {...textFieldProps}
                            label="Email or Phone Number"
                            name="identifier"
                            value={identifier}
                            onChange={e => setIdentifier(e.target.value)}
                        />
                        <TextField
                            {...textFieldProps}
                            label="Password"
                            type="password"
                            name="password"
                            value={password}
                            onChange={e => setPassword(e.target.value)}
                        />
                        {props.error && <Typography color="error" variant="body2">{props.error}</Typography>}
                        <Box sx={{ display: 'flex', gap: 1, mt: 2 }}>
                            <Button type="submit" variant="contained" color="primary" sx={{ flex: 1 }}>
                                Login
                            </Button>
                            <Box sx={{ flex: 1 }}>
                                <ShowSignUpComponent compact={!props.showSignupPrompt} goToSignup={props.goToSignup} />
                            </Box>
                        </Box>
                        <Button
                            type="button"
                            variant="text"
                            fullWidth
                            sx={{ mt: 1 }}
                            onClick={props.goToResetPassword}
                        >
                            Forgot password?
                        </Button>
                    </form>
                </CardContent>
            </Card>
        </Box>
    )
};
export default LoginComponent;