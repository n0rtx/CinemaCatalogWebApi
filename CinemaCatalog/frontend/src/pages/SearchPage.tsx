import { useEffect, useState } from 'react';
import { Link, useSearchParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { searchMovie } from '../api/movies';
import type { Movie } from '../types/movie';
import { MovieCard } from '../components/movies/MovieCard';
import { Loading } from '../components/ui/Loading';
import { Button } from '../components/ui/Button';

export function SearchPage() {
  const { t } = useTranslation();
  const [params] = useSearchParams();
  const title = params.get('title')?.trim() ?? '';
  const [movie, setMovie] = useState<Movie | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!title) return;
    let cancelled = false;
    (async () => {
      setLoading(true);
      setError('');
      setMovie(null);
      try {
        const data = await searchMovie(title);
        if (!cancelled) setMovie(data);
      } catch (err) {
        if (!cancelled) {
          setError(err instanceof Error ? err.message : t('search.notFound'));
        }
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();
    return () => {
      cancelled = true;
    };
  }, [title, t]);

  if (!title) {
    return (
      <div className="empty-state">
        <h2>{t('search.emptyTitle')}</h2>
        <Link to="/">
          <Button variant="secondary">{t('search.backToCatalog')}</Button>
        </Link>
      </div>
    );
  }

  return (
    <>
      <h1 className="section-title">{t('search.title', { title })}</h1>
      {loading && <Loading label={t('search.searching')} />}
      {error && (
        <div className="empty-state">
          <h2>{error}</h2>
          <p>{t('search.tryAgain')}</p>
          <Link to="/admin">
            <Button>{t('search.addMovie')}</Button>
          </Link>
        </div>
      )}
      {movie && (
        <div style={{ maxWidth: 220 }}>
          <MovieCard movie={movie} />
        </div>
      )}
    </>
  );
}