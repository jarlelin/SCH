using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SCH.Host.IndexedObjects;
using SCH.Host.IndexedObjects.SlackObjects;

namespace SCH.Host
{
    public class Indexer
    {
        private readonly string _sourceDataSmartebokaSlack;
        private readonly IMyElasticClient _elasticClient;
        private JsonSerializerSettings _jsonSerializerSettings;

        public Indexer(IMyElasticClient elasticClient, string sourceDataSmartebokaSlack)
        {
            _sourceDataSmartebokaSlack = sourceDataSmartebokaSlack;
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));

            _jsonSerializerSettings = new JsonSerializerSettings(){};
            _jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            _jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;

        }


        public void Index()
        {
            Log("Reading channel list...");
            var channels = GetChannels();
            Log($"Found {channels.Length} channels. Beggining to index messages on channels.");
            foreach (var channel in channels)
            {
                IndexChannel(channel);
            }
        }
        public User[] GetUsers(string fileName = "users.json")
        {
            var usersFilePath = Path.Combine(_sourceDataSmartebokaSlack, fileName);

            var objects = ReadArrayFromFile<User[]>(usersFilePath);

            return objects;
        }

        public Channel[] GetChannels(string fileName = "channels.json")
        {
            var channelFilePath = Path.Combine(_sourceDataSmartebokaSlack, fileName);
            var objects = ReadArrayFromFile<ChannelSlack[]>(channelFilePath);
            var channels = objects.Select(c => new Channel(c)).ToArray();
            return channels;
        }

        public void IndexChannel(Channel channel)
        {
            var channelpath = Path.Combine(_sourceDataSmartebokaSlack, channel.Name);
            var files = Directory.GetFiles(channelpath);

            Log($"Indexing channel {channel.Name}. Found {files.Length} log files from that channel...");
            LogOnSameLine("=>");
            foreach (var file in files)
            {
                var messages = GetMessages(file, channel.Name);
                _elasticClient.IndexMessages(messages);
                LogOnSameLine("O");
            }
            Log("");
            Log($"Finished indexing messages from channel {channel.Name}.");
            Log("");
        }



        public IEnumerable<Message> GetMessages(string filePath, string channel)
        {
            var objets = ReadArrayFromFile<MessageSlack[]>(filePath);
            var messages = objets.Select(o => new Message(o, channel));
            return messages;
          
        }

        private T ReadArrayFromFile<T>(string fullFilePath)
        {
            using (var stream = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                //Console.WriteLine(json);

                return JsonConvert.DeserializeObject<T>(json, _jsonSerializerSettings);
            }
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
        }

        private void LogOnSameLine(string message)
        {
            Console.Write(message);
        }

    }

    public class User
    {
        [JsonProperty("team_id")]
        public string TeamId;
        [JsonProperty("real_name")]
        public string RealName;
        [JsonProperty("tz_offset")]
        public int TzOffset;
        [JsonProperty("is_bot")]
        public bool IsBot;
        public string Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Tz { get; set; }
    }
}