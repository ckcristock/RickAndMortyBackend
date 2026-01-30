# Rick and Morty Backend API

Backend intermedio desarrollado en ASP.NET Core 8.0 que consume la API oficial de Rick and Morty y la expone a través de endpoints propios, con persistencia en MySQL.

## 🎯 Objetivo

Verificar la capacidad de consumir una API externa implementando:

- Listado de personajes con filtros
- Paginación
- Navegación a detalle de personaje
- Manejo de estados de interfaz (cargando, error, sin resultados)
- Persistencia de información en MySQL

## 🏗️ Arquitectura

El proyecto está organizado en capas siguiendo principios SOLID y Clean Architecture:

```
RickAndMortyBackend/
├── Controllers/          # Capa de presentación - Endpoints API
├── Services/            # Lógica de negocio
│   ├── Interfaces/
│   ├── CharacterService.cs
│   └── RickAndMortyApiService.cs
├── Repositories/        # Capa de acceso a datos
│   ├── Interfaces/
│   └── CharacterRepository.cs
├── Models/              # Entidades de base de datos
│   ├── ApiModels/      # Modelos de la API externa
│   └── Character.cs
├── DTOs/               # Data Transfer Objects
├── Data/               # DbContext y configuración EF Core
├── Middleware/         # Middleware personalizado
└── Database/           # Scripts SQL
```

### Patrones implementados:

- **Repository Pattern**: Abstracción del acceso a datos
- **Service Layer Pattern**: Lógica de negocio separada
- **Dependency Injection**: Inversión de control
- **DTO Pattern**: Separación entre modelos de API y dominio

## 🚀 Tecnologías

- **ASP.NET Core 8.0**: Framework web
- **Entity Framework Core**: ORM para persistencia
- **MySQL**: Base de datos relacional
- **Pomelo.EntityFrameworkCore.MySql**: Provider de MySQL para EF Core
- **HttpClient**: Consumo de API externa
- **Swagger/OpenAPI**: Documentación de API

## 📋 Requisitos previos

- .NET 8.0 SDK
- MySQL Server 8.0 o superior
- Visual Studio 2022 / VS Code / Rider

## ⚙️ Configuración

### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd RickAndMortyBackend
```

### 2. Configurar la base de datos

#### Crear la base de datos en MySQL:

```bash
mysql -u root -p < Database/schema.sql
```

O ejecutar manualmente el script `Database/schema.sql` en MySQL Workbench.

### 3. Configurar la cadena de conexión

Editar `appsettings.json` con tus credenciales de MySQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=rickandmorty_db;User=root;Password=TU_PASSWORD;"
  }
}
```

### 4. Restaurar paquetes NuGet

```bash
dotnet restore
```

### 5. Ejecutar el proyecto

```bash
dotnet run
```

La API estará disponible en:

- HTTPS: `https://localhost:7XXX`
- HTTP: `http://localhost:5XXX`
- Swagger UI: `https://localhost:7XXX/swagger`

## 📡 Endpoints disponibles

### GET /api/characters

Obtener lista paginada de personajes con filtros opcionales.

**Query Parameters:**

- `page` (int): Número de página (default: 1)
- `name` (string): Filtrar por nombre
- `status` (string): Filtrar por estado (alive, dead, unknown)
- `species` (string): Filtrar por especie

**Ejemplo:**

```
GET /api/characters?page=1&name=rick&status=alive
```

**Respuesta:**

```json
{
  "info": {
    "count": 826,
    "pages": 42,
    "next": 2,
    "prev": null
  },
  "results": [
    {
      "id": 1,
      "name": "Rick Sanchez",
      "status": "Alive",
      "species": "Human",
      "type": "",
      "gender": "Male",
      "image": "https://rickandmortyapi.com/api/character/avatar/1.jpeg",
      "origin": {
        "name": "Earth (C-137)",
        "url": "https://rickandmortyapi.com/api/location/1"
      },
      "location": {
        "name": "Citadel of Ricks",
        "url": "https://rickandmortyapi.com/api/location/3"
      }
    }
  ]
}
```

### GET /api/characters/{id}

Obtener detalles completos de un personaje incluyendo episodios.

**Ejemplo:**

```
GET /api/characters/1
```

**Respuesta:**

