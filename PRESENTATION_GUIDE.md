# MusicCatalog - Ръководство за представяне на изпит

## Времева рамка: 15 минути

---

## 📌 ЧАСТ 1: Вербално обяснение (2 мин)

### 1.1 Какъв проблем решава проектът? (20 сек)

> MusicCatalog решава точно този проблем - това е **система за каталогизиране и управление на музика**."

### 1.2 Функционалности на Admin (40 сек)

Администраторът може да:
- Добавя, редактира и изтрива **артисти** (изпълнители)
- Създава **албуми** и ги свързва с артист
- Добавя **песни** към албуми или като самостоятелни сингли
- Задава **жанрове** на песни (една песен може да е Rock и Pop едновременно)
- Задава **настроения** на албуми (Energetic, Calm, Happy)
- Управлява **звукозаписни компании** и техните държави
- Присвоява **награди** на артисти (Grammy, MTV Awards)

### 1.3 Функционалности на User (30 сек)

Обикновеният потребител може да:
- Разглежда целия каталог (артисти, албуми, песни)
- Създава свои **плейлисти**
- Добавя и премахва песни от плейлистите си
- Всеки потребител управлява само своите плейлисти

### 1.4 Техническа архитектура (20 сек)

> "Проектът е REST API с ASP.NET Core. Използвам JWT автентикация - при логване потребителят получава токен, валиден 30 минути. Entity Framework Core за SQL Server база данни. FluentValidation за валидиране на данните."

### 1.5 Защо е полезен? (10 сек)

> "Това е backend за музикална платформа като Spotify - без streaming частта. Може да се използва от музикални магазини, DJ-и или радио станции."

---

## 📌 ЧАСТ 2: Таблици в базата данни (2 мин)

### Кажете това докато показвате таблиците в SSMS/Azure Data Studio:

| Таблица | Какво казвате |
|---------|---------------|
| **Users** | "Съхранява потребителите - username, хеширана парола и роля (Admin или User)" |
| **Artists** | "Музикалните изпълнители със сценични имена и описание" |
| **Albums** | "Албумите - име, описание, принадлежат на един артист" |
| **Songs** | "Песните - заглавие, продължителност, свързани с артист и опционално с албум" |
| **Genres** | "Музикални жанрове като Rock, Pop, Hip-Hop, Jazz" |
| **Moods** | "Настроения на албумите - Energetic, Calm, Happy, Sad" |
| **Playlists** | "Потребителски плейлисти - всеки потребител има свои" |
| **Awards** | "Музикални награди като Grammy, MTV Awards" |
| **RecordLabels** | "Звукозаписни компании - Sony Music, Universal" |
| **Countries** | "Държави, в които са регистрирани компаниите" |

---

## 📌 ЧАСТ 3: Релации (2 мин)

### 3.1 One-to-Many релации

Кажете: *"Това са връзки където един запис има много свързани записи"*

| Parent | Child | Обяснение |
|--------|-------|-----------|
| Country | RecordLabels | "Една държава има много звукозаписни компании" |
| RecordLabel | Artists | "Една компания има много артисти" |
| Artist | Albums | "Един артист има много албуми" |
| Artist | Songs | "Един артист има много песни" |
| Album | Songs | "Един албум има много песни" |
| User | Playlists | "Един потребител има много плейлисти" |

### 3.2 Many-to-Many релации

Кажете: *"Това са връзки където и двете страни могат да имат много свързани записи. Реализират се чрез junction таблици."*

| Таблица 1 | Таблица 2 | Junction Table | Обяснение |
|-----------|-----------|----------------|-----------|
| Artist | Award | ArtistAward | "Артист може да има много награди, награда може да е на много артисти" |
| Song | Genre | GenreSong | "Песен може да има много жанрове, жанр има много песни" |
| Song | Playlist | PlaylistSong | "Песен може да е в много плейлисти, плейлист има много песни" |
| Album | Mood | AlbumMood | "Албум може да има много настроения" |

---

## 📌 ЧАСТ 4: Postman демонстрация (4 мин)

### Настройки преди демото:
- **Base URL**: `https://localhost:5001` (или вашият порт)
- **Authorization**: Bearer Token → `{{token}}`

