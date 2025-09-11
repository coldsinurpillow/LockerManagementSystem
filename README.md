# LockerManagementSystem

**API for distributing lockers to students / API для распределения шкафчиков студентам**

---

## Models / Модели

### User — Student / Студент
- **Fields / Поля**: `Id`, `FirstName`, `MiddleName`, `LastName`, `Group`, `BarCode`, `Iin`
- **Relation / Связь**: can occupy 0 or 1 place in `LockerPlace`  
  может занимать 0 или 1 место в `LockerPlace`

---

### Locker — Locker / Шкафчик
- **Fields / Поля**: `Id`, `Number` (string `"0001"`), `PlaceCount`, `Type` (`key`/`passcode`)
- **Relation / Связь**: contains `PlaceCount` places → `LockerPlaces`  
  содержит `PlaceCount` мест → `LockerPlaces`

---

### LockerPlace — Specific place / Конкретное место
- **Fields / Поля**: `Id`, `PlaceIndex` (1..N), `LockerId`, `UserId`
- **Relation / Связь**: belongs to `Locker`, can be occupied by `User`  
  принадлежит `Locker`, может быть занято `User`

---

## AppDbContext

`AppDbContext` manages access to the database via EF Core.  
`AppDbContext` управляет доступом к БД через EF Core.  

- `DbSet<User>` → `Users`  
- `DbSet<Locker>` → `Lockers`  
- `DbSet<LockerPlace>` → `LockerPlaces`

**Rules in `OnModelCreating`:**
- Unique `Locker.Number`  
- Uniqueness of `(LockerId, PlaceIndex)` (a place cannot be reused)  
- Deleting a `Locker` deletes all its `Places`  
- Deleting a `User` sets `UserId` in `LockerPlace` to `null` (frees place)

**Правила в `OnModelCreating`:**
- Уникальный `Locker.Number`  
- Уникальность `(LockerId, PlaceIndex)` (нельзя занять место дважды)  
- При удалении `Locker` → удаляются его `Places`  
- При удалении `User` → `UserId` в `LockerPlace` обнуляется (место освобождается)  

---

## API Controllers

### UsersController
- `POST /api/users` — create user / создать пользователя  
- `GET /api/users/{id}` — get user info (with active locker/place) / получить инфо (включая активный локер/место)  
- `PUT /api/users/{id}` — update user / обновить данные  
- `DELETE /api/users/{id}` — delete user and free locker / удалить + освободить место  
- `GET /api/users` — list users / список пользователей  

### LockersController
- `POST /api/lockers` — create locker (with places 1..N) / создать локер (и автоматически места 1..N)  
- `GET /api/lockers/{number}` — get locker info / информация о локере  
- `POST /api/lockers/assign` — assign user to place / назначить пользователя на место  
- `PUT /api/lockers/reassign` — reassign user / переназначить  
- `DELETE /api/lockers/assigned` — free place / освободить место  
- `POST /api/lockers/distribute` — auto-distribute N users / автораздать N новых пользователей  

---

##  Run Project / Запуск проекта

### 1. Установить зависимости
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for PostgreSQL) / (для PostgreSQL)
- [DBeaver](https://dbeaver.io/) or PgAdmin (optional, to view the database) / или PgAdmin (по желанию, для просмотра БД)

### 1. Start database in Docker / Поднять базу в Docker
```bash
docker compose up -d

### 2. Run migrations / Применить миграции
dotnet ef database update

### 3. Run the project / Запустить проект
dotnet run --project LockerManagementSystem

