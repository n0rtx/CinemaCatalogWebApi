import './Footer.css';

export function Footer() {
  return (
    <footer className="footer">
      <div className="container footer__inner">
        <p>© {new Date().getFullYear()} CinemaCatalog — учебный каталог фильмов</p>
        <p className="footer__muted">API · Firestore · React</p>
      </div>
    </footer>
  );
}