### Стъпка по стъпка:

#### 4.1 Логване (30 сек)
```
POST /api/auth
Body: { "username": "Admin", "password": "Admin" }
```
> "Логвам се и получавам JWT токен. Копирам го в Postman variables."

#### 4.2 Създаване на Country (30 сек)
```
POST /api/country
Authorization: Bearer {{token}}
Body: { "name": "USA" }
```
> "Създавам държава. Това е началото на One-to-Many веригата."

#### 4.3 Създаване на RecordLabel (30 сек)
```
POST /api/recordlabel
Body: { "name": "Sony Music", "countryId": 1, "foundedYear": 1929 }
```
> "Създавам компания, свързана с държавата - това е One-to-Many."

#### 4.4 Създаване на Artist (30 сек)
```
POST /api/artist
Body: { "stageName": "Eminem", "description": "American rapper", "recordLabelId": 1 }
```
> "Артистът е към компанията - пак One-to-Many."

#### 4.5 Създаване на Album (30 сек)
```
POST /api/album
Body: { "name": "The Eminem Show", "description": "4th studio album", "artistId": 1 }
```

#### 4.6 Създаване на Song (30 сек)
```
POST /api/song
Body: { "title": "Lose Yourself", "duration": "00:05:26", "artistId": 1, "albumId": 1 }
```

#### 4.7 Създаване на Playlist и добавяне на песен (1 мин)
```
POST /api/playlist
Body: { "name": "My Favorites" }

PUT /api/playlist/1/song/1
```
> "Създавам плейлист и добавям песен - това е **Many-to-Many** релация."

#### 4.8 Демонстрация на BadRequest (опционално)
```
PUT /api/playlist/1/song/1 (пак)
```
> "Ако се опитам да добавя същата песен пак, получавам BadRequest - вече съществува."

---

## 📌 ЧАСТ 5: Въпроси и отговори (5 мин)

### Често задавани въпроси:

#### В1: "Как работи JWT автентикацията?"
> "При логване сървърът създава токен с user ID и роля, подписан с HMAC-SHA256. Токенът е валиден 30 минути. При всяка заявка middleware-ът валидира подписа и проверява срока."

#### В2: "Защо използвате Many-to-Many за Song-Playlist?"
> "Защото една песен може да е в много плейлисти на различни потребители, и един плейлист съдържа много песни."

#### В3: "Какво е Cascade Delete?"
> "Когато изтрия артист, автоматично се изтриват неговите албуми и песни. Това е по подразбиране в EF Core за required foreign keys."

#### В4: "Lazy vs Eager Loading?"
> "Използвам Eager Loading с .Include() - зарежда свързаните данни с една SQL заявка. Lazy Loading би направил отделна заявка за всеки свързан обект, което е по-бавно."

#### В5: "Как валидирате данните?"
> "Използвам FluentValidation за автоматична валидация на request body. Плюс статични методи в Services за проверка на дублиращи се записи - връща BadRequest."

#### В6: "Защо AddScoped за services?"
> "Защото DbContext е Scoped - нова инстанция за всяка HTTP заявка. Services използват DbContext, затова трябва да имат същия lifetime."

#### В7: "Какво е Assembly?"
> "Assembly е компилираната единица - .dll файл. AddValidatorsFromAssemblyContaining сканира цялото assembly и автоматично регистрира всички валидатори."

#### В8: "Защо отделяте DTOs от Entities?"
> "Request/Response separation - не експонирам entity класовете директно. Така мога да контролирам какви данни приемам и връщам."

---

## ⏱️ Времева схема

```
00:00 - 02:00  →  Вербално обяснение (без код)
02:00 - 04:00  →  Показване на таблици в DB клиент
04:00 - 06:00  →  Релации (One-to-Many, Many-to-Many)
06:00 - 10:00  →  Postman демонстрация
10:00 - 15:00  →  Въпроси и отговори
```

---

## ✅ Checklist преди изпита

- [ ] Базата данни е създадена и има данни
- [ ] API-то стартира без грешки
- [ ] Postman колекцията е готова с всички requests
- [ ] Admin потребител съществува (Username: Admin, Password: Admin)
- [ ] Знам да обясня всяка релация
- [ ] Знам да отговоря на техническите въпроси

