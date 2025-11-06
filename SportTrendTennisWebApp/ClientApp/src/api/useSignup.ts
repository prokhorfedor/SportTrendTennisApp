
import { useQuery } from "../hooks/useQuery";
import { apiRequest } from "../api";
import { CreateUserResponse } from "./api.interfaces";
import { API_URLS } from "../config/apiConfig";
import { ApiMethods } from "../shared/api-methods.enum";

export type CreateUserRequest = {
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    passwordHash: string;
};

export const useSignup = () => {
    return useQuery<CreateUserResponse, CreateUserRequest>(async (args: CreateUserRequest) => {
        // Calls the API /login endpoint and returns the parsed response
        return apiRequest<CreateUserResponse>(API_URLS.SIGNUP, {
            method: ApiMethods.POST,
            body: JSON.stringify(args),
        });
    });
};
