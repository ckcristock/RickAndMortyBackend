# 🚀 Rick and Morty Backend API

Backend intermedio desarrollado en **ASP.NET Core 8.0** que consume la API oficial de Rick and Morty y la expone a través de endpoints propios, con persistencia en **MySQL**.

> **Para evaluadores**: Este README contiene todas las instrucciones necesarias para ejecutar el proyecto. Siga los pasos en orden para una configuración exitosa.

---

## 📋 Requisitos Previos

Antes de comenzar, asegúrese de tener instalado:

| Requisito        | Versión Mínima | Verificar Instalación |
| ---------------- | -------------- | --------------------- |
| **.NET SDK**     | 8.0            | `dotnet --version`    |
| **MySQL Server** | 8.0+           | `mysql --version`     |
| **Git**          | Cualquiera     | `git --version`       |

### Descargar herramientas necesarias:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)

---

## ⚡ Inicio Rápido (Quick Start)

### **Paso 1: Clonar el repositorio**

```bash
git clone <url-del-repositorio>
cd RickAndMortyBackend
```

### **Paso 2: Configurar MySQL**

**Opción A: Usando línea de comandos**

```bash
# Iniciar sesión en MySQL
mysql -u root -p

# Ejecutar el script de base de datos
source Database/schema.sql

# Verificar que se creó correctamente
USE rickandmorty_db;
SHOW TABLES;
```

**Opción B: Usando MySQL Workbench**

1. Abrir MySQL Workbench
2. Conectarse al servidor local
3. Abrir el archivo `Database/schema.sql`
4. Ejecutar el script (⚡ botón de rayo o Ctrl+Shift+Enter)

### **Paso 3: Configurar la cadena de conexión**

Editar el archivo `appsettings.json` y actualizar la contraseña de MySQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=rickandmorty_db;User=root;Password=TU_PASSWORD_AQUI;"
  }
}
```

> ⚠️ **Importante**: Reemplace `TU_PASSWORD_AQUI` con su contraseña real de MySQL.

### **Paso 4: Restaurar dependencias y ejecutar**

```bash
# Restaurar paquetes NuGet
dotnet restore

# Compilar el proyecto
dotnet build

# Ejecutar la aplicación
dotnet run
```

### **Paso 5: Verificar que funciona**

Una vez que el proyecto esté ejecutándose, verá algo como:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7127
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5127
```

**Acceder a Swagger UI**: Abra su navegador en `https://localhost:7127/swagger`

**Probar el primer endpoint**:

```bash
# Windows (PowerShell)
curl https://localhost:7127/api/characters

# O abrir en el navegador:
https://localhost:7127/api/characters
```

---

## 🎯 Funcionalidades Implementadas

✅ Consumo de API externa (Rick and Morty API)  
✅ Listado de personajes con paginación  
✅ Filtros por nombre, estado y especie  
✅ Detalle completo de personajes con episodios  
✅ Persistencia en base de datos MySQL  
✅ Manejo centralizado de errores  
✅ Documentación con Swagger/OpenAPI  
✅ CORS configurado para frontend  
✅ Arquitectura en capas (Clean Architecture)

---

## 🏗️ Arquitectura del Proyecto

```
RickAndMortyBackend/
├── Controllers/          # 🎮 API Endpoints (CharactersController)
├── Services/            # 💼 Lógica de negocio
│   ├── CharacterService.cs          # Orquestación de operaciones
│   └── RickAndMortyApiService.cs    # Consumo de API externa
├── Repositories/        # 💾 Acceso a datos
│   └── CharacterRepository.cs       # Operaciones con BD
├── Models/              # 📦 Entidades
│   └── Character.cs                 # Modelo de dominio
├── DTOs/               # 📋 Data Transfer Objects
│   └── CharacterDto.cs              # Objetos de transferencia
├── Data/               # 🗄️ DbContext
│   └── ApplicationDbContext.cs      # Contexto EF Core
├── Middleware/         # 🛡️ Middleware personalizado
│   └── GlobalExceptionHandlerMiddleware.cs
└── Database/           # 📊 Scripts SQL
    └── schema.sql                   # Estructura de BD
```

