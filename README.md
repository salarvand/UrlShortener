# UrlShortener

## Introduction

UrlShortener is a robust and efficient URL shortening service built with ASP.NET Core. It allows users to convert long, unwieldy URLs into short, easy-to-share links. This project demonstrates the implementation of a RESTful API using minimal APIs in .NET, along with Entity Framework Core for database operations.

## Features

- Shorten long URLs to compact, unique codes
- Redirect short URLs to their original long URLs
- RESTful API for easy integration
- Efficient database storage and retrieval
- Customizable short URL length

## Technologies Used

- ASP.NET Core 8.0
- Entity Framework Core 8.0.8
- SQL Server (can be easily adapted to other databases)
- Minimal API architecture
- xUnit for unit testing
- FluentAssertions for readable test assertions

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- SQL Server (or another compatible database)
- Visual Studio 2022, Visual Studio Code

### Installation

1. Clone the repository:
   ```
   git clone https://github.com/salarvand/UrlShortener.git
   ```

2. Navigate to the project directory:
   ```
   cd UrlShortener
   ```

3. Update the connection string in `appsettings.json` to point to your database:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=UrlShortener;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

4. Run the database migrations:
   ```
   dotnet ef database update
   ```

5. Build and run the project:
   ```
   dotnet run --project src/UrlShortener.Api
   ```

The API will be available at `https://localhost:5001` (or `http://localhost:5000`).

## API Usage

### Shorten a URL

**Request:**