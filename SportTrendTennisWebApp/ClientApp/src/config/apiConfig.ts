// src/config/apiConfig.ts
export const API_BASE_URL: string = import.meta.env.VITE_API_BASE_URL || "http://localhost:3000/api";

// You can add other global API settings here
export const API_TIMEOUT: number = 10000;