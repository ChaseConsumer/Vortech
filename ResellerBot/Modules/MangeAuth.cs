using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;

namespace ResellerBot.Modules; 

public class ManageAuth : InteractionModuleBase<SocketInteractionContext>{
    public InteractionService Commands { get; set; }
    private CommandHandler _handler;
    // Constructor injection is also a valid way to access the dependencies
    public ManageAuth(CommandHandler handler)
    {
        _handler = handler;
    }

    [SlashCommand("reset-hwid", "Resets a user's HWID")]
    public async Task ResetHwid(Product product, string license)
    {
        await RespondAsync("Loading...");
        var config = ConfigHelper.Get()!;

        if (!config.Administrators.Contains(Context.User.Id))
        {
            var r = config.Resellers.FirstOrDefault(x => x.Id == Context.User.Id);

            if (config.PanelOwners.ContainsKey(Context.User.Id))
            {
                var p = config.PanelOwners[Context.User.Id];
                if (!p.Contains(product))
                {
                    var errorEmbed = new EmbedBuilder()
                        .WithTitle("Vortech Auth - Error | Reset HWID")
                        .WithColor(Color.DarkPurple)
                        .WithDescription("You are not allowed to use this command")
                        .WithTimestamp(DateTimeOffset.Now)
                        .WithFooter(footer => {
                            footer
                                .Text = ".gg/cheatos | Vortech Auth"; // Set the Text directly
                            footer
                                .IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                        })
                        .Build(); // Convert EmbedBuilder to Embed
                    await FollowupAsync(embeds: new[] { errorEmbed }, ephemeral: true);
                    await Context.Interaction.DeleteOriginalResponseAsync();
                    return;
                }
            }
            else
            {
                if (r == null)
                {
                    var errorEmbed = new EmbedBuilder()
                        .WithTitle("Vortech Auth - Error | Reset HWID")
                        .WithColor(Color.DarkPurple)
                        .WithDescription("You are not allowed to use this command")
                        .WithTimestamp(DateTimeOffset.Now)
                        .WithFooter(footer => {
                            footer
                                .Text = ".gg/cheatos | Vortech Auth"; // Set the Text directly
                            footer
                                .IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                        })
                        .Build(); // Convert EmbedBuilder to Embed
                    await FollowupAsync(embeds: new[] { errorEmbed }, ephemeral: true);
                    await Context.Interaction.DeleteOriginalResponseAsync();
                    return;
                }
            }
        }

        var result = KeyAuth.ResetHwid(Infos.SellerKeys[product], license);

        if (result)
        {
            var successEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth | Reset HWID")
                .WithColor(Color.DarkPurple)
                .WithDescription("Reset HWID successfully")
                .WithTimestamp(DateTimeOffset.Now)
                .WithImageUrl("https://media.discordapp.net/attachments/1123031318884786187/1165835065259991080/6RpFceJ.png?ex=65484b83&is=6535d683&hm=29eacb0233fd6806d470ca156e548ae292e5dd98ab69221c86afb1ed592fef1e&=&width=1103&height=621")
                .WithFooter(footer => {
                    footer
                        .Text = ".gg/cheatos | Vortech Auth"; // Set the Text directly
                    footer
                        .IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build(); // Convert EmbedBuilder to Embed
            await FollowupAsync(embeds: new[] { successEmbed }, ephemeral: false);
        }
        else
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth | Reset HWID")
                .WithColor(Color.DarkPurple)
                .WithDescription("Failed to reset HWID")
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer => {
                    footer
                        .Text = ".gg/cheatos | Vortech Auth"; // Set the Text directly
                    footer
                        .IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build(); // Convert EmbedBuilder to Embed
            await FollowupAsync(embeds: new[] { errorEmbed }, ephemeral: true);
        }

        await Context.Interaction.DeleteOriginalResponseAsync();

        await Program.LogCommand($"HWID reset for {license} by {Context.User.Username} ({Context.User.Id})");
    }


    [SlashCommand("check-user", "Finds a user by their license")]
    public async Task CheckUser(SocketGuildUser user, Product product)
    {
        await RespondAsync("Loading...");

        var config = ConfigHelper.Get()!;

        if (!config.Administrators.Contains(Context.User.Id))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Check User")
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
            await Context.Interaction.DeleteOriginalResponseAsync();
            return;
        }

        var keys = KeyAuth.FetchAllKeys(Infos.SellerKeys[product]);
        if (keys == null)
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Check User")
                .WithColor(Color.DarkPurple)
                .WithDescription("Product doesn't exist")
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

        var key = keys.Keys.Where(x => x.Note == user.Id.ToString());
        if (key == null)
        {
            var userNotFoundEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth | Check User")
                .WithColor(Color.DarkPurple)
                .WithDescription("User not found!")
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer =>
                {
                    footer.Text = ".gg/cheatos | Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();

            await FollowupAsync(embeds: new[] { userNotFoundEmbed }, ephemeral: true);
            await Context.Interaction.DeleteOriginalResponseAsync();
            return;
        }

        var keyString = string.Join("\n", key.Select(x => x.Key));
        var successEmbed = new EmbedBuilder()
             .WithTitle("Vortech Auth | Check User")
             .WithColor(Color.DarkPurple)
             .WithDescription($@"
> <:_:1146978420891336814>Got 100% Of The Users Keys.
> <:_:1146978420891336814>These are all the keys displayed for the user mentioned.

```yaml
Keys For All Products: {keyString}

```
")
            .WithTimestamp(DateTimeOffset.Now)
            .WithFooter(footer =>
            {
                footer.Text = ".gg/cheatos | Vortech Auth";
                footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
            })
            .Build();

        await FollowupAsync(embeds: new[] { successEmbed }, ephemeral: false);
        await Context.Interaction.DeleteOriginalResponseAsync();

        await Program.LogCommand($"User found for {user.Username} by {Context.User.Username} ({Context.User.Id})");
    }


    [SlashCommand("add-panel", "Adds a panel owner")]
    public async Task AddPanel(SocketGuildUser user, Product product)
    {
        await RespondAsync("Loading...");

        var config = ConfigHelper.Get()!;

        if (!config.Administrators.Contains(Context.User.Id))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Add Panel")
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
            await Context.Interaction.DeleteOriginalResponseAsync();
            return;
        }

        if (config.PanelOwners.ContainsKey(user.Id))
        {
            var p = config.PanelOwners[user.Id];
            if (p.Contains(product))
            {
                var userAlreadyPanelOwnerEmbed = new EmbedBuilder()
                    .WithTitle("Vortech Auth - Error | Add Panel")
                    .WithColor(Color.DarkPurple)
                    .WithDescription("User is already a panel owner!")
                    .WithTimestamp(DateTimeOffset.Now)
                    .WithFooter(footer =>
                    {
                        footer.Text = ".gg/cheatos | Vortech Auth";
                        footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                    })
                    .Build();

                await FollowupAsync(embeds: new[] { userAlreadyPanelOwnerEmbed }, ephemeral: true);
                await Context.Interaction.DeleteOriginalResponseAsync();
                return;
            }

            p.Add(product);
        }
        else
        {
            config.PanelOwners.Add(user.Id, new() { product });
        }

        config.Save();

        var successEmbed = new EmbedBuilder()
            .WithTitle("Vortech Auth | Add Panel")
            .WithColor(Color.DarkPurple)
            .WithDescription($"Added **{user.Username}** as a panel owner!")
            .WithTimestamp(DateTimeOffset.Now)
             .WithImageUrl("https://media.discordapp.net/attachments/1123031318884786187/1165835065259991080/6RpFceJ.png?ex=65484b83&is=6535d683&hm=29eacb0233fd6806d470ca156e548ae292e5dd98ab69221c86afb1ed592fef1e&=&width=1103&height=621")
            .WithFooter(footer =>
            {
                footer.Text = ".gg/cheatos | Vortech Auth";
                footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
            })
            .Build();

        await FollowupAsync(embeds: new[] { successEmbed }, ephemeral: false);
        await Context.Interaction.DeleteOriginalResponseAsync();

        await Program.LogCommand($"Panel added for {user.Username} by {Context.User.Username} ({Context.User.Id})");
    }


    [SlashCommand("remove-panel", "Removes a panel owner")]
    public async Task RemovePanel(SocketGuildUser user, Product product)
    {
        await RespondAsync("Loading...");

        var config = ConfigHelper.Get()!;

        if (!config.Administrators.Contains(Context.User.Id))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Remove Panel")
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
            await Context.Interaction.DeleteOriginalResponseAsync();
            return;
        }

        if (!config.PanelOwners.ContainsKey(user.Id))
        {
            var userNotPanelOwnerEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Remove Panel")
                .WithColor(Color.DarkPurple)
                .WithDescription("User is not a panel owner!")
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer =>
                {
                    footer.Text = ".gg/cheatos | Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();

            await FollowupAsync(embeds: new[] { userNotPanelOwnerEmbed }, ephemeral: true);
            await Context.Interaction.DeleteOriginalResponseAsync();
            return;
        }

        config.PanelOwners.Remove(user.Id);
        config.Save();

        var successEmbed = new EmbedBuilder()
            .WithTitle("Vortech Auth | Remove Panel")
            .WithColor(Color.DarkPurple)
            .WithDescription($"Removed **{user.Username}** as a panel owner!")
            .WithTimestamp(DateTimeOffset.Now)
             .WithImageUrl("https://media.discordapp.net/attachments/1123031318884786187/1165835065259991080/6RpFceJ.png?ex=65484b83&is=6535d683&hm=29eacb0233fd6806d470ca156e548ae292e5dd98ab69221c86afb1ed592fef1e&=&width=1103&height=621")
            .WithFooter(footer =>
            {
                footer.Text = ".gg/cheatos | Vortech Auth";
                footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
            })
            .Build();

        await FollowupAsync(embeds: new[] { successEmbed }, ephemeral: true);
        await Context.Interaction.DeleteOriginalResponseAsync();

        await Program.LogCommand($"Panel removed for {user.Username} by {Context.User.Username} ({Context.User.Id})");
    }


    [SlashCommand("set-download", "Sets the download link for a product")]
    public async Task SetDownload(Product product, string link)
    {
        await RespondAsync("Loading...");

        var config = ConfigHelper.Get()!;

        if (!config.Administrators.Contains(Context.User.Id))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Set Download")
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
            await Context.Interaction.DeleteOriginalResponseAsync();
            return;
        }

        config.DownloadLinks[product] = link;
        config.Save();

        var successEmbed = new EmbedBuilder()
            .WithTitle("Vortech Auth | Set Download")
            .WithColor(Color.DarkPurple)
            .WithDescription($"Set download link for **{product}** to **{link}**")
            .WithTimestamp(DateTimeOffset.Now)
            .WithFooter(footer =>
            {
                footer.Text = ".gg/cheatos | Vortech Auth";
                footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
            })
            .Build();

        await FollowupAsync(embeds: new[] { successEmbed }, ephemeral: true);
        await Context.Interaction.DeleteOriginalResponseAsync();

        await Program.LogCommand($"Download link set for {product} by {Context.User.Username} ({Context.User.Id})");
    }


    [SlashCommand("set-webhook", "Sets the webhook")]
    public async Task SetWebhook(string link)
    {
        await RespondAsync("Loading...");

        var config = ConfigHelper.Get()!;

        if (!config.Administrators.Contains(Context.User.Id))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Set Webhook")
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
            await Context.Interaction.DeleteOriginalResponseAsync();
            return;
        }

        config.Webhook = link;
        config.Save();

        var successEmbed = new EmbedBuilder()
            .WithTitle("Vortech Auth | Set Webhook")
            .WithColor(Color.DarkPurple)
            .WithDescription("Webhook set successfully")
            .WithTimestamp(DateTimeOffset.Now)
            .WithFooter(footer =>
            {
                footer.Text = ".gg/cheatos | Vortech Auth";
                footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
            })
            .Build();

        await FollowupAsync(embeds: new[] { successEmbed }, ephemeral: true);
        await Context.Interaction.DeleteOriginalResponseAsync();
    }

    



