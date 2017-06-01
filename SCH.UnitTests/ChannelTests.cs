using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SCH.Host;

namespace SCH.UnitTests
{
    [TestClass]
    public class ChannelTests
    {
        [TestMethod]
        public void ReadChannels()
        {
            var indexer = new Indexer(new Mock<IMyElasticClient>().Object,  "Resources/");

            var channels = indexer.GetChannels("channels.txt");
            Assert.AreEqual(2, channels.Length);
        }

        [TestMethod]
        public void ParseAllAttributes()
        {
            var indexer = new Indexer(new Mock<IMyElasticClient>().Object, "Resources/");
            var channels = indexer.GetChannels( "channels.txt");

            var c = channels.First();
            Assert.AreEqual("C0EC2J5T9", c.Id);
            Assert.AreEqual("evens-yndlingskanal", c.Name);
            Assert.AreEqual(DateTimeOffset.Parse("12/11/2015 08:48:21 +00:00"), c.Created);
            Assert.AreEqual("U0EBWMGG4", c.Creator);
            Assert.AreEqual(false, c.IsArchived);
            Assert.AreEqual(false, c.IsGeneral);
            Assert.AreEqual("Non-work banter and water cooler conversation", c.Topic);
            Assert.AreEqual("om å kjøre en telemarksski opp i a nalen til Torgeir", c.Purpose);
        }
        

    }
}
