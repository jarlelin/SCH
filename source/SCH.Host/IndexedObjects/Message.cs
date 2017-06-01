using System;
using System.Diagnostics;
using SCH.Host.IndexedObjects.SlackObjects;

namespace SCH.Host
{
    public class Message
    {
        public string Id { get; set;  }
        public string Type { get; set; }
        public string User { get; set; }
        public string Channel { get; set; }
        public string Text { get; set; }
        public string Ts { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public Message()
        {
        }

        public Message(MessageSlack s, string channel)
        {
            Id = $"{channel}-{s.Ts}";
            Type = s.Type;
            User = s.User;
            Channel = channel;
            Text = s.Text;
            Ts = s.Ts;

            var ts = s.Ts.Split('.')[0];
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(ts));
            TimeStamp = timestamp;
        }
    }

}