import { FormEvent, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useAuth } from '../../context/AuthContext';
import { Button } from '../ui/Button';
import './AuthForm.css';

export function LoginForm() {
  const { t } = useTranslation();
  const { login } = useAuth();
  const navigate = useNavigate();
  const [loginName, setLoginName] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const onSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      await login(loginName.trim(), password);
      navigate('/');
    } catch (err) {
      setError(err instanceof Error ? err.message : t('auth.loginError'));
    } finally {
      setLoading(false);
    }
  };

  return (
    <form className="auth-form" onSubmit={onSubmit}>
      <h1>{t('auth.loginTitle')}</h1>
      <p className="auth-form__subtitle">{t('auth.loginSubtitle')}</p>

      {error && <div className="error-banner">{error}</div>}

      <label>
        <span>{t('auth.login')}</span>
        <input
          required
          autoComplete="username"
          value={loginName}
          onChange={(e) => setLoginName(e.target.value)}
        />
      </label>

      <label>
        <span>{t('auth.password')}</span>
        <input
          type="password"
          required
          autoComplete="current-password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </label>

      <Button type="submit" fullWidth disabled={loading}>
        {loading ? t('auth.submittingLogin') : t('auth.submitLogin')}
      </Button>

      <p className="auth-form__footer">
        {t('auth.noAccount')} <Link to="/register">{t('auth.registerLink')}</Link>
      </p>
    </form>
  );
}