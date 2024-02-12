using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public async Task SendMessage(string message, string user)
    {
        try
        {
            // Your existing logic for processing the message

            await Clients.All.SendAsync("ReceiveMessage", message, user);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            // You can also send an error message back to the client if needed
            await Clients.Caller.SendAsync("ReceiveErrorMessage", ex.Message);
        }
    }

    public async Task ResolveTicket(string user)
    {
        await Clients.User(user).SendAsync("Ticket Resolved");
    }
}
