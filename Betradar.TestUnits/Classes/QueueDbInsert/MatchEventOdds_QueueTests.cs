using System.IO;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sportradar.SDK.FeedProviders.Lcoo;

namespace Betradar.TestUnits.Classes.QueueDbInsert
{
    [TestClass()]
    public class MatchEventOdds_QueueTests
    {
        [TestMethod()]
        public void insertTextsTest()
        {

        }

        [TestMethod()]
        public void insertCardsTest()
        {
            var card = new CardsEntity();
            card.Cards.Capacity = 10;
            
            Assert.Fail();
        }

        [TestMethod()]
        public void WatchQueueMatchesTest()
        {
            using (StreamReader r = new StreamReader(@"C:\DB_Backup\objects\match.json"))
            {
              
                    string json = r.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                dynamic jsonObject = serializer.Deserialize<MatchEventOdds>(json);

                //MatchEventOdds account = JsonConvert.DeserializeObject<MatchEventOdds>(json);
                



            }
            Assert.Fail();
        }

        //private TextEntity FillEntity()
        //{
        //   var obj  = Object
        //}
    }
}