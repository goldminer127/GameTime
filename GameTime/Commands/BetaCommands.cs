using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace GameTime.Commands
{
    class BetaCommands : BaseCommandModule
    {
        //Emoji testing
        [Command("test")]
        public async Task Test(CommandContext ctx, string emoji)
        {
            var message = await ctx.Channel.SendMessageAsync("\\" + emoji);
            var messageContentSplit = message.Content.Split(":");
            //message.DeleteAsync();
            var emojiName = messageContentSplit[1];
            var emojiId = messageContentSplit[^1].Substring(0, messageContentSplit[^1].Length - 1);
            var emojiUrl = "https:" + $"//cdn.discordapp.com/emojis/{emojiId}.png?v=1";
            var guildArchive = await Bot.Client.GetGuildAsync(637049983916310558);
            byte[] imageData = null;
            using (var wc = new WebClient())
                imageData = wc.DownloadData(emojiUrl);
            var e = await guildArchive.CreateEmojiAsync(emojiName, new MemoryStream(imageData));
            message = await ctx.Channel.SendMessageAsync("New emoji sucessfully created!");
            await message.CreateReactionAsync(e);
        }
    }
}