### Patrones de diseño utilizados:

- **Repository Pattern**: Abstracción de acceso a datos
- **Service Layer Pattern**: Separación de lógica de negocio
- **Dependency Injection**: Inversión de control (IoC)
- **DTO Pattern**: Transferencia de datos entre capas

---

## 🚀 Tecnologías Utilizadas

| Tecnología                               | Propósito                   |
| ---------------------------------------- | --------------------------- |
| ASP.NET Core 8.0                         | Framework web principal     |
| Entity Framework Core                    | ORM para acceso a datos     |
| MySQL + Pomelo.EntityFrameworkCore.MySql | Base de datos y provider    |
| HttpClient                               | Consumo de API REST externa |
| Swagger/OpenAPI                          | Documentación interactiva   |
| Newtonsoft.Json                          | Serialización JSON          |

---

## 📡 API Endpoints

El backend expone dos endpoints principales:

### **1. GET /api/characters** - Listar personajes

Obtiene una lista paginada de personajes con filtros opcionales.

**Parámetros de consulta (Query Parameters):**

| Parámetro | Tipo   | Descripción                              | Ejemplo          |
| --------- | ------ | ---------------------------------------- | ---------------- |
| `page`    | int    | Número de página (default: 1)            | `?page=2`        |
| `name`    | string | Filtrar por nombre                       | `?name=rick`     |
| `status`  | string | Filtrar por estado: alive, dead, unknown | `?status=alive`  |
| `species` | string | Filtrar por especie                      | `?species=human` |

**Ejemplos de uso:**

```bash
# Página 1 (todos los personajes)
GET /api/characters

# Página 2
GET /api/characters?page=2

# Buscar por nombre
GET /api/characters?name=morty

# Filtrar por estado
GET /api/characters?status=alive

# Filtros combinados
GET /api/characters?name=rick&status=alive&species=human
```

**Respuesta exitosa (200 OK):**

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

---

### **2. GET /api/characters/{id}** - Detalle de personaje

Obtiene información completa de un personaje específico, incluyendo episodios.

**Parámetros de ruta:**

| Parámetro | Tipo | Descripción      |
| --------- | ---- | ---------------- |
| `id`      | int  | ID del personaje |

**Ejemplos de uso:**

```bash
# Obtener Rick Sanchez (ID: 1)
GET /api/characters/1

# Obtener Morty Smith (ID: 2)
GET /api/characters/2
```

**Respuesta exitosa (200 OK):**

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
    },
    {
      "id": 2,
      "name": "Lawnmower Dog",
      "airDate": "December 9, 2013",
      "episode": "S01E02"
    }
  ]
}
```

**Respuesta error (404 Not Found):**

```json
{
  "error": "Character with ID 99999 not found"
}
```

---

## 🧪 Cómo Probar la API

### **Opción 1: Usando Swagger UI (Recomendado)**

1. Ejecute el proyecto con `dotnet run`
2. Abra su navegador en `https://localhost:7127/swagger`
3. Verá la documentación interactiva con todos los endpoints
4. Click en cualquier endpoint → "Try it out" → Ingresar parámetros → "Execute"

### **Opción 2: Usando el archivo .http**

El proyecto incluye `RickAndMortyBackend.http` con ejemplos listos para usar:

1. Abra el archivo en VS Code
2. Instale la extensión "REST Client" si no la tiene
3. Click en "Send Request" sobre cualquier petición

### **Opción 3: Usando curl**

```bash
# Windows PowerShell
curl https://localhost:7127/api/characters
curl https://localhost:7127/api/characters/1
curl "https://localhost:7127/api/characters?name=rick"

# Con filtros múltiples
curl "https://localhost:7127/api/characters?name=morty&status=alive"
```

### **Casos de prueba sugeridos para el evaluador:**

