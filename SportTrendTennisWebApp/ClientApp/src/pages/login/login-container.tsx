import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useLogin } from "../../api/useLogin";
import LoginComponent from "./login-component";
import { LoginResult } from "../../config/consts";
import { hashPasswordSHA256 } from "../../helpers/userService";

function LoginContainer() {
    const [error, setError] = useState<string | null>(null);
    const [showSignupPrompt, setShowSignupPrompt] = useState(false);
    const navigate = useNavigate();
    const { execute, errors } = useLogin();

    const onLogin = (userData: { identifier: string }) => {
        // Handle post-login actions, e.g., redirecting
        navigate("/dashboard");
    };
    const goToSignup = () => {
        navigate("/signup");
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const formData = new FormData(e.currentTarget as HTMLFormElement);
        const identifier = formData.get("identifier") as string;
        const password = formData.get("password") as string;
        if (!identifier || !password) {
            setError("Please enter email or phone and password.");
            return;
        }
        setError("");
        setShowSignupPrompt(false);
        try {
            // Hash password before sending (SHA-256)
            const hashedPassword = await hashPasswordSHA256(password);
            const result = await execute({ identifier, passwordHash: hashedPassword });
            if (!result) {
                if (errors)
                    setError(String(errors));
                return;
            }
            if (result.result === LoginResult.NotFound)
                setShowSignupPrompt(true);

            if (onLogin)
                onLogin({ identifier });
        } catch (err: any) {

            setError("Login failed: " + err.message);
        }
    };
    return (
        <LoginComponent
            handleSubmit={handleSubmit}
            error={error}
            showSignupPrompt={showSignupPrompt}
            goToSignup={goToSignup}
            goToResetPassword={() => void 0}
        />
    );
}

export default LoginContainer;