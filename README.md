# email-marketing-api-asp
ASP.NET Core application and SQL Server with Docker

## Requirements

* [Visual Studio 2017](https://www.visualstudio.com/downloads/)
* [Docker for Windows](https://docs.docker.com/docker-for-windows/install/)
* [SQL Server 2017](https://hub.docker.com/r/microsoft/mssql-server-windows-express/)
* [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)


# Run on Docker
docker build -t email_marketing_web_api .
docker-compose up -d --build


## Getting Started

1. Clone this repository
2. Open the solution in Visual Studio 2017
3. Run the application on Docker: `docker-compose up -d --build`
4. Open SQL Server Management Studio and connect to the database
5. Run the following script to create the database and table
