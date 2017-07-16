using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace Betradar.Classes.DbInsert
{
    class BetCancelUndoHandle : Core
    {
        public BetCancelUndoHandle(BetCancelUndoEventArgs args)
        {
            RunTask(args);
        }
        public void RunTask(BetCancelUndoEventArgs args)
        {
            //var common = new Common();
            //var queue = new Queue<Globals.Rollback>();
            try
            {
                //Task.Factory.StartNew(() => common.insertMatchDataAllDetails((MatchHeader)args.BetCancelUndo.EventHeader, null));
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }

        }
    }
}
