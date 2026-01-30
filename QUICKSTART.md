# 🚀 Inicio Rápido

## Pasos para ejecutar el proyecto

### 1️⃣ Configurar MySQL

```bash
# Conectarse a MySQL
mysql -u root -p

# Ejecutar el script de base de datos
source Database/schema.sql
```

### 2️⃣ Configurar conexión

Editar `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=rickandmorty_db;User=root;Password=TU_PASSWORD;"
  }
}
```

### 3️⃣ Restaurar y ejecutar

```bash
dotnet restore
dotnet run
```

### 4️⃣ Probar la API

Abrir en el navegador:

```
https://localhost:7XXX/swagger
```

## 📡 Endpoints principales

```
GET /api/characters
GET /api/characters?page=2
GET /api/characters?name=rick
GET /api/characters?status=alive
GET /api/characters/{id}
```

## ✅ Verificación rápida

### Probar con curl:

```bash
# Obtener personajes
curl https://localhost:7XXX/api/characters

# Obtener personaje por ID
curl https://localhost:7XXX/api/characters/1
```

### Verificar base de datos:

```sql
USE rickandmorty_db;
SELECT COUNT(*) FROM Characters;
SELECT * FROM Characters LIMIT 10;
```

## 🐛 Troubleshooting

### Error de conexión a MySQL:

- Verificar que MySQL está corriendo
- Verificar usuario y contraseña en `appsettings.json`
- Verificar que el puerto es 3306

### Error al compilar:

```bash
dotnet clean
dotnet restore
dotnet build
```

### Puerto en uso:

Cambiar puertos en `Properties/launchSettings.json`

## 📝 Requisitos mínimos

- .NET 8.0 SDK
- MySQL 8.0+
- Puerto 3306 disponible (MySQL)
- Puertos 5XXX y 7XXX disponibles (Backend)
