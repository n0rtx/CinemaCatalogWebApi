const ONE_WEEK_SECONDS = 7 * 24 * 60 * 60;

export function setCookie(
  name: string,
  value: string,
  maxAgeSeconds: number = ONE_WEEK_SECONDS,
): void {
  const secure = window.location.protocol === 'https:' ? '; Secure' : '';
  document.cookie = `${encodeURIComponent(name)}=${encodeURIComponent(value)}; path=/; max-age=${maxAgeSeconds}; SameSite=Lax${secure}`;
}

export function getCookie(name: string): string | null {
  const key = `${encodeURIComponent(name)}=`;
  const parts = document.cookie.split(';');
  for (const part of parts) {
    const trimmed = part.trim();
    if (trimmed.startsWith(key)) {
      return decodeURIComponent(trimmed.slice(key.length));
    }
  }
  return null;
}

export function removeCookie(name: string): void {
  document.cookie = `${encodeURIComponent(name)}=; path=/; max-age=0; SameSite=Lax`;
}

export const COOKIE_MAX_AGE = {
  theme: 365 * 24 * 60 * 60, // 1 год
  auth: ONE_WEEK_SECONDS,    // 1 неделя
} as const;