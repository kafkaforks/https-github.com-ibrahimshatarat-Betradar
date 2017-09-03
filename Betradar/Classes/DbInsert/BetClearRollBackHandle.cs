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
    public class BetClearRollBackHandle : Core
    {

        public BetClearRollBackHandle(BetClearRollbackEventArgs args)
        {
            RunTask(args);
        }

        public void RunTask(BetClearRollbackEventArgs queueElement)
        {
            //var common = new Common();
            //Task.Factory.StartNew(() => common.insertMatchDataAllDetails((MatchHeader)queueElement.BetClearRollback.EventHeader, null));

        }
    }
}
