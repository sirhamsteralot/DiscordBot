using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Dice
{
    public class DiceCommands : ICommandGroup
    {
        Random random = new Random();
        StringBuilder sb = new StringBuilder();
        List<int> diceRolls = new List<int>();

        public void RegisterCommands(CommandManager commandManager)
        {
            commandManager.AddCommand("roll", DiceRoll, "rolls a dice, usage: roll {d4/d6/d8/d10/d12/d20/d100} *OR* roll {amount} {d4/d6/d8/d10/d12/d20/d100}");
        }

        private async Task DiceRoll(SocketMessage arg)
        {
            string[] split = arg.Content.Split(' ');

            if (split.Length < 2)
                await arg.Channel.SendMessageAsync("not enough arguments, dice type expected!");

            string diceType = split[1];
            int diceCount = 1;

            if (split.Length > 2)
            {
                diceType = split[2];
                if (!int.TryParse(split[1], out diceCount))
                {
                    await arg.Channel.SendMessageAsync("invalid count argument!");
                    return;
                }
                
            }

            sb.Append("Rolling ").Append(diceCount).Append(" ").AppendLine(diceType);
            

            switch (diceType)
            {
                case "d2":
                    RollDice(2, diceCount, diceRolls);
                    break;
                case "d4":
                    RollDice(4, diceCount, diceRolls);
                    break;
                case "d6":
                    RollDice(6, diceCount, diceRolls);
                    break;
                case "d8":
                    RollDice(8, diceCount, diceRolls);
                    break;
                case "d10":
                    RollDice(10, diceCount, diceRolls);
                    break;
                case "d12":
                    RollDice(12, diceCount, diceRolls);
                    break;
                case "d20":
                    RollDice(20, diceCount, diceRolls);
                    break;
                case "d100":
                    RollDice(100, diceCount, diceRolls);
                    break;
                default:
                    sb.Clear();
                    await arg.Channel.SendMessageAsync("invalid dice Type!");
                    return;
            }

            sb.AppendLine("```");

            foreach (var roll in diceRolls)
            {
                sb.Append("roll: ").AppendLine(roll.ToString());
            }

            sb.AppendLine("```");

            await arg.Channel.SendMessageAsync(sb.ToString());
            sb.Clear();
        }

        private void RollDice(int maxVal, int diceCount, List<int> rolls)
        {
            rolls.Clear();
            for (int i = 0; i < diceCount; i++)
            {
                rolls.Add(random.Next(1, maxVal + 1));
            }
        }
    }
}