| #   | Prueba             | Endpoint                                      | Resultado Esperado             |
| --- | ------------------ | --------------------------------------------- | ------------------------------ |
| 1   | Listado básico     | `GET /api/characters`                         | 200 OK - Lista de personajes   |
| 2   | Paginación         | `GET /api/characters?page=2`                  | 200 OK - Página 2              |
| 3   | Buscar "Rick"      | `GET /api/characters?name=rick`               | 200 OK - Personajes con "Rick" |
| 4   | Filtro por estado  | `GET /api/characters?status=alive`            | 200 OK - Solo vivos            |
| 5   | Detalle ID válido  | `GET /api/characters/1`                       | 200 OK - Rick Sanchez completo |
| 6   | ID inválido        | `GET /api/characters/99999`                   | 404 Not Found                  |
| 7   | Sin resultados     | `GET /api/characters?name=xyz123`             | 200 OK - Lista vacía           |
| 8   | Filtros combinados | `GET /api/characters?name=morty&status=alive` | 200 OK - Filtrado              |

---

## 💾 Estrategia de Persistencia

El backend implementa un sistema inteligente de caché con MySQL:

### **Para GET /api/characters/{id} (Detalle):**

```
1. Buscar en BD local ✓
   └─ Si existe → Devolver inmediatamente (rápido)
   └─ Si no existe → Consultar API externa → Guardar en BD → Devolver
```

**Ventajas:**

- ⚡ Respuestas instantáneas para personajes ya consultados
- 📉 Reduce carga a la API externa de Rick and Morty
- 💾 Mantiene historial de personajes consultados

### **Para GET /api/characters (Listado):**

```
1. Consultar API externa (siempre) ✓
2. Guardar/actualizar personajes en BD automáticamente
```

**Ventajas:**

- 🔄 Datos siempre actualizados
- 📊 Respeta paginación y filtros de la API original
- 💾 Backup histórico de datos

### **Modelo de Base de Datos:**

```sql
Characters
├─ Id (PK)                  # ID del personaje
├─ Name (INDEX)             # Nombre del personaje
├─ Status (INDEX)           # alive, dead, unknown
├─ Species (INDEX)          # Human, Alien, etc.
├─ Type                     # Tipo específico
├─ Gender                   # Male, Female, etc.
├─ Image                    # URL de avatar
├─ OriginName               # Planeta de origen
├─ OriginUrl                # URL de origen
├─ LocationName             # Ubicación actual
├─ LocationUrl              # URL de ubicación
├─ EpisodesJson (TEXT)      # Episodios en formato JSON
└─ Created                  # Fecha de creación
```

**Índices optimizados para búsquedas en:**

- `Name` - Para filtros por nombre
- `Status` - Para filtros por estado
- `Species` - Para filtros por especie

---

## 🔧 Características Técnicas

### **Manejo Centralizado de Errores**

Middleware `GlobalExceptionHandlerMiddleware` captura todas las excepciones:

```csharp
try {
    // Código de la aplicación
} catch (Exception ex) {
    return new {
        error = ex.Message,
        statusCode = 500
    };
}
```

**Códigos HTTP retornados:**

- `200 OK` - Solicitud exitosa
- `404 Not Found` - Recurso no encontrado
- `500 Internal Server Error` - Error del servidor

### **Configuración CORS**

```csharp
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

Permite que cualquier frontend consuma la API.

### **HttpClient Configurado**

```csharp
builder.Services.AddHttpClient<IRickAndMortyApiService, RickAndMortyApiService>();
```

Cliente HTTP inyectado y configurado para la API de Rick and Morty.

### **Inyección de Dependencias**

```csharp
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
```

Inversión de control para desacoplamiento y testabilidad.

---

## 🐛 Solución de Problemas Comunes

### **Error: "Unable to connect to MySQL server"**

**Problema:** No se puede conectar a MySQL

**Soluciones:**

```bash
# 1. Verificar que MySQL está corriendo
# Windows
Get-Service MySQL*

# 2. Verificar puerto MySQL (debe ser 3306)
netstat -an | findstr 3306

