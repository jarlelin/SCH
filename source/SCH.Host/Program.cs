using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Nest;
using Newtonsoft.Json.Linq;

namespace SCH.Host
{
    class Program
    {
        private static ElasticClient _elasticClient;
        private static string _indexName = "my-index-2";
        private const string slackDataPath = "F:/Source/Data/smarteboka Slack/";
        static void Main(string[] args)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200")) {};
            settings.DefaultIndex(_indexName);
            settings.BasicAuthentication("elastic", "changeme");
            _elasticClient = new ElasticClient(settings);


            var messages = GetMessages("teknologipreik/2017-01-11.json", "teknologipreik");

            IndexMessages(messages);
        }

        private static void IndexMessages(IEnumerable<Message> messages)
        {
            foreach (var m in messages)
            {
                var res = _elasticClient.Index(m, i=>i.Index(_indexName) );

                if (!res.IsValid)
                    throw res.OriginalException;
            }

        }

        private static IEnumerable<Message> GetMessages(string relativePath, string channelName)
        {
            var path = slackDataPath + relativePath;
            using(var stream = new FileStream(path,FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                Console.WriteLine(json);

                var jarray=  JArray.Parse(json);
                return CreateMessages(jarray, channelName);
            }
        }

        private static Message[] CreateMessages(JArray jarray, string channelName)
        {
            var messages = jarray.Select(o => o.ToObject<Message>()).ToArray();
            foreach (var message in messages)
            {
                var ts = message.Ts.Split('.')[0];
                var timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(ts));
                message.TimeStamp = timestamp;
                message.Channel = channelName;
            }

            return messages;
        }
    }

    internal class MessageTimeStamp
    {
        public DateTimeOffset TimeStamp { get; set; }
        public string Id { get; set; }
    }

    internal class Message
    {
        public string Id { get { return $"{Channel}-{Ts}"; } }
        public string Type { get; set; }
        public string User { get; set; }
        public string Channel { get; set; }
        public string Text { get; set; }
        public string Ts { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}