---

# 📚 ПОДРОБНИ ОБЯСНЕНИЯ НА КЛЮЧОВИ КОНЦЕПЦИИ

---

## 🔐 JWT ТОКЕН - Как работи?

### Какво е JWT?
JWT (JSON Web Token) е начин за сигурно предаване на информация между две страни като JSON обект. Токенът е **подписан**, така че може да се верифицира.

### Структура на токена
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJsb2dnZWRVc2VySWQiOiIxIiwicm9sZSI6IkFkbWluIn0.abc123
```

Токенът има **3 части**, разделени с точка:

| Част | Какво съдържа | Пример (декодиран) |
|------|---------------|-------------------|
| **Header** | Алгоритъм и тип | `{"alg": "HS256", "typ": "JWT"}` |
| **Payload** | Данните (claims) | `{"loggedUserId": "1", "role": "Admin", "exp": 1707561234}` |
| **Signature** | Подпис за верификация | `HMACSHA256(header + "." + payload, secretKey)` |

### Как работи в нашия проект?

**1. Потребителят се логва:**
```
POST /api/auth
Body: { "username": "Admin", "password": "Admin" }
```

**2. Сървърът проверява паролата и създава токен:**
```csharp
// TokenService.cs
public string CreateToken(User user)
{
    // 1. Създаваме claims (данни в токена)
    Claim[] claims = new Claim[]
    {
        new Claim("loggedUserId", user.Id.ToString()),  // ID на потребителя
        new Claim(ClaimTypes.Role, user.Role.ToString()) // Роля: Admin или User
    };

    // 2. Създаваме ключ за подписване
    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("!Password123!Password123!Password123"));
    var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    // 3. Създаваме токена
    JwtSecurityToken token = new JwtSecurityToken(
        issuer: "MusicCatalog",      // Кой издава токена
        audience: "Users",            // За кого е токенът
        claims: claims,               // Данните
        expires: DateTime.Now.AddMinutes(30),  // Валиден 30 минути
        signingCredentials: cred      // Подписът
    );

    // 4. Връщаме токена като string
    return new JwtSecurityTokenHandler().WriteToken(token);
}
```

**3. Сървърът връща токена на клиента:**
```json
"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

**4. Клиентът изпраща токена при всяка заявка:**
```
GET /api/artist
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**5. Сървърът валидира токена:**
- Проверява подписа (дали не е подправен)
- Проверява срока (дали не е изтекъл)
- Проверява issuer и audience
- Извлича claims (userId, role) и ги записва в `User` обекта

### Визуална схема:

```
┌─────────────┐     1. Login        ┌─────────────┐
│   Клиент    │ ─────────────────▶  │   Сървър    │
│  (Postman)  │                     │             │
└─────────────┘                     └─────────────┘
                                           │
                                           │ 2. Създава JWT
                                           ▼
┌─────────────┐     3. Връща JWT    ┌─────────────┐
│   Клиент    │ ◀─────────────────  │   Сървър    │
│             │                     │             │
└─────────────┘                     └─────────────┘
       │
       │ 4. Пази токена
       ▼
┌─────────────┐  5. Request + JWT   ┌─────────────┐
│   Клиент    │ ─────────────────▶  │   Сървър    │
│             │                     │             │
└─────────────┘                     └─────────────┘
                                           │
                                           │ 6. Валидира JWT
                                           │ 7. Извлича userId, role
                                           ▼
┌─────────────┐     8. Response     ┌─────────────┐
│   Клиент    │ ◀─────────────────  │   Сървър    │
│             │                     │             │
└─────────────┘                     └─────────────┘
```

---

## 🗄️ DbContext - Как работи?

### Какво е DbContext?
DbContext е **мостът** между C# кода и базата данни. Той:
- Управлява връзката към базата
- Следи промените в обектите
- Превежда LINQ заявки в SQL
- Записва промените в базата

### ApplicationDbContext в нашия проект:

```csharp
public class ApplicationDbContext : DbContext
{
    // Конструктор - получава настройките (connection string)
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSet = таблица в базата данни
    public DbSet<Album> Albums { get; set; }      // Albums таблица
    public DbSet<Artist> Artists { get; set; }    // Artists таблица
    public DbSet<Song> Songs { get; set; }        // Songs таблица
    public DbSet<User> Users { get; set; }        // Users таблица
    // ... останалите таблици

