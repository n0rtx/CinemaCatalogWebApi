import {
  createContext,
  useCallback,
  useContext,
  useMemo,
  useState,
  type ReactNode,
} from 'react';
import type { AuthUser } from '../types/auth';
import * as authApi from '../api/auth';
import {
  COOKIE_MAX_AGE,
  getCookie,
  removeCookie,
  setCookie,
} from '../utils/cookies';

interface AuthContextValue {
  user: AuthUser | null;
  isAuthenticated: boolean;
  login: (login: string, password: string) => Promise<void>;
  register: (login: string, password: string) => Promise<void>;
  logout: () => void;
}

interface StoredAuth extends AuthUser {
  expiresAt: number; // unix ms
}

const AuthContext = createContext<AuthContextValue | null>(null);
const AUTH_COOKIE = 'cinema_auth';

function loadUser(): AuthUser | null {
  try {
    const raw = getCookie(AUTH_COOKIE);
    if (!raw) {
      // миграция со старого localStorage
      const legacy = localStorage.getItem('cinema_auth');
      if (legacy) {
        localStorage.removeItem('cinema_auth');
        const parsed = JSON.parse(legacy) as AuthUser;
        if (parsed?.token) {
          persistToCookie({
            ...parsed,
            expiresAt: Date.now() + COOKIE_MAX_AGE.auth * 1000,
          });
          return { id: parsed.id, login: parsed.login, token: parsed.token };
        }
      }
      return null;
    }

    const data = JSON.parse(raw) as StoredAuth;
    if (!data?.token || !data.expiresAt) return null;

    if (Date.now() >= data.expiresAt) {
      removeCookie(AUTH_COOKIE);
      return null;
    }

    return { id: data.id, login: data.login, token: data.token };
  } catch {
    removeCookie(AUTH_COOKIE);
    return null;
  }
}

function persistToCookie(data: StoredAuth | null): void {
  if (data) {
    setCookie(AUTH_COOKIE, JSON.stringify(data), COOKIE_MAX_AGE.auth);
  } else {
    removeCookie(AUTH_COOKIE);
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthUser | null>(loadUser);

  const persist = useCallback((u: AuthUser | null) => {
    setUser(u);
    if (u) {
      persistToCookie({
        ...u,
        expiresAt: Date.now() + COOKIE_MAX_AGE.auth * 1000,
      });
    } else {
      persistToCookie(null);
    }
  }, []);

  const login = useCallback(
    async (loginName: string, password: string) => {
      const res = await authApi.login({ login: loginName, password });
      persist({ id: res.id, login: res.login, token: res.idToken });
    },
    [persist],
  );

  const register = useCallback(
    async (loginName: string, password: string) => {
      const res = await authApi.register({ login: loginName, password });
      persist({ id: res.id, login: res.login, token: res.idToken });
    },
    [persist],
  );

  const logout = useCallback(() => persist(null), [persist]);

  const value = useMemo(
    () => ({
      user,
      isAuthenticated: !!user,
      login,
      register,
      logout,
    }),
    [user, login, register, logout],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used within AuthProvider');
  return ctx;
}