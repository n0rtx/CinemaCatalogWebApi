# CinemaCatalog Frontend

React + TypeScript фронтенд для CinemaCatalog Web API в стиле кинокаталога (Kinogo-like).

## Возможности

- Каталог фильмов с пагинацией (сетка постеров)
- Поиск по названию (`GET /api/movies/search`)
- Карточка фильма, редактирование и удаление
- Создание фильма с постером (multipart form)
- Регистрация / вход (`/api/auth`)
- Тёмная и светлая тема

## Запуск

```bash
# 1. Backend на http://localhost:5164
cd CinemaCatalog/src/CinemaCatalog.WebApi
dotnet run

# 2. Frontend
cd cinema-frontend
npm install
npm run dev
```

Открой http://localhost:5173

Vite proxy перенаправляет `/api/*` на `http://localhost:5164`.

Если API на другом адресе — создай `.env`:

```
VITE_API_URL=http://localhost:5164
```

## CORS

Если ходишь на API напрямую (без proxy), добавь CORS в `Program.cs` бэкенда:

```csharp
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));
// ...
app.UseCors();
```

## Структура

```
src/
  api/          — клиент API
  components/   — UI, layout, movies, auth
  context/      — тема и авторизация
  pages/        — страницы
  styles/       — global + themes
  types/        — TypeScript-типы
```