    // Конфигурация на релациите
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Many-to-Many между Artist и Award
        modelBuilder.Entity<Artist>()
            .HasMany(a => a.Awards)
            .WithMany(a => a.Artists);
    }
}
```

### Как се използва в Service:

```csharp
public class ArtistService
{
    private readonly ApplicationDbContext _context;  // Инжектиран DbContext

    public ArtistService(ApplicationDbContext context)
    {
        _context = context;  // Запазваме референция
    }

    // ЧЕТЕНЕ от базата
    public async Task<List<Artist>> GetAll()
    {
        // _context.Artists = DbSet<Artist> = Artists таблица
        // .Include() = Eager Loading - зарежда свързаните данни
        // .ToListAsync() = изпълнява SQL заявката
        return await _context.Artists
            .Include(a => a.RecordLabel)  // JOIN с RecordLabels
            .ToListAsync();
        
        // Генерира SQL:
        // SELECT * FROM Artists a
        // INNER JOIN RecordLabels r ON a.RecordLabelId = r.Id
    }

    // СЪЗДАВАНЕ в базата
    public async Task<Artist> Create(Artist artist)
    {
        _context.Artists.Add(artist);      // Маркира за добавяне
        await _context.SaveChangesAsync(); // Изпълнява INSERT
        return artist;
        
        // Генерира SQL:
        // INSERT INTO Artists (StageName, Description, RecordLabelId)
        // VALUES (@p0, @p1, @p2)
    }

    // ОБНОВЯВАНЕ в базата
    public async Task<Artist?> Update(int id, Artist newData)
    {
        var existing = await _context.Artists.FindAsync(id);  // SELECT WHERE Id = @id
        if (existing == null) return null;

        existing.StageName = newData.StageName;  // Променяме стойностите
        existing.Description = newData.Description;
        
        await _context.SaveChangesAsync();  // Изпълнява UPDATE
        return existing;
        
        // Генерира SQL:
        // UPDATE Artists SET StageName = @p0, Description = @p1 WHERE Id = @id
    }

    // ИЗТРИВАНЕ от базата
    public async Task<bool> Delete(int id)
    {
        var artist = await _context.Artists.FindAsync(id);
        if (artist == null) return false;

        _context.Artists.Remove(artist);   // Маркира за изтриване
        await _context.SaveChangesAsync(); // Изпълнява DELETE
        return true;
        
        // Генерира SQL:
        // DELETE FROM Artists WHERE Id = @id
    }
}
```

### Change Tracking - Как DbContext следи промените:

```
┌─────────────────────────────────────────────────────┐
│                    DbContext                         │
│                                                      │
│  ┌─────────────────────────────────────────────┐    │
│  │            Change Tracker                    │    │
│  │                                              │    │
│  │   Artist { Id=1, State=Unchanged }          │    │
│  │   Artist { Id=2, State=Modified }  ←── changed│    │
│  │   Artist { Id=0, State=Added }     ←── new    │    │
│  │   Artist { Id=3, State=Deleted }   ←── removed│    │
│  │                                              │    │
│  └─────────────────────────────────────────────┘    │
│                                                      │
│  SaveChangesAsync() →  Генерира SQL за всички       │
│                        Modified, Added, Deleted      │
└─────────────────────────────────────────────────────┘
```

---

## 💉 Dependency Injection (DI) - Как работи?

### Какво е Dependency Injection?
DI е **шаблон за дизайн** където обектите получават своите зависимости отвън, вместо да ги създават сами.

### Без DI (лошо):
```csharp
public class ArtistController
{
    public IActionResult GetAll()
    {
        // Контролерът сам създава всичко - лошо!
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("connection string...")
            .Options;
        var context = new ApplicationDbContext(options);
        var service = new ArtistService(context);
        
        return Ok(service.GetAll());
    }
}
```
**Проблеми:** Трудно за тестване, дублиране на код, невъзможно за подмяна.

### С DI (добре):
```csharp
public class ArtistController
{
    private readonly ArtistService _artistService;

