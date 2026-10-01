import { FormEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './SearchBar.css';

export function SearchBar() {
  const [query, setQuery] = useState('');
  const navigate = useNavigate();

  const onSubmit = (e: FormEvent) => {
    e.preventDefault();
    const q = query.trim();
    if (!q) return;
    navigate(`/search?title=${encodeURIComponent(q)}`);
  };

  return (
    <form className="search-bar" onSubmit={onSubmit} role="search">
      <input
        type="search"
        placeholder="Найти фильм…"
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        aria-label="Поиск фильма"
      />
      <button type="submit" aria-label="Искать">
        <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.2">
          <circle cx="11" cy="11" r="7" />
          <path d="M20 20l-3.5-3.5" />
        </svg>
      </button>
    </form>
  );
}
