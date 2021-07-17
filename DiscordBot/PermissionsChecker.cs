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

        public static bool CheckMessageForBotBan(SocketMessage message)
        {
            foreach (var uid in Program.settings.systemSettings.bannedUsers)
            {
                if (uid == message.Author.Id)
                    return true;
            }

            return false;
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

        public static bool CheckMessageForRole(SocketMessage message, string RoleName)
        {
            SocketGuildUser author = message.Author as SocketGuildUser;
            if (author != null) {
            
                foreach (var role in author.Roles)
                {
                    if (role.Name == RoleName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool IsMessageFromTrustedUser(SocketMessage message)
        {
            foreach (var uid in Program.settings.systemSettings.trustedUsers)
            {
                if (uid == message.Author.Id)
                    return true;
            }

            return false;
        }
    }
}
