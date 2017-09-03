using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveScout;

namespace BetService.Classes.DbInsert
{
    class LiveScout_MatchList_Handle:Core
    {
        public LiveScout_MatchList_Handle(MatchListEventArgs args)
        {
            //insertMatchBooks(args);
        }
//        public void insertMatchBooks(MatchListEventArgs Lineupentity)
//        {
//            var entity = Lineupentity.MatchList;
//            entity[0].
//            var queue = new Queue<Globals.Rollback>();
//            var common = new Common();
//            var command = new NpgsqlCommand(Globals.DB_Functions.InsertLiveScoutMatchBooking.ToDescription());
//            try
//            {
//                if (entity.AdditionalData != null)
//                {
//                    var ret = common.insertAdditionalData(entity.AdditionalData);
//                    if (ret.id != -1)
//                    {
//                        command.Parameters.AddWithValue("p_fk_dictionary_additionaldata_id", NpgsqlDbType.Bigint, ret.id);
//                        queue.Enqueue(SetRollback(ret.id, Globals.Tables.LiveScoutMatchBooking, Globals.TransactionTypes.Insert));
//                        queue = CloneRollbackQueue(queue, ret.queue);
//                    }
//                    else
//                    {
//                        command.Parameters.AddWithValue("p_fk_dictionary_additionaldata_id", NpgsqlDbType.Bigint, DBNull.Value);
//                    }
//                }
//                else
//                {
//                    command.Parameters.AddWithValue("p_fk_dictionary_additionaldata_id", NpgsqlDbType.Bigint, DBNull.Value);
//                }


//                if (entity.MatchId != null)
//                {
//                    command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, entity.MatchId);
//                }
//                else
//                {
//                    command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, DBNull.Value);
//                }
//                if (entity.Message != null && !string.IsNullOrEmpty(entity.Message))
//                {
//                    command.Parameters.AddWithValue("p_message", NpgsqlDbType.Text, entity.Message);
//                }
//                else
//                {
//                    command.Parameters.AddWithValue("p_message", NpgsqlDbType.Text, DBNull.Value);
//                }
//                if (entity.Result != null)
//                {
//                    command.Parameters.AddWithValue("p_book_match_result", NpgsqlDbType.Text, entity.Result);
//                }
//                else
//                {
//                    command.Parameters.AddWithValue("p_book_match_result", NpgsqlDbType.Text, DBNull.Value);
//                }
//                var ObjId = common.insert(command);

//                if (ObjId == -1)
//                {
//                    queue.Enqueue(SetRollback(ObjId, Globals.Tables.LiveScoutMatchBooking, Globals.TransactionTypes.Insert));
//                    throw new Exception("Error in Insert Bet Stop");
//                }
//#if DEBUG
//                OutCount += 1;
//                Logg.logger.Error("InCount Count = " + InCount + "  ||||| OutCount = " + OutCount);
//#endif
//            }
//            catch (Exception ex)
//            {
//                common.RollBack(queue.ToList());
//                Logg.logger.Fatal(ex.Message);
//            }
//        }
    }
}
