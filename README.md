# Project Readme

## Technologies and Design Patterns Used
- **[.NET Core 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)**: Modern, high-performance, cross-platform framework.
- **[Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)**: Data access technology.
- **[ASP.NET Core Web Api](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-6.0)**: For building API controllers.

**Third-Party Libraries:**
- **[AutoMapper](https://automapper.org/)**: Object-to-object mapping.
- **[MediatR](https://github.com/jbogard/MediatR)**: For implementing mediator pattern.
- **[FluentValidation](https://docs.fluentvalidation.net/en/latest/aspnet.html)**: For validating models.
- **[NUnit](https://nunit.org/)**: For unit testing.
- **[NSubstitute]https://nsubstitute.github.io/)**: For mocking.

## Database Creation and Initialization
1. **Database Setup:**
   - Use Entity Framework migrations to create the database.
   - Run `dotnet ef migrations add InitialCreate` and `dotnet ef database update`.

2. **Data Seeding:**
   - Data seeding is implemented in the `DataInitializer` class.
   - In `Program.cs`, ensure `DataInitializer.SeedData(app)` is called during application startup.

## Source Code Preparation
1. **Restoring Dependencies:**
   - Run `dotnet restore` to restore all NuGet packages.

2. **Building the Project:**
   - Run `dotnet build` to build the solution.

3. **Running the Application:**
   - Execute `dotnet run` to start the application.

## Assumptions
- Assumed that the system should handle large volumes of read operations efficiently.
- User authentication is handled separately and not part of this scope.
- Error logging is missing and not implemented

## Feedback for Improvement
- Clarify requirements regarding user authentication and authorization.
- Consider including API versioning for future scalability.
- Provide more details on the expected data model relationships.
- Implement some kind of localization service.
