import { apiFetch } from './client';
import type { Movie, MovieFormData, PagedMovieResponse } from '../types/movie';

export async function getMovies(page = 1): Promise<PagedMovieResponse> {
  return apiFetch<PagedMovieResponse>(`/api/movies?page=${page}`);
}

export async function getMovieById(id: string): Promise<Movie> {
  return apiFetch<Movie>(`/api/movies/${id}`);
}

export async function searchMovie(title: string): Promise<Movie> {
  const q = encodeURIComponent(title);
  return apiFetch<Movie>(`/api/movies/search?title=${q}`);
}

function toFormData(data: MovieFormData): FormData {
  const fd = new FormData();
  fd.append('Title', data.title);
  fd.append('Plot', data.plot);
  fd.append('Director', data.director);
  fd.append('Genre', data.genre);
  fd.append('ReleaseYear', String(data.releaseYear));
  if (data.posterFile) {
    fd.append('formFile', data.posterFile);
  }
  if (data.posterUrl?.trim()) {
    fd.append('PosterUrl', data.posterUrl.trim());
  }
  return fd;
}

export async function createMovie(data: MovieFormData): Promise<Movie> {
  return apiFetch<Movie>('/api/movies', {
    method: 'POST',
    body: toFormData(data),
  });
}

export async function updateMovie(id: string, data: MovieFormData): Promise<void> {
  await apiFetch<void>(`/api/movies/${id}`, {
    method: 'PUT',
    body: toFormData(data),
  });
}

export async function deleteMovie(id: string): Promise<void> {
  await apiFetch<void>(`/api/movies/${id}`, { method: 'DELETE' });
}