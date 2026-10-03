import { useTranslation } from 'react-i18next';
import './Loading.css';

export function Loading({ label }: { label?: string }) {
  const { t } = useTranslation();
  return (
    <div className="loading" role="status" aria-live="polite">
      <div className="loading__spinner" />
      <span>{label ?? t('loading')}</span>
    </div>
  );
}