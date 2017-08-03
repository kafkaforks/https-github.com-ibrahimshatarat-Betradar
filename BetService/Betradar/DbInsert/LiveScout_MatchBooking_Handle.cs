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
    class LiveScout_MatchBooking_Handle:Core
    {
        public LiveScout_MatchBooking_Handle(MatchBookingReplyEventArgs args)
        {
            //insertMatchBooks(args);
        }
       
    }
}
