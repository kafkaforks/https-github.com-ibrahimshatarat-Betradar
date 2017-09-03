using System.Collections.Generic;
using Sportradar.SDK.Common.Interfaces;
using Sportradar.SDK.FeedProviders.OddsCreator;

namespace Betradar.Classes.Socket
{
    public class OddsCreatorModule : IStartable
    {
        private readonly string m_feed_name;
        private readonly IOddsCreator m_odds_creator;

        public OddsCreatorModule(IOddsCreator odds_creator, string feed_name)
        {
            m_odds_creator = odds_creator;
            m_feed_name = feed_name;
        }

        public void Start()
        {
            IList<IdName> sports = m_odds_creator.GetSports();
            Logg.logger.Info("{0}: Listing all sports", m_feed_name);
            foreach (IdName sport in sports)
            {
                Logg.logger.Info("{0}: Sport {1} has id {2}", m_feed_name, sport.Name, sport.Id);
            }
        }

        public void Stop()
        {
        }
    }
}