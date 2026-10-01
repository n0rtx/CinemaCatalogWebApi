import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { deleteMovie, getMovieById } from '../api/movies';
import type { Movie } from '../types/movie';
import { yearValue } from '../types/movie';
import { Loading } from '../components/ui/Loading';
import { Button } from '../components/ui/Button';
import { Modal } from '../components/ui/Modal';
import { MovieForm } from '../components/movies/MovieForm';
import { updateMovie } from '../api/movies';
import type { MovieFormData } from '../types/movie';
import './MovieDetailPage.css';

export function MovieDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [movie, setMovie] = useState<Movie | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [editOpen, setEditOpen] = useState(false);
  const [deleting, setDeleting] = useState(false);

  useEffect(() => {
    if (!id) return;
    let cancelled = false;
    (async () => {
      setLoading(true);
      setError('');
      try {
        const data = await getMovieById(id);
        if (!cancelled) setMovie(data);
      } catch (err) {
        if (!cancelled) setError(err instanceof Error ? err.message : 'Фильм не найден');
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();
    return () => {
      cancelled = true;
    };
  }, [id]);

  const handleUpdate = async (data: MovieFormData) => {
    if (!id) return;
    await updateMovie(id, data);
    const refreshed = await getMovieById(id);
    setMovie(refreshed);
    setEditOpen(false);
  };

  const handleDelete = async () => {
    if (!id || !confirm('Удалить этот фильм?')) return;
    setDeleting(true);
    try {
      await deleteMovie(id);
      navigate('/');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Ошибка удаления');
      setDeleting(false);
    }
  };

  if (loading) return <Loading />;
  if (error || !movie) {
    return (
      <div className="empty-state">
        <h2>{error || 'Фильм не найден'}</h2>
        <Link to="/">
          <Button variant="secondary">В каталог</Button>
        </Link>
      </div>
    );
  }

  const year = yearValue(movie.releaseYear);

  return (
    <article className="movie-detail">
      <div className="movie-detail__poster">
        {movie.poster ? (
          <img src={movie.poster} alt={movie.title} />
        ) : (
          <div className="movie-detail__placeholder">{movie.title.charAt(0)}</div>
        )}
      </div>

      <div className="movie-detail__content">
        <p className="movie-detail__genre">{movie.genre}</p>
        <h1>{movie.title}</h1>
        <p className="movie-detail__meta">
          {year} · {movie.director}
        </p>
        <p className="movie-detail__plot">{movie.plot}</p>

        <div className="movie-detail__actions">
          <Button variant="secondary" onClick={() => setEditOpen(true)}>
            Редактировать
          </Button>
          <Button variant="danger" onClick={handleDelete} disabled={deleting}>
            {deleting ? 'Удаление…' : 'Удалить'}
          </Button>
          <Link to="/">
            <Button variant="ghost">← Назад</Button>
          </Link>
        </div>
      </div>

      <Modal open={editOpen} title="Редактировать фильм" onClose={() => setEditOpen(false)}>
        <MovieForm
          initial={movie}
          submitLabel="Обновить"
          onSubmit={handleUpdate}
          onCancel={() => setEditOpen(false)}
        />
      </Modal>
    </article>
  );
}
