using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using SharedLibrary;
using Sportradar.SDK.Common.Interfaces;
using Sportradar.SDK.FeedProviders.Common;
using Sportradar.SDK.FeedProviders.LiveScout;

namespace Betradar.Classes.Socket
{
    public class LiveScoutModule : Core,IStartable
    {
        private readonly string m_feed_name;
        private readonly ILiveScout m_live_scout;
        private readonly Timer m_meta_timer;
        private readonly bool m_test;

        public LiveScoutModule(ILiveScout live_scout, string feed_name, bool test)
        {
            m_live_scout = live_scout;
            m_feed_name = feed_name;
            m_test = test;
            m_live_scout.OnOpened += OpenedHandler;
            m_live_scout.OnClosed += ClosedHandler;
            m_live_scout.OnLineups += LineupsHandler;
            m_live_scout.OnLineups += LineupsHandler;
            m_live_scout.OnMatchBookingReply += MatchBookingReplyHandler;
            m_live_scout.OnMatchData += MatchDataHandler;
            m_live_scout.OnMatchList += MatchListHandler;
            m_live_scout.OnMatchListUpdate += MatchListUpdateHandler;
            m_live_scout.OnMatchStop += MatchStopHandler;
            m_live_scout.OnMatchUpdate += MatchUpdateHandler;
            m_live_scout.OnMatchUpdateDelta += MatchUpdateDeltaHandler;
            m_live_scout.OnMatchUpdateDeltaUpdate += MatchUpdateDeltaUpdateHandler;
            m_live_scout.OnMatchUpdateFull += MatchUpdateFullHandler;
            m_live_scout.OnOddsSuggestion += OddsSuggestionHandler;
            m_live_scout.OnScoutInfo += ScoutInfoHandler;
            m_live_scout.OnFeedError += FeedErrorHandler;
            m_meta_timer = new Timer(TimeSpan.FromHours(2).TotalMilliseconds);
            m_meta_timer.Elapsed += (sender, args) => m_live_scout.GetMatchList(0, 3);
        }

        public void Start()
        {
            Logg.logger.Info("{0}: Starting", m_feed_name);
            m_live_scout.Start();
            m_meta_timer.Start();
            m_live_scout.GetMatchList(6, 2);
        }

        public void Stop()
        {
            Logg.logger.Info("{0}: Stopping", m_feed_name);
            m_meta_timer.Stop();
            m_live_scout.Stop();
        }

        private void ClosedHandler(object sender, ConnectionChangeEventArgs e)
        {
            Logg.logger.Info("LiveScout feed disconnected");
        }

        private void FeedErrorHandler(object sender, FeedErrorEventArgs e)
        {
            Logg.logger.Warn("{0}: Received FeedError with {1} severity", m_feed_name, e.Severity);
            if (e.Severity == ErrorSeverity.CRITICAL)
            {
                Stop();
            }
        }

        private void LineupsHandler(object sender, LineupsEventArgs e)
        {
            //Task.Factory.StartNew(() => new LiveScout_Lineup_Handle(e));
//#if DEBUG
//            InCount += 1;
//            Logg.logger.Warn("InCount Count = " + InCount);
//#endif
            Logg.logger.Info("{0}: Received Lineups for match {1} with {2} players", m_feed_name, e.Lineups.MatchId, e.Lineups.Players.Count);

        }

        private void MatchBookingReplyHandler(object sender, MatchBookingReplyEventArgs e)
        {
           // Task.Factory.StartNew(() => new LiveScout_MatchBooking_Handle(e));
#if DEBUG
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
            Logg.logger.Info("{0}: Received MatchBookingReply for match {1} with {2} result", m_feed_name, e.MatchBooking.MatchId, e.MatchBooking.Result);
        }

