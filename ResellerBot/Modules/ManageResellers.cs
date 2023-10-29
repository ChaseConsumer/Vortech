using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace ResellerBot.Modules;

public class ManageResellers : InteractionModuleBase<SocketInteractionContext>
{
    public InteractionService Commands { get; set; }
    private CommandHandler _handler;
    // Constructor injection is also a valid way to access the dependencies
    public ManageResellers(CommandHandler handler)
    {
        _handler = handler;
    }

    [SlashCommand("create-reseller", "Adds a reseller to the database")]
    public async Task CreateReseller(SocketGuildUser User, int startingBalance = 0)
    {
        var config = ConfigHelper.Get()!;

        var r = config.Resellers.FirstOrDefault(x => x.Id == User.Id);
        if (r != null)
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Create Reseller")
                .WithColor(Color.DarkPurple)
                .WithDescription("Reseller already exists!")
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer =>
                {
                    footer.Text = ".gg/cheatos | Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();

            await RespondAsync(embeds: new[] { errorEmbed }, ephemeral: true);
            return;
        }

        await RespondAsync("Loading...");

        if (!config.Administrators.Contains(Context.User.Id))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Create Reseller")
                .WithColor(Color.DarkPurple)
                .WithDescription("You are not allowed to run this command!")
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

        var reseller = new Reseller
        {
            Name = User.Username,
            Id = User.Id,
            Balance = startingBalance
        };

        config.Resellers.Add(reseller);
        config.Save();

        var successEmbed = new EmbedBuilder()
            .WithTitle("Vortech Auth | Create Reseller")
            .WithColor(Color.DarkPurple)
            .WithDescription("Reseller created!")
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

        await Program.LogCommand($"Reseller created by {Context.User.Username}#{Context.User.Discriminator}");
    }


    [SlashCommand("delete-reseller", "Deletes a reseller from the database")]
    public async Task DeleteReseller(SocketGuildUser User)
    {
        var config = ConfigHelper.Get()!;

        var r = config.Resellers.FirstOrDefault(x => x.Id == User.Id);
        if (r == null)
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Delete Reseller")
                .WithColor(Color.DarkPurple)
                .WithDescription("Reseller does not exist!")
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer =>
                {
                    footer.Text = ".gg/cheatos | Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();

            await RespondAsync(embeds: new[] { errorEmbed }, ephemeral: true);
            return;
        }

        await RespondAsync("Loading...");

        if (!config.Administrators.Contains(Context.User.Id))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Delete Reseller")
                .WithColor(Color.DarkPurple)
                .WithDescription("You are not allowed to run this command!")
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

        config.Resellers.Remove(r);
        config.Save();

        var successEmbed = new EmbedBuilder()
            .WithTitle("Vortech Auth | Delete Reseller")
            .WithColor(Color.DarkPurple)
            .WithDescription("Reseller deleted!")
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

        await Program.LogCommand($"Reseller deleted by {Context.User.Username}#{Context.User.Discriminator}");
    }


    [SlashCommand("add-balance", "Adds balance to a reseller")]
    public async Task AddBalanceToReseller(SocketGuildUser User, int amount)
    {
        var config = ConfigHelper.Get()!;

        var r = config.Resellers.FirstOrDefault(x => x.Id == User.Id);
        if (r == null)
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Add Balance")
                .WithColor(Color.DarkPurple)
                .WithDescription("Reseller does not exist!")
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer =>
                {
                    footer.Text = ".gg/cheatos | Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();

            await RespondAsync(embeds: new[] { errorEmbed }, ephemeral: true);
            return;
        }

        await RespondAsync("Loading...");

        if (!config.Administrators.Contains(Context.User.Id))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Add Balance")
                .WithColor(Color.DarkPurple)
                .WithDescription("You are not allowed to run this command!")
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

        r.Balance += amount;
        config.Save();

        var successEmbed = new EmbedBuilder()
            .WithTitle("Vortech Auth | Add Balance")
            .WithColor(Color.DarkPurple)
            .WithDescription("Balance added!")
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

        await Program.LogCommand($"Reseller balance added by {Context.User.Username}#{Context.User.Discriminator}");
    }


    [SlashCommand("remove-balance", "Removes balance from a reseller")]
    public async Task RemoveBalanceFromReseller(SocketGuildUser User, int amount)
    {
        var config = ConfigHelper.Get()!;

        var r = config.Resellers.FirstOrDefault(x => x.Id == User.Id);
        if (r == null)
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Remove Balance")
                .WithColor(Color.DarkPurple)
                .WithDescription("Reseller does not exist!")
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer =>
                {
                    footer.Text = ".gg/cheatos | Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();

            await RespondAsync(embeds: new[] { errorEmbed }, ephemeral: true);
            return;
        }

        await RespondAsync("Loading...");

        if (!config.Administrators.Contains(Context.User.Id))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Remove Balance")
                .WithColor(Color.DarkPurple)
                .WithDescription("You are not allowed to run this command!")
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

        r.Balance -= amount;
        config.Save();

        var successEmbed = new EmbedBuilder()
            .WithTitle("Vortech Auth | Remove Balance")
            .WithColor(Color.DarkPurple)
            .WithDescription("Balance removed!")
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

        await Program.LogCommand($"Reseller balance removed by {Context.User.Username}#{Context.User.Discriminator}");
    }