    // Конструкторът ПОЛУЧАВА зависимостта
    public ArtistController(ArtistService artistService)
    {
        _artistService = artistService;
    }

    public IActionResult GetAll()
    {
        return Ok(_artistService.GetAll());
    }
}
```
**DI контейнерът автоматично създава и подава `ArtistService`!**

### Как работи DI контейнерът?

**1. Регистрация (в Program.cs):**
```csharp
builder.Services.AddScoped<ArtistService>();
```
Това казва: "Когато някой поиска `ArtistService`, създай инстанция."

**2. Резолюция (автоматична):**
Когато дойде HTTP заявка към `ArtistController`:
```
1. Framework вижда че ArtistController иска ArtistService
2. Проверява дали ArtistService е регистриран → ДА
3. Проверява конструктора на ArtistService → иска ApplicationDbContext
4. Проверява дали ApplicationDbContext е регистриран → ДА (AddDbContext)
5. Създава ApplicationDbContext
6. Създава ArtistService с този context
7. Създава ArtistController с този service
8. Извиква action метода
```

### Lifetime-и (колко дълго живее инстанцията):

| Lifetime | Описание | Кога се използва |
|----------|----------|------------------|
| **Scoped** | Една инстанция за HTTP заявка | DbContext, Services |
| **Transient** | Нова инстанция всеки път | Леки, stateless обекти |
| **Singleton** | Една инстанция за цялото приложение | Конфигурация, кеш |

### Визуална схема на DI:

```
HTTP Request идва
        │
        ▼
┌───────────────────────────────────────────────────────┐
│                   DI Container                         │
│                                                        │
│  "Трябва ми ArtistController"                         │
│        │                                               │
│        ▼                                               │
│  ArtistController(ArtistService service)              │
│        │                                               │
│        │ "Трябва ми ArtistService"                    │
│        ▼                                               │
│  ArtistService(ApplicationDbContext context)          │
│        │                                               │
│        │ "Трябва ми ApplicationDbContext"             │
│        ▼                                               │
│  ApplicationDbContext(DbContextOptions options)       │
│        │                                               │
│        │ "Трябва ми DbContextOptions"                 │
│        ▼                                               │
│  Взимам connection string от appsettings.json         │
│                                                        │
│  ✓ Създавам DbContextOptions                          │
│  ✓ Създавам ApplicationDbContext                      │
│  ✓ Създавам ArtistService                             │
│  ✓ Създавам ArtistController                          │
│                                                        │
└───────────────────────────────────────────────────────┘
        │
        ▼
  Изпълнява се action метода
```

---

## 📄 PROGRAM.CS - Обяснение ред по ред

```csharp
// ═══════════════════════════════════════════════════════════════
// USING STATEMENTS - Импортиране на библиотеки
// ═══════════════════════════════════════════════════════════════

using System.Text;
// Предоставя Encoding.ASCII.GetBytes() за конвертиране на string към byte[]
// Използва се за JWT ключа

using Common.Enums;
// Съдържа Role enum (Admin, User)
// Използва се при създаване на Admin потребител

using FluentValidation;
// Библиотека за валидация на данни
// Предоставя AbstractValidator<T> базов клас

using FluentValidation.AspNetCore;
// Интеграция на FluentValidation с ASP.NET Core
// Позволява автоматична валидация на request body

using Microsoft.AspNetCore.Authentication.JwtBearer;
// JWT Bearer автентикация middleware
// Съдържа JwtBearerDefaults.AuthenticationScheme

using Microsoft.AspNetCore.Identity;
// ASP.NET Core Identity (не се използва пълноценно)

using Microsoft.EntityFrameworkCore;
// Entity Framework Core ORM
// Предоставя UseSqlServer(), DbContext

using Microsoft.IdentityModel.Tokens;
// Работа с токени
// Съдържа TokenValidationParameters, SymmetricSecurityKey

using MusicCatalog.Api.Services;
// Нашите API services (TokenService)

using MusicCatalog.Api.Validators;
// Нашите FluentValidation валидатори

