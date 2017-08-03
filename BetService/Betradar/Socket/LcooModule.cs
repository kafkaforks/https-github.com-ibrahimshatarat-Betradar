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
            m_lcoo.Start();
#if DEBUG
            Logg.logger.Info("{0} Starting", m_feed_name);
#endif
        }

        public void Stop()
        {
            m_lcoo.Stop();
        }

        private async Task CheckIfOldData(DateTime? timestamp)
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
#if DEBUG
                    Logg.logger.Warn("{0}: Received message with timestamp {1}, is it too old to accept bets?", m_feed_name, timestamp);
#endif
                }
            }
        }

        private async void lcoo_OnMatch(object sender, MatchEventOdds e)
        {
            await CheckIfOldData(e.MatchEntity.MessageTime);

            var r = new MatchEventOddsHandle();
            await r.MatchEventOddsHandler(e);
#if DEBUG
            Logg.logger.Info("{0}: Received Match with id {1}", m_feed_name, e.MatchEntity.MatchId);
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        private async void lcoo_OnOutright(object sender, OutrightEventOdds e)
        {
            await CheckIfOldData(e.OutrightEntity.MessageTime);
#if DEBUG
            Logg.logger.Info("{0}: Received Outright with id {1}", m_feed_name, e.OutrightEntity.Id);
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }

        private async void m_lcoo_OnThreeBall(object sender, ThreeBallEventArgs e)
        {
            await CheckIfOldData(e.ThreeBallEntity.MessageTime);

#if DEBUG
            Logg.logger.Info("{0}: Received ThreeBall with id {1}", m_feed_name, e.ThreeBallEntity.Id);
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
        }
    }
}