using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Betradar.Classes.DbInsert;
using Npgsql;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;
using Timer = System.Timers.Timer;

namespace Betradar.Classes
{
    static class TaskHandler
    {
        private static Timer timer1 = null;
        public static volatile Timer timer_odd_change;
        public static volatile bool loop_working = false;
        static TaskHandler()
        {
                StartOddChangeWatch();
        }
        public static void StartOddChangeWatch()
        {
            timer_odd_change = new Timer();
            timer_odd_change.Interval = 100;
            timer_odd_change.Elapsed += timer_odd_change_Tick;
            timer_odd_change.Enabled = true;
            timer_odd_change.AutoReset = true;
            timer_odd_change.Start();
        }

        public static void StartErrorWatch()
        {
            timer1 = new Timer();
            timer1.Interval = 10000;
            timer1.Elapsed += timer1_Tick;
            timer1.Enabled = true;
            timer1.Start();
            GC.KeepAlive(timer1);
        }
        private static void timer_odd_change_Tick(object sender, ElapsedEventArgs e)
        {
            timer_odd_change.Stop();

            //SharedLibrary.Logg.logger.Error("Queue_Odd_Change Count ::: " + Globals.Queue_Odd_Change.Count.ToString());
            if (Globals.Queue_Odd_Change != null)
            {
                if (!loop_working)
                {
                    if (Globals.Queue_Odd_Change.Count > 0)
                    {
                        SharedLibrary.Logg.logger.Error("Odd Change Queue Count ::: " +
                                                        Globals.Queue_Odd_Change.Count.ToString());
                        manage_odd_change();
                    }
                }
            }


        }
        private static void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            timer1.Stop();
            if (Globals.Queue_Errors != null)
            {
                // SharedLibrary.Logg.logger.Error("Error Queue Count ::: " + Globals.Queue_Errors.Count.ToString());
                if (Globals.Queue_Errors.Count > 0)
                {
                    work_errors();
                }
            }

            timer1.Start();
        }

        static void manage_odd_change()
        {
            loop_working = true;
           var common = new Common();
            var queue = new Queue<Globals.Rollback>();
            //var entity = queueElement.OddsChange;
            try
            {
                while (Globals.Queue_Odd_Change.Count > 0)
                {
                   
                    var l = Globals.Queue_Odd_Change.Dequeue();
                    var entity = l.arg.OddsChange;
                    bool active;

                    foreach (var odd in entity.Odds)
                    {
                        if (odd.Active != null)
                        {
                            active = odd.Active;
                            if (odd.OddsFields.Count > 0)
                            {
                                foreach (var field in odd.OddsFields)
                                {
                                    var val = field.Value;
                                    if (active)
                                    {
                                        active = val.Active;
                                    }
                                    common.insertLiveOdds(odd, val, active, val.Outcome, val.PlayerId,
                                        val.Probability.ToString() ?? "", val.Type, val.Value.ToString() ?? "",
                                        val.ViewIndex,
                                        val.VoidFactor.ToString() ?? "", field.Key, entity.EventHeader.Id,
                                        val.TypeId ?? 0);

                                    //SendToHybridgeSocket(entity.EventHeader.Id, odd.Id, val.TypeId, "", odd.SpecialOddsValue, val);
                                }
                            }
                            else
                            {
                                common.UpdateAllLiveOddsOutcomesActive(entity.EventHeader.Id, odd, odd.Active);
                            }
                        }
                    }
                    common.insertMatchDataAllDetails((MatchHeader)entity.EventHeader, null);

                }

                timer_odd_change.Start();
                loop_working = false;
            }
            catch (Exception ex)
            {
                //common.RollBack(queue.ToList());
                Logg.logger.Fatal(ex.Message);
            }
            finally
            {
                timer_odd_change.Enabled = true;
            }
        }

        static void work_errors()
        {
            var common = new Common();
            if (Globals.Queue_Errors != null)
            {
                while (Globals.Queue_Errors.Count > 0)
                {
                    var command = Globals.Queue_Errors.Dequeue();
                    try
                    {
                        common.insert(command);
                    }
                    catch (Exception ex)
                    {
                        SharedLibrary.Logg.logger.Fatal(ex.Message);
                        Globals.Queue_Errors.Enqueue(command);
                        return;
                    }

                }
            }

        }

    }
}
