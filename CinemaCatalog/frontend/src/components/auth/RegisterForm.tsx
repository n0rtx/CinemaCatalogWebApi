import { FormEvent, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { Button } from '../ui/Button';
import './AuthForm.css';

export function RegisterForm() {
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
      setError('Пароль не менее 6 символов');
      return;
    }
    setLoading(true);
    try {
      await register(loginName.trim(), password);
      navigate('/');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Ошибка регистрации');
    } finally {
      setLoading(false);
    }
  };

  return (
    <form className="auth-form" onSubmit={onSubmit}>
      <h1>Регистрация</h1>
      <p className="auth-form__subtitle">Создайте аккаунт CinemaCatalog</p>

      {error && <div className="error-banner">{error}</div>}

      <label>
        <span>Логин (мин. 3 символа)</span>
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
        <span>Пароль (мин. 6 символов)</span>
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
        {loading ? 'Регистрация…' : 'Зарегистрироваться'}
      </Button>

      <p className="auth-form__footer">
        Уже есть аккаунт? <Link to="/login">Войти</Link>
      </p>
    </form>
  );
}