        private void MatchDataHandler(object sender, MatchDataEventArgs e)
        {
            Logg.logger.Info("{0}: Received MatchData for match {1}", m_feed_name, e.MatchData.MatchId);
           // Task.Factory.StartNew(() => new LiveScout_MatchData_Handle(e));
#if DEBUG
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        private void MatchListHandler(object sender, MatchListEventArgs e)
        {
            Logg.logger.Info("{0}: Received MatchList with {1} matches", m_feed_name, e.MatchList.Length);
            //string xml = Globals.Serialization.XmlSerialize(e);
            Subscribe(e);
           // string json = JsonConvert.SerializeObject(e.MatchList);
           // string json2 = JsonConvert.SerializeObject(e.WasRequested);
        }

        private void MatchListUpdateHandler(object sender, MatchListEventArgs e)
        {
            Logg.logger.Info("{0}: Received MatchListUpdate with {1} matches", m_feed_name, e.MatchList.Length);
           // string xml = Globals.Serialization.XmlSerialize(e);
            //Globals.Queue_Feed.Enqueue(xml);
            Subscribe(e);
           // string json = JsonConvert.SerializeObject(e.MatchList);
           // string json2 = JsonConvert.SerializeObject(e.WasRequested);
        }

        private void MatchStopHandler(object sender, MatchStopEventArgs e)
        {
            Logg.logger.Info("{0}: Received MatchStop for match {1} for reason {2}", m_feed_name, e.MatchId, e.Reason);
            //string xml = Globals.Serialization.XmlSerialize(e);
            //Globals.Queue_Feed.Enqueue(xml);
           // string json = JsonConvert.SerializeObject(e.MatchId);
           // string json2 = JsonConvert.SerializeObject(e.Reason);
        }

        private void MatchUpdateDeltaHandler(object sender, MatchUpdateEventArgs e)
        {
            Logg.logger.Info("{0}: Received MatchUpdateDelta for match {1}", m_feed_name, e.MatchUpdate.MatchHeader.MatchId);
           // string xml = Globals.Serialization.XmlSerialize(e.MatchUpdate);
            //Globals.Queue_Feed.Enqueue(xml);
           // string json = JsonConvert.SerializeObject(e.MatchUpdate);
        }

        private void MatchUpdateDeltaUpdateHandler(object sender, MatchUpdateEventArgs e)
        {
            Logg.logger.Info("{0}: Received MatchUpdateDeltaUpdate for match {1}", m_feed_name, e.MatchUpdate.MatchHeader.MatchId);
           // string xml = Globals.Serialization.XmlSerialize(e.MatchUpdate);
            //Globals.Queue_Feed.Enqueue(xml);
            string json = JsonConvert.SerializeObject(e.MatchUpdate);
        }

        private void MatchUpdateFullHandler(object sender, MatchUpdateEventArgs e)
        {
            Logg.logger.Info("{0}: Received MatchUpdateFull for match {1}", m_feed_name, e.MatchUpdate.MatchHeader.MatchId);
           // string xml = Globals.Serialization.XmlSerialize(e.MatchUpdate);
            //Globals.Queue_Feed.Enqueue(xml);
          //  string json = JsonConvert.SerializeObject(e.MatchUpdate);
        }

        private void MatchUpdateHandler(object sender, MatchUpdateEventArgs e)
        {
            Logg.logger.Info("{0}: Received MatchUpdate for match {1}", m_feed_name, e.MatchUpdate.MatchHeader.MatchId);
            string xml = Globals.Serialization.XmlSerialize(e.MatchUpdate);
            //Globals.Queue_Feed.Enqueue(xml);
            string json = JsonConvert.SerializeObject(e.MatchUpdate);
        }

        private void OddsSuggestionHandler(object sender, OddsSuggestionEventArgs e)
        {
            Logg.logger.Info("{0}: Received OddsSuggestion for match {1} with {2} odds", m_feed_name, e.MatchId, e.Odds.Length);
           // string xml = Globals.Serialization.XmlSerialize(e);
            //Globals.Queue_Feed.Enqueue(xml);
           // string json = JsonConvert.SerializeObject(e.MatchId);
           // string json2 = JsonConvert.SerializeObject(e.Odds);
        }

        private void OpenedHandler(object sender, ConnectionChangeEventArgs connection_change_event_args)
        {
            Logg.logger.Info("LiveScout feed connected");
           // string json = JsonConvert.SerializeObject(connection_change_event_args.LocalTimestamp);
        }
        private void ScoutInfoHandler(object sender, ScoutInfoEventArgs e)
        {
            Logg.logger.Info("{0}: Received ScoutInfo for match {1} with {2} infos", m_feed_name, e.MatchId, e.ScoutInfos.Length);
           // string xml = Globals.Serialization.XmlSerialize(e);
            //Globals.Queue_Feed.Enqueue(xml);
           // string json = JsonConvert.SerializeObject(e.MatchId);
           // string json2 = JsonConvert.SerializeObject(e.ScoutInfos);
        }

        private void Subscribe(MatchListEventArgs e)
        {
            var to_subscribe = e.MatchList
                .Where(x => x.MatchHeader.IsBooked == true || x.MatchHeader.IsBooked == null)
                .Select(x => x.MatchHeader.MatchId)
                .ToList();
            string xml = Globals.Serialization.XmlSerialize(e);
            //Globals.Queue_Feed.Enqueue(xml);
            Logg.logger.Info("{0}: Subscribing to {1} events", m_feed_name, to_subscribe.Count);
            if (m_test)
            {
                Logg.logger.Info("Test subscribing to {0} events", to_subscribe.Count);
                foreach (long id in to_subscribe)
                {
                    m_live_scout.SubscribeTest(id);
                }
            }
            else
            {
                Logg.logger.Info("Subscribing to {0} events", to_subscribe.Count);
                //Max 100 events in single request
                const int MAX_COUNT = 100;
                while (to_subscribe.Any())
                {
                    m_live_scout.Subscribe(to_subscribe.Take(MAX_COUNT));
                    to_subscribe = to_subscribe.Skip(MAX_COUNT).ToList();
                }
            }
        }
    }
}