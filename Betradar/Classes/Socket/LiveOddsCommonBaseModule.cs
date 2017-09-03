using System;
using System.Threading;
using System.Threading.Tasks;
using Betradar.Classes.DbInsert;
using NLog;
using SharedLibrary;
using Sportradar.SDK.Common.Interfaces;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
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
            m_meta_timer.Elapsed += (sender, args) => MakeMetaRequest(TimeSpan.Zero, meta_interval);
        }

        public void Start()
        {
            m_live_odds.Start();
        }

        public void Stop()
        {
            m_live_odds.Stop();
        }

        protected virtual void BetCancelHandler(object sender, BetCancelEventArgs e)
        {
            g_log.Info("{0}: Received BetCancel for event {1} and odds id {2}", m_feed_name, e.BetCancel.EventHeader.Id, e.BetCancel.Odds[0].Id);

            Task.Factory.StartNew(
           () =>
           {
               new BetCancelHandle(e);
           }
           , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

            //Task.Factory.StartNew(() => new BetCancelHandle(e));
#if DEBUG
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual void BetCancelUndoHandler(object sender, BetCancelUndoEventArgs e)
        {
            g_log.Info("{0}: Received BetCancelUndo for event {1} and odds id {2}", m_feed_name,
                e.BetCancelUndo.EventHeader.Id, e.BetCancelUndo.Odds[0].Id);


            Task.Factory.StartNew(
                () =>
                {
                    new BetCancelUndoHandle(e);
                }
                , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);


            // Task.Factory.StartNew(() => new BetCancelUndoHandle(e));
#if DEBUG
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual void BetClearHandler(object sender, BetClearEventArgs e)
        {
            g_log.Info("{0}: Received BetClear for event {1} and odds id {2}", m_feed_name, e.BetClear.EventHeader.Id, e.BetClear.Odds[0].Id);
            // Task.Factory.StartNew(() => new BetClearHandle(e));

            Task.Factory.StartNew(
               () =>
               {
                   new BetClearHandle(e);
               }
               , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

            //var bet = new BetClearQueueElement();
            //foreach (var odd in e.BetClear.Odds)
            //{
            //    bet.MatchId = e.BetClear.EventHeader.Id;
            //    bet.OddsId = odd.Id;
            //    //RedisQueue.BetClear_Enqueue(bet);
            //    //TODO: add backup queue here!!!!
            //}

#if DEBUG
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual void BetClearRollbackHandler(object sender, BetClearRollbackEventArgs e)
        {
            g_log.Info("{0}: Received BetClear for event {1} and odds id {2}", m_feed_name, e.BetClearRollback.EventHeader.Id, e.BetClearRollback.Odds[0].Id);

            Task.Factory.StartNew(
             () =>
             {
                 new BetClearRollBackHandle(e);
             }
             , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
           
            //Task.Factory.StartNew(() => new BetClearRollBackHandle(e));
#if DEBUG
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual void BetStartHandler(object sender, BetStartEventArgs e)
        {
            g_log.Info("{0}: Received BetStart for event {1}", m_feed_name, e.BetStart.EventHeader.Id);
            Task.Factory.StartNew(
                  () =>
                  {
                      new BetStartHandle(e);
                  }
                  , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
          
            //Task.Factory.StartNew(() => new BetStartHandle(e));
#if DEBUG
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual void BetStopHandler(object sender, BetStopEventArgs e)
        {
            g_log.Info("{0}: Received BetStart for event {1}", m_feed_name, e.BetStop.EventHeader.Id);

            Task.Factory.StartNew(
               () =>
               {
                   new BetStopHandle(e);
               }
               , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

    

            // Task.Factory.StartNew(() => new BetStopHandle(e));
#if DEBUG
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual void ConnectionStableHandler(object sender, EventArgs e)
        {
            g_log.Info("{0} connection is stable. It is now safe to accept bets and make requests", m_feed_name);
            TimeSpan half = TimeSpan.FromMilliseconds(m_meta_timer.Interval);
            MakeMetaRequest(half, half);
            m_meta_timer.Start();
        }

        protected virtual void ConnectionUnstableHandler(object sender, EventArgs e)
        {
            g_log.Info("{0} connection is unstable. Don't accept any bets or call any requests", m_feed_name);
            m_meta_timer.Stop();
        }

        protected virtual void EventMessagesHandler(object sender, EventDataReceivedEventArgs e)
        {
            g_log.Info("{0}: Received {1} of error reply messages with reply number {2}", m_feed_name, e.Messages.Count, e.ReplyNr);

        }

        protected virtual void EventStatusHandler(object sender, EventDataReceivedEventArgs e)
        {
            g_log.Info("{0}: Received {1} of current reply messages with reply number {2}", m_feed_name, e.Messages.Count, e.ReplyNr);
        }

        protected virtual void OddsChangeHandler(object sender, OddsChangeEventArgs e)
        {
           
            g_log.Info("{0}: Received OddsChange for event {1} with {2} odds", m_feed_name, e.OddsChange.EventHeader.Id, e.OddsChange.Odds.Count);
            //var o_change = new OddsChangeHandle(e);

            Task.Factory.StartNew(
               () =>
               {
                   new OddsChangeHandle(e);
               }
               , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

            // Task.Factory.StartNew(() => new OddsChangeHandle(e));
#if DEBUG
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        protected virtual void MakeMetaRequest(TimeSpan back, TimeSpan forward)
        {
            DateTime now = DateTime.Now;
            var sofo = m_live_odds.GetEventList(now.Subtract(back), now.Add(forward));
        }
    }

    internal interface ILog
    {
    }
}
