using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SCH.Host;

namespace SCH.UnitTests
{
    [TestClass, TestCategory("Unit")]
    public class UserTests
    {
        [TestMethod]
        public void ReadUsers()
        {
            var indexer = new Indexer(new Mock<IMyElasticClient>().Object,  "Resources/");

            var users = indexer.GetUsers();
            Assert.AreEqual(2, users.Length);
        }


        [TestMethod]
        public void ParseAllAttributes()
        {
            var indexer = new Indexer(new Mock<IMyElasticClient>().Object, "Resources/");

            var users = indexer.GetUsers();

            var u = users.First();
            Assert.AreEqual("U0F6DHYCV", u.Id);
            Assert.AreEqual("ef", u.Name);
            Assert.AreEqual("T0EC3DG3A", u.TeamId);

            Assert.AreEqual("5b89d5", u.Color);
            Assert.AreEqual("Even Krogsveen", u.RealName);
            Assert.AreEqual("Europe/Amsterdam", u.Tz);
            Assert.AreEqual(3600, u.TzOffset);
            Assert.AreEqual(false, u.IsBot);
        }


    }
}
