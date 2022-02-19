using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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
        [Description("Replies pong.")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel
                .SendMessageAsync("pong")
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

        [Command("response")]
        [Description("None")]
        public async Task Response(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity
                .WaitForMessageAsync(x => x.Channel == ctx.Channel)
                .ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Content);

        }
    }
}
