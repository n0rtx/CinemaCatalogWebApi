import { useEffect, useState } from 'react';
import { Link, useSearchParams } from 'react-router-dom';
import { searchMovie } from '../api/movies';
import type { Movie } from '../types/movie';
import { MovieCard } from '../components/movies/MovieCard';
import { Loading } from '../components/ui/Loading';
import { Button } from '../components/ui/Button';

export function SearchPage() {
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
          setError(err instanceof Error ? err.message : 'Ничего не найдено');
        }
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();
    return () => {
      cancelled = true;
    };
  }, [title]);

  if (!title) {
    return (
      <div className="empty-state">
        <h2>Введите название в поиске</h2>
        <Link to="/">
          <Button variant="secondary">В каталог</Button>
        </Link>
      </div>
    );
  }

  return (
    <>
      <h1 className="section-title">Поиск: «{title}»</h1>
      {loading && <Loading label="Ищем фильм…" />}
      {error && (
        <div className="empty-state">
          <h2>{error}</h2>
          <p>Попробуйте другое название или добавьте фильм вручную.</p>
          <Link to="/admin">
            <Button>Добавить фильм</Button>
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
