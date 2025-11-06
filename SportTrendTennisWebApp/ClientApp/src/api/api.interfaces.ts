import { LoginResult } from "../config/consts";

export interface LoginResponse {
    result: LoginResult,
    tokenModel: TokenModel | null;
}

interface TokenModel {
    token: string;
    refreshToken: string;
}