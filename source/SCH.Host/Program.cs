using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Moq;
using Nest;
using Newtonsoft.Json.Linq;

namespace SCH.Host
{
    class Program
    {
        private static IMyElasticClient _elasticClient;
        private static string _indexName = "sch-messages";
        private static string _elasticUri= "http://localhost:9200";
        private const string slackDataPath = "F:/Source/Data/smarteboka Slack/";
        static void Main(string[] args)
        {
            _elasticClient = new MyElasticClient(_elasticUri, _indexName);
            //_elasticClient = new Mock<IMyElasticClient>().Object;
            var indexer = new Indexer(_elasticClient, slackDataPath);
            indexer.Index();

        }

        
    }
}