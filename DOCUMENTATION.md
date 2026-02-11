# Music Catalog API - Документация

## Съдържание
1. [Концепция на проекта](#1-концепция-на-проекта)
2. [Таблици в базата](#2-таблици-в-базата)
3. [Връзки едно към много (1:N)](#3-връзки-едно-към-много-1n)
4. [Връзки много към много (M:N)](#4-връзки-много-към-много-mn)
5. [JWT Автентикация](#5-jwt-автентикация)
6. [ControllerBase](#6-controllerbase)
7. [Атрибути за авторизация](#7-атрибути-за-авторизация)
8. [Program.cs конфигурация](#8-programcs-конфигурация)
9. [API Endpoints](#9-api-endpoints)

---

## 1. Концепция на проекта

### Обща характеристика

Този проект представлява RESTful Web API за управление на музикален каталог, разработен с ASP.NET Core и Entity Framework Core. Системата позволява съхранение и манипулация на информация за артисти, албуми, песни, плейлисти, музикални жанрове, настроения и награди. API-то е защитено чрез JWT (JSON Web Token) автентикация и поддържа ролева авторизация с два типа потребители.

### Функционалности

**Управление на артисти (Artists)**
- Създаване, преглед, редактиране и изтриване на музикални изпълнители
- Всеки артист има сценично име, описание и принадлежност към звукозаписна компания
- Артистите могат да получават награди и да създават песни и албуми

**Управление на албуми (Albums)**
- CRUD операции за музикални албуми
- Всеки албум има име, описание и е свързан с артист
- Албумите съдържат песни и могат да имат множество настроения (moods)

**Управление на песни (Songs)**
- Създаване и управление на музикални композиции
- Всяка песен има заглавие, продължителност и принадлежи на артист
- Песните могат да бъдат част от албум и да имат множество жанрове

**Плейлисти (Playlists)**
- Потребителите могат да създават собствени плейлисти
- Добавяне и премахване на песни от плейлисти
- Всеки плейлист принадлежи на конкретен потребител

**Жанрове и настроения (Genres & Moods)**
- Категоризация на музиката по жанрове (Rock, Pop, Jazz и др.)
- Определяне на настроението на албумите (Happy, Sad, Energetic и др.)

**Награди (Awards)**
- Регистриране на музикални награди с година на връчване
- Свързване на награди с артисти-победители

**Звукозаписни компании и държави (Record Labels & Countries)**
- Управление на информация за звукозаписни компании
- Всяка компания е базирана в определена държава

### Потребители и права за достъп

| Роля | Права |
|------|-------|
| **Анонимен** | Преглед на всички данни (GET заявки) |
| **Consumer** | Анонимни права + Създаване и управление на собствени плейлисти |
| **Admin** | Пълен достъп - CRUD операции върху всички ресурси |

### Технологии

- **ASP.NET Core** - уеб framework за изграждане на API
- **Entity Framework Core** - ORM за работа с база данни
- **SQL Server LocalDB** - релационна база данни
- **JWT Bearer Authentication** - автентикация чрез токени
- **FluentValidation** - валидация на входните данни
- **Swagger/OpenAPI** - документация и тестване на API

### Архитектура

Проектът следва многослойна архитектура:

1. **API слой (Controllers)** - обработва HTTP заявки и отговори
2. **Service слой** - бизнес логика и операции с данни
3. **Data слой (Entities, DbContext)** - модели и достъп до база данни
4. **DTOs** - обекти за трансфер на данни (Requests/Responses)

### Сигурност

- Паролите се хешират със SHA256 преди съхранение в базата
- JWT токените са валидни 30 минути
- Endpoints са защитени с `[Authorize]` атрибути според изискваните права
- Потребителите могат да редактират само собствените си плейлисти

---

## 2. Таблици в базата

| Таблица | Описание |
|---------|----------|
| **Users** | Потребители с username, password hash, роля |
| **Artists** | Артисти със сценично име, описание |
| **Albums** | Албуми с име, описание |
| **Songs** | Песни със заглавие, продължителност |
| **Playlists** | Плейлисти създадени от потребители |
| **Genres** | Музикални жанрове (Pop, Rock...) |
| **Moods** | Настроения (Happy, Sad...) |
| **Awards** | Музикални награди с година |
| **RecordLabels** | Звукозаписни компании |
| **Countries** | Държави (свързани с RecordLabels) |

---

## 3. Връзки Едно към Много (1:N)

| Parent | Child | Описание |
|--------|-------|----------|
| **Country** → RecordLabel | Една държава има много звукозаписни компании |
| **RecordLabel** → Artist | Една компания има много артисти |
| **Artist** → Album | Един артист има много албуми |
| **Artist** → Song | Един артист има много песни |
| **Album** → Song | Един албум съдържа много песни |
| **User** → Playlist | Един потребител има много плейлисти |

**Пример в код:**
```csharp
// В Artist.cs
public int RecordLabelId { get; set; }
public RecordLabel RecordLabel { get; set; }

// В RecordLabel.cs
public ICollection<Artist> Artists { get; } = new List<Artist>();
```

---

## 4. Връзки Много към Много (M:N)

| Таблица 1 | Таблица 2 | Junction Table |
|-----------|-----------|----------------|
| **Song** ↔ Genre | GenreSong |
| **Song** ↔ Playlist | PlaylistSong |
| **Album** ↔ Mood | AlbumMood |
| **Award** ↔ Artist | ArtistAward |

**Пример в код:**
```csharp
// В Song.cs
public ICollection<Genre> Genres { get; } = new List<Genre>();
public ICollection<Playlist> Playlists { get; } = new List<Playlist>();

// В Genre.cs
public ICollection<Song> Songs { get; } = new List<Song>();
```

---

## 5. JWT Автентикация

### Какво е JWT?
**JSON Web Token** - начин за сигурна автентикация между клиент и сървър.

### Как работи потокът:

```
┌──────────────────────────────────────────────────────────────┐
│  1. ЛОГИН (POST /api/auth)                                   │
│     ─────────────────────                                    │
│     Клиент изпраща: { username, password }                   │
│     Сървър проверява credentials → Създава JWT токен         │
│     Връща: "eyJhbGciOiJIUzI1NiIs..."                         │
└──────────────────────────────────────────────────────────────┘
                            ↓
┌──────────────────────────────────────────────────────────────┐
│  2. ЗАЯВКА С ТОКЕН                                           │
│     ──────────────────                                       │
│     Header: Authorization: Bearer eyJhbGciOiJIUzI1Ni...      │
│     ASP.NET валидира токена автоматично                      │
│     Ако е валиден → достъп до endpoint-а                     │
└──────────────────────────────────────────────────────────────┘
```

### JWT Token структура

Токенът се състои от 3 части (разделени с `.`):

```
eyJhbGciOiJIUzI1NiIs.eyJsb2dnZWRVc2VySWQiOiIx.SflKxwRJSMeKKF2QT4
   │                    │                         │
   │                    │                         └── SIGNATURE (подпис)
   │                    └── PAYLOAD (claims - данните)
   └── HEADER (алгоритъм)
```

**Payload (декодиран):**
```json
{
  "loggedUserId": "1",
  "role": "Admin",
  "exp": 1739117400
}
```

### TokenService.cs - Създаване на токен

```csharp
// 1. CLAIMS - данни за потребителя, вградени в токена
Claim[] claims = new Claim[]
{
    new Claim("loggedUserId", user.Id.ToString()),  // ID на потребителя
    new Claim(ClaimTypes.Role, user.Role.ToString()) // Роля: "Admin" или "Consumer"
};

// 2. КЛЮЧ - секретен ключ за подписване (трябва да е еднакъв навсякъде!)
var key = new SymmetricSecurityKey(
    Encoding.ASCII.GetBytes("!Password123!Password123!Password123")
);

// 3. ТОКЕН - създаване с всички параметри
JwtSecurityToken token = new JwtSecurityToken(
    issuer: "MusicCalalog",     // Кой е издал токена
    audience: "Users",          // За кого е предназначен
    claims: claims,             // Данни в токена
    expires: DateTime.Now.AddMinutes(30),  // Валидност: 30 мин
    signingCredentials: cred    // Подпис
);
```

### Достъп до данни от токена в контролер

```csharp
// Взимане на userId от токена
var userId = int.Parse(User.FindFirst("loggedUserId")!.Value);

// Проверка дали е Admin
var isAdmin = User.IsInRole("Admin");

// Взимане на ролята
var role = User.FindFirst(ClaimTypes.Role)?.Value;
```

**`User`** е property от `ControllerBase` - обект от тип `ClaimsPrincipal`, който се попълва автоматично от ASP.NET, когато JWT middleware-а валидира токена.

---

## 6. ControllerBase

**`ControllerBase`** е базов клас (родителски клас), от който всички API контролери наследяват.

```csharp
public class SongController : ControllerBase
//                            ↑
//                       наследява от ControllerBase
```

### Какво ти дава `ControllerBase`:

| Property/Method | Описание |
|-----------------|----------|
| `User` | Текущо автентикиран потребител (от JWT токена) |
| `Ok()` | Връща HTTP 200 с данни |
| `NotFound()` | Връща HTTP 404 |
| `BadRequest()` | Връща HTTP 400 |
| `NoContent()` | Връща HTTP 204 |
| `Unauthorized()` | Връща HTTP 401 |
| `Request` | HTTP заявката |
| `Response` | HTTP отговора |
| `ModelState` | Състояние на валидацията |

**Пример:**
```csharp
return Ok(data);        // от ControllerBase
return NotFound();      // от ControllerBase  
var userId = User...;   // от ControllerBase
```

---

## 7. Атрибути за авторизация

### `[HttpPut("{id}")]` - HTTP атрибут
```csharp
[HttpPut("{id}")]
```
- Дефинира **HTTP метода** (PUT) и **маршрута** (route)
- `{id}` е **route parameter** - взима стойността от URL-а
- Пример: `PUT /api/song/5` → `id = 5`

### `[Authorize]` - Изисква автентикация
```csharp
[Authorize]
```
- Изисква валиден JWT токен
- Ако няма токен → **401 Unauthorized**

### `[Authorize(Roles = "Admin")]` - Изисква роля
```csharp
[Authorize(Roles = "Admin")]
```
- Изисква валиден JWT токен **И** правилна роля
- Ако няма токен → **401 Unauthorized**
- Ако има токен, но грешна роля → **403 Forbidden**

### `[AllowAnonymous]` - Без автентикация
```csharp
[AllowAnonymous]
```
- Позволява достъп без токен
- Публичен endpoint

---

## 8. Program.cs конфигурация

### Регистрация на DbContext
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
        b => b.MigrationsAssembly("MusicCatalog.Api")));
```

### Регистрация на Services (Dependency Injection)
```csharp
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AlbumService>();
builder.Services.AddScoped<ArtistService>();
// ... останалите services
```
- `AddScoped` = нова инстанция за всяка HTTP заявка

### JWT конфигурация
```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,           // Провери issuer
        ValidateAudience = true,         // Провери audience
        ValidateLifetime = true,         // Провери дали не е изтекъл
        ValidateIssuerSigningKey = true, // Провери подписа
        
        // Очаквани стойности (трябва да съвпадат с TokenService!)
        ValidIssuer = "MusicCalalog",
        ValidAudience = "Users",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("!Password123!Password123!Password123")
        )
    };
});
```

### Middleware ред (ВАЖНО!)
```csharp
app.UseAuthentication();  // 1. Първо декодира токена
app.UseAuthorization();   // 2. После проверява [Authorize]
app.MapControllers();     // 3. Накрая маршрутизира към контролера
```

---

## 9. API Endpoints

### Auth
| Method | Endpoint | Auth | Описание |
|--------|----------|------|----------|
| POST | `/api/auth` | Anonymous | Логин, връща JWT токен |

### Country
| Method | Endpoint | Auth | Описание |
|--------|----------|------|----------|
| GET | `/api/country` | Anonymous | Всички държави |
| GET | `/api/country/{id}` | Anonymous | Държава по ID |
| POST | `/api/country` | Admin | Създай държава |
| PUT | `/api/country/{id}` | Admin | Редактирай държава |
| DELETE | `/api/country/{id}` | Admin | Изтрий държава |

### RecordLabel
| Method | Endpoint | Auth | Описание |
|--------|----------|------|----------|
| GET | `/api/recordlabel` | Anonymous | Всички лейбъли |
| GET | `/api/recordlabel/{id}` | Anonymous | Лейбъл по ID |
| POST | `/api/recordlabel` | Admin | Създай лейбъл |
| PUT | `/api/recordlabel/{id}` | Admin | Редактирай лейбъл |
| DELETE | `/api/recordlabel/{id}` | Admin | Изтрий лейбъл |

### Artist
| Method | Endpoint | Auth | Описание |
|--------|----------|------|----------|
| GET | `/api/artist` | Anonymous | Всички артисти |
| GET | `/api/artist/{id}` | Anonymous | Артист по ID |
| POST | `/api/artist` | Admin | Създай артист |
| PUT | `/api/artist/{id}` | Admin | Редактирай артист |
| DELETE | `/api/artist/{id}` | Admin | Изтрий артист |

### Album
| Method | Endpoint | Auth | Описание |
|--------|----------|------|----------|
| GET | `/api/album` | Anonymous | Всички албуми |
| GET | `/api/album/{id}` | Anonymous | Албум по ID |
| POST | `/api/album` | Admin | Създай албум |
| PUT | `/api/album/{id}` | Admin | Редактирай албум |
| DELETE | `/api/album/{id}` | Admin | Изтрий албум |
| PUT | `/api/album/{albumId}/song/{songId}` | Admin | Добави песен към албум |
| DELETE | `/api/album/{albumId}/song/{songId}` | Admin | Премахни песен от албум |
| PUT | `/api/album/{albumId}/mood/{moodId}` | Admin | Добави настроение |
| DELETE | `/api/album/{albumId}/mood/{moodId}` | Admin | Премахни настроение |

### Song
| Method | Endpoint | Auth | Описание |
|--------|----------|------|----------|
| GET | `/api/song` | Anonymous | Всички песни |
| GET | `/api/song/{id}` | Anonymous | Песен по ID |
| POST | `/api/song` | Admin | Създай песен |
| PUT | `/api/song/{id}` | Admin | Редактирай песен |
| DELETE | `/api/song/{id}` | Admin | Изтрий песен |
| PUT | `/api/song/{songId}/genre/{genreId}` | Admin | Добави жанр |
| DELETE | `/api/song/{songId}/genre/{genreId}` | Admin | Премахни жанр |

### Playlist
| Method | Endpoint | Auth | Описание |
|--------|----------|------|----------|
| GET | `/api/playlist` | Anonymous | Всички плейлисти |
| GET | `/api/playlist/{id}` | Anonymous | Плейлист по ID |
| GET | `/api/playlist/user/{userId}` | Anonymous | Плейлисти на потребител |
| GET | `/api/playlist/user/username/{username}` | Anonymous | Плейлисти по username |
| POST | `/api/playlist` | User | Създай плейлист |
| PUT | `/api/playlist/{id}` | User (owner) | Редактирай плейлист |
| DELETE | `/api/playlist/{id}` | User (owner) | Изтрий плейлист |
| PUT | `/api/playlist/{playlistId}/song/{songId}` | User (owner) | Добави песен |
| DELETE | `/api/playlist/{playlistId}/song/{songId}` | User (owner) | Премахни песен |

### Genre
| Method | Endpoint | Auth | Описание |
|--------|----------|------|----------|
| GET | `/api/genre` | Anonymous | Всички жанрове |
| GET | `/api/genre/{id}` | Anonymous | Жанр по ID |
| POST | `/api/genre` | Admin | Създай жанр |
| PUT | `/api/genre/{id}` | Admin | Редактирай жанр |
| DELETE | `/api/genre/{id}` | Admin | Изтрий жанр |

### Mood
| Method | Endpoint | Auth | Описание |
|--------|----------|------|----------|
| GET | `/api/mood` | Anonymous | Всички настроения |
| GET | `/api/mood/{id}` | Anonymous | Настроение по ID |
| POST | `/api/mood` | Admin | Създай настроение |
| PUT | `/api/mood/{id}` | Admin | Редактирай настроение |
| DELETE | `/api/mood/{id}` | Admin | Изтрий настроение |

### Award
| Method | Endpoint | Auth | Описание |
|--------|----------|------|----------|
| GET | `/api/award` | Anonymous | Всички награди |
| GET | `/api/award/{id}` | Anonymous | Награда по ID |
| POST | `/api/award` | Admin | Създай награда |
| PUT | `/api/award/{id}` | Admin | Редактирай награда |
| DELETE | `/api/award/{id}` | Admin | Изтрий награда |
| PUT | `/api/award/{awardId}/artist/{artistId}` | Admin | Добави победител |
| DELETE | `/api/award/{awardId}/artist/{artistId}` | Admin | Премахни победител |

### User
| Method | Endpoint | Auth | Описание |
|--------|----------|------|----------|
| GET | `/api/user` | Anonymous | Всички потребители |
| GET | `/api/user/{id}` | Anonymous | Потребител по ID |
| GET | `/api/user/username/{username}` | Anonymous | Потребител по username |
| POST | `/api/user` | Admin | Създай потребител |
| DELETE | `/api/user/{id}` | Admin | Изтрий потребител |

---

## Happy Path (Демо сценарий)

1. **Login като Admin** → получаване на JWT токен
2. **Създаване на Country** → `POST /api/country`
3. **Създаване на RecordLabel** → `POST /api/recordlabel`
4. **Създаване на Artist** → `POST /api/artist`
5. **Създаване на Genre** → `POST /api/genre`
6. **Създаване на Album** → `POST /api/album`
7. **Създаване на Song** → `POST /api/song`
8. **Добавяне на Genre към Song** → `PUT /api/song/1/genre/1`
9. **Login като User**
10. **Създаване на Playlist** → `POST /api/playlist`
11. **Добавяне на Song към Playlist** → `PUT /api/playlist/1/song/1`
12. **GET заявки за преглед** (без auth)
