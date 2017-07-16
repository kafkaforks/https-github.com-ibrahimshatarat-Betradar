using System;
using System.Threading;
using System.Threading.Tasks;
using BetService.Classes.DbInsert;
using log4net;
using log4net.Repository.Hierarchy;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace Betradar.Classes.Socket
{
    public abstract class LiveOddsCommonModule : LiveOddsCommonBaseModule
    {
        private readonly ILiveOddsCommon m_live_odds;

        protected LiveOddsCommonModule(ILiveOddsCommon live_odds, string feed_name, TimeSpan meta_interval)
            : base(live_odds, feed_name, meta_interval)
        {
            m_live_odds = live_odds;
            m_live_odds.OnAlive += AliveHandler;
        }

        protected virtual void AliveHandler(object sender, AliveEventArgs e)
        {
            var common = new Common();
            //ThreadStart action = () => {
            //                               common.WorkAlive(e);
            //};
            //var exe = new ThreadInterruptedException();
            //Thread threadAlive = new Thread(action) { IsBackground = true };
            //threadAlive.Start();
            

            Console.WriteLine(":::::::::::::::::::::: Header Count: {0} / Status: {1} ::::::::::::::::::::::", e.Alive.EventHeaders.Count, e.Alive.Status);
            foreach (var head in e.Alive.EventHeaders)
            {
                common.insertMatchDataAllDetails((MatchHeader)head, null);
            }
            
            
            
            //Task.Factory.StartNew(
            //    () =>
            //    {
            //        foreach (var head in e.Alive.EventHeaders)
            //        {
            //            common.insertMatchDataAllDetails((MatchHeader)head, null);
            //        }
            //        // common.WorkAlive(e);
            //    }
            //    , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

           
           

            //Task.Factory.StartNew(() => common.WorkAlive(e));
            //common.WorkAlive(e);
            // g_log.Info("{0}: Received alive with {1} events", m_feed_name, e.Alive.EventHeaders.Count);
        }
    }
}
