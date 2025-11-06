import React from "react";
import { Box, TextField, Button, Typography, Card, CardContent } from "@mui/material";

export interface SignupComponentProps {
    form: SignupForm,
    handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    handleSubmit: (e: React.FormEvent) => void;
    error: string | null;
}

export interface SignupForm {
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    password: string;
    confirmPassword: string;
}

export function SignupComponent({ form, handleChange, handleSubmit, error }: SignupComponentProps) {
    const formProps = {
        variant: "outlined" as const,
        fullWidth: true,
        margin: "normal" as const,
        onChange: handleChange,
    };
    return (<Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "60vh" }}>
        <Card sx={{ minWidth: 350 }}>
            <CardContent>
                <Typography variant="h5" gutterBottom>Sign Up</Typography>
                <form onSubmit={handleSubmit}>
                    <TextField
                        label="First Name"
                        name="firstName"
                        value={form.firstName}
                        {...formProps}
                    />
                    <TextField
                        label="Last Name"
                        name="lastName"
                        value={form.lastName}
                        {...formProps}
                    />
                    <TextField
                        label="Email"
                        name="email"
                        type="email"
                        value={form.email}
                        {...formProps}
                    />
                    <TextField
                        label="Phone"
                        name="phone"
                        value={form.phone}
                        {...formProps}
                    />
                    <TextField
                        label="Password"
                        name="password"
                        type="password"
                        value={form.password}
                        {...formProps}
                    />
                    <TextField
                        label="Confirm Password"
                        name="confirmPassword"
                        type="password"
                        value={form.confirmPassword}
                        {...formProps}
                    />
                    {error && <Typography color="error" variant="body2">{error}</Typography>}
                    <Button type="submit" variant="contained" color="primary" fullWidth sx={{ mt: 2 }}>
                        Sign Up
                    </Button>
                </form>
            </CardContent>
        </Card>
    </Box>);
}