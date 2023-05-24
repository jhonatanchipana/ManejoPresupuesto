# Manejo de Presupuesto

Este proyecto es una aplicación web desarrollada con ASP.NET Core MVC que permite a los usuarios gestionar su presupuesto personal.
Permite registrar ingresos, gastos y llevar un seguimiento del saldo disponible, a través de distintos reportes.

## Características

- Registro y autenticación de usuarios.
- Registro de categorías, cuentas y tipo de cuenta.
- Registro de ingresos y gastos con categorías y el tipo de cuenta.
- Exportación de los datos (excel).
- Visualización del saldo disponible diariamente, semanalmente y mensualmente.
- Visualización a través de un calendario de los ingresos y gastos.
- Validación de datos en el lado del servidor y del cliente.

## Tecnologías Utilizadas

- ASP.NET Core MVC: Framework de desarrollo web utilizado para crear la aplicación.
- C#: Lenguaje de programación utilizado para el desarrollo del backend.
- Dapper: ORM utilizado para la comunicación con la base de datos.
- HTML, CSS y JavaScript: Lenguajes utilizados para la creación de la interfaz de usuario.
- Bootstrap: Framework CSS utilizado para el diseño responsive.
- FullCalendar: librería usado para el calendario.

## Requisitos Previos

- .NET Core SDK (versión 6.0)
- Visual Studio 2022 (opcional)

## Instalación

1. Clona este repositorio: `git clone https://github.com/jhonatanchipana/ManejoPresupuesto.git`
2. Abre el proyecto en tu entorno de desarrollo preferido (por ejemplo, Visual Studio o Visual Studio Code).
3. Configura la cadena de conexión a la base de datos en el archivo appsettings.json. Asegúrate de tener una instancia de base de datos compatible y actualizada.
4. Ejecuta las migraciones de Entity Framework para crear la estructura de la base de datos `dotnet ef database update`.
6. Ejecuta el proyecto: `dotnet run`
7. Abre tu navegador web y visita `http://localhost:5043`

## Licencia

Este proyecto se encuentra bajo la licencia [MIT](https://opensource.org/licenses/MIT).

## Contacto

Si tienes alguna pregunta o sugerencia, no dudes en contactarme en jhonatan.chipana.barrientos@hotmail.com.
