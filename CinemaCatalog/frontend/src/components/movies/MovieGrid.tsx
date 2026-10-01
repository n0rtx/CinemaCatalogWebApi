import type { Movie } from '../../types/movie';
import { MovieCard } from './MovieCard';
import './MovieGrid.css';

interface Props {
  movies: Movie[];
}

export function MovieGrid({ movies }: Props) {
  if (movies.length === 0) {
    return (
      <div className="empty-state">
        <h2>Фильмов пока нет</h2>
        <p>Добавьте первый фильм в разделе «Управление» или найдите через поиск (OMDb).</p>
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
