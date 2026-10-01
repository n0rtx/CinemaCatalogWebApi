import { useCallback, useEffect, useState } from 'react';
import { getMovies } from '../api/movies';
import type { Movie } from '../types/movie';
import { MovieGrid } from '../components/movies/MovieGrid';
import { Pagination } from '../components/movies/Pagination';
import { Loading } from '../components/ui/Loading';

export function HomePage() {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const load = useCallback(async (p: number) => {
    setLoading(true);
    setError('');
    try {
      const data = await getMovies(p);
      setMovies(data.movies ?? []);
      setPage(data.page);
      setTotalPages(Math.max(1, data.totalPages));
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Не удалось загрузить каталог');
      setMovies([]);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    void load(page);
  }, [page, load]);

  return (
    <>
      <div className="home-hero">
        <h1 className="section-title">Каталог фильмов</h1>
        <p className="home-hero__sub">Смотри, ищи и добавляй фильмы в свою коллекцию</p>
      </div>

      {error && <div className="error-banner">{error}</div>}
      {loading ? <Loading /> : <MovieGrid movies={movies} />}
      {!loading && (
        <Pagination page={page} totalPages={totalPages} onChange={setPage} />
      )}
    </>
  );
}
