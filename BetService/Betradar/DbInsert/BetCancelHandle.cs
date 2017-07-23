using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace BetService.Classes.DbInsert
{
    
    public class BetCancelHandle : Core
    {

        public BetCancelHandle(BetCancelEventArgs args)
        {
            RunTask(args);
        }

        private void RunTask(BetCancelEventArgs args)
        {
            
            var common = new Common();
            var queue = new Queue<Globals.Rollback>();
            try
            {
                 common.insertMatchDataAllDetails((MatchHeader)args.BetCancel.EventHeader, null);
                Task.Factory.StartNew(() => common.insertMatchDataAllDetails((MatchHeader)args.BetCancel.EventHeader, null));
                if (args.BetCancel.Odds != null && args.BetCancel.Odds.Count > 0)
                {
                    // Task.Factory.StartNew(() => insertOdds(args));

                    Task.Factory.StartNew(
                       () =>
                       {
                           insertOdds(args);
                       }
                       , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
            try
            {
                Task.Factory.StartNew(() => common.insertMatchDataAllDetails((MatchHeader)args.BetCancel.EventHeader, null));
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
            

        }

        private void insertOdds(BetCancelEventArgs args)
        {
            var common = new Common();
            try
            {
                var entity = args.BetCancel;
                bool active = true;
                foreach (var Odd in entity.Odds)
                {
                    if (Odd.Active != null)
                    {
                        active = Odd.Active;
                    }
                   
                    common.insertLiveOdds(Odd, null, active, null, null,
                            "", null,  "", null,
                            "", null, entity.EventHeader.Id,  0, entity.Status.ToString(), entity.Timestamp.ToString());

                    foreach (var field in Odd.OddsFields)
                    {
                        var val = field.Value;
                        var oddUnique = new BetClearQueueElementLive();
                        oddUnique.MatchId = entity.EventHeader.Id;
                        oddUnique.OddId = Odd.Id;
                        if (Odd.TypeId != null)
                        {
                            oddUnique.TypeId = int.Parse(Odd.TypeId.ToString());
                        }
                        else
                        {
                            oddUnique.TypeId = null;
                        }
                        try
                        {
                            // sendToRpc(EncodeUnifiedBetClearQueueElementLive(oddUnique));
                            // Task.Factory.StartNew(() => sendToRpc(EncodeUnifiedBetClearQueueElementLive(oddUnique)));
                            Task.Factory.StartNew(() =>
                            {
                                var coupon = new Coupons();
                                coupon.BetCancelDB(EncodeUnifiedBetClearQueueElementLive(oddUnique));
                            });
                        }
                        catch (Exception ex)
                        {
                            Globals.Queue_BetClearQueueElementLive.Enqueue(oddUnique);
                            SharedLibrary.Logg.logger.Fatal("SEND TO PROXY ERROR: " + ex.Message);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
