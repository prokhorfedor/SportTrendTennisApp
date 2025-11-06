import { useQuery } from "../hooks/useQuery";
import { apiRequest } from "../api";
import { LoginResponse } from "./api.interfaces";
import { API_URLS } from "../config/apiConfig";
import { ApiMethods } from "../shared/api-methods.enum";

export type LoginRequest = { identifier: string; passwordHash: string };

export const useLogin = () => {
    return useQuery<LoginResponse, LoginRequest>(async (args: LoginRequest) => {
        // Calls the API /login endpoint and returns the parsed response
        return apiRequest<LoginResponse>(API_URLS.LOGIN, {
            method: ApiMethods.POST,
            body: JSON.stringify(args),
        });
    });
};
