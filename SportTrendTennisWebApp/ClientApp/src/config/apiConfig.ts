// src/config/apiConfig.ts
const API_BASE_URL: string = import.meta.env.VITE_API_BASE_URL || "http://localhost:3000/api";

// You can add other global API settings here
export const API_TIMEOUT: number = 10000;

export const API_URLS = {
    LOGIN: `${API_BASE_URL}/login`,
    SIGNUP: `${API_BASE_URL}/createuser`,
    REGISTER: `${API_BASE_URL}/register`,
    // Add other endpoints as needed
};