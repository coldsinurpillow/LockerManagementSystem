#LockerManagementSystem

**API for distributing lockers to students

## Models

**User — студент.
**Поля: Id, FirstName, MiddleName, LastName, Group, BarCode, Iin.
**Связь: может занимать 0 или 1 место в LockerPlace.

**Locker — шкафчик.
**Поля: Id, Number (строка "0001"), PlaceCount, Type (key/passcode).
**Связь: содержит PlaceCount мест → LockerPlaces.

**LockerPlace — конкретное место в шкафчике.
**Поля: Id, PlaceIndex (1..N), LockerId, UserId.
**Связь: принадлежит Locker, может быть занято User.

## AppDbContext

**AppDbContext управляет доступом к БД через EF Core:
**DbSet<User> — таблица Users,
**DbSet<Locker> — таблица Lockers,
**DbSet<LockerPlace> — таблица LockerPlaces.

**В OnModelCreating описываются правила:
**уникальный Locker.Number,
**уникальность (LockerId, PlaceIndex) (нельзя дважды занять одно место),
**при удалении Locker → удаляются его Places,
**при удалении User → UserId в LockerPlace обнуляется (место освобождается).

## API-Controllers
**UsersController
POST /api/users — создать пользователя.
GET /api/users/{id} — получить инфо (включая активный локер/место).
PUT /api/users/{id} — обновить данные.
DELETE /api/users/{id} — удалить + освободить место.
GET /api/users — список пользователей.

**LockersController
POST /api/lockers — создать локер (и автоматически места 1..N).
GET /api/lockers/{number} — информация о локере + список мест.
POST /api/lockers/assign — назначить пользователя на место (или на первое свободное).
PUT /api/lockers/reassign — переназначить (тот же assign).
DELETE /api/lockers/assigned — освободить место.
POST /api/lockers/distribute — автораздать N новых пользователей по свободным местам.