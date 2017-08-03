using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetService.Classes.DbInsert;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveScout;

namespace BetService.Classes.DbInsert
{
    class LiveScout_MatchData_Handle:Core
    {
        public LiveScout_MatchData_Handle(MatchDataEventArgs args)
        {
           // insertMatchData(args);
        }
     
    }
}
