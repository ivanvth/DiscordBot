using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    class TestCommands : BaseCommandModule
    {

        [Command("ping")]
        [Description("Replies Pong.")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel
                .SendMessageAsync("Pong")
                .ConfigureAwait(false);
        }

        [Command("add")]
        [Description("Adds two integers.")]
        public async Task Add(CommandContext ctx, 
            [Description("First integer.")] int a, 
            [Description("Second integer.")] int b)
        {
            var userMention = ctx.User.Mention;
            string s = String.Format("{0}: {1} + {2} = {3}", userMention, a, b, (a + b));
            await ctx.Channel
                .SendMessageAsync(s)
                .ConfigureAwait(false);
        }

        [Command("respondMsg")]
        [Description("None")]
        public async Task RespondMsg(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity
                .WaitForMessageAsync(x => x.Channel == ctx.Channel)
                .ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Content);

        }

        [Command("respondReaction")]
        [Description("None")]
        public async Task RespondReaction(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity
                .WaitForReactionAsync(x => x.Channel == ctx.Channel)
                .ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Emoji);

        }

        [Command("poll")]
        [Description("")]
        public async Task Poll(CommandContext ctx, TimeSpan duration, params DiscordEmoji[] emojiOptions)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var options = emojiOptions.Select(x => x.ToString());

            var pollEmbed = new DiscordEmbedBuilder
            {
                Title = "Poll",
                Description = string.Join(" ", options)
            };

           var pollMessage = await ctx.Channel.SendMessageAsync(embed: pollEmbed);

            foreach (DiscordEmoji option in emojiOptions)
            {
                await pollMessage.CreateReactionAsync(option);
            }

            var result = await interactivity.CollectReactionsAsync(pollMessage, duration);
            var distinctResult = result.Distinct();
            var results = distinctResult.Select(x => $"{x.Emoji}: {x.Total}");
            await ctx.Channel.SendMessageAsync(string.Join("\n", results));
        }
    }
}
