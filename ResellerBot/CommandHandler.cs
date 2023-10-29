using System;
using System.Reflection;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ResellerBot;

public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _handler;
    private readonly IServiceProvider _services;

    public CommandHandler(DiscordSocketClient client, IServiceProvider services, InteractionService handler)
    {
        _client = client;
        _services = services;
        _handler = handler;
        client.ChannelCreated += ChannelCreated;
    }

    public async Task InitializeAsync()
    {
        // Process when the client is ready, so we can register our commands.
        _client.Ready += ReadyAsync;
        _handler.Log += Program.LogAsync;

        // Add the public modules that inherit InteractionModuleBase<T> to the InteractionService
        await _handler.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        // Process the InteractionCreated payloads to execute Interactions commands
        _client.InteractionCreated += HandleInteraction;
    }


    private async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules.
            var context = new SocketInteractionContext(_client, interaction);

            // Execute the incoming command.
            var result = await _handler.ExecuteCommandAsync(context, _services);

            if (!result.IsSuccess)
            {
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        if (interaction.HasResponded)
                        {
                            await interaction.FollowupAsync(result.ErrorReason, ephemeral: true);
                        }
                        else
                        {
                            await interaction.RespondAsync(result.ErrorReason, ephemeral: true);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        catch
        {
            // If Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
            // response, or at least let the user know that something went wrong during the command execution.
            if (interaction.Type is InteractionType.ApplicationCommand)
                await interaction.GetOriginalResponseAsync()
                    .ContinueWith(async (msg) => await msg.Result.DeleteAsync());
        }
    }

    private async Task ReadyAsync()
    {
        await _handler.RegisterCommandsGloballyAsync();
    }


    private async Task ChannelCreated(SocketChannel channel)
    {
        if (channel is SocketTextChannel textChannel)
        {
            ulong categoryIdToDetect = 1163983820245180446; // Replace with the desired category ID

            if (textChannel.CategoryId == categoryIdToDetect)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Purchase Ticket | Thank you for making a ticket we appreciate your interest in our products")
                    .WithDescription("Thank you for creating a ticket in Hanzo. Please select a payment method by clicking one of the buttons below that corresponds to your payment method to purchase your product.")
                    .WithColor(Color.DarkRed)
                    .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer =>
                {
                    footer.Text = ".gg/hanzofn | Hanzo Ticket";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1134997470510854276/1163574488185720933/IMG_1329.gif?ex=65401230&is=652d9d30&hm=d0de20a17c251e21b70ed5c14a152c02230959dfbb0ef0fb9a0a6dacca7c6aa8&=&width=621&height=621";
                })
                    .Build();

                var button1 = new ButtonBuilder()
                    .WithLabel("<:_:1126186864668319846> Credit/Debit Card")
                    .WithStyle(ButtonStyle.Danger)
                    .WithCustomId("button_1");

                var button2 = new ButtonBuilder()
                    .WithLabel("<:_:1136943749637873738> Cryptocurrency")
                    .WithStyle(ButtonStyle.Danger)
                    .WithCustomId("button_2");

                var button3 = new ButtonBuilder()
                    .WithLabel("<:_:1126173588194611210> CashApp")
                    .WithStyle(ButtonStyle.Danger)
                    .WithCustomId("button_3");

                var button4 = new ButtonBuilder()
                    .WithLabel("<:_:1136942507087904778> Apple Pay")
                    .WithStyle(ButtonStyle.Danger)
                    .WithCustomId("button_4");

                var button5 = new ButtonBuilder()
                    .WithLabel("<:_:1163980327543390288> Google Pay")
                    .WithStyle(ButtonStyle.Danger)
                    .WithCustomId("button_5");

                var button6 = new ButtonBuilder()
                    .WithLabel("<:_:1126173583606022235> Paypal")
                    .WithStyle(ButtonStyle.Danger)
                    .WithCustomId("button_6");

                var button7 = new ButtonBuilder()
                    .WithLabel("<:_:1136943177249603736> Venmo")
                    .WithStyle(ButtonStyle.Danger)
                    .WithCustomId("button_7");

                var button8 = new ButtonBuilder()
                    .WithLabel("😀 Zelle")
                    .WithStyle(ButtonStyle.Danger)
                    .WithCustomId("button_8");

                var row = new ComponentBuilder().WithButton(button1).WithButton(button2).WithButton(button3).WithButton(button4).WithButton(button5).WithButton(button6).WithButton(button7).WithButton(button8).Build();

                var message = await textChannel.SendMessageAsync("", embed: embed, components: row);

                var client = _client as DiscordSocketClient;
                client.InteractionCreated += async (interaction) =>
                {
                    if (interaction is SocketMessageComponent buttonInteraction)
                    {
                        // Access the Message property using buttonInteraction.Message
                        if (buttonInteraction.Message.Id == message.Id)
                        {
                            if (buttonInteraction.Data.CustomId == "button_1")
                            {
                                await buttonInteraction.RespondAsync("You clicked Button 1.");
                                var backButton = new ButtonBuilder()
                                    .WithLabel("Back")
                                    .WithStyle(ButtonStyle.Secondary)
                                    .WithCustomId("back_button");

                                var secondButtons = new ComponentBuilder().WithButton(backButton).Build();

                                await textChannel.SendMessageAsync("This is the second set of buttons:", components: secondButtons);
                            }
                            else if (buttonInteraction.Data.CustomId == "button_2")
                            {
                                await buttonInteraction.RespondAsync("You clicked Button 2.");
                                var backButton = new ButtonBuilder()
                                    .WithLabel("Back")
                                    .WithStyle(ButtonStyle.Secondary)
                                    .WithCustomId("back_button");
                            }
                            else if (buttonInteraction.Data.CustomId == "button_3")
                            {
                                await buttonInteraction.RespondAsync("You clicked Button 2.");
                                var backButton = new ButtonBuilder()
                                    .WithLabel("Back")
                                    .WithStyle(ButtonStyle.Secondary)
                                    .WithCustomId("back_button");
                            }
                            else if (buttonInteraction.Data.CustomId == "button_4")
                            {
                                await buttonInteraction.RespondAsync("You clicked Button 2.");
                                var backButton = new ButtonBuilder()
                                    .WithLabel("Back")
                                    .WithStyle(ButtonStyle.Secondary)
                                    .WithCustomId("back_button");
                            }
                            else if (buttonInteraction.Data.CustomId == "button_5")
                            {
                                await buttonInteraction.RespondAsync("You clicked Button 2.");
                                var backButton = new ButtonBuilder()
                                    .WithLabel("Back")
                                    .WithStyle(ButtonStyle.Secondary)
                                    .WithCustomId("back_button");
                            }
                            else if (buttonInteraction.Data.CustomId == "button_6")
                            {
                                await buttonInteraction.RespondAsync("You clicked Button 2.");
                                var backButton = new ButtonBuilder()
                                    .WithLabel("Back")
                                    .WithStyle(ButtonStyle.Secondary)
                                    .WithCustomId("back_button");
                            }
                            else if (buttonInteraction.Data.CustomId == "button_7")
                            {
                                await buttonInteraction.RespondAsync("You clicked Button 2.");
                                var backButton = new ButtonBuilder()
                                    .WithLabel("Back")
                                    .WithStyle(ButtonStyle.Secondary)
                                    .WithCustomId("back_button");
                            }
                            else if (buttonInteraction.Data.CustomId == "button_8")
                            {
                                await buttonInteraction.RespondAsync("You clicked Button 2.");
                                var backButton = new ButtonBuilder()
                                    .WithLabel("Back")
                                    .WithStyle(ButtonStyle.Secondary)
                                    .WithCustomId("back_button");
                            }
                            else if (buttonInteraction.Data.CustomId == "back_button")
                            {
                                // Delete the original message with the "Back" button
                                await buttonInteraction.Message.DeleteAsync();
                            }
                        }
                    }
                };
            }
        }
    }
}