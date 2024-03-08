using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonaInformationRegistry.Api.Middlewares;
using PersonalInformationRegistry.Application;
using System.Text.Json;
using PersonalInformationRegistry.Domain;

namespace PersonaInformationRegistry.Api.Tests.Middlewares
{
    [TestFixture]
    public class ExceptionHandlingMiddlewareTests
    {
        private DefaultHttpContext _context;
        private ExceptionHandlingMiddleware _middleware;

        [SetUp]
        public void SetUp()
        {
            _context = new DefaultHttpContext
            {
                Response =
                {
                    Body = new MemoryStream()
                }
            };

            _middleware = new ExceptionHandlingMiddleware((_) => Task.CompletedTask);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Response.Body.Dispose();
        }

        private async Task<string> ReadResponseContent()
        {
            _context.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(_context.Response.Body);
            return await reader.ReadToEndAsync();
        }

        [Test]
        public async Task Middleware_CapturesNotFoundException_ReturnsNotFoundResponse()
        {
            _middleware = new ExceptionHandlingMiddleware((_) => throw new NotFoundException("Resource not found."));

            await _middleware.InvokeAsync(_context);

            var streamText = await ReadResponseContent();
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(streamText);

            Assert.Multiple(() =>
            {
                Assert.That(_context.Response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
                Assert.That(problemDetails?.Title, Is.EqualTo("NotFound"));
            });
        }

        [Test]
        public async Task Middleware_CapturesGenericException_ReturnsInternalServerErrorResponse()
        {
            _middleware = new ExceptionHandlingMiddleware((_) => throw new Exception("General error."));

            await _middleware.InvokeAsync(_context);

            var streamText = await ReadResponseContent();
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(streamText);
            Assert.Multiple(() =>
            {
                Assert.That(_context.Response.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
                Assert.That(problemDetails?.Title, Is.EqualTo("An error occurred while processing the request."));
                Assert.That(problemDetails?.Detail, Is.EqualTo("General error."));
            });
        }
    }
}
