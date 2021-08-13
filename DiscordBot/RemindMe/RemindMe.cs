using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.RemindMe
{
    public class RemindMe
    {
        public static bool ParseTime(string[] splitCommand, out TimeSpan time, out int lastTimeArgumentIndex)
        {
            TimeSpan totalTime = TimeSpan.Zero;
            lastTimeArgumentIndex = 0;

            for (int i = 1; i < splitCommand.Length - 2; i += 2)
            {
                TimeSpan tempSpan;
                if (!TryParseTimePart(splitCommand[i..(i + 1)], out tempSpan))
                {
                    lastTimeArgumentIndex = i - 1;
                    break;
                }

                totalTime += tempSpan;
            }

            time = totalTime;

            if (totalTime != TimeSpan.Zero)
                return true;

            return false;
        }

        private static bool TryParseTimePart(string[] commandPart, out TimeSpan value)
        {
            if (commandPart.Length != 2)
                throw new Exception("incorrect input argument! argument can only contain one identifier and one number");

            value = TimeSpan.Zero;

            double valuePart;

            if (!double.TryParse(commandPart[0], out valuePart))
                return false;

            switch (commandPart[1])
            {
                case "s":
                case "second":
                case "seconds":
                    value = TimeSpan.FromSeconds(valuePart);
                    return true;
                case "m":
                case "minute":
                case "minutes":
                    value = TimeSpan.FromMinutes(valuePart);
                    return true;
                case "h":
                case "hour":
                case "hours":
                    value = TimeSpan.FromHours(valuePart);
                    return true;
                case "d":
                case "day":
                case "days":
                    value = TimeSpan.FromDays(valuePart);
                    return true;
                case "w":
                case "week":
                case "weeks":
                    value = TimeSpan.FromDays(valuePart * 7);
                    return true;
                case "y":
                case "year":
                case "years":
                    value = TimeSpan.FromDays(valuePart * 365);
                    return true;
            }

            return false;
        }
    }
}
