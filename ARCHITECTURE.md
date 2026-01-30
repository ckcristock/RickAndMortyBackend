# Arquitectura del Backend - Rick and Morty

## Diagrama de Capas

```
┌─────────────────────────────────────────────────────────────┐
│                         FRONTEND                             │
│                    (Aplicación React/Angular/etc)            │
└────────────────────────┬────────────────────────────────────┘
                         │ HTTP/HTTPS
                         │ JSON
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                    BACKEND (ASP.NET Core)                    │
├─────────────────────────────────────────────────────────────┤
│  ┌───────────────────────────────────────────────────────┐  │
│  │           Controllers (API Endpoints)                  │  │
│  │  - CharactersController                                │  │
│  │    GET /api/characters                                 │  │
│  │    GET /api/characters/{id}                            │  │
│  └──────────────────┬────────────────────────────────────┘  │
│                     │                                        │
│  ┌──────────────────▼─────────────────────────────────────┐ │
│  │              Services (Lógica de Negocio)              │ │
│  │  - CharacterService                                    │ │
│  │    * Coordina API externa y BD                         │ │
│  │    * Mapea modelos API a DTOs                          │ │
│  │    * Gestiona caché en BD                              │ │
│  │                                                         │ │
│  │  - RickAndMortyApiService                              │ │
│  │    * Consume API externa                               │ │
│  │    * Maneja HttpClient                                 │ │
│  │    * Parsea JSON responses                             │ │
│  └──────────────┬──────────────────────┬─────────────────┘ │
│                 │                      │                    │
│  ┌──────────────▼─────────────┐   ┌───▼────────────────┐  │
│  │ Repositories (Datos)       │   │  HttpClient        │  │
│  │ - CharacterRepository      │   │  (API Externa)     │  │
│  │   * CRUD operations         │   └───┬────────────────┘  │
│  │   * Upsert logic           │       │                    │
│  └──────────────┬─────────────┘       │                    │
│                 │                      │                    │
│  ┌──────────────▼─────────────┐       │                    │
│  │   DbContext (EF Core)      │       │                    │
│  │   - ApplicationDbContext   │       │                    │
│  └──────────────┬─────────────┘       │                    │
│                 │                      │                    │
│  ┌──────────────▼─────────────┐       │                    │
│  │      Middleware            │       │                    │
│  │  - GlobalExceptionHandler  │       │                    │
│  │  - CORS                    │       │                    │
│  └────────────────────────────┘       │                    │
└─────────────────┬───────────────────────┬──────────────────┘
                  │                       │
                  ▼                       ▼
         ┌────────────────┐    ┌──────────────────────┐
         │  MySQL DB      │    │  Rick & Morty API    │
         │  rickandmorty  │    │  rickandmortyapi.com │
         └────────────────┘    └──────────────────────┘
```

## Flujo de Datos

### Caso 1: GET /api/characters?page=1&name=rick

```
1. Frontend → Backend Controller
   GET /api/characters?page=1&name=rick

2. Controller → CharacterService
   GetCharactersAsync(page: 1, name: "rick")

3. CharacterService → RickAndMortyApiService
   GetCharactersAsync(page: 1, name: "rick")

4. RickAndMortyApiService → API Externa
   GET https://rickandmortyapi.com/api/character?page=1&name=rick

5. API Externa → RickAndMortyApiService
   JSON Response (CharacterApi[])

6. RickAndMortyApiService → CharacterService
   RickAndMortyApiResponse<CharacterApi>

7. CharacterService → CharacterRepository
   UpsertCharactersAsync(characters) - Guarda en BD

8. CharacterService → CharacterService
   Mapeo de CharacterApi a CharacterDto

9. CharacterService → Controller
   PaginatedResponse<CharacterDto>

10. Controller → Frontend
    JSON Response 200 OK
```

### Caso 2: GET /api/characters/1

