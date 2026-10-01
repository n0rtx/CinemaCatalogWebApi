export interface LoginRequest {
  login: string;
  password: string;
}

export interface RegisterRequest {
  login: string;
  password: string;
}

export interface AuthResponse {
  id: string;
  login: string;
  idToken: string;
}

export interface AuthUser {
  id: string;
  login: string;
  token: string;
}