# 3. Probar conexión manual
mysql -u root -p
```

### **Error: "Access denied for user 'root'@'localhost'"**

**Problema:** Credenciales incorrectas

**Solución:**

1. Verifique su contraseña de MySQL
2. Actualice `appsettings.json` con la contraseña correcta:

```json
"DefaultConnection": "Server=localhost;Port=3306;Database=rickandmorty_db;User=root;Password=SU_PASSWORD;"
```

### **Error: "Unknown database 'rickandmorty_db'"**

**Problema:** La base de datos no existe

**Solución:**

```bash
mysql -u root -p
source Database/schema.sql
```

### **Error: "Port 7127 is already in use"**

**Problema:** El puerto está ocupado

**Solución:** Cambiar puerto en `Properties/launchSettings.json`:

```json
"applicationUrl": "https://localhost:7999;http://localhost:5999"
```

### **Error al compilar**

```bash
# Limpiar y reconstruir
dotnet clean
dotnet restore
dotnet build
```

### **La API devuelve datos vacíos**

**Verificar:**

1. Que tenga conexión a internet (necesita acceder a rickandmortyapi.com)
2. Revisar logs en la consola
3. Probar con Swagger UI directamente

---

## 📊 Verificación de la Instalación

Use este checklist para verificar que todo funciona correctamente:

- [ ] **MySQL funcionando**: `mysql -u root -p` (debe conectar)
- [ ] **Base de datos creada**: `USE rickandmorty_db; SHOW TABLES;` (debe mostrar Characters)
- [ ] **Proyecto compila**: `dotnet build` (debe decir "Build succeeded")
- [ ] **Proyecto ejecuta**: `dotnet run` (debe mostrar puertos)
- [ ] **Swagger abre**: Navegador en `https://localhost:7127/swagger`
- [ ] **GET /api/characters funciona**: Debe devolver lista de personajes
- [ ] **GET /api/characters/1 funciona**: Debe devolver Rick Sanchez
- [ ] **Datos se guardan en BD**: `SELECT COUNT(*) FROM Characters;` (debe ser > 0 después de consultas)

---

## 📁 Archivos Importantes

| Archivo                    | Descripción                                |
| -------------------------- | ------------------------------------------ |
| `Program.cs`               | Configuración principal y punto de entrada |
| `appsettings.json`         | Cadena de conexión y configuraciones       |
| `Database/schema.sql`      | Script de creación de base de datos        |
| `RickAndMortyBackend.http` | Ejemplos de peticiones HTTP                |
| `ARCHITECTURE.md`          | Documentación de arquitectura detallada    |
| `QUICKSTART.md`            | Guía rápida de inicio                      |

---

## 🚀 Comandos Útiles

```bash
# Restaurar dependencias
dotnet restore

# Compilar proyecto
dotnet build

# Ejecutar en modo desarrollo
dotnet run

# Ejecutar con hot reload
dotnet watch run

# Publicar para producción
dotnet publish -c Release -o ./publish

# Limpiar archivos de compilación
dotnet clean

# Ver información del proyecto
dotnet --info
```

---

## 📝 Notas para el Evaluador

### ✅ **Checklist de Evaluación:**

1. **Arquitectura limpia**: Repository + Service + Controller pattern
2. **Consumo de API externa**: HttpClient para rickandmortyapi.com
3. **Persistencia**: MySQL con Entity Framework Core
4. **Paginación**: Implementada en listado de personajes
5. **Filtros**: Por name, status, species
6. **Manejo de errores**: Middleware global + try-catch
7. **Documentación**: Swagger UI funcional
8. **CORS**: Configurado para frontend
9. **Código limpio**: Interfaces, inyección de dependencias
10. **Base de datos**: Índices optimizados para búsquedas

### 🎯 **Flujo de prueba recomendado:**

1. Verificar Swagger UI funcional
2. Probar GET /api/characters (listado básico)
3. Probar paginación con ?page=2
4. Probar filtros: ?name=rick, ?status=alive
5. Probar detalle con /api/characters/1
6. Verificar persistencia en MySQL
7. Probar caso de error con ID inválido

### 📞 **Soporte**

Si encuentra algún problema durante la evaluación:

- Revisar sección "Solución de Problemas Comunes"
- Verificar logs en la consola de la aplicación
- Confirmar conexión a MySQL y a Internet

---

## 📄 Licencia

Este proyecto es de código abierto para fines educativos.

---

**API Externa utilizada**: [Rick and Morty API](https://rickandmortyapi.com/)

**Desarrollado con**: ASP.NET Core 8.0 | Entity Framework Core | MySQL
