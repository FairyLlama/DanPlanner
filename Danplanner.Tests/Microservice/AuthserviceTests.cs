using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Danplanner.Client.Services;
using Moq;
using Xunit;

namespace Danplanner.Tests.Auth
{
    public class AuthserviceTests
    {
        private static string CreateTestJwt(string email, string role)
        {
            var claims = new[]
            {
                new Claim("sub", email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1));

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }

        private sealed class UserChangedProbe
        {
            public bool Fired { get; set; }
        }

        private static (Authservice service, UserChangedProbe probe) CreateAuthService(
            Func<HttpRequestMessage, HttpResponseMessage> responder)
        {
            var handler = new FakeHttpMessageHandler((request, ct) =>
            {
                return Task.FromResult(responder(request));
            });

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://fake-auth.local/")
            };

            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock
                .Setup(f => f.CreateClient("Auth"))
                .Returns(client);

            var probe = new UserChangedProbe();

            var service = new Authservice(factoryMock.Object);
            service.OnUserChanged += () => probe.Fired = true;

            return (service, probe);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_SetsCurrentUserAndReturnsToken()
        {
            // Arrange
            var jwt = CreateTestJwt("admin@example.com", "Admin");

            var (auth, probe) = CreateAuthService(request =>
            {
                Assert.EndsWith("auth/login", request.RequestUri!.ToString());

                var json = $"{{\"token\":\"{jwt}\"}}";
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            });

            // Act
            var returnedToken = await auth.LoginAsync("admin@example.com", "hemmelig123");

            // Assert
            Assert.NotNull(returnedToken);
            Assert.Equal(jwt, returnedToken);

            Assert.NotNull(auth.CurrentUser);
            Assert.Equal("admin@example.com", auth.CurrentUser!.Email);
            Assert.Equal("Admin", auth.CurrentUser.Role);

            Assert.Equal("Login successful", auth.LastMessage);
            Assert.True(probe.Fired);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidCredentials_SetsLastMessageAndReturnsNull()
        {
            // Arrange
            var (auth, probe) = CreateAuthService(request =>
            {
                var json = "{\"error\":\"Invalid email or password\"}";
                return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            });

            // Act
            var returnedToken = await auth.LoginAsync("admin@example.com", "forkert");

            // Assert
            Assert.Null(returnedToken);
            Assert.Equal("Login failed", auth.LastMessage);
            Assert.Null(auth.CurrentUser);
            Assert.False(probe.Fired);
        }
    }

    /// <summary>
    /// Simpel fake HttpMessageHandler, så vi kan styre HTTP-svar
    /// uden at lave rigtige HTTP-calls.
    /// </summary>
    internal class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;

        public FakeHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc)
        {
            _handlerFunc = handlerFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => _handlerFunc(request, cancellationToken);
    }
}