using MusicCatalog.Common.Entities;
// Entity класове (User, Artist, Album...)

using MusicCatalog.Common.Persistance;
// ApplicationDbContext

using MusicCatalog.Common.Services;
// Business logic services (ArtistService, AlbumService...)


// ═══════════════════════════════════════════════════════════════
// СЪЗДАВАНЕ НА BUILDER
// ═══════════════════════════════════════════════════════════════

var builder = WebApplication.CreateBuilder(args);
// WebApplication.CreateBuilder() - създава builder за конфигуриране
// args - аргументи от командния ред (--urls, --environment)
// builder.Services - DI контейнер за регистриране на services
// builder.Configuration - достъп до appsettings.json


// ═══════════════════════════════════════════════════════════════
// РЕГИСТРАЦИЯ НА DATABASE CONTEXT
// ═══════════════════════════════════════════════════════════════

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("MusicCatalog.Api")));

// AddDbContext<ApplicationDbContext>() 
//   - Регистрира DbContext в DI контейнера
//   - Lifetime: Scoped (нова инстанция за всяка HTTP заявка)
//
// UseSqlServer() 
//   - Конфигурира SQL Server като database provider
//
// GetConnectionString("DefaultConnection")
//   - Взима connection string от appsettings.json:
//     "ConnectionStrings": {
//       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MusicCatalogDb;..."
//     }
//
// MigrationsAssembly("MusicCatalog.Api")
//   - Казва че migration файловете са в API проекта
//   - Необходимо защото DbContext е в Common проекта


// ═══════════════════════════════════════════════════════════════
// РЕГИСТРАЦИЯ НА FLUENT VALIDATION
// ═══════════════════════════════════════════════════════════════

builder.Services.AddFluentValidationAutoValidation();
// Интегрира FluentValidation с ASP.NET Core Model Binding
// При всяка HTTP заявка автоматично валидира request body
// Ако валидацията fail-не → връща 400 Bad Request (не стига до контролера)

builder.Services.AddValidatorsFromAssemblyContaining<CreateGenreRequestValidator>();
// Сканира ЦЯЛОТО assembly (MusicCatalog.Api.dll)
// Намира всички класове наследяващи AbstractValidator<T>
// Регистрира ги автоматично в DI контейнера
//
// Еквивалентно на:
// builder.Services.AddScoped<IValidator<CreateGenreRequest>, CreateGenreRequestValidator>();
// builder.Services.AddScoped<IValidator<CreateAlbumRequest>, CreateAlbumRequestValidator>();
// ... за всеки валидатор


// ═══════════════════════════════════════════════════════════════
// РЕГИСТРАЦИЯ НА SERVICES (Dependency Injection)
// ═══════════════════════════════════════════════════════════════

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AlbumService>();
builder.Services.AddScoped<ArtistService>();
builder.Services.AddScoped<AwardService>();
builder.Services.AddScoped<CountryService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<MoodService>();
builder.Services.AddScoped<PlaylistService>();
builder.Services.AddScoped<RecordLabelService>();
builder.Services.AddScoped<SongService>();
builder.Services.AddScoped<UserService>();

// AddScoped<T>() - регистрира service с Scoped lifetime
//
// Scoped означава:
//   - Една инстанция за всяка HTTP заявка
//   - Всички класове в една заявка получават СЪЩАТА инстанция
//   - След края на заявката инстанцията се унищожава
//
// ЗАЩО Scoped?
//   - DbContext е Scoped
//   - Services използват DbContext
//   - Трябва да имат СЪЩИЯ lifetime


// ═══════════════════════════════════════════════════════════════
// КОНФИГУРАЦИЯ НА JWT АВТЕНТИКАЦИЯ
// ═══════════════════════════════════════════════════════════════

var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

// Взима настройки от appsettings.json:
// {
//   "Jwt": {
//     "Key": "!Password123!Password123!Password123",  // Таен ключ (мин. 32 символа)
//     "Issuer": "MusicCatalog",                       // Кой издава токена
//     "Audience": "Users"                              // За кого е токенът
//   }
// }
//
// ! (null-forgiving operator) - казва на компилатора "знам че не е null"


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

