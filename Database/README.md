# Instrucciones para configurar la base de datos MySQL

## Requisitos previos

- MySQL Server 8.0 o superior instalado
- Cliente MySQL (MySQL Workbench, phpMyAdmin, o CLI)

## Paso 1: Crear la base de datos

### Opción A: Usando MySQL Workbench

1. Abrir MySQL Workbench
2. Conectarse a tu servidor MySQL
3. Abrir el archivo `schema.sql`
4. Ejecutar el script completo

### Opción B: Usando línea de comandos

```bash
# Conectarse a MySQL
mysql -u root -p

# Ejecutar el script
source C:\Users\ckcristock\Documents\.net\RickAndMortyBackend\Database\schema.sql

# O alternativamente
mysql -u root -p < C:\Users\ckcristock\Documents\.net\RickAndMortyBackend\Database\schema.sql
```

## Paso 2: Configurar la conexión en el proyecto

Edita el archivo `appsettings.json` con tus credenciales de MySQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=rickandmorty_db;User=root;Password=TU_PASSWORD_AQUI;"
  }
}
```

## Paso 3: Verificar la conexión

Para verificar que la base de datos se creó correctamente:

```sql
USE rickandmorty_db;
SHOW TABLES;
DESCRIBE Characters;
```

Deberías ver la tabla `Characters` con todas sus columnas e índices.

## Notas adicionales

- La tabla `Characters` usa el ID de la API de Rick and Morty como clave primaria
- Los índices en `Name`, `Status` y `Species` optimizan las búsquedas con filtros
- El campo `EpisodesJson` almacena los episodios en formato JSON
- El charset `utf8mb4` permite almacenar caracteres especiales y emojis
