-- Rick and Morty Backend Database Schema
-- Database: rickandmorty_db
-- Description: Script para crear la base de datos y las tablas necesarias

-- Crear la base de datos
CREATE DATABASE IF NOT EXISTS rickandmorty_db
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

-- Usar la base de datos
USE rickandmorty_db;

-- Crear tabla de personajes
CREATE TABLE IF NOT EXISTS Characters (
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Verificar que la tabla se creó correctamente
SHOW TABLES;
DESCRIBE Characters;
