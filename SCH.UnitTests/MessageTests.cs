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
    public class UnitTest1
    {
        [TestMethod]
        public void ReadChannels()
        {
            var indexer = new Indexer(new Mock<IMyElasticClient>().Object,  "Resources/");

            var channels = indexer.GetChannels("channels.txt");
            Assert.AreEqual(2, channels.Length);
        }

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
