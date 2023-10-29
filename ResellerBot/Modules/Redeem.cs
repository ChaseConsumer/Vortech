using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace ResellerBot.Modules;

public class Redeem : InteractionModuleBase<SocketInteractionContext>
{
    public InteractionService Commands { get; set; }
    private CommandHandler _handler;
    // Constructor injection is also a valid way to access the dependencies
    public Redeem(CommandHandler handler)
    {
        _handler = handler;
    }

    [SlashCommand("redeem", "Sends a message to help the customer")]
    public async Task SendRedeemInstructions()
    {
        string message = "To install your loader, please execute /download and write in your **key**. Then you'll be redirected to the download page.";

        var embed = new EmbedBuilder()
            .WithTitle("How to Install Your Loader")
            .WithDescription(message)
            .WithColor(Color.DarkPurple)
            .WithImageUrl("https://media.discordapp.net/attachments/1123031318884786187/1165835065259991080/6RpFceJ.png?ex=65484b83&is=6535d683&hm=29eacb0233fd6806d470ca156e548ae292e5dd98ab69221c86afb1ed592fef1e&=&width=1103&height=621")
            .WithFooter(footer =>
            {
                footer.Text = ".gg/cheatos | Vortech Auth";
                footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
            })
            .Build();

        await Context.Channel.SendMessageAsync(embed: embed);
        await Program.LogCommand($"Redeem executed by {Context.User.Username} ({Context.User.Id})");
    }

}