import { getCookie, removeCookie } from '../utils/cookies';

const API_BASE = import.meta.env.VITE_API_URL ?? '';
const AUTH_COOKIE = 'cinema_auth';

export class ApiError extends Error {
  constructor(
    message: string,
    public status: number,
  ) {
    super(message);
    this.name = 'ApiError';
  }
}

function getToken(): string | null {
  try {
    const raw = getCookie(AUTH_COOKIE);
    if (!raw) return null;

    const data = JSON.parse(raw) as { token?: string; expiresAt?: number };
    if (!data?.token) return null;

    if (data.expiresAt && Date.now() >= data.expiresAt) {
      removeCookie(AUTH_COOKIE);
      return null;
    }

    return data.token;
  } catch {
    return null;
  }
}

export async function apiFetch<T>(
  path: string,
  options: RequestInit = {},
): Promise<T> {
  const headers = new Headers(options.headers);

  if (!(options.body instanceof FormData) && !headers.has('Content-Type')) {
    headers.set('Content-Type', 'application/json');
  }

  const token = getToken();
  if (token) headers.set('Authorization', `Bearer ${token}`);

  const res = await fetch(`${API_BASE}${path}`, {
    ...options,
    headers,
  });

  if (!res.ok) {
    let message = res.statusText;
    try {
      const text = await res.text();
      if (text) {
        try {
          const json = JSON.parse(text) as {
            title?: string;
            detail?: string;
            message?: string;
          };
          message = json.detail ?? json.title ?? json.message ?? text;
        } catch {
          message = text;
        }
      }
    } catch {

    }
    throw new ApiError(message, res.status);
  }

  if (res.status === 204) return undefined as T;

  return res.json() as Promise<T>;
}