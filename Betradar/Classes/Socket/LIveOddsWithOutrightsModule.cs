using System;
using System.Threading.Tasks;
using Betradar.Classes.DbInsert;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.Outrights;
using OutrightBetCancelHandle = Betradar.Classes.DbInsert.OutrightBetCancelHandle;
using OutrightOddsChangeHandle = Betradar.Classes.DbInsert.OutrightOddsChangeHandle;

namespace Betradar.Classes.Socket
{
    public class LIveOddsWithOutrightsModule : LiveOddsCommonBaseModule
    {
        private readonly ILiveOddsWithOutrights m_live_odds;

        public LIveOddsWithOutrightsModule(ILiveOddsWithOutrights live_odds, string feed_name, TimeSpan meta_interval)
            : base(live_odds, feed_name, meta_interval)
        {
            m_live_odds = live_odds;
            m_live_odds.OnAliveWithOutrights += AliveWithOutrightsHandler;
            m_live_odds.OnMetaInfo += MetaInfoHandler;
            m_live_odds.OnOutrightBetStart += OutrightBetStartHandler;
            m_live_odds.OnOutrightOddsChange += OutrightOddsChangeHandler;
            m_live_odds.OnOutrightBetClear += OutrightBetClearHandler;
            m_live_odds.OnOutrightBetStop += OutrightBetStopHandler;
            m_live_odds.OnOutrightBetCancel += OutrightBetCancelHandler;
            m_live_odds.OnOutrightStatus += OutrightStatusHandler;
        }

        private void AliveWithOutrightsHandler(object sender, AliveWithOutrightsEventArgs e)
        {
            Logg.logger.Info("{0}: Received alive with {1} events and {2} outrights",
                m_feed_name,
                e.AliveWithOutrights.EventHeaders.Count,
                e.AliveWithOutrights.OutrightHeaders.Count);
        }

        private void MetaInfoHandler(object sender, MetaInfoEventArgs e)
        {
            var meta = e.MetaInfo.MetaInfoDataContainer as MatchAndOutrightMetaData;
            if (meta == null)
            {
                Logg.logger.Info("{0}: Unexpected type of MetaInfoDataContainer received. Expected:{1}, Received:{2}",
                    typeof(MatchAndOutrightMetaData).Name,
                    e.MetaInfo.MetaInfoDataContainer.GetType().Name);
                return;
            }
            Logg.logger.Info("{0}: Received MetaInfo with {1} matches and {2} outrights",
                m_feed_name,
                meta.MatchHeaderInfos == null
                    ? 0
                    : meta.MatchHeaderInfos.Count,
                meta.OutrightHeaderInfos == null
                    ? 0
                    : meta.OutrightHeaderInfos.Count);
        }

        private void OutrightBetStartHandler(object sender, OutrightBetStartEventArgs e)
        {
            //Task.Factory.StartNew(() => new OutrightBetStartHandle(e));
            Logg.logger.Info("{0}: Received BetStart for event {1}", m_feed_name, e.OutrightBetStart.OutrightHeader.Id);
        }

        private void OutrightOddsChangeHandler(object sender, OutrightOddsChangeEventArgs e)
        {
            Logg.logger.Info("{0}: Received OddsChange for outright {1} with {2} odds",
                m_feed_name,
                e.OutrightOddsChange.OutrightHeader.Id,
                e.OutrightOddsChange.Odds == null
                    ? 0
                    : e.OutrightOddsChange.Odds.Count);
           // Task.Factory.StartNew(() => new OutrightOddsChangeHandle(e));
        }

        private void OutrightBetClearHandler(object sender, OutrightBetClearEventArgs e)
        {
           // Task.Factory.StartNew(() => new OutrightBetClearHandle(e));
            Logg.logger.Info("{0}: Received BetClear for outright {1} and odds id {2}", m_feed_name, e.OutrightBetClear.OutrightHeader.Id, e.OutrightBetClear.Odds[0].Id);
        }

        private void OutrightBetStopHandler(object sender, OutrightBetStopEventArgs e)
        {
            //Task.Factory.StartNew(() => new OutrightBetStopHandle(e));
            Logg.logger.Info("{0}: Received BetStart for outright {1}", m_feed_name, e.OutrightBetStop.OutrightHeader.Id);
        }

        private void OutrightBetCancelHandler(object sender, OutrightBetCancelEventArgs e)
        {
            //Task.Factory.StartNew(() => new OutrightBetCancelHandle(e));
            Logg.logger.Info("{0}: Received BetCancel for outright {1} and odds id {2}", m_feed_name, e.OutrightBetCancel.OutrightHeader.Id, e.OutrightBetCancel.Odds[0].Id);
        }

        private void OutrightStatusHandler(object sender, EventDataReceivedEventArgs e)
        {
            Logg.logger.Info("{0}: Received {1} of current reply messages with reply number {2}", m_feed_name, e.Messages.Count, e.ReplyNr);
        }
    }
}
