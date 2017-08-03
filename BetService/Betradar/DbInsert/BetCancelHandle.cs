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

        public async Task BetCancelHandler(BetCancelEventArgs args)
        {
            await RunTask(args);
        }

        private async Task RunTask(BetCancelEventArgs args)
        {

            var common = new Common();
            var queue = new Queue<Globals.Rollback>();
            try
            {
                if (args.BetCancel.Odds != null && args.BetCancel.Odds.Count > 0)
                {
                    await insertOdds(args);
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
            try
            {
                await common.insertMatchDataAllDetails((MatchHeader)args.BetCancel.EventHeader, null);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }


        }

        private async Task insertOdds(BetCancelEventArgs args)
        {
            var common = new Common();
            try
            {
               // var entity = args.BetCancel;
                bool active = true;
                foreach(var odd in args.BetCancel.Odds)
                    {
                    if (odd.Active != null)
                    {
                        active = odd.Active;
                    }

                    await common.insertLiveOdds(odd, null, active, null, null,
                        "", null, "", null,
                        "", null, args.BetCancel.EventHeader.Id, 0, args.BetCancel.Status.ToString(), args.BetCancel.Timestamp.ToString());

                    foreach (var field in odd.OddsFields)
                    {
                        var oddUnique = new BetClearQueueElementLive();
                        oddUnique.MatchId = args.BetCancel.EventHeader.Id;
                        oddUnique.OddId = odd.Id;
                        if (odd.TypeId != null)
                        {
                            oddUnique.TypeId = int.Parse(odd.TypeId.ToString());
                        }
                        else
                        {
                            oddUnique.TypeId = null;
                        }
                        try
                        {
                            // sendToRpc(EncodeUnifiedBetClearQueueElementLive(oddUnique));
                            // Task.Factory.StartNew(() => sendToRpc(EncodeUnifiedBetClearQueueElementLive(oddUnique)));

                            var coupon = new Coupons();
                            await coupon.BetCancelDB(await EncodeUnifiedBetClearQueueElementLive(oddUnique));

                        }
                        catch (Exception ex)
                        {
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
    }
}
