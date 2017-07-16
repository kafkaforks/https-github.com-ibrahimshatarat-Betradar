using System;
using System.Data;
using Betradar.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using NpgsqlTypes;

namespace Betradar.TestUnits
{
    [TestClass]
    public class DB_LoadTest
    {
        [TestMethod]
        public void Load()
        {

        }
        static NpgsqlCommand db_insert()
        {
            var DB_Host = "127.0.0.1";
            var DB_Port = "5432";
            var DB_Database = "BetradarDB";
            var DB_Username = "postgres";
            var DB_Password = "123";

            var json = "{'markers':[{'point':newGLatLng(40.266044,-74.718479),'homeTeam':'Lawrence Library','awayTeam':'LUGip','markerImage':'images/red.png','information':'Linux users group meets second Wednesday of each month.','fixture':'Wednesday 7pm','capacity':'','previousScore':''},{'point':newGLatLng(40.211600,-74.695702),'homeTeam':'Hamilton Library','awayTeam':'LUGip HW SIG','markerImage':'images/white.png','information':'Linux users can meet the first Tuesday of the month to work out harward and configuration issues.','fixture':'Tuesday 7pm','capacity':'','tv':''},{'point':newGLatLng(40.294535,-74.682012),'homeTeam':'Applebees','awayTeam':'After LUPip Mtg Spot','markerImage':'images/newcastle.png','information':'Some of us go there after the main LUGip meeting, drink brews, and talk.','fixture':'Wednesday whenever','capacity':'2 to 4 pints','tv':''},] }";
            var param = new NpgsqlParameter("json_test", NpgsqlDbType.Jsonb);
            param.Value = json;
            var connectionBuilder = new NpgsqlConnectionStringBuilder();
            connectionBuilder.Host = DB_Host;
            connectionBuilder.Port = Int32.Parse(DB_Port);
            connectionBuilder.Database = "Betradar";
            connectionBuilder.Username = DB_Username;
            connectionBuilder.Password = DB_Password;
            connectionBuilder.Timeout = 20;
            connectionBuilder.CommandTimeout = 20;
            var con = new NpgsqlConnection(connectionBuilder.ConnectionString);
            var objCommand = new NpgsqlCommand("json", con);
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Parameters.Add(param);
            objCommand.Connection = con;
            return objCommand;
        }

        private void json_test()
        {

        }
    }
}
