import { Link, NavLink, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useAuth } from '../../context/AuthContext';
import { ThemeToggle } from '../ui/ThemeToggle';
import { SearchBar } from '../movies/SearchBar';
import './Header.css';

export function Header() {
  const { t, i18n } = useTranslation();
  const { user, isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  const toggleLang = () => {
    const next = i18n.language === 'ru' ? 'en' : 'ru';
    void i18n.changeLanguage(next);
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
            {t('nav.catalog')}
          </NavLink>
          <NavLink to="/admin" className={({ isActive }) => (isActive ? 'active' : '')}>
            {t('nav.admin')}
          </NavLink>
        </nav>

        <div className="header__search">
          <SearchBar />
        </div>

        <div className="header__actions">
          <button
            type="button"
            className="header__lang"
            onClick={toggleLang}
            aria-label="Change language"
            title={i18n.language === 'ru' ? 'English' : 'Русский'}
          >
            {i18n.language === 'ru' ? t('lang.en') : t('lang.ru')}
          </button>
          <ThemeToggle />
          {isAuthenticated ? (
            <div className="header__user">
              <span className="header__login">{user?.login}</span>
              <button type="button" className="header__logout" onClick={handleLogout}>
                {t('nav.logout')}
              </button>
            </div>
          ) : (
            <div className="header__auth-links">
              <Link to="/login">{t('nav.login')}</Link>
              <Link to="/register" className="header__register">
                {t('nav.register')}
              </Link>
            </div>
          )}
        </div>
      </div>
    </header>
  );
}