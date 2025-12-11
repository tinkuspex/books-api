# Books API (.NET 8 + Dapper + Stored Procedures)

API REST para CRUD de libros. Implementada en .NET 8, Dapper y SQL Server (Stored Procedures).

## Contenido del repositorio

- `/` - código fuente (Api, Application, Domain, Infrastructure)
- `/scripts/sql-server` - scripts SQL: creación de base, tablas y stored procedures
- `/postman` - colección Postman para probar la API (cambiar la variable baseUrl en caso de levantar en un puerto distinto)

## Requisitos

- .NET 8 SDK
- SQL Server (local o Docker)
- (Opcional) Postman para consumir la API

## Configuración local

1. Clona el repositorio:
`bash`
git clone https://github.com/tinkuspex/books-api.git

2.	Crear la base de datos y ejecutar scripts SQL:
- Abre scripts/sql-server/lybrarydb.sql y ejecuta en tu SQL Server.
- Ejecuta create database y table. Y luego los stored procedures (SpBooks_*.).

3.	Configura connection string 
- Edita ConnectionStrings:Conexion con la cadena a tu SQL Server.

4. Restaurar dependencias y ejecutar:
cd PruebaUPCH.Api
dotnet restore
dotnet build
dotnet run

Por defecto la API correrá en https://localhost:7056 y http://localhost:5224.

# Endpoints
GET /api/books - Obtener todos los libros

GET /api/books/{id} - Obtener libro por id

POST /api/books - Crear libro
Body: BookCreateDto

PUT /api/books - Actualizar libro (id dentro del body)
Body: BookUpdateDto (incluye id)

DELETE /api/books/{id} - Eliminar libro

Ejemplo BookCreateDto:
{
  "title": "Clean Code",
  "author": "Robert C. Martin",
  "publicationYear": 2008,
  "publisher": "Prentice Hall",
  "pageCount": 464,
  "category": "Software Development"
}