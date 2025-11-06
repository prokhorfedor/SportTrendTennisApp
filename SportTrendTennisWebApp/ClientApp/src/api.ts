export async function apiRequest<TResponse>(path: string, options: RequestInit = {}): Promise<TResponse> {
    const response = await fetch(path, {
        ...options,
        headers: {
            'Content-Type': 'application/json',
            ...(options.headers || {})
        },
        credentials: 'include',
    });
    if (!response.ok) {
        throw new Error(await response.text());
    }
    return response.json();
}
