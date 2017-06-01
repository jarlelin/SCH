using System;
using System.Collections.Generic;
using Nest;

namespace SCH.Host
{
    public class MyElasticClient : IMyElasticClient
    {
        private readonly string _indexName;
        private ElasticClient _elasticClient;

        public MyElasticClient(string uri, string indexName)
        {
            _indexName = indexName;
            var settings = new ConnectionSettings(new Uri(uri) ){ };
            settings.DefaultIndex(indexName);
            settings.BasicAuthentication("elastic", "changeme");
            _elasticClient = new ElasticClient(settings);

        }

        public void IndexMessages(IEnumerable<Message> messages)
        {
            foreach (var m in messages)
            {
                var res = _elasticClient.Index(m, i => i.Index(_indexName));

                if (!res.IsValid)
                    throw res.OriginalException;
            }

        }
    }
}