using EDPProjectGrp2.Models;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private readonly IDictionary<string, UserConnection> _connections;

    public ChatHub(IDictionary<string, UserConnection> connections)
    {
        _connections = connections;
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
        {
            _connections.Remove(Context.ConnectionId);
            Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", $"{userConnection.User} has left");
            SendUsersConnected(userConnection.Room);
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task JoinRoom(UserConnection userConnection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);

        _connections[Context.ConnectionId] = userConnection;

        await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", $"{userConnection.User} has joined {userConnection.Room}");

        await SendUsersConnected(userConnection.Room);
    }

    public async Task SendMessage(string message)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
        {
            await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", message, userConnection.User);
        }
    }

    public Task SendUsersConnected(string room)
    {
        var users = _connections.Values
            .Where(c => c.Room == room)
            .Select(c => c.User);

        return Clients.Group(room).SendAsync("UsersInRoom", users);
    }

    public async Task ResolveTicket(string user)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
        {
            await Clients.Group(userConnection.Room).SendAsync("ResolveTicket", user);
        }
    }

    public async Task StartTyping(string user)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
        {
            await Clients.Group(userConnection.Room).SendAsync("OtherUserTyping", true, user);
        }
    }

    public async Task StopTyping(string user)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
        {
            await Clients.Group(userConnection.Room).SendAsync("OtherUserTyping", false, user);
        }
    }
}

