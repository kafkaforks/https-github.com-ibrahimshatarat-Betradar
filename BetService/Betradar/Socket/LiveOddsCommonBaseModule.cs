using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using BetService.Classes.DbInsert;
using NLog;
using SharedLibrary;
using Sportradar.SDK.Common;
using Sportradar.SDK.Common.Interfaces;
using Sportradar.SDK.ConnectionProviders;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.Internal;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;
using Sportradar.SDK.ProtocolProviders;
using Sportradar.SDK.Services.Sdk;
using Sportradar.SDK.Services.SdkConfiguration;
using Sportradar.SDK.Services.SdkLogger;
using Timer = System.Timers.Timer;

namespace Betradar.Classes.Socket
{
    public abstract class LiveOddsCommonBaseModule : Core, IStartable
    {
        protected readonly string m_feed_name;
        private static readonly Logger g_log = LogManager.GetLogger(typeof(LiveOddsCommonModule).ToString());
        private readonly ILiveOddsCommonBase m_live_odds;
        private readonly Timer m_meta_timer;

        protected LiveOddsCommonBaseModule(ILiveOddsCommonBase live_odds, string feed_name, TimeSpan meta_interval)
        {
            m_feed_name = feed_name;
            m_live_odds = live_odds;
            m_live_odds.OnBetCancel += BetCancelHandler;
            m_live_odds.OnBetCancelUndo += BetCancelUndoHandler;
            m_live_odds.OnBetClear += BetClearHandler;
            m_live_odds.OnBetClearRollback += BetClearRollbackHandler;
            m_live_odds.OnBetStart += BetStartHandler;
            m_live_odds.OnBetStop += BetStopHandler;
            m_live_odds.OnOddsChange += OddsChangeHandler;
            m_live_odds.OnConnectionStable += ConnectionStableHandler;
            m_live_odds.OnConnectionUnstable += ConnectionUnstableHandler;
            m_live_odds.OnEventMessages += EventMessagesHandler;
            m_live_odds.OnEventStatus += EventStatusHandler;
            m_meta_timer = new Timer(meta_interval.TotalMilliseconds / 2.0);
            m_meta_timer.Elapsed += async (sender, args) => await MakeMetaRequest(TimeSpan.Zero, meta_interval);
        }

        public void Start()
        {
            m_live_odds.Start();
        }

        public void Stop()
        {
            m_live_odds.Stop();
        }

        protected virtual async void BetCancelHandler(object sender, BetCancelEventArgs e)
        {
            var r = new BetCancelHandle();
            await r.BetCancelHandler(e);
#if DEBUG
            g_log.Info("{0}: Received BetCancel for event {1} and odds id {2}", m_feed_name, e.BetCancel.EventHeader.Id, e.BetCancel.Odds[0].Id);
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual async void BetCancelUndoHandler(object sender, BetCancelUndoEventArgs e)
        {
            var r = new BetCancelUndoHandle();
            await r.BetCancelUndoHandler(e);
#if DEBUG
            g_log.Info("{0}: Received BetCancelUndo for event {1} and odds id {2}", m_feed_name,
               e.BetCancelUndo.EventHeader.Id, e.BetCancelUndo.Odds[0].Id);
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual async void BetClearHandler(object sender, BetClearEventArgs e)
        {
            var r = new BetClearHandle();
            await  r.BetClearHandler(e);
#if DEBUG
            g_log.Info("{0}: Received BetClear for event {1} and odds id {2}", m_feed_name, e.BetClear.EventHeader.Id, e.BetClear.Odds[0].Id);
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual async void BetClearRollbackHandler(object sender, BetClearRollbackEventArgs e)
        {
            var r = new BetClearRollBackHandle();
            await r.BetClearRollBackHandler(e);
#if DEBUG
            g_log.Info("{0}: Received BetClear for event {1} and odds id {2}", m_feed_name, e.BetClearRollback.EventHeader.Id, e.BetClearRollback.Odds[0].Id);
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual async void BetStartHandler(object sender, BetStartEventArgs e)
        {
            var r = new BetStartHandle();
            await  r.BetStartHandler(e);
#if DEBUG
            g_log.Info("{0}: Received BetStart for event {1}", m_feed_name, e.BetStart.EventHeader.Id);
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual async void BetStopHandler(object sender, BetStopEventArgs e)
        {
            var r = new BetStopHandle();
            await r.BetStopHandler(e);
#if DEBUG
            g_log.Info("{0}: Received BetStart for event {1}", m_feed_name, e.BetStop.EventHeader.Id);
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual async void ConnectionStableHandler(object sender, EventArgs e)
        {

            TimeSpan half = TimeSpan.FromMilliseconds(m_meta_timer.Interval);
            await  MakeMetaRequest(half, half);
            m_meta_timer.Start();
#if DEBUG
            g_log.Info("{0} connection is stable. It is now safe to accept bets and make requests", m_feed_name);
#endif
        }

        protected virtual void ConnectionUnstableHandler(object sender, EventArgs e)
        {
            
            m_meta_timer.Stop();
#if DEBUG
            g_log.Info("{0} connection is unstable. Don't accept any bets or call any requests", m_feed_name);
#endif
        }

        protected virtual void EventMessagesHandler(object sender, EventDataReceivedEventArgs e)
        {
#if DEBUG
            g_log.Info("{0}: Received {1} of error reply messages with reply number {2}", m_feed_name, e.Messages.Count, e.ReplyNr);
#endif

        }

        protected virtual void EventStatusHandler(object sender, EventDataReceivedEventArgs e)
        {
#if DEBUG
            g_log.Info("{0}: Received {1} of current reply messages with reply number {2}", m_feed_name, e.Messages.Count, e.ReplyNr);
#endif
        }

        protected virtual async void OddsChangeHandler(object sender, OddsChangeEventArgs e)
        {
            var r = new OddsChangeHandle();
            await r.OddsChangeHandler(e);
#if DEBUG
            g_log.Info("{0}: Received OddsChange for event {1} with {2} odds", m_feed_name, e.OddsChange.EventHeader.Id, e.OddsChange.Odds.Count);
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual async Task MakeMetaRequest(TimeSpan back, TimeSpan forward)
        {
            DateTime now = DateTime.Now;
            var sofo =  m_live_odds.GetEventList(now.Subtract(back), now.Add(forward));
        }

    }
}