```json
{
  "id": 1,
  "name": "Rick Sanchez",
  "status": "Alive",
  "species": "Human",
  "type": "",
  "gender": "Male",
  "image": "https://rickandmortyapi.com/api/character/avatar/1.jpeg",
  "origin": {
    "name": "Earth (C-137)",
    "url": "https://rickandmortyapi.com/api/location/1"
  },
  "location": {
    "name": "Citadel of Ricks",
    "url": "https://rickandmortyapi.com/api/location/3"
  },
  "created": "2017-11-04T18:48:46.250Z",
  "episodes": [
    {
      "id": 1,
      "name": "Pilot",
      "airDate": "December 2, 2013",
      "episode": "S01E01"
    }
  ]
}
```

## 🔧 Características técnicas

### Manejo centralizado de errores

Implementado mediante middleware `GlobalExceptionHandlerMiddleware` que captura todas las excepciones no manejadas y devuelve respuestas JSON consistentes.

### Persistencia inteligente

- Los personajes consultados se almacenan automáticamente en la base de datos MySQL
- Uso de patrón Upsert (Insert o Update) para mantener datos actualizados
- Índices en campos comunes de búsqueda (Name, Status, Species)

### Configuración CORS

CORS habilitado para permitir el consumo desde el frontend.

### Modelos tipados

Todos los modelos están fuertemente tipados con propiedades no nulables donde corresponda.

### HttpClient configurado

Inyección de HttpClient con configuración específica para la API de Rick and Morty.

## 📊 Estructura de la base de datos

### Tabla: Characters

```sql
CREATE TABLE Characters (
    Id INT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Status VARCHAR(50),
    Species VARCHAR(100),
    Type VARCHAR(100),
    Gender VARCHAR(50),
    Image VARCHAR(500),
    Url VARCHAR(500),
    Created DATETIME NOT NULL,
    OriginName VARCHAR(255),
    OriginUrl VARCHAR(500),
    LocationName VARCHAR(255),
    LocationUrl VARCHAR(500),
    EpisodesJson TEXT,
    INDEX idx_name (Name),
    INDEX idx_status (Status),
    INDEX idx_species (Species)
);
```

## 🧪 Pruebas

### Probar con Swagger

1. Ejecutar el proyecto
2. Navegar a `https://localhost:XXXX/swagger`
3. Probar los endpoints directamente desde la interfaz

### Probar con archivo .http

El proyecto incluye `RickAndMortyBackend.http` con ejemplos de requests.

### Casos de prueba sugeridos:

1. **Listado básico**: GET `/api/characters?page=1`
2. **Filtro por nombre**: GET `/api/characters?name=rick`
3. **Filtro por estado**: GET `/api/characters?status=alive`
4. **Filtro combinado**: GET `/api/characters?name=morty&status=alive`
5. **Detalle de personaje**: GET `/api/characters/1`
6. **Paginación**: GET `/api/characters?page=2`
7. **Sin resultados**: GET `/api/characters?name=personaje_inexistente`
8. **ID inválido**: GET `/api/characters/99999`

## 🛠️ Manejo de estados

La API devuelve diferentes códigos HTTP según el resultado:

- **200 OK**: Solicitud exitosa con datos
- **400 Bad Request**: Parámetros inválidos
- **404 Not Found**: Recurso no encontrado
- **500 Internal Server Error**: Error del servidor

## 📝 Logs

El proyecto utiliza el sistema de logging integrado de ASP.NET Core:

- `Information`: Operaciones normales
- `Warning`: Situaciones que requieren atención (recursos no encontrados)
- `Error`: Errores y excepciones

Los logs se pueden ver en:

- Console durante desarrollo
- Archivos configurables para producción

## 🔐 Seguridad

- Validación de parámetros de entrada
- Manejo seguro de excepciones sin exponer detalles internos
- Conexión a base de datos mediante variables de configuración

## 🚀 Despliegue

Para desplegar en producción:

1. Publicar el proyecto:

```bash
dotnet publish -c Release -o ./publish
```

2. Configurar `appsettings.Production.json` con cadena de conexión de producción

3. Configurar el servidor web (IIS, Nginx, etc.)

## 🤝 Contribuciones

Este proyecto es una prueba técnica. Para contribuir:

1. Fork el proyecto
2. Crear una rama feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit los cambios (`git commit -m 'Añadir nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Crear un Pull Request

## 📄 Licencia

Este proyecto es de código abierto para fines educativos.

## 📧 Contacto

Para dudas o sugerencias, contactar al desarrollador.

---

**Nota**: Este backend está diseñado para consumir exclusivamente la API oficial de Rick and Morty disponible en https://rickandmortyapi.com/