// AddAuthentication() - регистрира authentication services
//
// DefaultAuthenticateScheme = "Bearer"
//   - Коя схема да се използва за автентикация по подразбиране
//
// DefaultChallengeScheme = "Bearer"  
//   - Какво да върне когато потребителят не е автентикиран (401)


.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        // Проверява "iss" claim в токена
        // Ако не съвпада с ValidIssuer → токенът е невалиден
        
        ValidateAudience = true,
        // Проверява "aud" claim в токена
        // Предотвратява използване на токен за друго приложение
        
        ValidateLifetime = true,
        // Проверява "exp" claim (expiration)
        // Ако токенът е изтекъл → невалиден
        
        ValidateIssuerSigningKey = true,
        // НАЙ-ВАЖНО: Верифицира подписа на токена
        // Гарантира че токенът не е подправен
        
        ValidIssuer = jwtIssuer,
        // Очакваната стойност за issuer: "MusicCatalog"
        
        ValidAudience = jwtAudience,
        // Очакваната стойност за audience: "Users"
        
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey))
        // Ключът за верификация на подписа
        // Encoding.ASCII.GetBytes() конвертира string → byte[]
        // SymmetricSecurityKey създава криптографски ключ
        // СИМЕТРИЧЕН = същият ключ за подписване и верификация
    };
});


// ═══════════════════════════════════════════════════════════════
// РЕГИСТРАЦИЯ НА AUTHORIZATION И CONTROLLERS
// ═══════════════════════════════════════════════════════════════

builder.Services.AddAuthorization();
// Регистрира authorization services
// Позволява използването на [Authorize], [Authorize(Roles = "Admin")]

builder.Services.AddControllers();
// Регистрира MVC Controller services:
//   - Routing (маршрутизиране на URL към контролери)
//   - Model Binding (JSON → C# обекти)
//   - Action Filters
//   - Response Formatting (C# обекти → JSON)


// ═══════════════════════════════════════════════════════════════
// BUILD - СЪЗДАВАНЕ НА ПРИЛОЖЕНИЕТО
// ═══════════════════════════════════════════════════════════════

var app = builder.Build();
// Компилира service collection
// Създава DI container
// Създава WebApplication instance
//
// ВАЖНО: След този ред НЕ МОЖЕ да се добавят нови services!


// ═══════════════════════════════════════════════════════════════
// DEVELOPMENT MODE
// ═══════════════════════════════════════════════════════════════

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
// Проверява ASPNETCORE_ENVIRONMENT environment variable
// Ако е "Development" → включва OpenAPI/Swagger документация
// В Production не искаме да експонираме API документацията


// ═══════════════════════════════════════════════════════════════
// MIDDLEWARE PIPELINE - РЕДЪТ Е ВАЖЕН!
// ═══════════════════════════════════════════════════════════════

app.UseHttpsRedirection();
// Пренасочва HTTP заявки към HTTPS
// http://localhost:5000/api/artist → https://localhost:5001/api/artist

app.UseAuthentication();
// JWT Authentication Middleware
// 1. Чете "Authorization: Bearer <token>" header
// 2. Валидира токена според TokenValidationParameters
// 3. Ако е валиден → създава ClaimsPrincipal с claims от токена
// 4. Записва го в HttpContext.User
//
// ТРЯБВА да е ПРЕДИ UseAuthorization!

app.UseAuthorization();
// Authorization Middleware
// 1. Проверява [Authorize] атрибути на controller/action
// 2. Проверява [Authorize(Roles = "Admin")]
// 3. Ако не е оторизиран → 403 Forbidden или 401 Unauthorized
//
// ТРЯБВА да е СЛЕД UseAuthentication!

app.MapControllers();
// Регистрира controller endpoints в routing системата
// Сканира всички класове с [ApiController]
// Маршрутизира по [Route("api/[controller]")]
// Маршрутизира по [HttpGet], [HttpPost], [HttpPut], [HttpDelete]


