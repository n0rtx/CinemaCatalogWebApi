import { useTranslation } from 'react-i18next';
import './Footer.css';

export function Footer() {
  const { t } = useTranslation();

  return (
    <footer className="footer">
      <div className="container footer__inner">
        <p>{t('footer.copyright', { year: new Date().getFullYear() })}</p>
        <p className="footer__muted">{t('footer.stack')}</p>
      </div>
    </footer>
  );
}