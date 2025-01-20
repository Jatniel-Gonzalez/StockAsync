# Stock Management System

## 📦 Descripción
Este es un sistema de gestión de inventario desarrollado con:
- **Backend:** ASP.NET Core (API)
- **Frontend:** Blazor WebAssembly

## 🚀 Características
- Gestión de productos, categorías y stock.
- Registro de movimientos de inventario.
- Interfaz intuitiva y fácil de usar.

## 📂 Estructura del Proyecto
- **`/src/StockApi`:** Contiene el código fuente del backend.
- **`/src/StockFrontend`:** Contiene el código fuente del frontend (Blazor WebAssembly).

## 🛠️ Requisitos
- **SDK .NET 6 o superior**: [Descargar aquí](https://dotnet.microsoft.com/download)
- **Node.js** (opcional, si usas paquetes npm): [Descargar aquí](https://nodejs.org)

## ⚙️ Configuración Inicial
1. Clonar el repositorio:
   ```bash
   [git clone (https://github.com/Jatniel-Gonzalez/StockAsync.git)
   cd tu-repositorio
   ```

2. Abrir el proyecto en Visual Studio.

3. Configurar la base de datos en el archivo `appsettings.json` dentro del proyecto backend (StockApi). Asegúrate de actualizar la cadena de conexión según tu entorno.

4. Ejecutar las migraciones para preparar la base de datos:
   - Abre la Consola del Administrador de Paquetes en Visual Studio.
   - Selecciona el proyecto `StockApi` como proyecto de inicio.


5. Asegúrate de que tanto el proyecto `StockApi` como `StockFrontend` estén seleccionados como proyectos de inicio en Visual Studio. Esto se puede hacer desde la configuración de solución.

6. Ejecuta la solución presionando `F5` o haciendo clic en el botón de inicio en Visual Studio.

## 🔧 Uso
- Accede al frontend en tu navegador en la dirección que se muestra al ejecutar el proyecto.
- Usa las herramientas para gestionar productos, categorías y movimientos de inventario.

## 🗂️ Despliegue
1. Publicar el backend:
   ```bash
   cd src/StockApi
   dotnet publish -c Release -o ./publish
   `
- El script de la base de datos estará disponible en este repositorio en la carpeta correspondiente. Asegúrate de ejecutarlo para inicializar la base de datos.

- Verifica los puertos en uso para evitar conflictos con otras aplicaciones.

## 📬 Soporte
Para cualquier duda o inconveniente, abre un issue en este repositorio o contacta al administrador del proyecto.
