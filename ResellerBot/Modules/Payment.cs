using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace ResellerBot.Modules
{
    public class payment : InteractionModuleBase<SocketInteractionContext>
    {
        private DiscordSocketClient _client;
        private bool messageSent = false; // Flag to track whether the message has been sent

        public payment(DiscordSocketClient client)
        {
            _client = client;
            _client.MessageReceived += MessageReceived; // Hook up the MessageReceived event
        }

        private async Task MessageReceived(SocketMessage message)
        {
            var channel = message.Channel;

            if (channel.Id == 1126169115275567176)
            {
                if (channel is SocketTextChannel textChannel)
                {
                    try
                    {
                        // Send your message
                        await textChannel.SendMessageAsync("Your Message");
                        messageSent = true; // Set the flag to indicate the message has been sent
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions and log the error for diagnosis
                        Console.WriteLine("Error sending message: " + ex.Message);
                    }
                }
            }
            else if (channel.Name.Contains("coon"))
            {
                if (channel is SocketTextChannel textChannel)
                {
                    try
                    {
                        // Send your message for "ticket"
                        await textChannel.SendMessageAsync("Your Message for Ticket");
                        messageSent = true; // Set the flag to indicate the message has been sent
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions and log the error for diagnosis
                        Console.WriteLine("Error sending message for ticket: " + ex.Message);
                    }
                }
            }
        }
    }
}
