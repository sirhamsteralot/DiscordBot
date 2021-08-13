using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DiscordBot.RemindMe
{
    public class RemindMeItem
    {
        // ALL TIMES WILL BE UTC!

        public string reminder { get; set; }
        public ulong responseChannel { get; set; }

        public DateTime reminderTime { get { return _reminderTime; } set { _reminderTime = value; SetTimer(); } }

        private DateTime _reminderTime;

        private Timer timer;

        private void SetTimer()
        {
            timer = new Timer(TimerAction, this, (long)(DateTime.UtcNow - _reminderTime).TotalMilliseconds, Timeout.Infinite);
        }

        private void TimerAction(object state)
        {
            try
            {
                var channel = Program._client.GetChannel(responseChannel) as ISocketMessageChannel;
                channel.SendMessageAsync(reminder).GetAwaiter().GetResult();
            } catch (Exception) { // fucking NOM
            }
        }
    }
}
