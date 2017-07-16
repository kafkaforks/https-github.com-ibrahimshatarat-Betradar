using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Betradar.Classes.DbInsert;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace Betradar.Classes.DbInsert
{
    
    public class BetCancelHandle : Core,IDisposable
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
                // common.insertMatchDataAllDetails((MatchHeader)args.BetStop.EventHeader, null);
                //Task.Factory.StartNew(() => common.insertMatchDataAllDetails((MatchHeader)args.BetCancel.EventHeader, null));
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
                //Task.Factory.StartNew(() => common.insertMatchDataAllDetails((MatchHeader)args.BetCancel.EventHeader, null));
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
                foreach (var odd in entity.Odds)
                {
                    foreach (var field in odd.OddsFields)
                    {
                        var val = field.Value;
                        bool active;

                        if (odd.Active != null)
                        {
                            active = odd.Active;
                        }
                        else
                        {
                            active = val.Active;
                        }
                        common.insertLiveOdds(odd,val, active, val.Outcome, val.PlayerId,
                            val.Probability.ToString() ?? "", val.Type, val.Value.ToString() ?? "", val.ViewIndex,
                            val.VoidFactor.ToString() ?? "", field.Key, entity.EventHeader.Id, val.TypeId ?? 0);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
