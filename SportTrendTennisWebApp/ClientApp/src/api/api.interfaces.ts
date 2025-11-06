import { LoginResult } from "../config/consts";

export interface LoginResponse {
    result: LoginResult,
    tokenModel: TokenModel | null;
}

export interface CreateUserResponse {
    userId: string,
    firstName: string,
    lastName: string,
    email: string,
    phone: string,
}

interface TokenModel {
    token: string;
    refreshToken: string;
}