import { useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { getMovies } from '../api/movies';
import type { Movie } from '../types/movie';
import { MovieGrid } from '../components/movies/MovieGrid';
import { Pagination } from '../components/movies/Pagination';
import { Loading } from '../components/ui/Loading';

export function HomePage() {
  const { t } = useTranslation();
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
      setError(err instanceof Error ? err.message : t('home.loadError'));
      setMovies([]);
    } finally {
      setLoading(false);
    }
  }, [t]);

  useEffect(() => {
    void load(page);
  }, [page, load]);

  return (
    <>
      <div className="home-hero">
        <h1 className="section-title">{t('home.title')}</h1>
        <p className="home-hero__sub">{t('home.subtitle')}</p>
      </div>

      {error && <div className="error-banner">{error}</div>}
      {loading ? <Loading /> : <MovieGrid movies={movies} />}
      {!loading && (
        <Pagination page={page} totalPages={totalPages} onChange={setPage} />
      )}
    </>
  );
}