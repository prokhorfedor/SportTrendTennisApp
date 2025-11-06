import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { SignupComponent, SignupForm } from "./signup-component";
import { useSignup } from "../../api/useSignup";
import { hashPasswordSHA256 } from "../../helpers/userService";

export function SignupContainer() {
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const { execute, errors } = useSignup();

    const [form, setForm] = useState<SignupForm>({
        firstName: "",
        lastName: "",
        email: "",
        phone: "",
        password: "",
        confirmPassword: "",
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setForm((prev) => ({ ...prev, [name]: value }));
    };
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        if (!form.firstName || !form.lastName || !form.email || !form.phone || !form.password || !form.confirmPassword) {
            setError("Please fill in all fields.");
            return;
        }
        if (form.password !== form.confirmPassword) {
            setError("Passwords do not match.");
            return;
        }
        try {
            const passwordHash = await hashPasswordSHA256(form.password);
            const result = await execute({
                firstName: form.firstName,
                lastName: form.lastName,
                email: form.email,
                phone: form.phone,
                passwordHash,
            });
            if (!result) {
                if (errors)
                    setError(String(errors));
                return;
            }
            // on success redirect to main page
            navigate('/main');
        } catch (err: any) {
            setError(String(err?.message ?? err));
        }
    };

    return <SignupComponent
        form={form}
        handleChange={handleChange}
        handleSubmit={handleSubmit}
        error={error}
    />;
}