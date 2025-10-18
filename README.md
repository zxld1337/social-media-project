# Social Media Project

Full-stack social media alkalmazás React, .NET Core és MySQL használatával.

## 🚀 Gyors indítás

### Követelmények
- Docker Desktop
- Git

### Telepítés és futtatás

1. **Repository klónozása**
```bash
git clone <repository-url>
cd social-media-project
```

2. **Környezet indítása**
```bash
docker-compose up --build
```

3. **Elérés**
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:5000
   - phpMyAdmin: http://localhost:8080
   - MySQL: localhost:3306

## 📦 Szolgáltatások

- **Frontend**: React + Nginx (Port 3000)
- **Backend**: .NET Core Web API (Port 5000)
- **Database**: MySQL 8.0 (Port 3306)
- **Admin**: phpMyAdmin (Port 8080)

## 🛠️ Fejlesztés

### Backend fejlesztés (.NET)
```bash
cd backend
dotnet run
```

### Frontend fejlesztés (React/Vite)
```bash
cd frontend
npm install
npm run dev
```

## 🗄️ Adatbázis hozzáférés

### phpMyAdmin
- URL: http://localhost:8080
- Felhasználó: `root`
- Jelszó: `rootpassword`

### Direct MySQL kapcsolat
- Host: `localhost`
- Port: `3306`
- Database: `socialmedia`
- User: `apiuser`
- Password: `apipassword`

## 🏗️ Projekt struktúra

```
social-media-project/
├── docker-compose.yml      # Docker orchestration
├── README.md              # Ez a fájl
├── .gitignore            # Git ignore rules
├── frontend/             # React frontend
│   ├── src/
│   ├── public/
│   ├── Dockerfile
│   ├── nginx.conf
│   └── package.json
├── backend/              # .NET Core API
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Dockerfile
│   └── Program.cs
└── database/             # Database scripts
    └── init.sql          # Kezdeti séma és adatok
```

## 🔧 Docker parancsok

### Első indítás (build-del együtt)
```bash
docker-compose up --build
```

### Normál indítás
```bash
docker-compose up
```

### Háttérben futtatás
```bash
docker-compose up -d
```

### Leállítás
```bash
docker-compose down
```

### Egy szolgáltatás újraindítása
```bash
docker-compose restart backend
```

## 📝 API Endpoints (tervezett)

### Authentication
- `POST /api/auth/register` - Regisztráció
- `POST /api/auth/login` - Bejelentkezés
- `POST /api/auth/logout` - Kijelentkezés

### Users
- `GET /api/users` - Összes felhasználó
- `GET /api/users/{id}` - Egy felhasználó
- `PUT /api/users/{id}` - Profil módosítás
- `DELETE /api/users/{id}` - Felhasználó törlése

### Posts
- `GET /api/posts` - Összes poszt
- `GET /api/posts/{id}` - Egy poszt
- `POST /api/posts` - Új poszt létrehozása
- `PUT /api/posts/{id}` - Poszt módosítása
- `DELETE /api/posts/{id}` - Poszt törlése

### Comments
- `GET /api/posts/{postId}/comments` - Poszt kommentjei
- `POST /api/posts/{postId}/comments` - Új komment
- `DELETE /api/comments/{id}` - Komment törlése

### Likes
- `POST /api/posts/{postId}/like` - Like
- `DELETE /api/posts/{postId}/like` - Unlike

## 🎯 Fejlesztési feladatok

### Sprint 1 - Alapok


## 👥 Csapat

- **Karina Suhajda**: Quality Assurance Tester, UI/UX Designer, Docs
- **Andor Endre Rohály**: Backend Developer, Database Engineer
- **Levente Silkó**: Frontend, Scrum Master, Project Manager, Lead Support Specialist, Cybersecurity Specialist, DevOps, Generalist

## 🐛 Hibakeresés

### Frontend nem indul
```bash
cd frontend
npm install
npm run dev
```

### Backend nem indul
```bash
cd backend
dotnet restore
dotnet run
```

### Adatbázis kapcsolat hiba
1. Ellenőrizd, hogy a MySQL konténer fut: `docker ps`
2. Nézd meg a logokat: `docker-compose logs db`
3. Újraindítás: `docker-compose restart db`

### Port foglalt hiba
Változtasd meg a portokat a `docker-compose.yml`-ben:
```yaml
ports:
  - "3001:80"  # Frontend új port
```

## 📚 Dokumentáció

- [React Docs](https://react.dev)
- [.NET Core Docs](https://docs.microsoft.com/dotnet)
- [MySQL Docs](https://dev.mysql.com/doc/)
- [Docker Docs](https://docs.docker.com)

## 📄 Licensz

MIT License - szabadon használható tanulási célokra.