using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.Serialization;
using System.Net;
using Newtonsoft.Json.Linq;

namespace DiscordBot.CustomCommands
{
    public class CustomResponseCommands : ICommandGroup
    {
        StringBuilder sb = new StringBuilder();
        Random r = new Random();

        public void RegisterCommands(CommandManager manager)
        {
            manager.AddCommand("addresponse", AddResponseCommand, "adds a response, usage: addresponse *response_name* *response_content*");
            manager.AddCommand("removeresponse", RemoveResponseCommand, "removes a response, usage: removeresponse *response_name*");
            manager.AddCommand("listcustom", ListCustomCommandsCommand, "lists the custom commands");
            //manager.AddCommand("roni", RoniCommand, "try it"); fuck this lmao
        }

        public async Task RoniCommand(SocketMessage message)
        {
            string Url = "https://pixabay.com/api/?key=22956172-a492888d223e5d927df9dbbf6&q=bare+feet&safesearch=false&per_page=150";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

            request.Method = "Get";
            request.Timeout = 5000;

            using (var s = request.GetResponse().GetResponseStream())
            {
                using (var sr = new System.IO.StreamReader(s))
                {

                    var jsonObject = JObject.Parse(sr.ReadToEnd());
                    JArray items = (JArray)jsonObject["hits"];

                    JToken item;
                    do
                    {
                        item = items[r.Next(items.Count)]; 
                        
                    } while (item["tags"].ToString().Contains("baby") || item["tags"].ToString().Contains("infant"));

                    await message.Channel.SendMessageAsync(item["webformatURL"].ToString());
                }
            }
        }

        public async Task ListCustomCommandsCommand(SocketMessage message)
        {
            sb.AppendLine("custom responses:");

            var responseList = new List<CustomResponse>();
            responseList.AddRange(Program.settings.customResponses.responses);
            responseList.Sort((x, y) => x.Name.CompareTo(y.Name));

            foreach (var response in responseList)
            {
                sb.AppendLine(response.Name);
            }

            await message.Channel.SendMessageAsync(sb.ToString());

            sb.Clear();
        }

        public async Task AddResponseCommand(SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            string[] splitCommand = message.Content.Split(' ');

            if (splitCommand.Length < 3)
                await message.Channel.SendMessageAsync("Not enough arguments!");

            CustomResponse response = new CustomResponse();
            if (splitCommand[1] == "")
                await message.Channel.SendMessageAsync("command name is required!");

            response.Name = splitCommand[1];

            for (int i = 2; i < splitCommand.Length; i++) {
                sb.Append(splitCommand[i]).Append(' ');
            }

            response.Content = sb.ToString();
            sb.Clear();

            var settings = Program.settings.customResponses;
            settings.responses.Add(response);
            settings.RequiresSaving();
            Program.settings.SerializeAsync(false);
            await message.Channel.SendMessageAsync("Response Added!");
        }

        public async Task RemoveResponseCommand(SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            string[] splitCommand = message.Content.Split(' ');

            if (splitCommand.Length < 2)
                await message.Channel.SendMessageAsync("Not enough arguments!");

            CustomResponse toRemove = null;

            foreach (var response in Program.settings.customResponses.responses)
            {
                if (response.Name == splitCommand[1])
                {
                    toRemove = response;
                    break;
                }
            }

            if (toRemove != null)
            {
                Program.settings.customResponses.responses.RemoveWhere(x => x.Name == splitCommand[1]);
                await message.Channel.SendMessageAsync("Removed!");
                return;
            }

            await message.Channel.SendMessageAsync("Could not find message to remove!");
        }
    }
}
