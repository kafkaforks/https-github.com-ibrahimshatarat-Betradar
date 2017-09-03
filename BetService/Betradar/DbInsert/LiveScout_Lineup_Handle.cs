using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpgsqlTypes;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveScout;

namespace BetService.Classes.DbInsert
{
    class LiveScout_Lineup_Handle:Core
    {
        public LiveScout_Lineup_Handle(LineupsEventArgs args)
        {
        }
    }
}
