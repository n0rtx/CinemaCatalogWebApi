import { FormEvent, useState } from 'react';
import type { Movie, MovieFormData } from '../../types/movie';
import { yearValue } from '../../types/movie';
import { Button } from '../ui/Button';
import './MovieForm.css';

interface Props {
  initial?: Movie | null;
  submitLabel?: string;
  onSubmit: (data: MovieFormData) => Promise<void>;
  onCancel?: () => void;
}

function isHttpUrl(value: string | null | undefined): boolean {
  if (!value) return false;
  return /^https?:\/\//i.test(value);
}

export function MovieForm({ initial, submitLabel = 'Сохранить', onSubmit, onCancel }: Props) {
  const [title, setTitle] = useState(initial?.title ?? '');
  const [plot, setPlot] = useState(initial?.plot ?? '');
  const [director, setDirector] = useState(initial?.director ?? '');
  const [genre, setGenre] = useState(initial?.genre ?? '');
  const [releaseYear, setReleaseYear] = useState(
    initial ? yearValue(initial.releaseYear) : new Date().getFullYear(),
  );
  const [posterFile, setPosterFile] = useState<File | null>(null);
  const [posterUrl, setPosterUrl] = useState(
    isHttpUrl(initial?.poster) ? (initial!.poster as string) : '',
  );
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      await onSubmit({
        title: title.trim(),
        plot: plot.trim(),
        director: director.trim(),
        genre: genre.trim(),
        releaseYear,
        posterFile,
        posterUrl: posterUrl.trim() || undefined,
      });
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Ошибка сохранения');
    } finally {
      setLoading(false);
    }
  };

  const previewSrc =
    posterFile != null
      ? URL.createObjectURL(posterFile)
      : posterUrl.trim() || (initial?.poster && !isHttpUrl(initial.poster) ? initial.poster : '') || '';

  return (
    <form className="movie-form" onSubmit={handleSubmit}>
      {error && <div className="error-banner">{error}</div>}

      <label>
        <span>Название *</span>
        <input required minLength={1} maxLength={100} value={title} onChange={(e) => setTitle(e.target.value)} />
      </label>

      <label>
        <span>Сюжет *</span>
        <textarea
          required
          minLength={10}
          maxLength={200}
          rows={4}
          value={plot}
          onChange={(e) => setPlot(e.target.value)}
        />
      </label>

      <div className="movie-form__row">
        <label>
          <span>Режиссёр *</span>
          <input required value={director} onChange={(e) => setDirector(e.target.value)} />
        </label>
        <label>
          <span>Жанр *</span>
          <input required value={genre} onChange={(e) => setGenre(e.target.value)} />
        </label>
      </div>

      <div className="movie-form__row">
        <label>
          <span>Год *</span>
          <input
            type="number"
            required
            min={1888}
            max={new Date().getFullYear() + 5}
            value={releaseYear}
            onChange={(e) => setReleaseYear(Number(e.target.value))}
          />
        </label>
        <label>
          <span>Файл постера</span>
          <input
            type="file"
            accept="image/*"
            onChange={(e) => setPosterFile(e.target.files?.[0] ?? null)}
          />
        </label>
      </div>

      <label>
        <span>URL постера</span>
        <input
          type="url"
          placeholder="https://..."
          value={posterUrl}
          onChange={(e) => setPosterUrl(e.target.value)}
        />
      </label>

      {previewSrc ? (
        <div className="movie-form__preview">
          <img src={previewSrc} alt="Превью постера" />
        </div>
      ) : null}

      <div className="movie-form__actions">
        {onCancel && (
          <Button type="button" variant="secondary" onClick={onCancel}>
            Отмена
          </Button>
        )}
        <Button type="submit" disabled={loading}>
          {loading ? 'Сохранение…' : submitLabel}
        </Button>
      </div>
    </form>
  );
}