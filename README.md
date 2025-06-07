
# 🎬 SakilaWebCRUD (.NET Core + MySQL)

Este proyecto ASP.NET Core MVC implementa un mantenimiento CRUD profesional basado en la base de datos de ejemplo **Sakila** de MySQL, siguiendo buenas prácticas, uso de Entity Framework Core, AJAX, vistas parciales, Bootstrap y DataTables.

## 📌 Características principales

- CRUD completo para entidades Sakila como:
  - `City`
  - `Country`
  - `Address`
  - `Staff`
  - `Rental`
  - `Payment`
- Uso de `ApplicationDbContext` personalizado con todas las entidades correctamente mapeadas.
- Controladores separados por entidad, con métodos `Create`, `Edit`, `Delete` e `Index`.
- Formularios en vistas parciales (`_CreateEditModal.cshtml`) cargados con AJAX.
- Validaciones de datos tanto del lado del servidor como del cliente.
- Uso de `Toastr` para notificaciones.
- Listados interactivos con `DataTables`, búsqueda dinámica, ordenamiento y paginación.
- Uso de `ViewModel` fuertemente tipado con `SelectListItem` para dropdowns relacionados (FKs).
- Manejo de errores de base de datos, incluyendo restricciones (`MySqlException 1451`).

## ⚙️ Tecnologías utilizadas

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core + Scaffold-DbContext
- MySQL + Base de datos Sakila
- jQuery + AJAX
- Bootstrap 5
- DataTables
- Toastr

## 🧱 Estructura del proyecto

```text
/Controllers/                   → Controladores por entidad (CityController, CountryController, etc.)
/Models/                        → Entidades generadas desde la base de datos Sakila usando Scaffold-DbContext
/ViewModels/                    → ViewModels personalizados para manejar la lógica de presentación
/Views/
  ├── City/                     → Vistas para la entidad City (Index.cshtml, _CreateEditModal.cshtml)
  ├── Country/                  → Vistas para la entidad Country
  ├── Staff/                    → Vistas para la entidad Staff
  ├── Address/                  → Vistas para la entidad Address
  └── Payment/                  → Vistas para la entidad Payment
/wwwroot/js/                    → Archivos JavaScript para DataTables y funciones AJAX por entidad
/Pages/Shared/_Layout.cshtml   → Layout principal compartido por todas las vistas
/Datos/ApplicationDbContext.cs → DbContext personalizado con configuraciones, relaciones y entidades mapeadas

## 🚀 Cómo ejecutar el proyecto

1. Clona el repositorio:
   ```bash
   git clone https://github.com/tu-usuario/SakilaWebCRUD.git
   cd SakilaWebCRUD
   ```

2. Configura el archivo `appsettings.json` con tu cadena de conexión a MySQL:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "server=localhost;database=sakila;user=root;password=tu_clave;"
   }
   ```

3. Ejecuta el proyecto desde Visual Studio o con:

   ```bash
   dotnet run
   ```

4. Navega a `http://localhost:xxxx` (el puerto que Visual Studio asigne).

## 📷 Ejemplo de Interfaz

- Listado de ciudades con DataTables y acciones por fila.
- Modal de creación/edición cargado dinámicamente.
- Dropdowns con datos relacionados (p. ej. países en `City`).

## 📌 Créditos

Creditos:

Bienvenido Cuevas,
Ana Esther Segura Reyes,
Ayzel Lavinia Mateo Luciano



