using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCH.Host.IndexedObjects;
using SCH.Host.IndexedObjects.SlackObjects;

namespace SCH.Host
{
    public class Indexer
    {
        private readonly string _sourceDataSmartebokaSlack;
        private readonly IMyElasticClient _elasticClient;

        public Indexer(IMyElasticClient elasticClient, string sourceDataSmartebokaSlack)
        {
            _sourceDataSmartebokaSlack = sourceDataSmartebokaSlack;
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
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

        public Channel[] GetChannels(string fileName = "channels.json")
        {
            var objects = ReadArrayFromFile<ChannelSlack[]>(_sourceDataSmartebokaSlack, fileName);
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
            var objets = ReadArrayFromFile<MessageSlack[]>(_sourceDataSmartebokaSlack, filePath);
            var messages = objets.Select(o => new Message(o, channel));
            return messages;
            //using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            //using (var reader = new StreamReader(stream))
            //{
            //    var json = reader.ReadToEnd();
            //    Console.WriteLine(json);

            //    var jarray = JArray.Parse(json);
            //    return CreateMessages(jarray, channel);
            //}
        }

        //public static Message[] CreateMessages(JArray jarray, string channelName)
        //{
        //    var messages = jarray.Select(o => o.ToObject<Message>()).ToArray();
        //    foreach (var message in messages)
        //    {
        //        var ts = message.Ts.Split('.')[0];
        //        var timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(ts));
        //        message.TimeStamp = timestamp;
        //        message.Channel = channelName;
        //    }

        //    return messages;
        //}


        private T ReadArrayFromFile<T>(string sourceDataPath, string fileName)
        {
            var path = Path.Combine(sourceDataPath, fileName);
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                //Console.WriteLine(json);
                return JsonConvert.DeserializeObject<T>(json);
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
}