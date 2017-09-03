using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;

namespace Betradar.Classes.DbInsert
{
 
      
  
    class Tournament_Upsert
    {
        public long? sport_id { get; set; }
        public long? category_id { get; set; }
        public long? tournament_id { get; set; }
        public long? unique_tournament_id { get; set; }
        public string sport { get; set; }
        public string category { get; set; }
        public string tournament { get; set; }
        public string unique_tournament_name { get; set; }
        public long? team_id { get; set; }
        public string team_name { get; set; }
        public long? super_team_id { get; set; }




        public Globals.ReturnQueueLong insertCpTournament()
        {
            var common = new Common();
            var queue = new Queue<Globals.Rollback>();
            var ObjCommand = new NpgsqlCommand(Globals.DB_Functions.InsertCpTournament.ToDescription());
            try
            {

                ObjCommand.Parameters.AddWithValue("p_sport_id", NpgsqlDbType.Bigint, (object)sport_id ?? DBNull.Value);

                ObjCommand.Parameters.AddWithValue("p_category_id", NpgsqlDbType.Bigint, (object)category_id ?? DBNull.Value);

                ObjCommand.Parameters.AddWithValue("p_tournament_id", NpgsqlDbType.Bigint, (object)tournament_id ?? DBNull.Value);

                ObjCommand.Parameters.AddWithValue("p_unique_tournament_id", NpgsqlDbType.Bigint, (object)unique_tournament_id ?? DBNull.Value);

                ObjCommand.Parameters.AddWithValue("p_sport", NpgsqlDbType.Text, (object)sport ?? DBNull.Value);

                ObjCommand.Parameters.AddWithValue("p_category", NpgsqlDbType.Text, (object)category ?? DBNull.Value);

                ObjCommand.Parameters.AddWithValue("p_tournament", NpgsqlDbType.Text, (object)tournament ?? DBNull.Value);

                ObjCommand.Parameters.AddWithValue("p_unique_tournament_name", NpgsqlDbType.Text, (object)unique_tournament_name ?? DBNull.Value);

                ObjCommand.Parameters.AddWithValue("p_team_id", NpgsqlDbType.Bigint, (object)team_id ?? DBNull.Value);

                ObjCommand.Parameters.AddWithValue("p_team_name", NpgsqlDbType.Text, (object)team_name ?? DBNull.Value);

                ObjCommand.Parameters.AddWithValue("p_super_team_id", NpgsqlDbType.Bigint, (object)super_team_id ?? DBNull.Value);

                var RetId = common.insert(ObjCommand);
                if (RetId > 0)
                {
                    return new Globals.ReturnQueueLong(queue, RetId);
                }
                else
                {
                    return new Globals.ReturnQueueLong(queue, -1);
                }

            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return new Globals.ReturnQueueLong(queue, -1);
            }
        }
    }
}
