using System.Collections.Generic;

namespace SCH.Host
{
    public interface IMyElasticClient
    {
        void IndexMessages(IEnumerable<Message> messages);
    }
}