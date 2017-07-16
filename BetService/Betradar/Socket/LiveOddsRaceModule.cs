using System;
using Newtonsoft.Json;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.RaceLiveOdds.Common;

namespace Betradar.Classes.Socket
{
    public class LiveOddsRaceModule : LiveOddsCommonModule
    {
        private readonly ILiveOddsRace m_live_odds;

        public LiveOddsRaceModule(ILiveOddsRace live_odds, string feed_name, TimeSpan meta_interval)
            : base(live_odds, feed_name, meta_interval)
        {
            m_live_odds = live_odds;
            m_live_odds.OnRaceResult += RaceResultHandler;
            m_live_odds.OnMetaInfo += MetaInfoHandler;
        }

        private void MetaInfoHandler(object sender, MetaInfoEventArgs e)
        {
            var meta_data = e.MetaInfo.MetaInfoDataContainer as RaceMetaData;
            if (meta_data != null)
            {
               Logg.logger.Info("{0}: Received MetaInfo with {1} races and {2} racedays", m_feed_name, meta_data.RaceHeaderInfos.Count, meta_data.RaceDays.Count);
                string xml = Globals.Serialization.XmlSerialize(e.MetaInfo);
                //Globals.Queue_Feed.Enqueue(xml);
                string json = JsonConvert.SerializeObject(e.MetaInfo);
               
            }
        }

        private void RaceResultHandler(object sender, RaceResultEventArgs e)
        {
            Logg.logger.Info("{0}: Received RaceResult for race {1}", m_feed_name, e.RaceResult.EventHeader.Id);
        }
    }
}