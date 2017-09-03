using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveScout;

namespace Betradar.Classes.DbInsert
{
    class LiveScout_MatchData_Handle:Core
    {
        public LiveScout_MatchData_Handle(MatchDataEventArgs args)
        {
           // insertMatchData(args);
        }
        public void insertMatchData(MatchDataEventArgs MatchDataEntity)
        {
            var entity = MatchDataEntity.MatchData;
            var queue = new Queue<Globals.Rollback>();
            var common = new Common();
            var command = new NpgsqlCommand(Globals.DB_Functions.InsertLiveScoutMatchData.ToDescription());
            try
            {
                
                if (entity.AdditionalData != null)
                {
                    var ret = common.insertAdditionalData(entity.AdditionalData);
                    if (ret.id != -1)
                    {
                        command.Parameters.AddWithValue("p_fk_dictionary_additionaldata_id", NpgsqlDbType.Bigint, ret.id);
                        queue.Enqueue(SetRollback(ret.id, Globals.Tables.LiveScoutMatchData, Globals.TransactionTypes.Insert));
                        queue = CloneRollbackQueue(queue, ret.queue);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("p_fk_dictionary_additionaldata_id", NpgsqlDbType.Bigint, DBNull.Value);
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("fk_dictionary_additionaldata_id", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entity.MatchId != null)
                {
                    command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, entity.MatchId);
                }
                else
                {
                    command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entity.MatchTime != null && !string.IsNullOrEmpty(entity.MatchTime))
                {
                    command.Parameters.AddWithValue("p_match_time", NpgsqlDbType.Text, entity.MatchTime);
                }
                else
                {
                    command.Parameters.AddWithValue("p_match_time", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entity.RemainingTimeInPeriod != null)
                {
                    command.Parameters.AddWithValue("p_remaining_time_in_period", NpgsqlDbType.Text,entity.RemainingTimeInPeriod);
                }
                else
                {
                    command.Parameters.AddWithValue("p_remaining_time_in_period", NpgsqlDbType.Text, DBNull.Value);
                }

                var ObjId = common.insert(command);

                if (ObjId == -1)
                {
                    queue.Enqueue(SetRollback(ObjId, Globals.Tables.LiveScoutMatchData, Globals.TransactionTypes.Insert));
                    throw new Exception("Error in Insert Bet Stop");
                }
#if DEBUG
                OutCount += 1;
                Logg.logger.Error("InCount Count = " + InCount + "  ||||| OutCount = " + OutCount);
#endif
            }
            catch (Exception ex)
            {
                common.RollBack(queue.ToList());
                Logg.logger.Fatal(ex.Message);
            }
        }
    }
}
