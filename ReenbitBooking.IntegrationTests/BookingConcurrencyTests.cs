using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Application.Features.Bookings.Commands.CreateBooking;
using ReenbitBooking.Domain.Entities;
using ReenbitBooking.Domain.Enums;
using ReenbitBooking.Infrastructure.Context;

namespace ReenbitBooking.IntegrationTests;

public class BookingConcurrencyTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private Guid _slotId;
    private Guid _firstUserId;
    private Guid _secondUserId;
    private string _firstUserToken = string.Empty;
    private string _secondUserToken = string.Empty;

    public BookingConcurrencyTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = "Conference Room A",
            Location = "Floor 1",
            Capacity = 10,
            IsActive = true
        };

        var slot = new Slot
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            StartTimeUtc = DateTimeOffset.UtcNow.AddHours(1),
            EndTimeUtc = DateTimeOffset.UtcNow.AddHours(2),
            Status = SlotStatus.Available
        };

        var firstUser = new User
        {
            Id = Guid.NewGuid(),
            FullName = "First User",
            Email = "first.user@reenbit.test",
            Role = UserRole.RegularUser,
            PasswordHash = "hashed-password",
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        var secondUser = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Second User",
            Email = "second.user@reenbit.test",
            Role = UserRole.RegularUser,
            PasswordHash = "hashed-password",
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        context.Rooms.Add(room);
        context.Slots.Add(slot);
        context.Users.AddRange(firstUser, secondUser);
        await context.SaveChangesAsync();

        _slotId = slot.Id;
        _firstUserId = firstUser.Id;
        _secondUserId = secondUser.Id;

        var jwtProvider = scope.ServiceProvider.GetRequiredService<IJwtProvider>();
        _firstUserToken = jwtProvider.GenerateToken(firstUser);
        _secondUserToken = jwtProvider.GenerateToken(secondUser);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task CreateBooking_WhenConcurrentRequests_ReturnsOneSuccessAndOneConflict()
    {
        using var firstClient = _factory.CreateClient();
        using var secondClient = _factory.CreateClient();

        firstClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _firstUserToken);
        secondClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _secondUserToken);

        var firstRequest = new CreateBookingCommand(_slotId, _firstUserId);
        var secondRequest = new CreateBookingCommand(_slotId, _secondUserId);

        var firstResponseTask = firstClient.PostAsJsonAsync("/api/bookings", firstRequest);
        var secondResponseTask = secondClient.PostAsJsonAsync("/api/bookings", secondRequest);

        var responses = await Task.WhenAll(firstResponseTask, secondResponseTask);

        responses.Should().ContainSingle(r => r.StatusCode == HttpStatusCode.OK);
        responses.Should().ContainSingle(r => r.StatusCode == HttpStatusCode.Conflict);
    }
}
