import { useCallback, useState } from "react";

export type TQueryError = unknown;
export type UseQueryResult<TResponse, TRequest> = {
    data: TResponse | null;
    errors: TQueryError;
    isLoading: boolean;
    execute: (args: TRequest) => Promise<TResponse | null>;
};
export function useQuery<TResponse, TRequest>(callback: (args: TRequest) => Promise<TResponse>): UseQueryResult<TResponse, TRequest> {
    const [data, setData] = useState<TResponse | null>(null);
    const [error, setError] = useState<TQueryError>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const execute = useCallback(async (args: TRequest): Promise<TResponse | null> => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await callback(args);
            setData(response);
            return response;
        } catch (err) {
            setError(err);
            return null;
        } finally {
            setIsLoading(false);
        }
    }, [callback]);

    return {
        data,
        errors: error,
        isLoading,
        execute,
    };
}