```
1. Frontend → Backend Controller
   GET /api/characters/1

2. Controller → CharacterService
   GetCharacterByIdAsync(1)

3. CharacterService → RickAndMortyApiService
   GetCharacterByIdAsync(1)

4. RickAndMortyApiService → API Externa
   GET https://rickandmortyapi.com/api/character/1

5. API Externa → RickAndMortyApiService
   JSON Response (CharacterApi)

6. CharacterService → CharacterRepository
   UpsertCharacterAsync(character) - Guarda en BD

7. CharacterService → RickAndMortyApiService
   GetEpisodesByIdsAsync([1, 2, 3...])

8. RickAndMortyApiService → API Externa
   GET https://rickandmortyapi.com/api/episode/1,2,3

9. API Externa → RickAndMortyApiService
   JSON Response (EpisodeApi[])

10. CharacterService → CharacterService
    Mapeo a CharacterDetailDto (incluye episodios)

11. CharacterService → Controller
    CharacterDetailDto

12. Controller → Frontend
    JSON Response 200 OK
```

## Modelos y DTOs

### Transformación de datos:

```
API Externa (CharacterApi)
         ↓
    [Mapping]
         ↓
Entidad BD (Character)
         ↓
    [Mapping]
         ↓
DTO Respuesta (CharacterDto)
         ↓
      Frontend
```

## Responsabilidades por Capa

### 1. Controllers
- Validar parámetros de entrada
- Manejar códigos de respuesta HTTP
- Documentación OpenAPI/Swagger
- **NO** contiene lógica de negocio

### 2. Services
- Lógica de negocio
- Coordinación entre API externa y BD
- Transformación de datos (Mapping)
- Decisiones de cuándo usar caché o API

### 3. Repositories
- Acceso a base de datos
- Operaciones CRUD
- Queries optimizadas
- **NO** contiene lógica de negocio

### 4. Models
- Entidades de base de datos (EF Core)
- Modelos de API externa
- Sin lógica, solo datos

### 5. DTOs
- Contratos de API
- Respuestas al frontend
- Separación de concerns

## Inyección de Dependencias

```csharp
// Program.cs
builder.Services.AddDbContext<ApplicationDbContext>(...);
builder.Services.AddHttpClient<IRickAndMortyApiService, RickAndMortyApiService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
```

**Beneficios:**
- Testabilidad (mocks/stubs)
- Bajo acoplamiento
- Mantenibilidad
- Facilita cambios futuros

## Manejo de Errores

```
Exception en cualquier capa
         ↓
GlobalExceptionHandlerMiddleware
         ↓
Response 500 JSON consistente
         ↓
      Frontend
```

## Persistencia de Datos

**Estrategia: Write-through Cache**

- Cada consulta a la API externa guarda datos en MySQL
- Operación Upsert (Insert o Update)
- Los datos locales pueden usarse para analytics o backup
- No se consulta BD antes de la API (siempre datos frescos)

## Escalabilidad Futura

### Posibles mejoras:
1. **Redis Cache**: Caché intermedia para reducir llamadas a API
2. **Background Jobs**: Sincronización periódica de datos
3. **Circuit Breaker**: Protección contra fallos de API externa
4. **Rate Limiting**: Proteger el backend de sobrecarga
5. **Health Checks**: Monitoreo de servicios
6. **GraphQL**: Alternative a REST endpoints
7. **Autenticación**: JWT tokens para seguridad

## Testing

```
Unit Tests → Services y Repositories (mocking dependencies)
Integration Tests → Controllers + Services (TestServer)
E2E Tests → Full stack con BD de prueba
```

## Configuración por Ambiente

```
appsettings.json               → Base
appsettings.Development.json   → Local development
appsettings.Production.json    → Production
```

## Seguridad

- HTTPS enforced
- Input validation
- SQL Injection prevention (EF Core parametrized queries)
- Exception details hidden in production
- CORS configurado
- Secrets en configuration, no hardcoded
