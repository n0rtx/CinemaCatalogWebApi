import './Loading.css';

export function Loading({ label = 'Загрузка…' }: { label?: string }) {
  return (
    <div className="loading" role="status" aria-live="polite">
      <div className="loading__spinner" />
      <span>{label}</span>
    </div>
  );
}