    [SlashCommand("delete-key", "Deletes a key")]
    public async Task DeleteKey(Product product, string license)
    {
        await RespondAsync("Loading...");

        var config = ConfigHelper.Get()!;

        if (!config.Administrators.Contains(Context.User.Id))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Delete Key")
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
            await Context.Interaction.DeleteOriginalResponseAsync();
            return;
        }

        var result = KeyAuth.DeleteLicense(Infos.SellerKeys[product], license);

        var response = result ? "Deleted key successfully" : "Failed to delete key";
        var successEmbed = new EmbedBuilder()
            .WithTitle("Vortech Auth | Delete Key")
            .WithColor(Color.DarkPurple)
            .WithDescription(response)
            .WithTimestamp(DateTimeOffset.Now)
            .WithFooter(footer =>
            {
                footer.Text = ".gg/cheatos | Vortech Auth";
                footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
            })
            .Build();

        await FollowupAsync(embeds: new[] { successEmbed }, ephemeral: true);
        await Context.Interaction.DeleteOriginalResponseAsync();
    }


    [SlashCommand("list-panel", "Lists all panel owners")]
    public async Task ListPanel(Product product)
    {
        await RespondAsync("Loading...");

        var config = ConfigHelper.Get()!;

        if (!config.Administrators.Contains(Context.User.Id))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | List Panel")
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
            await Context.Interaction.DeleteOriginalResponseAsync();
            return;
        }

        var panel = config.PanelOwners.Where(x => x.Value.Contains(product));
        var panelString = string.Join("\n", panel.Select(x => $"<@{x.Key}>"));

        var successEmbed = new EmbedBuilder()
            .WithTitle("Vortech Auth | List Panel")
            .WithColor(Color.DarkPurple)
            .WithDescription($"Panel owners for **{product}**:\n{panelString}")
            .WithTimestamp(DateTimeOffset.Now)
             .WithImageUrl("https://media.discordapp.net/attachments/1123031318884786187/1165835065259991080/6RpFceJ.png?ex=65484b83&is=6535d683&hm=29eacb0233fd6806d470ca156e548ae292e5dd98ab69221c86afb1ed592fef1e&=&width=1103&height=621")
            .WithFooter(footer =>
            {
                footer.Text = ".gg/cheatos | Vortech Auth";
                footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
            })
            .Build();

        await FollowupAsync(embeds: new[] { successEmbed }, ephemeral: false);
        await Context.Interaction.DeleteOriginalResponseAsync();
    }
}