using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BetService.DbInsert.Betradar;
using NetMQ;
using NetMQ.Sockets;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using Sportradar.SDK.Common;
using Sportradar.SDK.FeedProviders.Common;
using Sportradar.SDK.FeedProviders.Lcoo;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;
using Sportradar.SDK.FeedProviders.LiveOdds.Outrights;
using Sportradar.SDK.FeedProviders.LiveScout;
using Attribute = Sportradar.SDK.FeedProviders.LiveScout.Attribute;
using MatchHeader = Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds.MatchHeader;

namespace BetService.Classes.DbInsert
{
    public class Common : Core, IDisposable
    {
        // private NpgsqlConnection con;
        private element entity;

        public Common()
        {

        }

        /// <summary>
        /// Selects the enum value
        /// </summary>
        /// <param name="enumMaster"></param>
        /// <param name="enumSlave"></param>
        /// Returns custom type Return Queue Long;        /// Queue: is the sum of other inserts " if applied " to add to the mother queue in case of rollback        /// Long: is the main inserted row id        /// </returns>
        public int selectEnumValue(string enumMaster, string enumSlave)
        {
            var adObjCommand = new NpgsqlCommand(Globals.DB_Functions.SelectEnumsByNames.ToDescription());
            adObjCommand.Parameters.AddWithValue("enum_group_name", enumMaster);
            adObjCommand.Parameters.AddWithValue("enum_name", enumSlave);
            var ds = select(adObjCommand);
            var result = -1;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = int.Parse(ds.Tables[0].Rows[0]["select_enum_by_names"].ToString());
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueElement"></param>
        /// Returns custom type Return Queue Long;        /// Queue: is the sum of other inserts " if applied " to add to the mother queue in case of rollback        /// Long: is the main inserted row id        /// </returns>
        //public Globals.ReturnQueueLong insertMatchHeader(MatchHeader queueElement)
        //{
        //    var common = new Common();
        //    var queue = new Queue<Globals.Rollback>();
        //    try
        //    {
        //        /// Now insert the detailed Match Header to match_header table

        //        var messageObjCommand = new NpgsqlCommand(Globals.DB_Functions.InserMatchHeader.ToDescription());
        //        if (queueElement.Active != null)
        //            messageObjCommand.Parameters.AddWithValue("active", queueElement.Active);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("active", NpgsqlDbType.Boolean, DBNull.Value);

        //        if (queueElement.AutoTraded != null)
        //            messageObjCommand.Parameters.AddWithValue("auto_traded", queueElement.AutoTraded);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("auto_traded", NpgsqlDbType.Boolean, DBNull.Value);

        //        if (queueElement.Balls != null)
        //            messageObjCommand.Parameters.AddWithValue("balls", queueElement.Balls);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("balls", NpgsqlDbType.Integer, DBNull.Value);

        //        if (queueElement.Bases != null)
        //            messageObjCommand.Parameters.AddWithValue("bases", queueElement.Bases);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("bases", NpgsqlDbType.Text, DBNull.Value);

        //        if (queueElement.Batter != null)
        //        {
        //            var homeAway_id = insertHomeAway(queueElement.Batter);
        //            if (homeAway_id != null && homeAway_id > 0)
        //            {
        //                queue.Enqueue(SetRollback(homeAway_id, Globals.Tables.HomeAway, Globals.TransactionTypes.Insert));
        //                messageObjCommand.Parameters.AddWithValue("fk_home_away_id_batter", NpgsqlDbType.Bigint, queueElement.Active);
        //            }
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_id_batter", NpgsqlDbType.Bigint, DBNull.Value);

        //        if (queueElement.BetStatus != null)
        //        {
        //            var enum_id = selectEnumValue(Globals.MasterEnum.EventBetStatus.ToDescription(),
        //                queueElement.BetStatus.ToString());
        //            var ggd = new Globals.enum_grouping();
        //            if (queueElement.BetStatus.HasValue)
        //                ggd.master = int.Parse(queueElement.BetStatus.Value.ToString());
        //            ggd.slave = enum_id;
        //            messageObjCommand.Parameters.AddWithValue("bet_status", ggd);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("bet_status", NpgsqlDbType.Composite, DBNull.Value);

        //        if (queueElement.BetStopReason != null)
        //        {
        //            var id = inserBetStopReason(queueElement.BetStopReason.Id, queueElement.BetStopReason.Reason);
        //            queue.Enqueue(SetRollback(id, Globals.Tables.BetStopReason, Globals.TransactionTypes.Insert));
        //            messageObjCommand.Parameters.AddWithValue("fk_bet_stop_reason_id", NpgsqlDbType.Bigint, id);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("fk_bet_stop_reason_id", NpgsqlDbType.Bigint, DBNull.Value);

        //        if (queueElement.Booked != null)
        //            messageObjCommand.Parameters.AddWithValue("booked", NpgsqlDbType.Boolean, queueElement.Booked.Value);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("booked", NpgsqlDbType.Boolean, DBNull.Value);
        //        if (queueElement.ClearedScore != null)
        //            messageObjCommand.Parameters.AddWithValue("cleared_score", NpgsqlDbType.Text, queueElement.ClearedScore);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("cleared_score", NpgsqlDbType.Text, DBNull.Value);
        //        if (queueElement.ClockStopped != null)
        //            messageObjCommand.Parameters.AddWithValue("clock_stopped", NpgsqlDbType.Boolean, queueElement.ClockStopped);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("clock_stopped", NpgsqlDbType.Boolean, DBNull.Value);
        //        if (queueElement.Corners != null)
        //        {
        //            var id = insertHomeAway(queueElement.Corners);
        //            queue.Enqueue(SetRollback(id, Globals.Tables.Corners, Globals.TransactionTypes.Insert));
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_id_corners", NpgsqlDbType.Bigint, queueElement.Active);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_id_corners", NpgsqlDbType.Bigint, DBNull.Value);
        //        if (queueElement.CurrentCtTeam != null)
        //        {
        //            var id = selectEnumValue(Globals.MasterEnum.Team.ToDescription(), queueElement.CurrentCtTeam.ToString());
        //            messageObjCommand.Parameters.AddWithValue("current_ct_team", id);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("current_ct_team", NpgsqlDbType.Composite, DBNull.Value);
        //        if (queueElement.CurrentEnd != null)
        //            messageObjCommand.Parameters.AddWithValue("current_end", NpgsqlDbType.Integer, queueElement.CurrentEnd);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("current_end", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.Delivery != null)
        //        {
        //            var enums = selectEnumValue(Globals.MasterEnum.Team.ToDescription(), queueElement.Delivery.ToString());
        //            messageObjCommand.Parameters.AddWithValue("delivery", enums);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("delivery", NpgsqlDbType.Composite, DBNull.Value);
        //        if (queueElement.Dismissals != null)
        //        {
        //            var id = insertHomeAway(queueElement.Dismissals);
        //            queue.Enqueue(SetRollback(id, Globals.Tables.HomeAway, Globals.TransactionTypes.Insert));
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_dismissals_id", NpgsqlDbType.Bigint, id);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_dismissals_id", NpgsqlDbType.Bigint, DBNull.Value);
        //        if (queueElement.EarlyBetStatus != null)
        //        {
        //            var enums = selectEnumValue(Globals.MasterEnum.OddsMatchEarlyBetStatus.ToDescription(),
        //                 queueElement.EarlyBetStatus.ToString());
        //            messageObjCommand.Parameters.AddWithValue("early_bet_status", enums);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("early_bet_status", NpgsqlDbType.Composite, DBNull.Value);
        //        if (queueElement.Expedite != null)
        //            messageObjCommand.Parameters.AddWithValue("expedite", NpgsqlDbType.Boolean, queueElement.Expedite);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("expedite", NpgsqlDbType.Boolean, DBNull.Value);
        //        if (queueElement.GameScore != null)
        //            messageObjCommand.Parameters.AddWithValue("game_score", NpgsqlDbType.Text, queueElement.GameScore);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("game_score", NpgsqlDbType.Text, DBNull.Value);
        //        if (queueElement.Id != null)
        //            messageObjCommand.Parameters.AddWithValue("event_id", queueElement.Id);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("event_id", NpgsqlDbType.Bigint, DBNull.Value);
        //        if (queueElement.Innings != null)
        //            messageObjCommand.Parameters.AddWithValue("innings", queueElement.Active);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("innings", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.LegScore != null)
        //            messageObjCommand.Parameters.AddWithValue("leg_score", queueElement.Active);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("leg_score", NpgsqlDbType.Text, DBNull.Value);
        //        if (queueElement.MatchTime != null)
        //            messageObjCommand.Parameters.AddWithValue("match_time", NpgsqlDbType.Bigint, queueElement.MatchTime);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("match_time", NpgsqlDbType.Bigint, DBNull.Value);
        //        if (queueElement.MatchTimeExtended != null)
        //            messageObjCommand.Parameters.AddWithValue("match_time_extended", NpgsqlDbType.Text, queueElement.MatchTimeExtended);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("match_time_extended", NpgsqlDbType.Text, DBNull.Value);
        //        if (queueElement.Message != null)
        //        {
        //            var ret = insertMessages(queueElement.Message);
        //            queue.Enqueue(SetRollback(ret.id, Globals.Tables.Messages, Globals.TransactionTypes.Insert));
        //            queue = CloneRollbackQueue(queue, ret.queue);
        //            messageObjCommand.Parameters.AddWithValue("fk_messages_id", ret.id);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("fk_messages_id", NpgsqlDbType.Bigint, DBNull.Value);
        //        if (queueElement.Msgnr != null)
        //            messageObjCommand.Parameters.AddWithValue("msgnr", NpgsqlDbType.Bigint, queueElement.Msgnr);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("msgnr", NpgsqlDbType.Bigint, DBNull.Value);
        //        if (queueElement.Outs != null)
        //            messageObjCommand.Parameters.AddWithValue("outs", NpgsqlDbType.Integer, queueElement.Outs);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("outs", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.Over != null)
        //            messageObjCommand.Parameters.AddWithValue("over", NpgsqlDbType.Integer, queueElement.Over);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("over", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.PentaltyRuns != null)
        //        {
        //            var id = insertHomeAway(queueElement.PentaltyRuns);
        //            queue.Enqueue(SetRollback(id, Globals.Tables.HomeAway, Globals.TransactionTypes.Insert));
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_id_pental_runs", id);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_id_pental_runs", NpgsqlDbType.Bigint, DBNull.Value);
        //        if (queueElement.Position != null)
        //            messageObjCommand.Parameters.AddWithValue("position", NpgsqlDbType.Integer, queueElement.Position);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("position", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.Possession != null)
        //        {
        //            var enums = selectEnumValue(Globals.MasterEnum.Team.ToDescription(), queueElement.Possession.ToString());
        //            messageObjCommand.Parameters.AddWithValue("possession", enums);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("possession", NpgsqlDbType.Composite, DBNull.Value);
        //        if (queueElement.RedCards != null)
        //        {
        //            var id = insertHomeAway(queueElement.RedCards);
        //            queue.Enqueue(SetRollback(id, Globals.Tables.HomeAway, Globals.TransactionTypes.Insert));
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_id_red_cards", id);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_id_red_cards", NpgsqlDbType.Bigint, DBNull.Value);
        //        if (queueElement.RemainingBowls != null)
        //            messageObjCommand.Parameters.AddWithValue("remaining_bowls", NpgsqlDbType.Integer, queueElement.RemainingBowls);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("remaining_bowls", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.RemainingReds != null)
        //            messageObjCommand.Parameters.AddWithValue("remaining_reds", NpgsqlDbType.Integer, queueElement.RemainingReds);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("remaining_reds", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.RemainingTime != null)
        //            messageObjCommand.Parameters.AddWithValue("remaining_time", NpgsqlDbType.Text, queueElement.RemainingTime);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("remaining_time", NpgsqlDbType.Text, DBNull.Value);
        //        if (queueElement.RemainingTimeInPeriod != null)
        //            messageObjCommand.Parameters.AddWithValue("remaining_time_in_period", queueElement.RemainingTimeInPeriod);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("remaining_time_in_period", DBNull.Value);
        //        if (queueElement.Score != null)
        //            messageObjCommand.Parameters.AddWithValue("score", NpgsqlDbType.Text, queueElement.Score);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("score", NpgsqlDbType.Text, DBNull.Value);
        //        if (queueElement.Server != null)
        //            messageObjCommand.Parameters.AddWithValue("server", NpgsqlDbType.Integer, queueElement.Server);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("server", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.SetScores != null)
        //        {
        //            var ret = insertSetScores(queueElement.SetScores);
        //            queue.Enqueue(SetRollback(ret.id, Globals.Tables.SetScores, Globals.TransactionTypes.Insert));
        //            messageObjCommand.Parameters.AddWithValue("fk_set_scores_id", NpgsqlDbType.Bigint, ret.id);
        //            queue = CloneRollbackQueue(queue, ret.queue);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("fk_set_scores_id", NpgsqlDbType.Bigint, DBNull.Value);
        //        if (queueElement.SourceId != null)
        //            messageObjCommand.Parameters.AddWithValue("source_id", NpgsqlDbType.Text, queueElement.SourceId);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("source_id", NpgsqlDbType.Text, DBNull.Value);
        //        if (queueElement.Status != null)
        //        {
        //            var enums = selectEnumValue(Globals.MasterEnum.EventStatus.ToDescription(), queueElement.Status.ToString());
        //            messageObjCommand.Parameters.AddWithValue("status", enums);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("status", NpgsqlDbType.Composite, DBNull.Value);
        //        if (queueElement.Strikes != null)
        //            messageObjCommand.Parameters.AddWithValue("strikes", NpgsqlDbType.Integer, queueElement.Strikes);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("strikes", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.Suspend != null)
        //        {
        //            var id = insertHomeAway(queueElement.Suspend);
        //            queue.Enqueue(SetRollback(id, Globals.Tables.HomeAway, Globals.TransactionTypes.Insert));
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_id_suspend", NpgsqlDbType.Bigint, id);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_id_suspend", NpgsqlDbType.Bigint, DBNull.Value);
        //        if (queueElement.Throw != null)
        //            messageObjCommand.Parameters.AddWithValue("throw", NpgsqlDbType.Integer, queueElement.Throw);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("throw", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.TieBreak != null)
        //            messageObjCommand.Parameters.AddWithValue("tie_break", NpgsqlDbType.Integer, queueElement.TieBreak);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("tie_break", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.Try != null)
        //            messageObjCommand.Parameters.AddWithValue("try", NpgsqlDbType.Integer, queueElement.Try);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("try", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.Visit != null)
        //            messageObjCommand.Parameters.AddWithValue("visit", NpgsqlDbType.Integer, queueElement.Visit);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("visit", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.Yards != null)
        //            messageObjCommand.Parameters.AddWithValue("yards", NpgsqlDbType.Integer, queueElement.Yards);
        //        else
        //            messageObjCommand.Parameters.AddWithValue("yards", NpgsqlDbType.Integer, DBNull.Value);
        //        if (queueElement.YellowCards != null)
        //        {
        //            var id = insertHomeAway(queueElement.YellowCards);
        //            queue.Enqueue(SetRollback(id, Globals.Tables.HomeAway, Globals.TransactionTypes.Insert));
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_id_yellow_cards", NpgsqlDbType.Bigint, id);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_id_yellow_cards", NpgsqlDbType.Bigint, DBNull.Value);
        //        if (queueElement.YellowRedCards != null)
        //        {
        //            var id = insertHomeAway(queueElement.YellowRedCards);
        //            queue.Enqueue(SetRollback(id, Globals.Tables.HomeAway, Globals.TransactionTypes.Insert));
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_id_yellow_red_cards", NpgsqlDbType.Bigint, id);
        //        }
        //        else
        //            messageObjCommand.Parameters.AddWithValue("fk_home_away_id_yellow_red_cards", NpgsqlDbType.Bigint, DBNull.Value);

        //        return new Globals.ReturnQueueLong(queue, insert(messageObjCommand));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logg.logger.Fatal(ex.Message);
        //        return new Globals.ReturnQueueLong(queue, -1);
        //    }
        //}
        /// <summary>
        /// Additional data Insert
        /// </summary>
        /// <param name="entity"></param>
        /// Returns custom type Return Queue Long;        /// Queue: is the sum of other inserts " if applied " to add to the mother queue in case of rollback        /// Long: is the main inserted row id        /// </returns>
        public Globals.ReturnQueueLong insertAdditionalData(ScoreCardSummary entity)
        {
            var queue = new Queue<Globals.Rollback>();
            try
            {
                //TODO: this function is not finished
                ///Insert into the main dictionary table (1 => ∞) first
                var adObjCommand = new NpgsqlCommand(Globals.DB_Functions.InsertDictionaries.ToDescription());
                adObjCommand.Parameters.AddWithValue("fk_table_id", Globals.Tables.Live_Common_Feed.ToDescription());
                var dictionaries_id = insert(adObjCommand);

                /// Now insert the detailed dictionaries to dictionary table
                foreach (var dictionary in entity.AdditionalData)
                {
                    var dictionaryObjCommand = new NpgsqlCommand(Globals.DB_Functions.InserDictionary.ToDescription());
                    dictionaryObjCommand.Parameters.AddWithValue("fk_table_id", NpgsqlDbType.Integer, Globals.Tables.Dictionary.ToDescription());
                    dictionaryObjCommand.Parameters.AddWithValue("dictionaries_id", dictionaries_id);
                    dictionaryObjCommand.Parameters.AddWithValue("key", dictionary.Key);
                    dictionaryObjCommand.Parameters.AddWithValue("value", dictionary.Value);
                    queue.Enqueue(SetRollback(insert(dictionaryObjCommand), Globals.Tables.Dictionary, Globals.TransactionTypes.Insert));
                }
                return new Globals.ReturnQueueLong(queue, dictionaries_id);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex);
                return new Globals.ReturnQueueLong(queue, -1);
            }
        }
        /// <summary>
        /// Additional data Insert
        /// </summary>
        /// <param name="entity"></param>
        /// Returns custom type Return Queue Long;        /// Queue: is the sum of other inserts " if applied " to add to the mother queue in case of rollback        /// Long: is the main inserted row id        /// </returns>
        public Globals.ReturnQueueLong insertAdditionalData(LineupsEventArgs entity)
        {
            var queue = new Queue<Globals.Rollback>();
            try
            {
                //TODO: this function is not finished
                ///Insert into the main dictionary table (1 => ∞) first
                var adObjCommand = new NpgsqlCommand(Globals.DB_Functions.InsertDictionaries.ToDescription());
                adObjCommand.Parameters.AddWithValue("fk_table_id", Globals.Tables.Live_Common_Feed);
                var dictionaries_id = insert(adObjCommand);

                /// Now insert the detailed dictionaries to dictionary table
                foreach (var dictionary in entity.Lineups.AdditionalData)
                {
                    var dictionaryObjCommand = new NpgsqlCommand(Globals.DB_Functions.InserDictionary.ToDescription());
                    dictionaryObjCommand.Parameters.AddWithValue("fk_table_id", NpgsqlDbType.Integer, Globals.Tables.Dictionary.ToDescription());
                    dictionaryObjCommand.Parameters.AddWithValue("dictionaries_id", dictionaries_id);
                    dictionaryObjCommand.Parameters.AddWithValue("key", dictionary.Key);
                    dictionaryObjCommand.Parameters.AddWithValue("value", dictionary.Value);
                    queue.Enqueue(SetRollback(insert(dictionaryObjCommand), Globals.Tables.Dictionary, Globals.TransactionTypes.Insert));
                }
                return new Globals.ReturnQueueLong(queue, dictionaries_id);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex);
                return new Globals.ReturnQueueLong(queue, -1);
            }
        }
        /// <summary>
        /// Insert Additional data in Dictionaries
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Globals.ReturnQueueLong insertAdditionalData(IDictionary<string, string> entity)
        {
            var queue = new Queue<Globals.Rollback>();
            ///Insert into the main dictionary table (1 => ∞) first
            var adObjCommand = new NpgsqlCommand(Globals.DB_Functions.InsertDictionaries.ToDescription());
            adObjCommand.Parameters.AddWithValue("fk_table_id", NpgsqlDbType.Integer, Globals.Tables.Live_Common_Feed);
            try
            {

                var dictionaries_id = insert(adObjCommand);
                if (dictionaries_id > 0)
                {
                    /// Now insert the detailed dictionaries to dictionary table
                    foreach (var dictionary in entity)
                    {
                        var dictionaryObjCommand = new NpgsqlCommand(Globals.DB_Functions.InserDictionary.ToDescription());
                        dictionaryObjCommand.Parameters.AddWithValue("fk_table_id", NpgsqlDbType.Bigint, DBNull.Value);
                        dictionaryObjCommand.Parameters.AddWithValue("dictionaries_id", NpgsqlDbType.Bigint, dictionaries_id);
                        if (!string.IsNullOrEmpty(dictionary.Key))
                        {
                            dictionaryObjCommand.Parameters.AddWithValue("key", NpgsqlDbType.Text, dictionary.Key);
                        }
                        else
                        {
                            dictionaryObjCommand.Parameters.AddWithValue("key", NpgsqlDbType.Text, DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(dictionary.Value))
                        {
                            dictionaryObjCommand.Parameters.AddWithValue("value", NpgsqlDbType.Text, dictionary.Value);
                        }
                        else
                        {
                            dictionaryObjCommand.Parameters.AddWithValue("value", NpgsqlDbType.Text, DBNull.Value);
                        }

                        queue.Enqueue(SetRollback(insert(dictionaryObjCommand), Globals.Tables.Dictionary, Globals.TransactionTypes.Insert));
                    }
                }
                return new Globals.ReturnQueueLong(queue, dictionaries_id);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return new Globals.ReturnQueueLong(queue, -1);
            }
        }
        /// <summary>
        /// Insert outright Results
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Globals.ReturnQueueLong insertOutrightResults(List<OutrightResultEntity> entities)
        {
            var queue = new Queue<Globals.Rollback>();
            var command = new NpgsqlCommand(Globals.DB_Functions.InsertOutrightResults.ToDescription());
            try
            {
                command.Parameters.AddWithValue("is_deleted", NpgsqlDbType.Boolean, false);
                var results_id = insert(command);
                foreach (var entity in entities)
                {
                    queue.Enqueue(SetRollback(insertOutrightResult(entity, results_id), Globals.Tables.OutrightResult, Globals.TransactionTypes.Insert));
                }
                return new Globals.ReturnQueueLong(queue, results_id);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return new Globals.ReturnQueueLong(queue, -1);
            }
        }
        /// <summary>
        /// Insert Single Outright Result
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private long insertOutrightResult(OutrightResultEntity entity, long ResultsId)
        {
            var command = new NpgsqlCommand(Globals.DB_Functions.InsertOutrightResult.ToDescription());
            try
            {
                if (entity.DeadHeatFactor != null)
                {
                    command.Parameters.AddWithValue("dead_heat_factor", NpgsqlDbType.Double, entity.DeadHeatFactor);
                }
                else
                {
                    command.Parameters.AddWithValue("dead_heat_factor", NpgsqlDbType.Double, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.Id))
                {
                    command.Parameters.AddWithValue("outright_result_id", NpgsqlDbType.Text, entity.Id);
                }
                else
                {
                    command.Parameters.AddWithValue("outright_result_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.Value))
                {
                    command.Parameters.AddWithValue("value", NpgsqlDbType.Text, entity.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("value", NpgsqlDbType.Text, DBNull.Value);
                }
                if (ResultsId != null && ResultsId > 0)
                {
                    command.Parameters.AddWithValue("fk_outright_results_id", NpgsqlDbType.Bigint, ResultsId);
                }
                else
                {
                    command.Parameters.AddWithValue("fk_outright_results_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                return insert(command);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return -1;
            }
        }
        /// <summary>
        /// Insert winning is the winning team of a tournament.
        /// </summary>
        /// <param name="entity"></param>
        /// Returns custom type Return Queue Long;        /// Queue: is the sum of other inserts " if applied " to add to the mother queue in case of rollback        /// Long: is the main inserted row id        /// </returns> 
        public long insertWinningOutcome(WinningOutcomeEntity entity)
        {
            var command = new NpgsqlCommand(Globals.DB_Functions.InsertWinningOutcome.ToDescription());
            try
            {
                if (!string.IsNullOrEmpty(entity.Outcome))
                {
                    command.Parameters.AddWithValue("outcome", entity.Outcome);
                }
                else
                {
                    command.Parameters.AddWithValue("outcome", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.Reason))
                {
                    command.Parameters.AddWithValue("reason", entity.Reason);
                }
                else
                {
                    command.Parameters.AddWithValue("reason", DBNull.Value);
                }
                return insert(command);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return -1;
            }
        }
        /// <summary>
        /// Insert Match Roles
        /// </summary>
        /// <param name="entity"></param>
        /// Returns custom type Return Queue Long;        /// Queue: is the sum of other inserts " if applied " to add to the mother queue in case of rollback        /// Long: is the main inserted row id        /// 
        public Globals.ReturnQueueLong insertMatchRoles(List<MatchRole> entity)
        {
            var queue = new Queue<Globals.Rollback>();
            var command = new NpgsqlCommand(Globals.DB_Functions.InsertMatchRoles.ToDescription());
            var matchroles_id = insert(command);
            try
            {
                foreach (var role in entity)
                {
                    var com = new NpgsqlCommand(Globals.DB_Functions.InsertMatchRole.ToDescription());
                    if (role.Description != null)
                    {
                        com.Parameters.AddWithValue("description", NpgsqlDbType.Text, role.Description);
                    }
                    else
                    {
                        com.Parameters.AddWithValue("description", NpgsqlDbType.Text, DBNull.Value);
                    }
                    if (role.Id != null)
                    {
                        com.Parameters.AddWithValue("match_role_id", NpgsqlDbType.Integer, role.Id);
                    }
                    else
                    {
                        com.Parameters.AddWithValue("match_role_id", NpgsqlDbType.Integer, DBNull.Value);
                    }
                    com.Parameters.AddWithValue("fk_match_roles_id", NpgsqlDbType.Bigint, matchroles_id);
                    var id = insert(com);
                    if (id != null && id > -1)
                    {
                        queue.Enqueue(SetRollback(id, Globals.Tables.MatchRole, Globals.TransactionTypes.Insert));
                    }
                }
                return new Globals.ReturnQueueLong(queue, matchroles_id);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return new Globals.ReturnQueueLong(queue, -1);
            }
        }
        /// <summary>
        /// Update new match table 
        /// </summary>
        /// <param name="p_home_team_id"></param>
        /// <param name="p_away_team_id"></param>
        /// <param name="p_home_team_name"></param>
        /// <param name="p_away_team_name"></param>
        /// <param name="p_category_id"></param>
        /// <param name="p_category_name"></param>
        /// <param name="p_category_name_tr"></param>
        /// <param name="p_match_start_date"></param>
        /// <param name="p_sport_id"></param>
        /// <param name="p_sport_name"></param>
        /// <param name="p_sport_name_tr"></param>
        /// <param name="p_tournament_id"></param>
        /// <param name="p_tournament_name"></param>
        /// <param name="p_tournament_name_tr"></param>
        /// <param name="p_match_id"></param>
        /// <returns></returns>
        public long updateDyMatchAllData(long p_home_team_id, long p_away_team_id, string p_home_team_name, string p_away_team_name, long p_category_id, string p_category_name,
           string p_category_name_tr, DateTime p_match_start_date, long p_sport_id, string p_sport_name, string p_sport_name_tr, long p_tournament_id, string p_tournament_name,
           string p_tournament_name_tr, long p_match_id)
        {

            var command = new NpgsqlCommand(Globals.DB_Functions.UpdateDyMatchAllData.ToDescription());

            try
            {
                if (p_home_team_id != null)
                {
                    command.Parameters.AddWithValue("p_home_team_id", NpgsqlDbType.Bigint, p_home_team_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_home_team_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (p_away_team_id != null)
                {
                    command.Parameters.AddWithValue("p_away_team_id", NpgsqlDbType.Bigint, p_away_team_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_away_team_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_home_team_name))
                {
                    command.Parameters.AddWithValue("p_home_team_name", NpgsqlDbType.Text, p_home_team_name);
                }
                else
                {
                    command.Parameters.AddWithValue("p_home_team_name", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_away_team_name))
                {
                    command.Parameters.AddWithValue("p_away_team_name", NpgsqlDbType.Text, p_away_team_name);
                }
                else
                {
                    command.Parameters.AddWithValue("p_away_team_name", NpgsqlDbType.Text, DBNull.Value);
                }
                if (p_category_id != null)
                {
                    command.Parameters.AddWithValue("p_category_id", NpgsqlDbType.Bigint, p_category_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_category_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_category_name))
                {
                    command.Parameters.AddWithValue("p_category_name", NpgsqlDbType.Text, p_category_name);
                }
                else
                {
                    command.Parameters.AddWithValue("p_category_name", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_category_name_tr))
                {
                    command.Parameters.AddWithValue("p_category_name_tr", NpgsqlDbType.Text, p_category_name_tr);
                }
                else
                {
                    command.Parameters.AddWithValue("p_category_name_tr", NpgsqlDbType.Text, DBNull.Value);
                }
                if (p_match_start_date != null)
                {

                    command.Parameters.AddWithValue("p_match_start_date", NpgsqlDbType.Timestamp, p_match_start_date);
                }
                else
                {
                    command.Parameters.AddWithValue("p_match_start_date", NpgsqlDbType.Timestamp, DBNull.Value);
                }
                if (p_sport_id != null)
                {
                    command.Parameters.AddWithValue("p_sport_id", NpgsqlDbType.Bigint, p_sport_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_sport_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_sport_name))
                {
                    command.Parameters.AddWithValue("p_sport_name", NpgsqlDbType.Text, p_sport_name);
                }
                else
                {
                    command.Parameters.AddWithValue("p_sport_name", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_sport_name_tr))
                {
                    command.Parameters.AddWithValue("p_sport_name_tr", NpgsqlDbType.Text, p_sport_name_tr);
                }
                else
                {
                    command.Parameters.AddWithValue("p_sport_name_tr", NpgsqlDbType.Text, DBNull.Value);
                }
                if (p_tournament_id != null)
                {
                    command.Parameters.AddWithValue("p_tournament_id", NpgsqlDbType.Bigint, p_tournament_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_tournament_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_tournament_name))
                {
                    command.Parameters.AddWithValue("p_tournament_name", NpgsqlDbType.Text, p_tournament_name);
                }
                else
                {
                    command.Parameters.AddWithValue("p_tournament_name", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_tournament_name_tr))
                {
                    command.Parameters.AddWithValue("p_tournament_name_tr", NpgsqlDbType.Text, p_tournament_name_tr);
                }
                else
                {
                    command.Parameters.AddWithValue("p_tournament_name_tr", NpgsqlDbType.Text, DBNull.Value);
                }
                if (p_match_id != null)
                {
                    command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, p_match_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, DBNull.Value);
                }

                return insert(command);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return -1;
            }
        }

        public long insertCpCouponOdds(long p_coupon_id, int p_feed_type, long p_match_id, Boolean p_odd_active, Boolean p_odd_changed, Boolean p_odd_finalised
            , long p_odd_id, string p_odd_name, string p_odd_odd, string p_odd_outcome, string p_odd_outcome_id, string p_odd_probability,
            string p_odd_special, string p_odd_type, long p_odd_type_id)
        {

            try
            {
                var command = new NpgsqlCommand(Globals.DB_Functions.InsertCpCouponOdds.ToDescription());

                if (p_coupon_id != null)
                {

                    command.Parameters.AddWithValue("p_coupon_id", NpgsqlDbType.Bigint, p_coupon_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_coupon_id", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (p_feed_type != null)
                {

                    command.Parameters.AddWithValue("p_feed_type", NpgsqlDbType.Integer, p_feed_type);
                }
                else
                {
                    command.Parameters.AddWithValue("p_feed_type", NpgsqlDbType.Integer, DBNull.Value);
                }

                if (p_match_id != null)
                {

                    command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, p_match_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (p_odd_active != null)
                {
                    command.Parameters.AddWithValue("p_odd_active", NpgsqlDbType.Boolean, p_odd_active);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_active", NpgsqlDbType.Boolean, DBNull.Value);
                }

                if (p_odd_changed != null)
                {
                    command.Parameters.AddWithValue("p_odd_changed", NpgsqlDbType.Boolean, p_odd_changed);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_changed", NpgsqlDbType.Boolean, DBNull.Value);
                }

                if (p_odd_finalised != null)
                {
                    command.Parameters.AddWithValue("p_odd_finalised", NpgsqlDbType.Boolean, p_odd_finalised);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_finalised", NpgsqlDbType.Boolean, DBNull.Value);
                }

                if (p_odd_id != null)
                {

                    command.Parameters.AddWithValue("p_odd_id", NpgsqlDbType.Bigint, p_odd_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_id", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(p_odd_name))
                {
                    command.Parameters.AddWithValue("p_odd_name", NpgsqlDbType.Text, p_odd_name);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_name", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(p_odd_odd))
                {
                    command.Parameters.AddWithValue("p_odd_odd", NpgsqlDbType.Text, p_odd_odd);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_odd", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(p_odd_outcome))
                {
                    command.Parameters.AddWithValue("p_odd_outcome", NpgsqlDbType.Text, p_odd_outcome);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_outcome", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(p_odd_outcome_id))
                {
                    command.Parameters.AddWithValue("p_odd_outcome_id", NpgsqlDbType.Text, p_odd_outcome_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_outcome_id", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(p_odd_probability))
                {
                    command.Parameters.AddWithValue("p_odd_probability", NpgsqlDbType.Text, p_odd_probability);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_probability", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(p_odd_special))
                {
                    command.Parameters.AddWithValue("p_odd_special", NpgsqlDbType.Text, p_odd_special);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_special", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(p_odd_type))
                {
                    command.Parameters.AddWithValue("p_odd_type", NpgsqlDbType.Text, p_odd_type);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_type", NpgsqlDbType.Text, DBNull.Value);
                }

                if (p_odd_type_id != null)
                {

                    command.Parameters.AddWithValue("p_odd_type_id", NpgsqlDbType.Bigint, p_odd_type_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_type_id", NpgsqlDbType.Bigint, DBNull.Value);
                }

                return insert(command);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return -1;
            }
        }
        public bool updateCpLcooOdds(long match_id, long odd_type_id)
        {
            try
            {
                var command = new NpgsqlCommand(Globals.DB_Functions.UpdateCpLcooOdds.ToDescription());
                command.Parameters.AddWithValue("p_odd_odd", NpgsqlDbType.Text, "OFF");
                command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, match_id);
                command.Parameters.AddWithValue("p_odd_type_id", NpgsqlDbType.Bigint, odd_type_id);
                insert(command);
                return true;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Insert Pre Match Odds
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="p_match_id"></param>
        /// <param name="p_odd_type_id"></param>
        /// <param name="p_odd_type_name"></param>
        /// <param name="p_odd_name"></param>
        /// <param name="p_odd_visible"></param>
        /// <returns></returns>
        public long insertCpLcooOdds(OddsEntity entity, long p_match_id, long p_odd_type_id, string p_odd_type_name, string p_odd_name, Boolean p_odd_visible, string p_odd_probability)
        {

            var command = new NpgsqlCommand(Globals.DB_Functions.InsertCpLcooOdds.ToDescription());

            try
            {
                if (p_match_id != null)
                {
                    command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, p_match_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (p_odd_type_id != null)
                {
                    command.Parameters.AddWithValue("p_odd_type_id", NpgsqlDbType.Bigint, p_odd_type_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_type_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_odd_type_name))
                {
                    command.Parameters.AddWithValue("p_odd_type_name", NpgsqlDbType.Text, p_odd_type_name);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_type_name", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entity.Id != null)
                {
                    command.Parameters.AddWithValue("p_odd_id", NpgsqlDbType.Bigint, entity.Id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.OutCome))
                {
                    command.Parameters.AddWithValue("p_odd_outcome", NpgsqlDbType.Text, entity.OutCome);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_outcome", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.OutcomeId))
                {
                    command.Parameters.AddWithValue("p_odd_outcome_id", NpgsqlDbType.Text, entity.OutcomeId);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_outcome_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.PlayerId))
                {
                    command.Parameters.AddWithValue("p_odd_player_id", NpgsqlDbType.Text, entity.PlayerId);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_player_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.TeamId))
                {
                    command.Parameters.AddWithValue("p_odd_team_id", NpgsqlDbType.Text, entity.TeamId);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_team_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_odd_name))
                {
                    command.Parameters.AddWithValue("p_odd_name", NpgsqlDbType.Text, p_odd_name);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_name", NpgsqlDbType.Text, DBNull.Value);
                }
                if (p_odd_visible != null)
                {
                    command.Parameters.AddWithValue("p_odd_visible", NpgsqlDbType.Boolean, p_odd_visible);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_visible", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.SpecialBetValue))
                {
                    command.Parameters.AddWithValue("p_odd_special", NpgsqlDbType.Text, entity.SpecialBetValue);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_special", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.Value))
                {
                    command.Parameters.AddWithValue("p_odd_odd", NpgsqlDbType.Text, entity.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_odd", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_odd_probability))
                {
                    command.Parameters.AddWithValue("p_odd_probability", NpgsqlDbType.Text, p_odd_probability);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_probability", NpgsqlDbType.Text, DBNull.Value);
                }
                return insert(command);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return -1;
            }
        }
        /// <summary>
        /// Insert 
        /// </summary>
        /// <param name="Lineupentity"></param>
        /// Returns custom type Return Queue Long;        /// Queue: is the sum of other inserts " if applied " to add to the mother queue in case of rollback        /// Long: is the main inserted row id        /// 
        /// <returns>ReturnQueueLong</returns>
        public static List<List<string>> duplicateJsonToArray(string json)
        {
            json = json.Replace("[", "");
            json = json.Replace("]", "");
            json = json.Replace("{", "");
            json = json.Replace("}", "");
            json = json.Replace("\"", "");
            json = json.Replace("(", "");
            json = json.Replace(")", "");
            json = json.Replace(" ", "");

            List<List<string>> list = new List<List<string>>();
            List<string> sublist = new List<string>();
            String key = "";
            String value = "";
            bool key_door = true;
            bool value_door = false;
            foreach (var element in json)
            {

                if (Convert.ToInt64(element) != 58 && key_door == true && Convert.ToInt64(element) != 44)
                {
                    key += element;
                }
                if (Convert.ToInt64(element) == 58)
                {
                    key_door = false;
                    value_door = true;
                }
                if (Convert.ToInt64(element) != 44 && value_door == true && Convert.ToInt64(element) != 58)
                {
                    value += element;
                }
                if (Convert.ToInt64(element) == 44)
                {
                    key_door = true;
                    value_door = false;
                    sublist.Add(key);
                    sublist.Add(value);
                    /*ı need to delete the content of the string right here but i could not because i got a error what can i do*/
                    value = "";
                    key = "";
                }


            }
            sublist.Add(key);
            sublist.Add(value);
            list.Add(sublist);

            return list;
        }
        public void WorkAlive(AliveEventArgs e)
        {
            foreach (var head in e.Alive.EventHeaders)
            {
                //Task.Factory.StartNew(() => insertMatchDataAllDetails((MatchHeader)head, null));
                insertMatchDataAllDetails((MatchHeader)head, null);
            }
        }
        public Merchants selectDyMerchants(long p_merchant_id, string p_username)
        {
            var dyMerchants = new Merchants();
            var command = new NpgsqlCommand(Globals.DB_Functions.selectDyMerchants.ToDescription());

            if (!string.IsNullOrEmpty(p_username))
            {
                command.Parameters.AddWithValue("p_username", NpgsqlDbType.Text, p_username);
            }
            else
            {
                command.Parameters.AddWithValue("p_username", NpgsqlDbType.Text, DBNull.Value);
            }

            if (p_merchant_id != null)
            {
                command.Parameters.AddWithValue("p_merchant_id", NpgsqlDbType.Bigint, p_merchant_id);
            }
            else
            {
                command.Parameters.AddWithValue("p_merchant_id", NpgsqlDbType.Bigint, DBNull.Value);
            }

            var ds = select(command);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dyMerchants.id = Convert.ToInt64((ds.Tables[0].Rows[0]["id"]));
                    dyMerchants.username = Convert.ToString((ds.Tables[0].Rows[0]["username"]));
                    dyMerchants.merchant_name = Convert.ToString((ds.Tables[0].Rows[0]["merchant_name"]));
                    dyMerchants.prefix = Convert.ToString((ds.Tables[0].Rows[0]["prefix"]));
                    dyMerchants.vendor_id = Convert.ToInt32((ds.Tables[0].Rows[0]["vendor_id"]));
                    dyMerchants.domain_m = Convert.ToString((ds.Tables[0].Rows[0]["domain_m"]));
                    dyMerchants.last_update = Convert.ToDateTime((ds.Tables[0].Rows[0]["last_update"]));
                    dyMerchants.profit_margin = Convert.ToDecimal((ds.Tables[0].Rows[0]["profit_margin"]));
                    dyMerchants.seamlessurl = Convert.ToString((ds.Tables[0].Rows[0]["seamlessurl"]));
                    dyMerchants.skin = Convert.ToString((ds.Tables[0].Rows[0]["skin"]));
                }
            }

            if (dyMerchants != null)
            {
                return dyMerchants;
            }
            else
            {
                return null;
            }

        }
        public DataSet selectotherOutcomesMarket(long p_match_id, long p_odd_type_id,string p_mid_otid_ocid_sid, string p_odd_special)
        {
            var dyMerchants = new Merchants();
            var command = new NpgsqlCommand(Globals.DB_Functions.GetOtherMarketsOutcimes.ToDescription());



            if (p_match_id != null)
            {
                command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, p_match_id);
            }
            else
            {
                command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, DBNull.Value);
            }

            if (p_odd_type_id != null)
            {
                command.Parameters.AddWithValue("p_odd_type_id", NpgsqlDbType.Bigint, p_odd_type_id);
            }
            else
            {
                command.Parameters.AddWithValue("p_odd_type_id", NpgsqlDbType.Bigint, DBNull.Value);
            }
            if (!string.IsNullOrEmpty(p_mid_otid_ocid_sid))
            {
                command.Parameters.AddWithValue("p_mid_otid_ocid_sid", NpgsqlDbType.Text, p_mid_otid_ocid_sid);
            }
            else
            {
                command.Parameters.AddWithValue("p_mid_otid_ocid_sid", NpgsqlDbType.Text, DBNull.Value);
            }
            if (!string.IsNullOrEmpty(p_odd_special))
            {
                command.Parameters.AddWithValue("p_odd_special", NpgsqlDbType.Text, p_odd_special);
            }
            else
            {
                command.Parameters.AddWithValue("p_odd_special", NpgsqlDbType.Text, DBNull.Value);
            }

            var ds = select(command);
           
            if (ds != null)
            {
                return ds;
            }
            else
            {
                return null;
            }

        }
        public long insertDyMerchants(Merchants merch)
        {

            var command = new NpgsqlCommand(Globals.DB_Functions.InsertDyMerchants.ToDescription());

            try
            {

                if (!string.IsNullOrEmpty(merch.username))
                {
                    command.Parameters.AddWithValue("p_username", NpgsqlDbType.Text, merch.username);
                }
                else
                {
                    command.Parameters.AddWithValue("p_username", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(merch.merchant_name))
                {
                    command.Parameters.AddWithValue("p_merchant_name", NpgsqlDbType.Text, merch.merchant_name);
                }
                else
                {
                    command.Parameters.AddWithValue("p_merchant_name", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(merch.prefix))
                {
                    command.Parameters.AddWithValue("p_prefix", NpgsqlDbType.Text, merch.prefix);
                }
                else
                {
                    command.Parameters.AddWithValue("p_prefix", NpgsqlDbType.Text, DBNull.Value);
                }

                if (merch.vendor_id != null)
                {
                    command.Parameters.AddWithValue("p_vendor_id", NpgsqlDbType.Integer, merch.vendor_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_vendor_id", NpgsqlDbType.Integer, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(merch.domain_m))
                {
                    command.Parameters.AddWithValue("p_domain_m", NpgsqlDbType.Text, merch.domain_m);
                }
                else
                {
                    command.Parameters.AddWithValue("p_domain_m", NpgsqlDbType.Text, DBNull.Value);
                }

                if (merch.last_update != null)
                {
                    command.Parameters.AddWithValue("p_last_update", NpgsqlDbType.Timestamp, merch.last_update);
                }
                else
                {
                    command.Parameters.AddWithValue("p_last_update", NpgsqlDbType.Timestamp, DBNull.Value);
                }

                if (merch.profit_margin != null)
                {
                    command.Parameters.AddWithValue("p_profit_margin", NpgsqlDbType.Numeric, merch.profit_margin);
                }
                else
                {
                    command.Parameters.AddWithValue("p_profit_margin", NpgsqlDbType.Numeric, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(merch.seamlessurl))
                {
                    command.Parameters.AddWithValue("p_seamlessurl", NpgsqlDbType.Text, merch.seamlessurl);
                }
                else
                {
                    command.Parameters.AddWithValue("p_seamlessurl", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(merch.skin))
                {
                    command.Parameters.AddWithValue("p_skin", NpgsqlDbType.Text, merch.skin);
                }
                else
                {
                    command.Parameters.AddWithValue("p_skin", NpgsqlDbType.Text, DBNull.Value);
                }

                var RetId = insert(command);
                if (RetId > 0)
                    return RetId;
                else
                    return -1;

            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
                return -1;
            }

        }
        public void insertMatch(MatchEntity m_entity, int feedtype)
        {
            var command = new NpgsqlCommand(Globals.DB_Functions.InsertMatchAllData.ToDescription());
            var sport_text = "";
            var tournament_name = "";
            var home_name = "";
            var away_name = "";
            long home_id = 0;
            long away_id = 0;
            var match = m_entity;
            try
            {

                command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, match.MatchId);

                if (match.Fixture != null && match.Fixture.Competitors != null)
                {
                    //home_name = match.Fixture.Competitors.Texts[0].Texts[0].Text[2].Value;
                    if (match.Fixture.Competitors != null)
                    {
                        home_name = TextsToJson(match.Fixture.Competitors.Texts, 0);
                        var superid = match.Fixture.Competitors.Texts[0].Texts[0].Superid;
                        if (superid != null)
                            home_id = superid.Value;
                        var superid2 = match.Fixture.Competitors.Texts[1].Texts[0].Superid;
                        if (superid2 != null)
                            away_id = superid2.Value;
                        away_name = TextsToJson(match.Fixture.Competitors.Texts, 1);
                    }
                    var first ="";
                    var second ="";
                    if (match.Fixture.Competitors.Texts.Count>0)
                    { 
                        if (match.Fixture.Competitors.Texts[0].Texts.Count> 0 && match.Fixture.Competitors.Texts[0].Texts[0].Text.Count>1)
                        {
                            first = match.Fixture.Competitors.Texts[0].Texts[0].Text[2].Value;
                            second = match.Fixture.Competitors.Texts[1].Texts[0].Text[2].Value;
                        }
                    }
                   
                    // away_name = match.Fixture.Competitors.Texts[1].Texts[0].Text[2].Value;
                    command.Parameters.AddWithValue("p_match_name", NpgsqlDbType.Text, first + @"|" + second);

                    if (home_id > 0)
                    {
                        command.Parameters.AddWithValue("p_home_team_id", NpgsqlDbType.Bigint, home_id);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("p_home_team_id", NpgsqlDbType.Bigint, DBNull.Value);
                    }
                    if (away_id > 0)
                    {
                        command.Parameters.AddWithValue("p_away_team_id", NpgsqlDbType.Bigint, away_id);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("p_away_team_id", NpgsqlDbType.Bigint, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(home_name))
                    {
                        command.Parameters.AddWithValue("p_home_team_name", NpgsqlDbType.Text, home_name);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("p_home_team_name", NpgsqlDbType.Text, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(away_name))
                    {
                        command.Parameters.AddWithValue("p_away_team_name", NpgsqlDbType.Text, away_name);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("p_away_team_name", NpgsqlDbType.Text, DBNull.Value);
                    }
                }
                if (match.Sport != null)
                {
                    command.Parameters.AddWithValue("p_sport_id", NpgsqlDbType.Bigint, match.Sport.Id);
                    //var dictionary = new Dictionary<string, string>();
                    if (match.Sport.Texts != null && match.Sport != null)
                    {
                        sport_text = TextsToJson(match.Sport.Texts);
                        command.Parameters.AddWithValue("p_sport_name", NpgsqlDbType.Text, sport_text);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("p_sport_name", NpgsqlDbType.Text, DBNull.Value);
                    }

                }
                else
                {
                    command.Parameters.AddWithValue("p_sport_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (match.Tournament != null)
                {
                    command.Parameters.AddWithValue("p_tournament_id", NpgsqlDbType.Bigint, match.Tournament.Id);
                    if (match.Tournament.Texts != null && match.Tournament != null)
                    {
                        command.Parameters.AddWithValue("p_tournament_name", NpgsqlDbType.Text, TextsToJson(match.Tournament.Texts));
                    }
                    else
                    {
                        command.Parameters.AddWithValue("p_tournament_name", NpgsqlDbType.Text, DBNull.Value);
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("p_tournament_id", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (match.Fixture.StatusInfo != null)
                {
                    command.Parameters.AddWithValue("p_is_active", NpgsqlDbType.Boolean, match.Fixture.StatusInfo.IsActive);
                }
                else
                {
                    command.Parameters.AddWithValue("p_is_active", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (match.Category.IsoName != null)
                {
                    command.Parameters.AddWithValue("country_iso", NpgsqlDbType.Text, match.Category.IsoName);
                }
                else
                {
                    command.Parameters.AddWithValue("country_iso", NpgsqlDbType.Text, DBNull.Value);
                }
                if (match.Fixture.DateInfo != null)
                {
                    command.Parameters.AddWithValue("match_start_date", NpgsqlDbType.Timestamp,
                        match.Fixture.DateInfo.MatchDate);
                }
                else
                {
                    command.Parameters.AddWithValue("match_start_date", NpgsqlDbType.Timestamp, DBNull.Value);
                }
                command.Parameters.AddWithValue("p_feed_type", NpgsqlDbType.Integer, feedtype);
                var id = insert(command);

            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }
        public void insertMatchLocal(MatchLocal m_entity, int feedtype)
        {
            var match = m_entity;
            var command = new NpgsqlCommand(Globals.DB_Functions.InsertMatchAllData.ToDescription());
            long home_id = match.home_team_id;
            long away_id = match.away_team_id;
            long sport_id = match.sport_id;
            long tournament_id = match.tournament_id;
            try
            {

                command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, match.match_id);
                command.Parameters.AddWithValue("p_match_name", NpgsqlDbType.Text, match.match_name);
                if (home_id > 0)
                {
                    command.Parameters.AddWithValue("p_home_team_id", NpgsqlDbType.Bigint, home_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_home_team_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (away_id > 0)
                {
                    command.Parameters.AddWithValue("p_away_team_id", NpgsqlDbType.Bigint, away_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_away_team_id", NpgsqlDbType.Bigint, DBNull.Value);
                }

                command.Parameters.AddWithValue("p_home_team_name", NpgsqlDbType.Text, match.home_team_name);
                command.Parameters.AddWithValue("p_away_team_name", NpgsqlDbType.Text, match.away_team_name);
                if (sport_id > 0)
                {
                    command.Parameters.AddWithValue("p_sport_id", NpgsqlDbType.Bigint, sport_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_sport_id", NpgsqlDbType.Bigint, DBNull.Value);
                }

                command.Parameters.AddWithValue("p_sport_name", NpgsqlDbType.Text, match.sport_name);
                if (tournament_id > 0)
                {
                    command.Parameters.AddWithValue("p_tournament_id", NpgsqlDbType.Bigint, tournament_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_tournament_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                command.Parameters.AddWithValue("p_tournament_name", NpgsqlDbType.Text, match.tournament_name);
                command.Parameters.AddWithValue("p_is_active", NpgsqlDbType.Boolean, match.is_active);
                command.Parameters.AddWithValue("country_iso", NpgsqlDbType.Text, DBNull.Value);
                command.Parameters.AddWithValue("match_start_date", NpgsqlDbType.Timestamp, match.match_start_date);
                command.Parameters.AddWithValue("p_feed_type", NpgsqlDbType.Integer, feedtype);

                var id = insert(command);
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }
        public MatchLocal ConvertLiveMatchToLcooMatch(MatchHeader header, MatchInfo info)
        {
            var match = new MatchLocal();
            try
            {
                match.match_id = header.Id;
                match.away_team_id = info.AwayTeam.Id ?? 0;
                match.away_team_name = IdNameTupleToJson(info.AwayTeam);
                match.country_iso = IdNameTupleToJson(info.Category);
                match.home_team_id = info.HomeTeam.Id ?? 0;
                match.home_team_name = IdNameTupleToJson(info.HomeTeam);
                match.is_active = header.Active;
                match.match_name = info.HomeTeam.Name.International + @"|" + info.AwayTeam.Name.International;
                match.match_start_date = info.DateOfMatch ?? DateTime.Parse(null);
                match.sport_id = info.Sport.Id ?? 0;
                match.sport_name = IdNameTupleToJson(info.Sport);
                match.tournament_id = info.Tournament.Id ?? 0;
                match.tournament_name = IdNameTupleToJson(info.Tournament);
                return match;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }
        public long insertMatchDataAllDetails(MatchHeader entityMheader, MatchInfo entityMinfo)
        {

            var queue = new Queue<Globals.Rollback>();
            var ObjCommand = new NpgsqlCommand(Globals.DB_Functions.InsertDyMatchDataAllDetails.ToDescription());
            try
            {
                var gogogog = entityMheader.Id;
                if (entityMinfo != null)
                {
                    insertMatchLocal(ConvertLiveMatchToLcooMatch(entityMheader, entityMinfo), 2);
                }

                if (entityMheader.Active != null)
                {

                    ObjCommand.Parameters.AddWithValue("p_active", NpgsqlDbType.Boolean, entityMheader.Active);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_active", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (entityMheader.AutoTraded != null)
                {

                    ObjCommand.Parameters.AddWithValue("p_auto_traded", NpgsqlDbType.Boolean, entityMheader.AutoTraded);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_auto_traded", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (entityMheader.Balls != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_balls", NpgsqlDbType.Bigint, entityMheader.Balls);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_balls", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entityMheader.Bases))
                {
                    ObjCommand.Parameters.AddWithValue("p_bases", NpgsqlDbType.Text, entityMheader.Bases);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_bases", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.Batter != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_batter", NpgsqlDbType.Text, new JavaScriptSerializer().Serialize(entityMheader.Batter));
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_batter", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.BetStatus != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_bet_status", NpgsqlDbType.Text, entityMheader.BetStatus);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_bet_status", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.BetStopReason != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_bet_stop_reason", NpgsqlDbType.Text, new JavaScriptSerializer().Serialize(entityMheader.BetStopReason));
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_bet_stop_reason", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.Booked != null)
                {

                    ObjCommand.Parameters.AddWithValue("p_booked", NpgsqlDbType.Boolean, entityMheader.Booked);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_booked", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entityMheader.ClearedScore))
                {
                    ObjCommand.Parameters.AddWithValue("p_cleared_score", NpgsqlDbType.Text, entityMheader.ClearedScore);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_cleared_score", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.ClockStopped != null)
                {

                    ObjCommand.Parameters.AddWithValue("p_clock_stopped", NpgsqlDbType.Boolean, entityMheader.ClockStopped);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_clock_stopped", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (entityMheader.Corners != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_corners", NpgsqlDbType.Text, new JavaScriptSerializer().Serialize(entityMheader.Corners));
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_corners", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.CurrentCtTeam != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_current_ct_team", NpgsqlDbType.Text, entityMheader.CurrentCtTeam);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_current_ct_team", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.CurrentEnd != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_current_end", NpgsqlDbType.Bigint, entityMheader.CurrentEnd);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_current_end", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entityMheader.Delivery != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_delivery", NpgsqlDbType.Text, entityMheader.Delivery);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_delivery", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.Dismissals != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_dismissals", NpgsqlDbType.Text, new JavaScriptSerializer().Serialize(entityMheader.Dismissals));
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_dismissals", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.EarlyBetStatus != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_early_bet_status", NpgsqlDbType.Text, entityMheader.EarlyBetStatus);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_early_bet_status", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.Expedite != null)
                {

                    ObjCommand.Parameters.AddWithValue("p_expedite", NpgsqlDbType.Boolean, entityMheader.Expedite);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_expedite", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entityMheader.GameScore))
                {
                    ObjCommand.Parameters.AddWithValue("p_game_score", NpgsqlDbType.Text, entityMheader.GameScore);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_game_score", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.Innings != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_innings", NpgsqlDbType.Bigint, entityMheader.Innings);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_innings", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entityMheader.LegScore))
                {
                    ObjCommand.Parameters.AddWithValue("p_leg_score", NpgsqlDbType.Text, entityMheader.LegScore);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_leg_score", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.Id != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, entityMheader.Id);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entityMheader.MatchTime != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_match_time", NpgsqlDbType.Bigint, entityMheader.MatchTime);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_match_time", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entityMheader.MatchTimeExtended))
                {
                    ObjCommand.Parameters.AddWithValue("p_match_time_extended", NpgsqlDbType.Text, entityMheader.MatchTimeExtended);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_match_time_extended", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.Message != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_message", NpgsqlDbType.Text, new JavaScriptSerializer().Serialize(entityMheader.Message));
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_message", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.Msgnr != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_msgnr", NpgsqlDbType.Bigint, entityMheader.Msgnr);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_msgnr", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entityMheader.Outs != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_outs", NpgsqlDbType.Bigint, entityMheader.Outs);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_outs", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entityMheader.Over != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_mh_over", NpgsqlDbType.Bigint, entityMheader.Over);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_mh_over", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entityMheader.PentaltyRuns != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_pentalty_runs", NpgsqlDbType.Text, new JavaScriptSerializer().Serialize(entityMheader.PentaltyRuns));
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_pentalty_runs", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.Position != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_mh_position", NpgsqlDbType.Bigint, entityMheader.Position);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_mh_position", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entityMheader.Possession != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_possession", NpgsqlDbType.Text, entityMheader.Possession);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_possession", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.RedCards != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_red_cards", NpgsqlDbType.Text, new JavaScriptSerializer().Serialize(entityMheader.RedCards));
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_red_cards", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.RemainingBowls != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_remaining_bowls", NpgsqlDbType.Text, new JavaScriptSerializer().Serialize(entityMheader.RemainingBowls));
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_remaining_bowls", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.RemainingReds != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_remaining_reds", NpgsqlDbType.Bigint, entityMheader.RemainingReds);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_remaining_reds", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entityMheader.RemainingTime))
                {
                    ObjCommand.Parameters.AddWithValue("p_remaining_time", NpgsqlDbType.Text, entityMheader.RemainingTime);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_remaining_time", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entityMheader.RemainingTimeInPeriod))
                {
                    ObjCommand.Parameters.AddWithValue("p_remaining_time_in_period", NpgsqlDbType.Text, entityMheader.RemainingTimeInPeriod);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_remaining_time_in_period", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entityMheader.Score))
                {
                    ObjCommand.Parameters.AddWithValue("p_score", NpgsqlDbType.Text, entityMheader.Score);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_score", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.Server != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_mh_server", NpgsqlDbType.Bigint, entityMheader.Server);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_mh_server", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.SetScores != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_set_scores", NpgsqlDbType.Text, new JavaScriptSerializer().Serialize(entityMheader.SetScores));
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_set_scores", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(entityMheader.SourceId))
                {
                    ObjCommand.Parameters.AddWithValue("p_source_id", NpgsqlDbType.Text, entityMheader.SourceId);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_source_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.Status != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_status", NpgsqlDbType.Text, entityMheader.Status);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_status", NpgsqlDbType.Text, DBNull.Value);
                }


                if (entityMheader.Strikes != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_strikes", NpgsqlDbType.Bigint, entityMheader.Strikes);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_strikes", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entityMheader.Suspend != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_suspend", NpgsqlDbType.Text, new JavaScriptSerializer().Serialize(entityMheader.Suspend));
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_suspend", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.Throw != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_throw", NpgsqlDbType.Bigint, entityMheader.Throw);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_throw", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entityMheader.TieBreak != null)
                {

                    ObjCommand.Parameters.AddWithValue("p_tie_break", NpgsqlDbType.Boolean, entityMheader.TieBreak);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_tie_break", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (entityMheader.Try != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_try", NpgsqlDbType.Bigint, entityMheader.Try);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_try", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entityMheader.Visit != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_visit", NpgsqlDbType.Bigint, entityMheader.Visit);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_visit", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entityMheader.Yards != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_yards", NpgsqlDbType.Bigint, entityMheader.Yards);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_yards", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entityMheader.YellowCards != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_yellow_cards", NpgsqlDbType.Text, new JavaScriptSerializer().Serialize(entityMheader.YellowCards));
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_yellow_cards", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMheader.YellowRedCards != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_yellow_red_cards", NpgsqlDbType.Text, new JavaScriptSerializer().Serialize(entityMheader.YellowRedCards));
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_yellow_red_cards", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entityMinfo == null)
                {
                    ObjCommand.Parameters.AddWithValue("p_away_team", NpgsqlDbType.Text, DBNull.Value);
                    ObjCommand.Parameters.AddWithValue("p_bet_pal", NpgsqlDbType.Text, DBNull.Value);
                    ObjCommand.Parameters.AddWithValue("p_category", NpgsqlDbType.Text, DBNull.Value);
                    ObjCommand.Parameters.AddWithValue("p_coverage_info", NpgsqlDbType.Text, DBNull.Value);
                    ObjCommand.Parameters.AddWithValue("p_date_of_match", NpgsqlDbType.Timestamp, DBNull.Value);
                    ObjCommand.Parameters.AddWithValue("p_extra_info", NpgsqlDbType.Text, DBNull.Value);
                    ObjCommand.Parameters.AddWithValue("p_home_team", NpgsqlDbType.Text, DBNull.Value);
                    ObjCommand.Parameters.AddWithValue("p_round", NpgsqlDbType.Text, DBNull.Value);
                    ObjCommand.Parameters.AddWithValue("p_season", NpgsqlDbType.Text, DBNull.Value);
                    ObjCommand.Parameters.AddWithValue("p_sport", NpgsqlDbType.Text, DBNull.Value);
                    ObjCommand.Parameters.AddWithValue("p_streaming_channels", NpgsqlDbType.Text, DBNull.Value);
                    ObjCommand.Parameters.AddWithValue("p_tournament", NpgsqlDbType.Text, DBNull.Value);
                    ObjCommand.Parameters.AddWithValue("p_tv_channels", NpgsqlDbType.Text, DBNull.Value);
                }
                else
                {
                    if (entityMinfo.AwayTeam != null)
                    {
                        ObjCommand.Parameters.AddWithValue("p_away_team", NpgsqlDbType.Text, IdNameTupleToJson(entityMinfo.AwayTeam));
                    }
                    else
                    {
                        ObjCommand.Parameters.AddWithValue("p_away_team", NpgsqlDbType.Text, DBNull.Value);
                    }
                    if (entityMinfo.BetPal != null)
                    {
                        ObjCommand.Parameters.AddWithValue("p_bet_pal", NpgsqlDbType.Text,
                            new JavaScriptSerializer().Serialize(entityMinfo.BetPal));
                    }
                    else
                    {
                        ObjCommand.Parameters.AddWithValue("p_bet_pal", NpgsqlDbType.Text, DBNull.Value);
                    }
                    if (entityMinfo.Category != null)
                    {
                        ObjCommand.Parameters.AddWithValue("p_category", NpgsqlDbType.Text, IdNameTupleToJson(entityMinfo.Category));
                    }
                    else
                    {
                        ObjCommand.Parameters.AddWithValue("p_category", NpgsqlDbType.Text, DBNull.Value);
                    }

                    if (entityMinfo.CoverageInfo != null)
                    {
                        ObjCommand.Parameters.AddWithValue("p_coverage_info", NpgsqlDbType.Text, IdNameTupleToJson(entityMinfo.CoverageInfo));
                    }
                    else
                    {
                        ObjCommand.Parameters.AddWithValue("p_coverage_info", NpgsqlDbType.Text, DBNull.Value);
                    }

                    if (entityMinfo.DateOfMatch != null)
                    {

                        ObjCommand.Parameters.AddWithValue("p_date_of_match", NpgsqlDbType.Timestamp,
                            entityMinfo.DateOfMatch);
                    }
                    else
                    {
                        ObjCommand.Parameters.AddWithValue("p_date_of_match", NpgsqlDbType.Timestamp, DBNull.Value);
                    }

                    if (entityMinfo.ExtraInfo != null)
                    {
                        ObjCommand.Parameters.AddWithValue("p_extra_info", NpgsqlDbType.Text,
                            new JavaScriptSerializer().Serialize(entityMinfo.ExtraInfo));
                    }
                    else
                    {
                        ObjCommand.Parameters.AddWithValue("p_extra_info", NpgsqlDbType.Text, DBNull.Value);
                    }

                    if (entityMinfo.HomeTeam != null)
                    {
                        ObjCommand.Parameters.AddWithValue("p_home_team", NpgsqlDbType.Text, IdNameTupleToJson(entityMinfo.HomeTeam));
                    }
                    else
                    {
                        ObjCommand.Parameters.AddWithValue("p_home_team", NpgsqlDbType.Text, DBNull.Value);
                    }

                    if (entityMinfo.Round != null)
                    {
                        ObjCommand.Parameters.AddWithValue("p_round", NpgsqlDbType.Text, IdNameTupleToJson(entityMinfo.Round));
                    }
                    else
                    {
                        ObjCommand.Parameters.AddWithValue("p_round", NpgsqlDbType.Text, DBNull.Value);
                    }
                    if (entityMinfo.Season != null)
                    {
                        ObjCommand.Parameters.AddWithValue("p_season", NpgsqlDbType.Text, IdNameTupleToJson(entityMinfo.Season));
                    }
                    else
                    {
                        ObjCommand.Parameters.AddWithValue("p_season", NpgsqlDbType.Text, DBNull.Value);
                    }
                    if (entityMinfo.Sport != null)
                    {
                        ObjCommand.Parameters.AddWithValue("p_sport", NpgsqlDbType.Text, IdNameTupleToJson(entityMinfo.Sport));
                    }
                    else
                    {
                        ObjCommand.Parameters.AddWithValue("p_sport", NpgsqlDbType.Text, DBNull.Value);
                    }
                    if (entityMinfo.StreamingChannels != null && entityMinfo.StreamingChannels.Count > 0)
                    {
                        ObjCommand.Parameters.AddWithValue("p_streaming_channels", NpgsqlDbType.Text, IdNameTupleToJson(entityMinfo.StreamingChannels));
                    }
                    else
                    {
                        ObjCommand.Parameters.AddWithValue("p_streaming_channels", NpgsqlDbType.Text, DBNull.Value);
                    }
                    if (entityMinfo.Tournament != null)
                    {
                        ObjCommand.Parameters.AddWithValue("p_tournament", NpgsqlDbType.Text, IdNameTupleToJson(entityMinfo.Tournament));
                    }
                    else
                    {
                        ObjCommand.Parameters.AddWithValue("p_tournament", NpgsqlDbType.Text, DBNull.Value);
                    }
                    if (entityMinfo.TvChannels != null)
                    {
                        ObjCommand.Parameters.AddWithValue("p_tv_channels", NpgsqlDbType.Text,
                            new JavaScriptSerializer().Serialize(entityMinfo.TvChannels));
                    }
                    else
                    {
                        ObjCommand.Parameters.AddWithValue("p_tv_channels", NpgsqlDbType.Text, DBNull.Value);
                    }
                }

                //var ret = new Globals.ReturnQueueLong(queue, insert(ObjCommand));
                //if (ret.id > 0 )
                //{
                //    SharedLibrary.Logg.logger.Error("Match Details Inserted !!!");
                //}
                return insert(ObjCommand);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return -1;
            }
        }
        public bool updateDmadYellowRedCard(ScoreCardSummary entity, long p_match_id)
        {

            try
            {
                var command = new NpgsqlCommand(Globals.DB_Functions.UpdateDmadYellowRedCard.ToDescription());
                command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, p_match_id);
                if (entity.CardsByTime != null)
                {
                    command.Parameters.AddWithValue("p_yellow_red_cards", NpgsqlDbType.Text,
                            new JavaScriptSerializer().Serialize(entity.CardsByTime));
                }
                else
                {
                    command.Parameters.AddWithValue("p_yellow_red_cards", NpgsqlDbType.Text, DBNull.Value);
                }
                insert(command);
                return true;
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
                return false;
            }
        }
        private long checkIfLiveOddExcists(string mid)
        {
            try
            {
                var commandText = "SELECT id from cp_live_odds WHERE mid_oid_oftid ='" + mid + "'";
                var command = new NpgsqlCommand(commandText);
                var id = selectOne(command);
                return id;
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
                return -1;
            }
        }
        public bool UpdateAllLiveOddsOutcomesActive(long match_id, EventOdds entity, bool Active)
        {
            try
            {
                var command = new NpgsqlCommand(Globals.DB_Functions.UpdateCpLiveOddsActive.ToDescription());
                command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, match_id);
                command.Parameters.AddWithValue("p_odd_id", NpgsqlDbType.Bigint, entity.Id);
                command.Parameters.AddWithValue("p_active", NpgsqlDbType.Boolean, Active);
                var ret = insert(command);
                if (ret == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
                return false;
            }
        }
        public Globals.ReturnQueueLong insertLiveOdds(EventOdds entity, EventOddsField odd_field,
             bool p_odd_eventoddsfield_active, bool? p_odd_eventoddsfield_outcomestatus,
             long? p_odd_eventoddsfield_playerid, string p_odd_eventoddsfield_probability, LocalizedString p_odd_eventoddsfield_type,
             string p_odd_eventoddsfield_value, int? p_odd_eventoddsfield_viewindex, string p_odd_eventoddsfield_voidfactor,
             string p_odd_eventoddsfield_name,
             long match_id, int? p_odd_eventoddsfield_typeid, string status, string timestamp)
        {

            var oddUnique = new BetClearQueueElementLive();
            oddUnique.MatchId = match_id;
            oddUnique.OddId = entity.Id;
            if (p_odd_eventoddsfield_typeid != null)
            {
                oddUnique.TypeId = p_odd_eventoddsfield_typeid;
            }
            else
            {
                oddUnique.TypeId = null;
            }
            var before_unique = EncodeUnifiedBetClearQueueElementLive(oddUnique);
            //var RowId = checkIfLiveOddExcists(before_unique);
            var TypeDictionary = new Dictionary<string, string>();
            var NameDictionary = new Dictionary<string, string>();
            try
            {

                if (p_odd_eventoddsfield_type != null)
                {
                    TypeDictionary.Add("BET", p_odd_eventoddsfield_type.International);
                    TypeDictionary.Add("en", p_odd_eventoddsfield_type.International);
                    foreach (var language in p_odd_eventoddsfield_type.AvailableTranslationLanguages)
                    {
                        TypeDictionary.Add(language, p_odd_eventoddsfield_type.GetTranslation(language));
                    }
                }

                if (entity.Name != null)
                {
                    NameDictionary.Add("BET", entity.Name.International);
                    NameDictionary.Add("en", entity.Name.International);
                    foreach (var language in entity.Name.AvailableTranslationLanguages)
                    {
                        NameDictionary.Add(language, entity.Name.GetTranslation(language));
                    }
                }

                //if (p_odd_eventoddsfield_outcomestatus == null)
                //{
                //    foreach (var name in NameDictionary)
                //    {
                //        var channel = CreateLiveOddsChannelName(match_id, name.Key);
                //        SendToHybridgeSocketNewOdd(match_id, entity.Id, p_odd_eventoddsfield_typeid,
                //            new JavaScriptSerializer().Serialize(name), entity.SpecialOddsValue, odd_field, channel);
                //    }
                //}


            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
            var queue = new Queue<Globals.Rollback>();
            var command = new NpgsqlCommand(Globals.DB_Functions.InsertLiveOdds.ToDescription());
            try
            {
                //if (RowId < 0 && p_odd_eventoddsfield_outcomestatus == null)
                //{
                //    if (p_odd_eventoddsfield_outcomestatus == null)
                //    {
                //        foreach (var name in NameDictionary)
                //        {
                //            var channel = CreateLiveOddsChannelName(match_id, name.Key);
                //            SendToHybridgeSocketNewOdd(match_id, entity.Id, p_odd_eventoddsfield_typeid,new JavaScriptSerializer().Serialize(name), entity.SpecialOddsValue, odd_field, channel);
                //        }
                //    }

                command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, match_id);
                command.Parameters.AddWithValue("p_odd_active", NpgsqlDbType.Boolean, entity.Active);
                if (entity.Changed != null)
                {
                    command.Parameters.AddWithValue("p_odd_changed", NpgsqlDbType.Boolean, entity.Changed);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_changed", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (entity.Combination != null)
                {
                    command.Parameters.AddWithValue("p_odd_combination", NpgsqlDbType.Bigint, entity.Combination);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_combination", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!String.IsNullOrEmpty(entity.ForTheRest))
                {
                    command.Parameters.AddWithValue("p_odd_for_the_rest", NpgsqlDbType.Text, entity.ForTheRest);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_for_the_rest", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!String.IsNullOrEmpty(entity.Freetext))
                {
                    command.Parameters.AddWithValue("p_odd_free_text", NpgsqlDbType.Text, entity.Freetext);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_free_text", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entity.Id != null)
                {
                    command.Parameters.AddWithValue("p_odd_id", NpgsqlDbType.Bigint, entity.Id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entity.MostBalanced != null)
                {
                    command.Parameters.AddWithValue("p_odd_most_balanced", NpgsqlDbType.Boolean, entity.MostBalanced);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_most_balanced", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (NameDictionary.Count > 0)
                {
                    command.Parameters.AddWithValue("p_odd_name", NpgsqlDbType.Text,
                        new JavaScriptSerializer().Serialize(NameDictionary));
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_name", NpgsqlDbType.Text, DBNull.Value);
                }
                if (p_odd_eventoddsfield_active != null)
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_active", NpgsqlDbType.Boolean,
                        p_odd_eventoddsfield_active);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_active", NpgsqlDbType.Boolean,
                       DBNull.Value);
                }

                if (p_odd_eventoddsfield_outcomestatus != null)
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_outcomestatus", NpgsqlDbType.Boolean,
                        p_odd_eventoddsfield_outcomestatus);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_outcomestatus", NpgsqlDbType.Boolean,
                        DBNull.Value);
                }
                if (p_odd_eventoddsfield_playerid != null)
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_playerid", NpgsqlDbType.Bigint,
                        p_odd_eventoddsfield_playerid);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_playerid", NpgsqlDbType.Bigint,
                        DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_odd_eventoddsfield_probability))
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_probability", NpgsqlDbType.Text,
                        p_odd_eventoddsfield_probability);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_probability", NpgsqlDbType.Text,
                        DBNull.Value);
                }


                if (TypeDictionary.Count > 0)
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_type", NpgsqlDbType.Text,
                        new JavaScriptSerializer().Serialize(TypeDictionary));
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_type", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(p_odd_eventoddsfield_value))
                {

                    command.Parameters.AddWithValue("p_odd_eventoddsfield_value", NpgsqlDbType.Text,
                        p_odd_eventoddsfield_value);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_value", NpgsqlDbType.Text, DBNull.Value);
                }
                if(p_odd_eventoddsfield_viewindex != null)
                    { 
                command.Parameters.AddWithValue("p_odd_eventoddsfield_viewindex", NpgsqlDbType.Integer,
                    p_odd_eventoddsfield_viewindex);
                    }
                else
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_viewindex", NpgsqlDbType.Integer,
                  DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_odd_eventoddsfield_voidfactor))
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_voidfactor", NpgsqlDbType.Text,
                        p_odd_eventoddsfield_voidfactor);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_voidfactor", NpgsqlDbType.Text,
                        DBNull.Value);
                }

                if (!string.IsNullOrEmpty(p_odd_eventoddsfield_name))
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_name", NpgsqlDbType.Text,
                        p_odd_eventoddsfield_name);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_name", NpgsqlDbType.Text,
                       DBNull.Value);
                }

                if (!string.IsNullOrEmpty(entity.SpecialOddsValue))
                {
                    command.Parameters.AddWithValue("p_odd_special_odds_value", NpgsqlDbType.Text,
                        entity.SpecialOddsValue);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_special_odds_value", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entity.SubType != null)
                {
                    command.Parameters.AddWithValue("p_odd_sub_type", NpgsqlDbType.Bigint, entity.SubType);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_sub_type", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entity.Type != null)
                {
                    command.Parameters.AddWithValue("p_odd_type", NpgsqlDbType.Text, entity.Type);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_type", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entity.TypeId != null)
                {
                    command.Parameters.AddWithValue("p_odd_type_id", NpgsqlDbType.Bigint, entity.TypeId);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_type_id", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (p_odd_eventoddsfield_typeid != null)
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_typeid", NpgsqlDbType.Integer, p_odd_eventoddsfield_typeid);
                }
                else
                {
                    command.Parameters.AddWithValue("p_odd_eventoddsfield_typeid", NpgsqlDbType.Integer, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(status))
                {
                    command.Parameters.AddWithValue("p_status", NpgsqlDbType.Text, status);
                }
                else
                {
                    command.Parameters.AddWithValue("p_status", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(timestamp))
                {
                    command.Parameters.AddWithValue("p_time_stamp", NpgsqlDbType.Timestamp, DateTime.Parse(timestamp));
                }
                else
                {
                    command.Parameters.AddWithValue("p_time_stamp", NpgsqlDbType.Timestamp, DBNull.Value);
                }

                return new Globals.ReturnQueueLong(queue, insert(command));
                // }
                //else
                //{


                //    return updateLiveOdds(new JavaScriptSerializer().Serialize(NameDictionary), entity.Active,
                //        entity.Changed, entity.Combination.Value, entity.ForTheRest, entity.Freetext,
                //        entity.MostBalanced, entity.SpecialOddsValue,
                //        entity.TypeId, p_odd_eventoddsfield_active, p_odd_eventoddsfield_outcomestatus,
                //        entity.Type.ToString(), p_odd_eventoddsfield_playerid,
                //        p_odd_eventoddsfield_probability, new JavaScriptSerializer().Serialize(TypeDictionary),
                //        p_odd_eventoddsfield_value, (int)p_odd_eventoddsfield_viewindex
                //        , p_odd_eventoddsfield_voidfactor, p_odd_eventoddsfield_name, RowId);
                //}
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
                return new Globals.ReturnQueueLong(queue, -1);
            }
        }

        public Globals.ReturnQueueLong updateLiveOdds(string entity_Name, bool p_odd_active, bool? p_odd_changed, long p_odd_combination,
             string p_odd_for_the_rest, string p_odd_free_text, bool? p_odd_most_balanced, string p_odd_special_odds_value, long p_odd_type_id,
              bool p_odd_eventoddsfield_active, bool? p_odd_eventoddsfield_outcomestatus, string p_odd_type,
              long? p_odd_eventoddsfield_playerid, string p_odd_eventoddsfield_probability, string p_odd_eventoddsfield_type,
              string p_odd_eventoddsfield_value, int p_odd_eventoddsfield_viewindex, string p_odd_eventoddsfield_voidfactor,
              string p_odd_eventoddsfield_name, long row_id)
        {

            var queue = new Queue<Globals.Rollback>();
            var Updatecommand = new NpgsqlCommand(Globals.DB_Functions.UpdateLiveOdds.ToDescription());
            try
            {
                Updatecommand.Parameters.AddWithValue("p_odd_active", NpgsqlDbType.Boolean, p_odd_active);
                if (p_odd_changed != null)
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_changed", NpgsqlDbType.Boolean, p_odd_changed);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_changed", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (p_odd_combination != null)
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_combination", NpgsqlDbType.Bigint, p_odd_combination);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_combination", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!String.IsNullOrEmpty(p_odd_for_the_rest))
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_for_the_rest", NpgsqlDbType.Text, p_odd_for_the_rest);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_for_the_rest", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!String.IsNullOrEmpty(p_odd_free_text))
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_free_text", NpgsqlDbType.Text, p_odd_free_text);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_free_text", NpgsqlDbType.Text, DBNull.Value);
                }

                if (p_odd_most_balanced != null)
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_most_balanced", NpgsqlDbType.Boolean, p_odd_most_balanced);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_most_balanced", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (!String.IsNullOrEmpty(entity_Name))
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_name", NpgsqlDbType.Text, entity_Name);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_name", NpgsqlDbType.Text, DBNull.Value);
                }

                Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_active", NpgsqlDbType.Boolean, p_odd_eventoddsfield_active);

                if (p_odd_eventoddsfield_outcomestatus != null)
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_outcomestatus", NpgsqlDbType.Boolean, p_odd_eventoddsfield_outcomestatus);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_outcomestatus", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (p_odd_eventoddsfield_playerid != null)
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_playerid", NpgsqlDbType.Bigint,
                        p_odd_eventoddsfield_playerid);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_playerid", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(p_odd_eventoddsfield_probability))
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_probability", NpgsqlDbType.Text,
                        p_odd_eventoddsfield_probability);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_probability", NpgsqlDbType.Text, DBNull.Value);
                }


                if (!string.IsNullOrEmpty(p_odd_eventoddsfield_type))
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_type", NpgsqlDbType.Text,
                        p_odd_eventoddsfield_type);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_type", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(p_odd_eventoddsfield_value))
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_value", NpgsqlDbType.Text, p_odd_eventoddsfield_value);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_value", NpgsqlDbType.Text, DBNull.Value);
                }

                Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_viewindex", NpgsqlDbType.Integer, p_odd_eventoddsfield_viewindex);

                if (!string.IsNullOrEmpty(p_odd_eventoddsfield_voidfactor))
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_voidfactor", NpgsqlDbType.Text, p_odd_eventoddsfield_voidfactor);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_voidfactor", NpgsqlDbType.Text, DBNull.Value);
                }


                Updatecommand.Parameters.AddWithValue("p_odd_eventoddsfield_name", NpgsqlDbType.Text, p_odd_eventoddsfield_name);

                if (!string.IsNullOrEmpty(p_odd_special_odds_value))
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_special_odds_value", NpgsqlDbType.Text, p_odd_special_odds_value);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_special_odds_value", NpgsqlDbType.Text, DBNull.Value);
                }

                if (p_odd_type != null)
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_type", NpgsqlDbType.Text, p_odd_type);
                }
                else
                {
                    Updatecommand.Parameters.AddWithValue("p_odd_type", NpgsqlDbType.Text, DBNull.Value);
                }

                Updatecommand.Parameters.AddWithValue("row_id", NpgsqlDbType.Bigint, row_id);

                return new Globals.ReturnQueueLong(queue, update(Updatecommand));
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
                return new Globals.ReturnQueueLong(queue, -1);
            }
        }

       

        #region Live

        #endregion

        #region Coupons
        /// <summary>
        /// Insert Bet Clears for Lcoo
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="MatchID"></param>
        /// <returns></returns>
        public Globals.ReturnQueueLong insertCpLcooBetclearOdds(BetResultEntity entity, long MatchID, string mid_otid_ocid_sid)
        {

            var queue = new Queue<Globals.Rollback>();
            var ObjCommand = new NpgsqlCommand(Globals.DB_Functions.InsertCpLcooBetClearOdds.ToDescription());
            try
            {
                if (MatchID != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, MatchID);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entity.OddsType != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_odds_type", NpgsqlDbType.Bigint, entity.OddsType);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_odds_type", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.Outcome))
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome", NpgsqlDbType.Text, entity.Outcome);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.OutcomeId))
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome_id", NpgsqlDbType.Text, entity.OutcomeId);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.PlayerId))
                {
                    ObjCommand.Parameters.AddWithValue("p_player_id", NpgsqlDbType.Text, entity.PlayerId);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_player_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.Reason))
                {
                    ObjCommand.Parameters.AddWithValue("p_reason", NpgsqlDbType.Text, entity.Reason);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_reason", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.SpecialBetValue))
                {
                    ObjCommand.Parameters.AddWithValue("p_special_bet_value", NpgsqlDbType.Text, entity.SpecialBetValue);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_special_bet_value", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entity.Status != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_status", NpgsqlDbType.Boolean, entity.Status);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_status", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.TeamId))
                {
                    ObjCommand.Parameters.AddWithValue("p_team_id", NpgsqlDbType.Text, entity.TeamId);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_team_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.VoidFactor))
                {
                    ObjCommand.Parameters.AddWithValue("p_void_factor", NpgsqlDbType.Text, entity.VoidFactor);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_void_factor", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(mid_otid_ocid_sid))
                {
                    ObjCommand.Parameters.AddWithValue("p_mid_otid_ocid_sid", NpgsqlDbType.Text, mid_otid_ocid_sid);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_mid_otid_ocid_sid", NpgsqlDbType.Text, DBNull.Value);
                }
                var RetId = insert(ObjCommand);
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
        public Globals.ReturnQueueLong insertCpLcooBetclearOdds(BetResultEntity entity, long MatchID, string mid_otid_ocid_sid,bool status)
        {

            var queue = new Queue<Globals.Rollback>();
            var ObjCommand = new NpgsqlCommand(Globals.DB_Functions.InsertCpLcooBetClearOdds.ToDescription());
            try
            {
                if (MatchID != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, MatchID);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entity.OddsType != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_odds_type", NpgsqlDbType.Bigint, entity.OddsType);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_odds_type", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.Outcome))
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome", NpgsqlDbType.Text, entity.Outcome);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.OutcomeId))
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome_id", NpgsqlDbType.Text, entity.OutcomeId);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.PlayerId))
                {
                    ObjCommand.Parameters.AddWithValue("p_player_id", NpgsqlDbType.Text, entity.PlayerId);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_player_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.Reason))
                {
                    ObjCommand.Parameters.AddWithValue("p_reason", NpgsqlDbType.Text, entity.Reason);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_reason", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.SpecialBetValue))
                {
                    ObjCommand.Parameters.AddWithValue("p_special_bet_value", NpgsqlDbType.Text, entity.SpecialBetValue);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_special_bet_value", NpgsqlDbType.Text, DBNull.Value);
                }
                if (status != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_status", NpgsqlDbType.Boolean, status);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_status", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.TeamId))
                {
                    ObjCommand.Parameters.AddWithValue("p_team_id", NpgsqlDbType.Text, entity.TeamId);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_team_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.VoidFactor))
                {
                    ObjCommand.Parameters.AddWithValue("p_void_factor", NpgsqlDbType.Text, entity.VoidFactor);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_void_factor", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(mid_otid_ocid_sid))
                {
                    ObjCommand.Parameters.AddWithValue("p_mid_otid_ocid_sid", NpgsqlDbType.Text, mid_otid_ocid_sid);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_mid_otid_ocid_sid", NpgsqlDbType.Text, DBNull.Value);
                }
                var RetId = insert(ObjCommand);
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
        public Globals.ReturnQueueLong insertCpLcooBetclearOdds( long MatchID,long OddsType,string Outcome,string OutcomeId,string PlayerId, string Reason,
            string SpecialBetValue, string TeamId,string VoidFactor,
            string mid_otid_ocid_sid,bool status)
        {

            var queue = new Queue<Globals.Rollback>();
            var ObjCommand = new NpgsqlCommand(Globals.DB_Functions.InsertCpLcooBetClearOdds.ToDescription());
            try
            {
                if (MatchID != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, MatchID);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (OddsType != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_odds_type", NpgsqlDbType.Bigint, OddsType);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_odds_type", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(Outcome))
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome", NpgsqlDbType.Text, Outcome);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(OutcomeId))
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome_id", NpgsqlDbType.Text, OutcomeId);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(PlayerId))
                {
                    ObjCommand.Parameters.AddWithValue("p_player_id", NpgsqlDbType.Text,PlayerId);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_player_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(Reason))
                {
                    ObjCommand.Parameters.AddWithValue("p_reason", NpgsqlDbType.Text, Reason);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_reason", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(SpecialBetValue))
                {
                    ObjCommand.Parameters.AddWithValue("p_special_bet_value", NpgsqlDbType.Text, SpecialBetValue);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_special_bet_value", NpgsqlDbType.Text, DBNull.Value);
                }
                if (status != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_status", NpgsqlDbType.Boolean, status);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_status", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(TeamId))
                {
                    ObjCommand.Parameters.AddWithValue("p_team_id", NpgsqlDbType.Text, TeamId);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_team_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(VoidFactor))
                {
                    ObjCommand.Parameters.AddWithValue("p_void_factor", NpgsqlDbType.Text, VoidFactor);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_void_factor", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(mid_otid_ocid_sid))
                {
                    ObjCommand.Parameters.AddWithValue("p_mid_otid_ocid_sid", NpgsqlDbType.Text, mid_otid_ocid_sid);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_mid_otid_ocid_sid", NpgsqlDbType.Text, DBNull.Value);
                }
                var RetId = insert(ObjCommand);
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

        public Globals.ReturnQueueLong insertCpLcooBetclearOdds_test(BetResultEntity_test entity, long MatchID, string mid_otid_ocid_sid)
        {

            var queue = new Queue<Globals.Rollback>();
            var ObjCommand = new NpgsqlCommand(Globals.DB_Functions.InsertCpLcooBetClearOdds.ToDescription());
            try
            {
                if (MatchID != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, MatchID);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (entity.OddsType != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_odds_type", NpgsqlDbType.Bigint, entity.OddsType);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_odds_type", NpgsqlDbType.Bigint, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.Outcome))
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome", NpgsqlDbType.Text, entity.Outcome);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.OutcomeId))
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome_id", NpgsqlDbType.Text, entity.OutcomeId);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_outcome_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.PlayerId))
                {
                    ObjCommand.Parameters.AddWithValue("p_player_id", NpgsqlDbType.Text, entity.PlayerId);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_player_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.Reason))
                {
                    ObjCommand.Parameters.AddWithValue("p_reason", NpgsqlDbType.Text, entity.Reason);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_reason", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.SpecialBetValue))
                {
                    ObjCommand.Parameters.AddWithValue("p_special_bet_value", NpgsqlDbType.Text, entity.SpecialBetValue);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_special_bet_value", NpgsqlDbType.Text, DBNull.Value);
                }
                if (entity.Status != null)
                {
                    ObjCommand.Parameters.AddWithValue("p_status", NpgsqlDbType.Boolean, entity.Status);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_status", NpgsqlDbType.Boolean, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.TeamId))
                {
                    ObjCommand.Parameters.AddWithValue("p_team_id", NpgsqlDbType.Text, entity.TeamId);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_team_id", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(entity.VoidFactor))
                {
                    ObjCommand.Parameters.AddWithValue("p_void_factor", NpgsqlDbType.Text, entity.VoidFactor);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_void_factor", NpgsqlDbType.Text, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(mid_otid_ocid_sid))
                {
                    ObjCommand.Parameters.AddWithValue("p_mid_otid_ocid_sid", NpgsqlDbType.Text, mid_otid_ocid_sid);
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("p_mid_otid_ocid_sid", NpgsqlDbType.Text, DBNull.Value);
                }
                var RetId = insert(ObjCommand);
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



        #endregion

        public string IdNameTupleToJson(IdNameTuple tuple)
        {
            var Hdictionary = new Dictionary<string, string>();
            try
            {
                Hdictionary.Add("BET", tuple.Name.International);
                Hdictionary.Add("en", tuple.Name.International);
                foreach (var language in tuple.Name.AvailableTranslationLanguages)
                {
                    Hdictionary.Add(language, tuple.Name.GetTranslation(language));
                }
                return new JavaScriptSerializer().Serialize(Hdictionary);
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
                return null;
            }
        }
        public string IdNameTupleToJson(List<IdNameTuple> tuples)
        {
            var Hdictionary = new Dictionary<string, string>();
            var DictionaryList = new List<Dictionary<string, string>>();
            try
            {
                foreach (var tuple in tuples)
                {
                    Hdictionary.Add("BET", tuple.Name.International);
                    Hdictionary.Add("en", tuple.Name.International);
                    foreach (var language in tuple.Name.AvailableTranslationLanguages)
                    {
                        Hdictionary.Add(language, tuple.Name.GetTranslation(language));
                    }
                    DictionaryList.Add(Hdictionary);
                    Hdictionary.Clear();
                }
                return new JavaScriptSerializer().Serialize(DictionaryList);
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
                return null;
            }
        }
        public string TextsToJson(List<TextsEntity> texts, int? number)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                if (number != null)
                {
                    foreach (var text in texts[(int)number].Texts[0].Text)
                    {
                        dictionary.Add(text.Language, text.Value);
                    }
                }
                else
                {
                    foreach (var mainTexts in texts)
                    {
                        foreach (var text in mainTexts.Texts[0].Text)
                        {
                            dictionary.Add(text.Language, text.Value);
                        }
                    }

                }

                return new JavaScriptSerializer().Serialize(dictionary);
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
                return null;
            }
        }
        public string TextsToJson(List<TextEntity> texts)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                foreach (var text in texts)
                {
                    dictionary.Add(text.Language, text.Value);
                }
                return new JavaScriptSerializer().Serialize(dictionary);
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
                return null;
            }
        }

        public void UpdateAliveMatches(List<string> matches)
        {
            try
            {
                if (matches.Count>0)
                {
                    string csv = String.Join(",", matches.Select(x => x.ToString()).ToArray());
                    string query = "UPDATE dy_alive_matches SET matches_on = '" + csv + "', last_update = now() WHERE id = 1;  UPDATE dy_match_all_data SET feed_type = 2 WHERE match_id in ("+csv+")";
                    NpgsqlCommand objCommand = new NpgsqlCommand(query);
                    objCommand.CommandText = query;
                    insertNonProc(objCommand);
                }
               
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }

        public bool LiveOddsMoveToArch(DateTime lastUpdate)
        {
            string rollbackArch = "BEGIN;" +
                                  "SAVEPOINT HERE;" +
                                  "INSERT INTO cp_live_odds_arch (id, match_id, odd_active, odd_changed, odd_combination, odd_for_the_rest, odd_free_text, odd_id, odd_most_balanced, odd_name, odd_eventoddsfield_active, odd_eventoddsfield_outcomestatus," +
                                                                 "odd_eventoddsfield_playerid, odd_eventoddsfield_probability, odd_eventoddsfield_type, odd_eventoddsfield_value, odd_eventoddsfield_viewindex," +
                                                                 "odd_eventoddsfield_voidfactor, odd_eventoddsfield_name, odd_special_odds_value, odd_sub_type, odd_type, odd_type_id, is_deleted, created_at," +
                                                                 "odd_eventoddsfield_typeid, last_update, last_odd, mid_oid_oftid,status,time_stamp) " +
                                  "SELECT id, match_id, odd_active, odd_changed, odd_combination, odd_for_the_rest, odd_free_text, odd_id, odd_most_balanced, odd_name, odd_eventoddsfield_active, odd_eventoddsfield_outcomestatus," +
                                  "odd_eventoddsfield_playerid, odd_eventoddsfield_probability, odd_eventoddsfield_type, odd_eventoddsfield_value, odd_eventoddsfield_viewindex," +
                                  "odd_eventoddsfield_voidfactor, odd_eventoddsfield_name, odd_special_odds_value, odd_sub_type, odd_type, odd_type_id, is_deleted, created_at," +
                                  "odd_eventoddsfield_typeid, last_update, last_odd, mid_oid_oftid,status,time_stamp " +
                                  "FROM cp_live_odds WHERE created_at <= '" + lastUpdate + "'; " +
                                  "DELETE FROM cp_live_odds WHERE created_at <= '" + lastUpdate + "'; ";

            NpgsqlCommand objCommand = new NpgsqlCommand(rollbackArch);
            objCommand.CommandText = rollbackArch;

            try
            {
                insertNonProc(objCommand);
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
                string rollbackArchOperation = "ROLLBACK TO SAVEPOINT HERE; END;";
                objCommand.CommandText += rollbackArchOperation;
                try
                {
                    insertNonProc(objCommand);
                }
                catch (Exception exc)
                {
                    SharedLibrary.Logg.logger.Fatal(exc.Message);
                }
                return false;
            }

            String rollbackEnd = " END;";
            objCommand.CommandText += rollbackEnd;
            try
            {
                insertNonProc(objCommand);
            }
            catch (Exception e)
            {
                SharedLibrary.Logg.logger.Fatal(e.Message);
            }
            return true;

        }
        /// <summary>
        /// This function sets the Roll Back object and the calling function sets the queue in case of a roll back
        /// </summary>
        /// <param name="RowId"></param>
        /// <param name="Table"></param>
        /// <param name="Transaction"></param>
        /// <returns>Globals.Rollback</returns>
        public Globals.Rollback SetRollback(long RowId, Globals.Tables Table, Globals.TransactionTypes Transaction)
        {
            var rollbackObject = new Globals.Rollback();
            try
            {
                rollbackObject.RowId = RowId;
                rollbackObject.TableId = (int)Table;
                rollbackObject.TransactionType = (int)Transaction;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
            return rollbackObject;
        }
        public void RollBack(List<Globals.Rollback> rolls)
        {
            // Get call stack
            StackTrace stackTrace = new StackTrace();

            // Get calling method name
            Console.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);
            try
            {
                foreach (var roll in rolls)
                {
                    var adObjCommand = new NpgsqlCommand(Globals.DB_Functions.Rollback.ToDescription());
                    adObjCommand.Parameters.AddWithValue("table_id", roll.TableId);
                    adObjCommand.Parameters.AddWithValue("row_id", roll.RowId);
                    adObjCommand.Parameters.AddWithValue("tran_type", roll.TransactionType);
                    update(adObjCommand);
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
            finally
            {

            }
        }
        public NpgsqlConnection connection()
        {
            var con = new NpgsqlConnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                {

                    var connectionBuilder = new NpgsqlConnectionStringBuilder();
                    connectionBuilder.Host = config.AppSettings.Get("DB_Host");
                    connectionBuilder.Port = int.Parse(config.AppSettings.Get("DB_Port"));
                    connectionBuilder.Database = config.AppSettings.Get("DB_Database");
                    connectionBuilder.Username = config.AppSettings.Get("DB_Username");
                    connectionBuilder.Password = config.AppSettings.Get("DB_Password");
                    connectionBuilder.Timeout = 300;
                    connectionBuilder.Pooling = true;
                    connectionBuilder.CommandTimeout = 300;
                    con = new NpgsqlConnection(connectionBuilder.ConnectionString);
                    return con;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }

        }
        public long insert(NpgsqlCommand objCommand)
        {
            long errorNumber = -1;
            long result = -20;
            object one;
            objCommand.CommandType = CommandType.StoredProcedure;
            try
            {

                objCommand.Connection = connection();
                //objCommand.CommandTimeout = 5;
                objCommand.Connection.Open();
                one = objCommand.ExecuteScalar();
                bool successfullyParsed = long.TryParse(one.ToString(), out result);
                long val = 0;
                if (successfullyParsed)
                {
                    if (result != null && long.TryParse(result.ToString(), out val))
                    {
                        if (val > 0)
                        {
#if DEBUG
                            WriteFullLine(objCommand.CommandText);
#endif
                            return val;
                        }
                        else
                        {
                            errorNumber = val;
                            throw new DataException();
                        }
                    }
                }
                else
                {
                }

                return -1;
            }
            catch (Exception ex)
            {
                StackTrace trace = new StackTrace(true);
                var frame = trace.GetFrame(1);
                var altMessage = "  Error#: " + errorNumber.ToString() + "  METHOD: " + frame.GetMethod().Name + "  LINE:  " + frame.GetFileLineNumber();
                Logg.logger.Fatal(ex.Message + altMessage);
                // Task.Factory.StartNew(() => Globals.Queue_Errors.Enqueue(objCommand));
                return -1;
            }
            finally
            {
                objCommand.Connection.Close();
            }
        }
        public long insertNonProc(NpgsqlCommand objCommand)
        {

            long errorNumber = -1;
            objCommand.CommandType = CommandType.Text;
            try
            {

                objCommand.Connection = connection();
                objCommand.Connection.Open();
                objCommand.CommandTimeout = 100;
                var result = objCommand.ExecuteNonQuery();
                long val = 0;
                if (result != null && long.TryParse(result.ToString(), out val))
                {
                    if (val > 0)
                    {
#if DEBUG
                        WriteFullLine(objCommand.CommandText);
#endif
                        return val;
                    }
                    else
                    {
                        errorNumber = val;
                        //throw new DataException();
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                objCommand.Connection.Close();
                StackTrace trace = new StackTrace(true);
                var frame = trace.GetFrame(1);
                var altMessage = "  Error#: " + errorNumber.ToString() + "  METHOD: " + frame.GetMethod().Name + "  LINE:  " + frame.GetFileLineNumber();
                Logg.logger.Fatal(ex.Message + altMessage);
                return errorNumber;
            }
            finally
            {
                objCommand.Connection.Close();
            }
        }
        public long update(NpgsqlCommand objCommand)
        {

            objCommand.CommandType = CommandType.StoredProcedure;
            try
            {
                objCommand.Connection = connection();
                objCommand.Connection.Open();
#if DEBUG
                WriteFullLine(objCommand.CommandText);
#endif
                var result = objCommand.ExecuteScalar();
                long val = 0;
                if (long.TryParse(result.ToString(), out val))
                {
                    return val;
                }
                return -1;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return -1;
            }
            finally
            {
                objCommand.Connection.Close();
            }
        }

        public DataSet selectReader(NpgsqlCommand objCommand)
        {
            var lbetclear = new LcooBetClear();
            var ldata = new List<LcooBetClear>();
            objCommand.Connection = connection();
            objCommand.Connection.Open();
            objCommand.CommandType = CommandType.StoredProcedure;
            var ds = new DataSet();
            try
            {
                ds.Tables.Add();
                NpgsqlDataReader dr = objCommand.ExecuteReader();
                while (dr.Read())
                {
                  
                        //lbetclear.id = long.TryParse(dr[0].ToString(),);
                   
                    
                    lbetclear.match_id = long.Parse(dr[1].ToString());
                    lbetclear.odd_type_id = long.Parse(dr[2].ToString());
                    lbetclear.odd_type_name = dr[3].ToString();
                    lbetclear.odd_id = long.Parse(dr[4].ToString());
                    lbetclear.odd_outcome = dr[5].ToString();
                    lbetclear.odd_outcome_id = dr[6].ToString();
                    lbetclear.odd_player_id = dr[7].ToString();
                    lbetclear.odd_team_id = dr[8].ToString();
                    lbetclear.odd_name = dr[9].ToString();
                    lbetclear.odd_visible = bool.Parse(dr[10].ToString());
                    lbetclear.odd_special = dr[11].ToString();
                    lbetclear.odd_odd = dr[12].ToString();
                    lbetclear.is_deleted = bool.Parse(dr[13].ToString());
                    lbetclear.created_at = DateTime.Parse(dr[14].ToString());
                    lbetclear.odd_probability = dr[15].ToString();
                    lbetclear.last_update = DateTime.Parse(dr[16].ToString());
                    lbetclear.last_odd = dr[17].ToString();
                    lbetclear.mid_otid_ocid_sid = dr[18].ToString();
                    ldata.Add(lbetclear);
                }
#if DEBUG
                WriteFullLine(objCommand.CommandText);
#endif
                return ds;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
            finally
            {
                objCommand.Connection.Close();
            }
        }
        public DataSet select(NpgsqlCommand objCommand)
        {
            objCommand.Connection = connection();
            objCommand.Connection.Open();
            objCommand.CommandType = CommandType.StoredProcedure;
            var ds = new DataSet();
            try
            {
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(objCommand);
                da.Fill(ds);
#if DEBUG
                WriteFullLine(objCommand.CommandText);
#endif
                return ds;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
            finally
            {
                objCommand.Connection.Close();
            }
        }
        public long selectOne(NpgsqlCommand objCommand)
        {

            objCommand.CommandType = CommandType.Text;
            try
            {
                objCommand.Connection = connection();
                objCommand.Connection.Open();
                var res = objCommand.ExecuteReader(CommandBehavior.SingleResult);
                if (res.Read())
                {
#if DEBUG
                    WriteFullLine(objCommand.CommandText);
#endif
                    return res.GetInt64(0);
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return -1;
            }
            finally
            {
                objCommand.Connection.Close();
            }
        }
        public void TraceMessage(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Logg.logger.Fatal(message + "  |||  " + memberName + "  |||  " + sourceFilePath + "  |||  " + sourceLineNumber);
        }
        public void SendToCoupon(BetClearQueueElement bet)
        {
            try
            {
                RequestSocket client = new RequestSocket(">tcp://127.0.0.1:5556");
                var msg_client = new NetMQMessage();
                var json = new JavaScriptSerializer().Serialize(bet);
                msg_client.Append(json);
                client.SendMultipartMessage(msg_client);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }
        public static void WriteFullLine(string value)
        {
            //
            // This method writes an entire line to the console with the string.
            //
            //Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(value.PadRight(Console.WindowWidth - 1)); // <-- see note
                                                                        //
                                                                        // Reset the color.
                                                                        //
            Console.ResetColor();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class element
    {
    }
}
