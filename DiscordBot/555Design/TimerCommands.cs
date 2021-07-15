using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot._555Design
{
    public class TimerCommands : ICommandGroup
    {
        StringBuilder sb = new StringBuilder();

        public void RegisterCommands(CommandManager manager)
        {
            manager.AddCommand("555astable", AstableCommand, "calculates 555 timer properties, usage: 555astable *R1* *R2* *C1*");
        }

        public async Task AstableCommand(SocketMessage message)
        {
            double R1, R2, C1;
            double TimeH, TimeL, TimeP, Freq, DutyCycle;

            string[] split = message.Content.Split(' ');

            if (split.Length < 4)
            {
                await message.Channel.SendMessageAsync("Not enough arguments!");
                return;
            }


            if (!double.TryParse(split[1], out R1))
                await message.Channel.SendMessageAsync("Could not parse R1!");

            if (!double.TryParse(split[2], out R2))
                await message.Channel.SendMessageAsync("Could not parse R2");

            if (!double.TryParse(split[3], out C1))
                await message.Channel.SendMessageAsync("Could not parse C1");

            C1 *= 0.000000001;

            TimeH = 0.693 * (R1 + R2) * C1;
            TimeL = 0.693 * R2 * C1;
            TimeP = 0.693 * (R1 + 2*R2) * C1;
            Freq = 1.44 / ((R1 + 2 * R2) * C1);
            DutyCycle = (TimeH / TimeP)*100;

            sb.AppendLine("Calculation variables: ");
            sb.Append("R1: ").Append(R1).AppendLine(" Ohm");
            sb.Append("R2: ").Append(R2).AppendLine(" Ohm"); 
            sb.Append("C1: ");
            AppendNumberFormatted(sb, C1, "F");

            sb.AppendLine();
            sb.AppendLine("Results: ");

            sb.Append("Time High: ");
            AppendNumberFormatted(sb, TimeH, "S");

            sb.Append("Time Low: ");
            AppendNumberFormatted(sb, TimeL, "S");

            sb.Append("Time Period: ");
            AppendNumberFormatted(sb, TimeP, "S");

            sb.Append("Freq: ").Append($"{Freq:0.##}").AppendLine(" Hz");
            sb.Append("DutyCycle: ").Append($"{DutyCycle:0.##}").AppendLine("%");

            await message.Channel.SendMessageAsync(sb.ToString());
        }

        private void AppendNumberFormatted(StringBuilder sb, double value, string Unit)
        {
            if (value > 1)
            {
                sb.Append($"{value:0.###}").Append(" ").AppendLine(Unit);

            }
            else if (value > 0.001)
            {
                sb.Append($"{value * 1000:0.###}").Append(" m").AppendLine(Unit);
            }
            else
            {
                sb.Append($"{value * 1000000:0.###}").Append(" u").AppendLine(Unit);
            }
        }
    }
}
