import { FormEvent, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useAuth } from '../../context/AuthContext';
import { Button } from '../ui/Button';
import './AuthForm.css';

export function RegisterForm() {
  const { t } = useTranslation();
  const { register } = useAuth();
  const navigate = useNavigate();
  const [loginName, setLoginName] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const onSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError('');
    if (password.length < 6) {
      setError(t('auth.passwordTooShort'));
      return;
    }
    setLoading(true);
    try {
      await register(loginName.trim(), password);
      navigate('/');
    } catch (err) {
      setError(err instanceof Error ? err.message : t('auth.registerError'));
    } finally {
      setLoading(false);
    }
  };

  return (
    <form className="auth-form" onSubmit={onSubmit}>
      <h1>{t('auth.registerTitle')}</h1>
      <p className="auth-form__subtitle">{t('auth.registerSubtitle')}</p>

      {error && <div className="error-banner">{error}</div>}

      <label>
        <span>{t('auth.loginMin')}</span>
        <input
          required
          minLength={3}
          maxLength={100}
          autoComplete="username"
          value={loginName}
          onChange={(e) => setLoginName(e.target.value)}
        />
      </label>

      <label>
        <span>{t('auth.passwordMin')}</span>
        <input
          type="password"
          required
          minLength={6}
          maxLength={100}
          autoComplete="new-password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </label>

      <Button type="submit" fullWidth disabled={loading}>
        {loading ? t('auth.submittingRegister') : t('auth.submitRegister')}
      </Button>

      <p className="auth-form__footer">
        {t('auth.hasAccount')} <Link to="/login">{t('auth.loginLink')}</Link>
      </p>
    </form>
  );
}