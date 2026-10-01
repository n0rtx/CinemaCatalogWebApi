import { Link, NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { ThemeToggle } from '../ui/ThemeToggle';
import { SearchBar } from '../movies/SearchBar';
import './Header.css';

export function Header() {
  const { user, isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <header className="header">
      <div className="container header__inner">
        <Link to="/" className="header__logo">
          <span className="header__logo-mark">C</span>
          <span className="header__logo-text">
            Cinema<span>Catalog</span>
          </span>
        </Link>

        <nav className="header__nav">
          <NavLink to="/" end className={({ isActive }) => (isActive ? 'active' : '')}>
            Каталог
          </NavLink>
          <NavLink to="/admin" className={({ isActive }) => (isActive ? 'active' : '')}>
            Управление
          </NavLink>
        </nav>

        <div className="header__search">
          <SearchBar />
        </div>

        <div className="header__actions">
          <ThemeToggle />
          {isAuthenticated ? (
            <div className="header__user">
              <span className="header__login">{user?.login}</span>
              <button type="button" className="header__logout" onClick={handleLogout}>
                Выйти
              </button>
            </div>
          ) : (
            <div className="header__auth-links">
              <Link to="/login">Вход</Link>
              <Link to="/register" className="header__register">
                Регистрация
              </Link>
            </div>
          )}
        </div>
      </div>
    </header>
  );
}