    [SlashCommand("set-balance", "Sets balance of a reseller")]
    public async Task SetBalanceOfReseller(SocketGuildUser User, int amount)
    {
        var config = ConfigHelper.Get()!;
        var r = config.Resellers.FirstOrDefault(x => x.Id == User.Id);
        if (r == null)
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Set Balance")
                .WithColor(Color.DarkPurple)
                .WithDescription("Reseller does not exist!")
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer =>
                {
                    footer.Text = ".gg/cheatos | Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();

            await RespondAsync(embeds: new[] { errorEmbed }, ephemeral: true);
            return;
        }

        await RespondAsync("Loading...");

        if (!config.Administrators.Contains(Context.User.Id))
        {
            var errorEmbed = new EmbedBuilder()
                .WithTitle("Vortech Auth - Error | Set Balance")
                .WithColor(Color.DarkPurple)
                .WithDescription("You are not allowed to run this command!")
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

        r.Balance = amount;
        config.Save();

        var successEmbed = new EmbedBuilder()
            .WithTitle("Vortech Auth | Set Balance")
            .WithColor(Color.DarkPurple)
            .WithDescription("Balance set!")
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

        await Program.LogCommand($"Reseller balance set to {amount} by {Context.User.Username}#{Context.User.Discriminator}");
    }


    [SlashCommand("reseller-info", "Gets info about a reseller")]
    public async Task GetResellerInfo(SocketGuildUser reseller)
    {
        await RespondAsync("Loading...");

        var config = ConfigHelper.Get()!;
        var r = config.Resellers.FirstOrDefault(x => x.Id == reseller.Id);
        if (!config.Administrators.Contains(Context.User.Id))
            if (r == null)
            {
                var errorEmbed = new EmbedBuilder()
                    .WithTitle("Vortech Auth - Error | Reseller Info")
                    .WithColor(Color.DarkPurple)
                    .WithDescription("Reseller does not exist!")
                    .WithTimestamp(DateTimeOffset.Now)
                    .WithFooter(footer =>
                    {
                        footer.Text = ".gg/cheatos | Vortech Auth";
                        footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                    })
                    .Build();

                await RespondAsync(embeds: new[] { errorEmbed }, ephemeral: true);
                return;
            }

        var embed = new EmbedBuilder();
        embed.Color = Color.DarkPurple;
        embed.WithTitle("Reseller Info");

        embed.AddField("Name", r.Name);
        embed.AddField("Balance", r.Balance.ToString() ?? "0");
        embed.AddField("Keys Generated", r.KeysGenerated.ToString() ?? "0");
        embed.WithTimestamp(DateTimeOffset.Now)
             .WithImageUrl("https://media.discordapp.net/attachments/1123031318884786187/1165835065259991080/6RpFceJ.png?ex=65484b83&is=6535d683&hm=29eacb0233fd6806d470ca156e548ae292e5dd98ab69221c86afb1ed592fef1e&=&width=1103&height=621")
             .WithFooter(footer =>
                {
                    footer.Text = ".gg/cheatos | Vortech Auth";
                    footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                })
                .Build();
        await FollowupAsync(embeds: new[] { embed.Build() });
        await Context.Interaction.DeleteOriginalResponseAsync();

        await Program.LogCommand($"Reseller info requested by {Context.User.Username}#{Context.User.Discriminator} ({Context.User.Id})");
    }


    [SlashCommand("list-resellers", "Lists all resellers")]
    public async Task ListResellers()
    {
        var config = ConfigHelper.Get()!;

        await RespondAsync("Loading...");
        if (!config.Administrators.Contains(Context.User.Id))
        {
            var resellers = config.Resellers;
            var embed = new EmbedBuilder();
            embed.Color = Color.DarkPurple;
            embed.WithTitle("Resellers");

            foreach (var reseller in resellers)
            {
                embed.AddField(reseller.Name, $"Balance: {reseller.Balance}\nKeys Generated: {reseller.KeysGenerated}");
                embed.WithTimestamp(DateTimeOffset.Now)
                    .WithImageUrl("https://media.discordapp.net/attachments/1123031318884786187/1165835065259991080/6RpFceJ.png?ex=65484b83&is=6535d683&hm=29eacb0233fd6806d470ca156e548ae292e5dd98ab69221c86afb1ed592fef1e&=&width=1103&height=621")
                    .WithFooter(footer =>
                    {
                        footer.Text = ".gg/cheatos | Vortech Auth";
                        footer.IconUrl = "https://media.discordapp.net/attachments/1123031318884786187/1165835249981337640/15.png?ex=65484baf&is=6535d6af&hm=ff6f89b26bd028e6ba7f393c4083cc6cb504e0198c7b7e1c33f5faf39cec043b&=";
                    })
                    .Build();
            }

            var original = await Context.Interaction.GetOriginalResponseAsync();

            await original.ModifyAsync(x =>
            {
                x.Content = "";
                x.Embed = embed.Build();
            });

            await Program.LogCommand($"Resellers list requested by {Context.User.Username}#{Context.User.Discriminator} ({Context.User.Id})");
        }
    }
}