import { useCallback, useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { createMovie, deleteMovie, getMovies } from '../api/movies';
import type { Movie, MovieFormData } from '../types/movie';
import { yearValue } from '../types/movie';
import { MovieForm } from '../components/movies/MovieForm';
import { Modal } from '../components/ui/Modal';
import { Button } from '../components/ui/Button';
import { Loading } from '../components/ui/Loading';
import { Pagination } from '../components/movies/Pagination';
import './AdminPage.css';

export function AdminPage() {
  const { t } = useTranslation();
  const [movies, setMovies] = useState<Movie[]>([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [createOpen, setCreateOpen] = useState(false);

  const load = useCallback(async (p: number) => {
    setLoading(true);
    setError('');
    try {
      const data = await getMovies(p);
      setMovies(data.movies ?? []);
      setPage(data.page);
      setTotalPages(Math.max(1, data.totalPages));
    } catch (err) {
      setError(err instanceof Error ? err.message : t('admin.loadError'));
    } finally {
      setLoading(false);
    }
  }, [t]);

  useEffect(() => {
    void load(page);
  }, [page, load]);

  const handleCreate = async (data: MovieFormData) => {
    await createMovie(data);
    setCreateOpen(false);
    setSuccess(t('admin.movieAdded'));
    await load(1);
    setPage(1);
  };

  const handleDelete = async (id: string, title: string) => {
    if (!confirm(t('admin.confirmDelete', { title }))) return;
    try {
      await deleteMovie(id);
      setSuccess(t('admin.movieDeleted'));
      await load(page);
    } catch (err) {
      setError(err instanceof Error ? err.message : t('admin.deleteError'));
    }
  };

  return (
    <>
      <div className="admin-header">
        <div>
          <h1 className="section-title">{t('admin.title')}</h1>
          <p className="admin-header__sub">{t('admin.subtitle')}</p>
        </div>
        <Button onClick={() => setCreateOpen(true)}>{t('admin.addMovie')}</Button>
      </div>

      {error && <div className="error-banner">{error}</div>}
      {success && <div className="success-banner">{success}</div>}

      {loading ? (
        <Loading />
      ) : (
        <div className="admin-table-wrap">
          <table className="admin-table">
            <thead>
              <tr>
                <th>{t('admin.colTitle')}</th>
                <th>{t('admin.colYear')}</th>
                <th>{t('admin.colGenre')}</th>
                <th>{t('admin.colDirector')}</th>
                <th />
              </tr>
            </thead>
            <tbody>
              {movies.length === 0 && (
                <tr>
                  <td colSpan={5} className="admin-table__empty">
                    {t('admin.empty')}
                  </td>
                </tr>
              )}
              {movies.map((m) => (
                <tr key={m.id}>
                  <td>
                    <Link to={`/movies/${m.id}`}>{m.title}</Link>
                  </td>
                  <td>{yearValue(m.releaseYear)}</td>
                  <td>{m.genre}</td>
                  <td>{m.director}</td>
                  <td className="admin-table__actions">
                    <Link to={`/movies/${m.id}`}>
                      <Button variant="ghost">{t('admin.open')}</Button>
                    </Link>
                    <Button variant="danger" onClick={() => handleDelete(m.id, m.title)}>
                      {t('admin.delete')}
                    </Button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      <Pagination page={page} totalPages={totalPages} onChange={setPage} />

      <Modal open={createOpen} title={t('admin.newMovie')} onClose={() => setCreateOpen(false)}>
        <MovieForm
          submitLabel={t('admin.create')}
          onSubmit={handleCreate}
          onCancel={() => setCreateOpen(false)}
        />
      </Modal>
    </>
  );
}