// ═══════════════════════════════════════════════════════════════
// MIDDLEWARE PIPELINE - ВИЗУАЛНА СХЕМА
// ═══════════════════════════════════════════════════════════════
//
//   HTTP Request
//        │
//        ▼
//   ┌─────────────────┐
//   │ HttpsRedirection │  →  Пренасочва HTTP към HTTPS
//   └─────────────────┘
//        │
//        ▼
//   ┌─────────────────┐
//   │ Authentication   │  →  Валидира JWT, създава User
//   └─────────────────┘
//        │
//        ▼
//   ┌─────────────────┐
//   │ Authorization    │  →  Проверява [Authorize], роли
//   └─────────────────┘
//        │
//        ▼
//   ┌─────────────────┐
//   │ MapControllers   │  →  Рутира към контролер/action
//   └─────────────────┘
//        │
//        ▼
//   HTTP Response


// ═══════════════════════════════════════════════════════════════
// SEED ADMIN USER - Създаване на начални данни
// ═══════════════════════════════════════════════════════════════

using (var scope = app.Services.CreateScope())
{
    // CreateScope() създава нов DI scope
    // Необходимо защото Scoped services не могат да се вземат от root provider
    // using гарантира че scope-ът ще бъде disposed
    
    var services = scope.ServiceProvider;
    // IServiceProvider за този scope
    
    var db = services.GetRequiredService<ApplicationDbContext>();
    // Взима DbContext от DI
    // GetRequiredService хвърля exception ако не е регистриран

    if (!db.Users.Any(u => u.Username == "Admin"))
    {
        // LINQ заявка: проверява дали съществува Admin потребител
        // .Any() генерира SQL: SELECT CASE WHEN EXISTS(...) THEN 1 ELSE 0 END
        
        var admin = new User
        {
            Username = "Admin",
            Role = Role.Admin
        };

        var hash = UserService.HashPassword("Admin");
        // Хешира паролата - НИКОГА не пазим пароли в чист текст!
        
        admin.PasswordHash = hash;

        db.Users.Add(admin);
        // Маркира User за добавяне (Added state)
        
        db.SaveChanges();
        // Изпълнява SQL: INSERT INTO Users (Username, PasswordHash, Role) VALUES (...)
    }
}
// Scope се dispose-ва → DbContext се dispose-ва → Connection се затваря


// ═══════════════════════════════════════════════════════════════
// START APPLICATION
// ═══════════════════════════════════════════════════════════════

app.Run();
// 1. Стартира Kestrel уеб сървър
// 2. Слуша на конфигурираните портове (https://localhost:5001)
// 3. БЛОКИРА текущия thread докато приложението не бъде спряно
// 4. При Ctrl+C → graceful shutdown
```

---

## 🔄 ПЪЛЕН FLOW НА HTTP ЗАЯВКА

```
1. Клиентът изпраща: POST /api/artist
   Headers: Authorization: Bearer eyJ...
   Body: { "stageName": "Eminem", "recordLabelId": 1 }
        │
        ▼
2. UseHttpsRedirection
   - Проверява дали е HTTPS (да)
   - Продължава
        │
        ▼
3. UseAuthentication
   - Чете Authorization header
   - Извлича JWT токена
   - Валидира подписа ✓
   - Проверява срока ✓
   - Създава ClaimsPrincipal с:
     - loggedUserId = "1"
     - role = "Admin"
   - Записва в HttpContext.User
        │
        ▼
4. UseAuthorization
   - Вижда [Authorize(Roles = "Admin")] на action-а
   - Проверява User.IsInRole("Admin") → TRUE ✓
   - Продължава
        │
        ▼
5. MapControllers (Routing)
   - POST /api/artist → ArtistController.Create()
        │
        ▼
6. Model Binding
   - JSON body → CreateArtistRequest обект
        │
        ▼
7. FluentValidation
   - Валидира CreateArtistRequest
   - StageName не е празен ✓
   - Продължава
        │
        ▼
8. DI Resolution
   - Създава ArtistController
   - Инжектира ArtistService
   - ArtistService получава ApplicationDbContext
        │
        ▼
9. Action Execution
   - ArtistController.Create() се изпълнява
   - Извиква _artistService.Create(artist)
   - DbContext.Artists.Add(artist)
   - DbContext.SaveChangesAsync() → INSERT SQL
        │
        ▼
10. Response
    - return Ok(ArtistResponse.FromEntity(artist))
    - 200 OK + JSON body
```
