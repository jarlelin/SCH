using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SCH.Host;
using SCH.Host.IndexedObjects;

namespace SCH.UnitTests
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void IndexChannel()
        {
            var client = new Mock<IMyElasticClient>();
            var indexer = new Indexer(client.Object, "Resources/");

            var channel = new Channel(){Name= "evens-yndlingskanal" };
             indexer.IndexChannel( channel);

            client.Verify(c=>c.IndexMessages(It.IsAny<IEnumerable<Message>>()),Times.AtLeastOnce);

           
        }
        

    }
}
