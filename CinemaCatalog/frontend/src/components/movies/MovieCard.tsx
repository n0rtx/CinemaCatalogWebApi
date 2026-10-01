import { Link } from 'react-router-dom';
import type { Movie } from '../../types/movie';
import { yearValue } from '../../types/movie';
import './MovieCard.css';

interface Props {
  movie: Movie;
}

export function MovieCard({ movie }: Props) {
  const year = yearValue(movie.releaseYear);
  const poster = movie.poster || null;

  return (
    <Link to={`/movies/${movie.id}`} className="movie-card">
      <div className="movie-card__poster">
        {poster ? (
          <img src={poster} alt={movie.title} loading="lazy" />
        ) : (
          <div className="movie-card__placeholder">
            <span>{movie.title.charAt(0)}</span>
          </div>
        )}
        <div className="movie-card__year">{year || '—'}</div>
      </div>
      <div className="movie-card__info">
        <h3 className="movie-card__title" title={movie.title}>
          {movie.title}
        </h3>
        <p className="movie-card__meta">
          {movie.genre}
          {movie.director ? ` · ${movie.director}` : ''}
        </p>
      </div>
    </Link>
  );
}
