using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BetService.Classes.DbInsert;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;

namespace BetService.Classes.DbInsert
{
    public class RawFeed_Queue:Core
    {
   
    public RawFeed_Queue()
    {
            RawFeed_Queue_WatchQueueMatches();
    }

    public void RawFeed_Queue_WatchQueueMatches()
    {
        int maxRetries = 0;
        var common = new Common();
        var queue = new Queue<Globals.Rollback>();
        int.TryParse(config.AppSettings.Get("MaxQueueRetries"), out maxRetries);
        while (maxRetries > 0)
        {
            if (Globals.Queue_Feed != null && Globals.Queue_Feed.Count > 0)
            {
                try
                {
                    var queueElement = Globals.Queue_Feed.Dequeue();
                    var entity = queueElement;

                    //Additional Data 
                    var objCommand = new NpgsqlCommand(Globals.DB_Functions.InsertRawFeed.ToDescription());
                    objCommand.Parameters.AddWithValue("feed", NpgsqlDbType.Xml,entity);
                    
                    var ObjId = common.insert(objCommand);

                    if (ObjId == -1)
                    {
                        queue.Enqueue(SetRollback(ObjId, Globals.Tables.Live_Common_Feed, Globals.TransactionTypes.Insert));
                        throw new Exception();
                    }
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    common.RollBack(queue.ToList());
                    Logg.logger.Fatal(ex.Message);
                }

                Thread.Sleep(10);
            }


        }
    }
    
}
}
