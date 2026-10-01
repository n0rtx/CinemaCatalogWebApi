export interface Movie {
  id: string;
  title: string;
  plot: string;
  director: string;
  genre: string;
  releaseYear: number | { value: number };
  poster: string | null;
}

export interface PagedMovieResponse {
  page: number;
  totalPages: number;
  movies: Movie[];
}

export interface MovieFormData {
  title: string;
  plot: string;
  director: string;
  genre: string;
  releaseYear: number;
  posterFile?: File | null;
  posterUrl?: string;
}

export function yearValue(y: number | { value: number }): number {
  return typeof y === 'number' ? y : y.value;
}