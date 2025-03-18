Obligatorio I de Diseño de Aplicaciones I

- Francisco Sosa
- Santiago Sparkov
- Francisco Suarez

### Descripción General
TaskPanel es un sistema de gestión de tareas (Task Management System) desarrollado como una aplicación web moderna. El proyecto está estructurado siguiendo los principios de Clean Architecture y está dividido en varios módulos para mantener una separación clara de responsabilidades.

### Arquitectura del Proyecto
El proyecto está organizado en las siguientes capas principales:

1. **TaskPanel (Capa de Presentación)**
   - Aplicación web Blazor Server
   - Maneja la interfaz de usuario y la lógica de presentación
   - Implementa el patrón MVC (Model-View-Controller)

2. **TaskPanelLibrary (Capa de Lógica de Negocio)**
   - Contiene la lógica principal de la aplicación
   - Implementa los servicios y la lógica de negocio
   - Define las interfaces y contratos

3. **TaskPanelDataAccess (Capa de Acceso a Datos)**
   - Maneja la interacción con la base de datos
   - Implementa los repositorios concretos
   - Utiliza Entity Framework Core para el ORM

4. **TaskPanelModels (Capa de Modelos)**
   - Define las entidades y modelos de datos
   - Contiene las clases de dominio
   - Define las estructuras de datos principales

5. **TaskPanelTest (Capa de Pruebas)**
   - Contiene pruebas unitarias
   - Implementa pruebas de integración
   - Asegura la calidad del código

### Tecnologías Principales

1. **Backend y Framework**
   - .NET Core/6.0
   - Blazor Server para la interfaz de usuario
   - Entity Framework Core para el ORM
   - SQL Server como base de datos principal

2. **Frontend**
   - Blazor Server
   - Razor Pages
   - HTML/CSS/JavaScript

3. **Base de Datos**
   - SQL Server 2019
   - Docker para la contenedorización de la base de datos

4. **Herramientas de Desarrollo**
   - Docker y Docker Compose para contenedorización
   - Git para control de versiones
   - Visual Studio/Rider como IDE

### Características Principales
El sistema parece estar diseñado para manejar:
- Gestión de Usuarios
- Gestión de Equipos
- Gestión de Paneles
- Gestión de Tareas
- Sistema de Comentarios
- Gestión de Epics
- Sistema de Papelera (Trash)

### Patrones de Diseño y Principios
- Clean Architecture
- Repository Pattern
- Dependency Injection
- Interface-based Programming
- SOLID Principles

### Infraestructura
- Contenedorización con Docker
- Base de datos SQL Server en contenedor
- Sistema de migraciones para la base de datos
- Configuración flexible mediante appsettings.json

### Seguridad y Autenticación
- Sistema de autenticación integrado
- Manejo de sesiones
- Control de acceso basado en roles

