using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace ResellerBot.Modules; 

public class Other : InteractionModuleBase<SocketInteractionContext>{
    public InteractionService Commands { get; set; }
    private CommandHandler _handler;
    // Constructor injection is also a valid way to access the dependencies
    public Other(CommandHandler handler)
    {
        _handler = handler;
    }

    [SlashCommand("ticket-change", "Changes ticket channel")]
    public async Task ChangeTicketChannelName(TicketType type, string name)
    {
        await RespondAsync("Loading...");

        if (!await IsTicketChannel((SocketChannel)Context.Channel))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Ticket Change")
                .WithColor(Color.DarkPurple)
                .WithDescription("This is not a ticket channel!")
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer =>
                {
                    footer.Text = ".gg/cheatos | Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();

            await FollowupAsync(embeds: new[] { errorEmbed }, ephemeral: true);
            await Context.Interaction.DeleteOriginalResponseAsync();
            return;
        }

        string newName = string.Empty;

        switch (type)
        {
            case TicketType.Osu:
            case TicketType.OsuReset:
                newName = $"Osu-{name}";
                break;
            case TicketType.Reset:
                newName = $"Reset-{name}";
                break;
            case TicketType.Perm:
                newName = $"Perm-{name}";
                break;
            case TicketType.Temp:
                newName = $"Temp-{name}";
                break;
            case TicketType.Private:
                newName = $"Private-{name}";
                break;
            case TicketType.Public:
                newName = $"Public-{name}";
                break;
            default:
                newName = name;
                break;
        }

        var channel = (SocketTextChannel)Context.Channel;
        await channel.ModifyAsync(x => x.Name = newName);
    }


    private async Task<bool> IsTicketChannel(SocketChannel channel) {
        var categories = Context.Guild.CategoryChannels;
        foreach (var category in categories) {
            if (category.Id == 1144854024055697448 || category.Id == 1147528743703810068 || category.Id == 1154233867759263885 || category.Id == 1147533968808353812 || category.Id == 1160724559738183821 || category.Id == 1165435884905177268 || category.Id == 1137256654325420093) {
                var channels = category.Channels;
                foreach (var c in channels) {
                    if (c.Id == channel.Id) {
                        return true;
                    }
                }
            }
        }
        
        return false;
    }

    public enum TicketType {
        Osu,
        Reset,
        Perm,
        Temp,
        Private,
        Public,
        OsuReset
    }
}