import { useTranslation } from 'react-i18next';
import type { Movie } from '../../types/movie';
import { MovieCard } from './MovieCard';
import './MovieGrid.css';

interface Props {
  movies: Movie[];
}

export function MovieGrid({ movies }: Props) {
  const { t } = useTranslation();

  if (movies.length === 0) {
    return (
      <div className="empty-state">
        <h2>{t('movie.emptyTitle')}</h2>
        <p>{t('movie.emptyHint')}</p>
      </div>
    );
  }

  return (
    <div className="movie-grid">
      {movies.map((m) => (
        <MovieCard key={m.id} movie={m} />
      ))}
    </div>
  );
}