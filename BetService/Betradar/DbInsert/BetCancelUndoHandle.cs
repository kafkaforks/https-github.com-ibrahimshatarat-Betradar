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
        public async Task BetCancelUndoHandler(BetCancelUndoEventArgs args)
        {
            await RunTask(args);
        }
        public async Task RunTask(BetCancelUndoEventArgs args)
        {
            var common = new Common();
            try
            {
                 await insertOdds(args);
                 await common.insertMatchDataAllDetails((MatchHeader)args.BetCancelUndo.EventHeader, null);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }
        private async Task insertOdds(BetCancelUndoEventArgs args)
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

                    await common.insertLiveOdds(odd, null, active, null, null,
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
