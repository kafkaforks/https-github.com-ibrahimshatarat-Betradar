using System;
using System.Threading;
using System.Threading.Tasks;
using BetService.Classes.DbInsert;
using SharedLibrary;
using Sportradar.SDK.Common.Interfaces;
using Sportradar.SDK.FeedProviders.Lcoo;

namespace Betradar.Classes.Socket
{
    public class LcooModule : Core, IStartable
    {
        private readonly string m_feed_name;
        private readonly ILcoo m_lcoo;
        private DateTime m_last_timestamp;

        public LcooModule(ILcoo lcoo, string feed_name)
        {
            m_feed_name = feed_name;
            m_lcoo = lcoo;
            m_lcoo.OnMatch += lcoo_OnMatch;
            m_lcoo.OnOutright += lcoo_OnOutright;
            m_lcoo.OnThreeBall += m_lcoo_OnThreeBall;
        }

        public void Start()
        {
            Logg.logger.Info("{0} Starting", m_feed_name);
            m_lcoo.Start();
        }

        public void Stop()
        {
            m_lcoo.Stop();
        }

        private void CheckIfOldData(DateTime? timestamp)
        {
            if (timestamp == null)
            {
                return;
            }
            DateTime time = (DateTime)timestamp;
            if (!time.Equals(m_last_timestamp))
            {
                m_last_timestamp = time;
                if (DateTime.UtcNow.Subtract(time) > TimeSpan.FromMinutes(30))
                {
                    //TODO: check the logic on the Ticket#: 201612021314004944
                    //Lets delete queue as data fetching for 60+ messages will take too long (max 6 per minute)
                    //Task.Factory.StartNew(() =>
                    //{
                    //    //LCoO HTTP has 10 second access limit
                    //    Thread.Sleep(TimeSpan.FromSeconds(10));
                    //    Logg.logger.Info("{0}: Deleting queue, make sure to call full update from betradar.com", m_feed_name);
                    //    m_lcoo.ClearQueue();
                    //});
                }
                else if (DateTime.UtcNow.Subtract(time) > TimeSpan.FromMinutes(5))
                {
                    Logg.logger.Warn("{0}: Received message with timestamp {1}, is it too old to accept bets?", m_feed_name, timestamp);
                }
            }
        }

        private void lcoo_OnMatch(object sender, MatchEventOdds e)
        {
            CheckIfOldData(e.MatchEntity.MessageTime);
            Logg.logger.Info("{0}: Received Match with id {1}", m_feed_name, e.MatchEntity.MatchId);

            Task.Factory.StartNew(
             () =>
             {
                 new MatchEventOddsHandle(e);
             }
             , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

            // Task.Factory.StartNew(() => new MatchEventOddsHandle(e));
#if DEBUG
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        private void lcoo_OnOutright(object sender, OutrightEventOdds e)
        {
            CheckIfOldData(e.OutrightEntity.MessageTime);
            Logg.logger.Info("{0}: Received Outright with id {1}", m_feed_name, e.OutrightEntity.Id);
            //Task.Factory.StartNew(() => new OutrightEventOddsHandle(e));
#if DEBUG
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        private void m_lcoo_OnThreeBall(object sender, ThreeBallEventArgs e)
        {
            CheckIfOldData(e.ThreeBallEntity.MessageTime);
            Logg.logger.Info("{0}: Received ThreeBall with id {1}", m_feed_name, e.ThreeBallEntity.Id);
           // Task.Factory.StartNew(() => new ThreeBallEventHandle(e));
#if DEBUG
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }
    }
}