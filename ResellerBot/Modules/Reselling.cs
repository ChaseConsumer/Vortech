using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace ResellerBot.Modules; 

public class Reselling : InteractionModuleBase<SocketInteractionContext> {
    public InteractionService Commands { get; set; }
    private CommandHandler _handler;
    // Constructor injection is also a valid way to access the dependencies
    public Reselling(CommandHandler handler)
    {
        _handler = handler;
    }

    [SlashCommand("gen", "Generates a key for a product")]
    public async Task GenerateKey(Product product, SocketUser user, KeyExpiry duration)
    {
        await RespondAsync("Loading...");
        var config = ConfigHelper.Get()!;

        var r = config.Resellers.FirstOrDefault(x => x.Id == Context.User.Id);
        if (r == null && !config.Administrators.Contains(Context.User.Id) && !config.PanelOwners.ContainsKey(Context.User.Id))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Generate Key")
                .WithColor(Color.DarkPurple)
                .WithDescription("You are not allowed to use this command")
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer =>
                {
                    footer.Text = ".gg/cheatos | Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();

            await FollowupAsync(embeds: new[] { errorEmbed }, ephemeral: true);
            return;
        }

        string userType = "";
        if (config.Administrators.Contains(Context.User.Id))
        {
            userType = "SellerAPI";
        }
        else if (config.PanelOwners.ContainsKey(Context.User.Id))
        {
            userType = "PanelOwner";
        }
        else
        {
            userType = r.Name;
        }

        int price = 0;
        int level = 1;


        switch (duration)
        {
            case KeyExpiry.Day:
                switch (product)
                {
                    case Product.FortnitePrivate:
                        price = 7;
                        break;
                    case Product.FortnitePublic:
                        price = 5;
                        break;
                    case Product.PermSpoofer:
                        price = 10;
                        break;

                }
                break;
            case KeyExpiry.Week:
                switch (product)
                {
                    case Product.FortnitePrivate:
                        price = 25;
                        break;
                    case Product.FortnitePublic:
                        price = 13;
                        break;
                }
                break;
            case KeyExpiry.Month:
                switch (product)
                {
                    case Product.FortnitePrivate:
                        price = 50;
                        break;
                    case Product.FortnitePublic:
                        price = 25;
                        break;
                }
                break;
            case KeyExpiry.Lifetime:
                switch (product)
                {
                    case Product.FortnitePrivate:
                        price = 150;
                        break;
                    case Product.FortnitePublic:
                        price = 75;
                        break;
                    case Product.PermSpoofer:
                        price = 25;
                        break;
                }
                break;
        }

        if (userType == "PanelOwner")
        {
            var panelOwner = config.PanelOwners[Context.User.Id];

            if (panelOwner == null)
            {
                var errorEmbed = new EmbedBuilder()
                    .WithTitle("Vortech Auth - Error | Generate Key")
                    .WithColor(Color.DarkPurple)
                    .WithDescription("You are not allowed to generate keys for this product")
                    .WithTimestamp(DateTimeOffset.Now)
                    .WithFooter(footer =>
                    {
                        footer.Text = ".gg/cheatos | Vortech Auth";
                        footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                    })
                    .Build();

                await FollowupAsync(embeds: new[] { errorEmbed }, ephemeral: true);
                return;
            }

            if (!panelOwner.Contains(product))
            {
                var errorEmbed = new EmbedBuilder()
                    .WithTitle("Vortech Auth - Error | Generate Key")
                    .WithColor(Color.DarkPurple)
                    .WithDescription("You are not allowed to generate keys for this product")
                    .WithTimestamp(DateTimeOffset.Now)
                    .WithFooter(footer =>
                    {
                        footer.Text = ".gg/cheatos | Vortech Auth";
                        footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                    })
                    .Build();

                await FollowupAsync(embeds: new[] { errorEmbed }, ephemeral: true);
                return;
            }
        }

        if (r != null && r.Balance < price)
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Generate Key")
                .WithColor(Color.DarkPurple)
                .WithDescription("You do not have enough balance to generate this key")
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer =>
                {
                    footer.Text = ".gg/cheatos | Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();

            await FollowupAsync(embeds: new[] { errorEmbed }, ephemeral: true);
            return;
        }

        var result = KeyAuth.GenerateLicense(Infos.SellerKeys[product], duration, owner: Context.User.Username, userid: user.Id.ToString(), level: level);

        if (string.IsNullOrEmpty(result))
        {
            result = "Failed to generate key";
        }
        else
        {
            if (r != null)
            {
                r.Balance -= price;
                r.KeysGenerated++;
                config.Save();
            }
        }

        var embed = new EmbedBuilder()
    .WithTitle("Vortech Auth | Generate License")
    .WithColor(Color.DarkPurple)
    .WithDescription($@"
> <:_:1146978420891336814>Key Generation Request Successful.
> <:_:1146978420891336814>Your License for `{product}` has been generated.
  
```yaml
{result}
```

> <:_:1158610914044551168> Product: **{product}**
> <:_:1132367989509980170> User: {user}
> <a:_:1165843057896325191> Duration: **{duration}**")
    .WithImageUrl("https://media.discordapp.net/attachments/1123031318884786187/1165835065259991080/6RpFceJ.png?ex=65484b83&is=6535d683&hm=29eacb0233fd6806d470ca156e548ae292e5dd98ab69221c86afb1ed592fef1e&=&width=1103&height=621")
    .Build();

        await Context.Channel.SendMessageAsync(embed: embed);

        await Context.Interaction.DeleteOriginalResponseAsync();



        await Program.LogCommand($"Key generated for {user.Username}#{user.Discriminator} by {Context.User.Username} ({Context.User.Id})\n```{result}```");
    }

    [SlashCommand("download", "download loader")]
    public async Task Download(Product product, string license)
    {
        await RespondAsync("Loading...");

        var config = ConfigHelper.Get()!;

        if (!config.DownloadLinks.ContainsKey(product))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Download")
                .WithColor(Color.DarkPurple)
                .WithDescription("Product doesn't have a download link.")
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer =>
                {
                    footer.Text = "Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();

            await FollowupAsync(embeds: new[] { errorEmbed }, ephemeral: true);
            return;
        }

        if (!KeyAuth.KeyExists(Infos.SellerKeys[product], license))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Download")
                .WithColor(Color.DarkPurple)
                .WithDescription("Invalid license.")
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer =>
                {
                    footer.Text = "Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();

            await FollowupAsync(embeds: new[] { errorEmbed }, ephemeral: true);
            return;
        }

        var downloadEmbed = new EmbedBuilder()
            .WithTitle("Here Is Your Loader!")
            .WithColor(Color.DarkPurple)
            .WithDescription($"Click the button below to download the loader for {product}.")
            .WithTimestamp(DateTimeOffset.Now)
            .WithImageUrl("https://media.discordapp.net/attachments/1123031318884786187/1165835065259991080/6RpFceJ.png?ex=65484b83&is=6535d683&hm=29eacb0233fd6806d470ca156e548ae292e5dd98ab69221c86afb1ed592fef1e&=&width=1103&height=621")
                .WithFooter(footer =>
                {
                    footer.Text = "Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();

        var button = new ButtonBuilder()
            .WithLabel($"Download for {product}")
            .WithStyle(ButtonStyle.Link)
            .WithUrl(config.DownloadLinks[product]);

        await FollowupAsync(embeds: new[] { downloadEmbed }, components: new ComponentBuilder().WithButton(button).Build(), ephemeral: true);
        await Context.Interaction.DeleteOriginalResponseAsync();

        await Program.LogCommand($"Download link sent to {Context.User.Username} ({Context.User.Id})");
    }





}