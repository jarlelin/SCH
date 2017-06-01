namespace SCH.Host.IndexedObjects.SlackObjects
{
    public class ChannelSlack
    {
        public bool Is_Archived;

        public string Name { get; set; }
        public string Id { get; set; }
        public long Created { get; set; }
        public string Creator { get; set; }
        public bool Is_General { get; set; }
        public string[] Members { get; set; }
        public ValueNode Topic { get; set; }
        public ValueNode Purpose { get; set; }
    }

    public class ValueNode
    {
        public string Value { get; set; }
    }
}