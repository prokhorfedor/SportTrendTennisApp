import { useQuery } from "../hooks/useQuery";
import { apiRequest } from "../api";
import { LoginResponse } from "./api.interfaces";

export type LoginRequest = { identifier: string; passwordHash: string };

export const useLogin = () => {
    return useQuery<LoginResponse, LoginRequest>(async (args: LoginRequest) => {
        // Calls the API /login endpoint and returns the parsed response
        return apiRequest<LoginResponse>("login", {
            method: "POST",
            body: JSON.stringify(args),
        });
    });
};
