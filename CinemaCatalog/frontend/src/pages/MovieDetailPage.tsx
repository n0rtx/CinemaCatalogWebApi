import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { deleteMovie, getMovieById, updateMovie } from '../api/movies';
import type { Movie, MovieFormData } from '../types/movie';
import { yearValue } from '../types/movie';
import { Loading } from '../components/ui/Loading';
import { Button } from '../components/ui/Button';
import { Modal } from '../components/ui/Modal';
import { MovieForm } from '../components/movies/MovieForm';
import './MovieDetailPage.css';

export function MovieDetailPage() {
  const { t } = useTranslation();
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
        if (!cancelled) setError(err instanceof Error ? err.message : t('movie.notFound'));
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();
    return () => {
      cancelled = true;
    };
  }, [id, t]);

  const handleUpdate = async (data: MovieFormData) => {
    if (!id) return;
    await updateMovie(id, data);
    const refreshed = await getMovieById(id);
    setMovie(refreshed);
    setEditOpen(false);
  };

  const handleDelete = async () => {
    if (!id || !confirm(t('movie.confirmDelete'))) return;
    setDeleting(true);
    try {
      await deleteMovie(id);
      navigate('/');
    } catch (err) {
      setError(err instanceof Error ? err.message : t('movie.deleteError'));
      setDeleting(false);
    }
  };

  if (loading) return <Loading />;
  if (error || !movie) {
    return (
      <div className="empty-state">
        <h2>{error || t('movie.notFound')}</h2>
        <Link to="/">
          <Button variant="secondary">{t('search.backToCatalog')}</Button>
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
            {t('movie.edit')}
          </Button>
          <Button variant="danger" onClick={handleDelete} disabled={deleting}>
            {deleting ? t('movie.deleting') : t('movie.delete')}
          </Button>
          <Link to="/">
            <Button variant="ghost">{t('movie.back')}</Button>
          </Link>
        </div>
      </div>

      <Modal open={editOpen} title={t('movie.editTitle')} onClose={() => setEditOpen(false)}>
        <MovieForm
          initial={movie}
          submitLabel={t('movie.update')}
          onSubmit={handleUpdate}
          onCancel={() => setEditOpen(false)}
        />
      </Modal>
    </article>
  );
}