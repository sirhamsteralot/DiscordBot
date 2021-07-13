using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot
{
    public class PermissionsChecker
    {
        public static bool IsSentByDiscordAdministrator(SocketMessage message)
        {
            return CheckMessageForGuildPerm(message, Discord.GuildPermission.Administrator);
        }

        public static bool CheckMessageForGuildPerm(SocketMessage message, Discord.GuildPermission permission)
        {
            SocketGuildUser author = message.Author as SocketGuildUser;
            if (author != null)
            {
                if (author.GuildPermissions.Has(permission))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
