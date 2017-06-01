using System;
using SCH.Host.IndexedObjects.SlackObjects;

namespace SCH.Host.IndexedObjects
{
    public class Channel
    {
        public Channel()
        {
        }

        public Channel(ChannelSlack s)
        {
            this. Id = s.Id;
            this.Name = s.Name;
            Created = DateTimeOffset.FromUnixTimeSeconds(s.Created);
            Creator = s.Creator;
            IsArchived = s.Is_Archived;
            IsGeneral = s.Is_General;
            Members = s.Members;
            Topic = s.Topic.Value;
            Purpose = s.Purpose.Value;

        }

        public string Topic { get; set; }
        public string Purpose { get; set; }
        public string[] Members { get; set; }
        public bool IsGeneral { get; set; }
        public bool IsArchived { get; set; }
        public string Creator { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}