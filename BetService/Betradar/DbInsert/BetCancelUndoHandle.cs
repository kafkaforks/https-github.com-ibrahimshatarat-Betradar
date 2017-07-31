using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace BetService.Classes.DbInsert
{
    class BetCancelUndoHandle : Core
    {
        public BetCancelUndoHandle(BetCancelUndoEventArgs args)
        {
            RunTask(args);
        }
        public void RunTask(BetCancelUndoEventArgs args)
        {
            var common = new Common();
            //var queue = new Queue<Globals.Rollback>();
            try
            {
                Task.Factory.StartNew(() => insertOdds(args));
                Task.Factory.StartNew(() => common.insertMatchDataAllDetails((MatchHeader)args.BetCancelUndo.EventHeader, null));
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }

        }
        private void insertOdds(BetCancelUndoEventArgs args)
        {
            
            var common = new Common();
            try
            {
                var entity = args.BetCancelUndo;
                bool active = true;
                foreach (var odd in entity.Odds)
                {
                    if (odd.Active != null)
                    {
                        active = odd.Active;
                    }

                    common.insertLiveOdds(odd, null, active, null, null,
                            "", null, "", null,
                            "", null, entity.EventHeader.Id, 0, entity.Status.ToString(), entity.Timestamp.ToString());

                }
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }
    }
}
