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
    public class BetClearRollBackHandle : Core
    {

        public  void BetClearRollBackHandler(BetClearRollbackEventArgs args)
        { 
             RunTask(args);
        }

        private void  RunTask(BetClearRollbackEventArgs queueElement)
        {

        }
    }
}
