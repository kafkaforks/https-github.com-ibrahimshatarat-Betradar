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
    public class Common : Core
    {
        
        public bool updateCpLcooOdds(long match_id, long odd_type_id)
        {
            try
            {
                var command = new NpgsqlCommand( Globals.DB_Functions.UpdateCpLcooOdds.ToDescription().ToString());
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
        
        //public long insertDyMatchs(MatchHeader entityMheader, MatchInfo entityMinfo, Boolean p_match_on, Boolean p_is_active, Boolean p_is_visible, int p_feed_type)
        //{
        //    var command = new NpgsqlCommand(Globals.DB_Functions.insertDyMatchs.ToDescription());
        //    return 1;
        //}
        public long insertDyMatchs(MatchHeader entityMheader, MatchInfo entityMinfo, Boolean p_match_on, Boolean p_is_active, Boolean p_is_visible, int p_feed_type)
        {

            var command = new NpgsqlCommand(Globals.DB_Functions.insertDyMatchs.ToDescription());

           


            try
            {
                if (entityMheader.Id != null)
                {
                    command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, entityMheader.Id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, DBNull.Value);
                }

                //command.Parameters.AddWithValue("p_match_name", NpgsqlDbType.Text, LocalizedStringToJson(entityMinfo.HomeTeam.Name) + "|" + LocalizedStringToJson(entityMinfo.AwayTeam.Name));
               

                //if (entityMinfo.HomeTeam.Id != null)
                //{
                //    command.Parameters.AddWithValue("p_home_team_id", NpgsqlDbType.Bigint, entityMinfo.HomeTeam.Id);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_home_team_id", NpgsqlDbType.Bigint, DBNull.Value);
                //}
                //if (entityMinfo.AwayTeam.Id != null)
                //{
                //    command.Parameters.AddWithValue("p_away_team_id", NpgsqlDbType.Bigint, entityMinfo.AwayTeam.Id);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_away_team_id", NpgsqlDbType.Bigint, DBNull.Value);
                ////}
                //if (entityMinfo.HomeTeam.Name != null)
                //{
                //    command.Parameters.AddWithValue("p_home_team_name", NpgsqlDbType.Text, entityMinfo.AwayTeam.Name);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_home_team_name", NpgsqlDbType.Text, DBNull.Value);
                //}
                //if (entityMinfo.AwayTeam.Name != null)
                //{
                //    command.Parameters.AddWithValue("p_away_team_name", NpgsqlDbType.Text, entityMinfo.AwayTeam.Name);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_away_team_name", NpgsqlDbType.Text, DBNull.Value);
                //}

                //if (entityMinfo.Sport.Id != null)
                //{
                //    command.Parameters.AddWithValue("p_sport_id", NpgsqlDbType.Bigint, entityMinfo.Sport.Id);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_sport_id", NpgsqlDbType.Bigint, DBNull.Value);
                //}

                //if (entityMinfo.Sport.Name != null)
                //{
                //    command.Parameters.AddWithValue("p_sport_name", NpgsqlDbType.Text, entityMinfo.Sport.Name);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_sport_name", NpgsqlDbType.Text, DBNull.Value);
                //}

                //if (entityMinfo.Tournament.Id != null)
                //{
                //    command.Parameters.AddWithValue("p_tournament_id", NpgsqlDbType.Bigint, entityMinfo.Tournament.Id);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_tournament_id", NpgsqlDbType.Bigint, DBNull.Value);
                //}

                //if (entityMinfo.Tournament.Name != null)
                //{
                //    command.Parameters.AddWithValue("p_tournament_name", NpgsqlDbType.Text, entityMinfo.Tournament.Name);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_tournament_name", NpgsqlDbType.Text, DBNull.Value);
                //}

                if (p_is_active != null) // is_active
                {
                    command.Parameters.AddWithValue("p_is_activ", NpgsqlDbType.Boolean, p_is_active);
                }
                else
                {
                    command.Parameters.AddWithValue("p_is_activ", NpgsqlDbType.Boolean, DBNull.Value);
                }
                //p_country_iso
                
                //if (entityMinfo.Category != null)
                //{
                //    //IdNameTupleToJson(entityMinfo.Category);

                //    command.Parameters.AddWithValue("p_country_iso", NpgsqlDbType.Text, IdNameTupleToJson(entityMinfo.Category));
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_country_iso", NpgsqlDbType.Text, DBNull.Value);
                //}

                if (entityMheader.MatchTime != null)
                {
                    command.Parameters.AddWithValue("match_start_date", NpgsqlDbType.Timestamp, entityMheader.MatchTime);
                }
                else
                {
                    command.Parameters.AddWithValue("match_start_date", NpgsqlDbType.Timestamp, DBNull.Value);
                }

                //if (entityMinfo.Category.Name != null)
                //{
                //    command.Parameters.AddWithValue("p_category_name", NpgsqlDbType.Text, entityMinfo.Category.Name);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_category_name", NpgsqlDbType.Text, DBNull.Value);
                //}



                //p_feed_type not 
                if (p_feed_type != null)
                {
                    command.Parameters.AddWithValue("feed_type", NpgsqlDbType.Text, p_feed_type);
                }
                else
                {
                    command.Parameters.AddWithValue("feed_type", NpgsqlDbType.Text, DBNull.Value);
                }
                
                //p_is_visible
                if (p_is_visible != null) // p_tournament_name_tr ?
                {
                    command.Parameters.AddWithValue("p_is_visible", NpgsqlDbType.Boolean, p_is_visible);
                }
                else
                {
                    command.Parameters.AddWithValue("p_is_visible", NpgsqlDbType.Boolean, DBNull.Value);
                }

                if (entityMheader.Active != null) // p_active like p_is_active ??
                {

                    command.Parameters.AddWithValue("p_active", NpgsqlDbType.Boolean, entityMheader.Active);
                }
                else
                {
                    command.Parameters.AddWithValue("p_active", NpgsqlDbType.Boolean, DBNull.Value);
                }

                if (entityMheader.AutoTraded != null)
                {

                    command.Parameters.AddWithValue("p_auto_traded", NpgsqlDbType.Boolean, entityMheader.AutoTraded);
                }
                else
                {
                    command.Parameters.AddWithValue("p_auto_traded", NpgsqlDbType.Boolean, DBNull.Value);
                }

                if (entityMheader.Balls != null)
                {

                    command.Parameters.AddWithValue("p_balls", NpgsqlDbType.Bigint, entityMheader.Balls);
                }
                else
                {
                    command.Parameters.AddWithValue("p_balls", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(entityMheader.Bases))
                {

                    command.Parameters.AddWithValue("p_bases", NpgsqlDbType.Text, entityMheader.Bases);
                }
                else
                {
                    command.Parameters.AddWithValue("p_bases", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.Batter != null) // in db text 
                {

                    command.Parameters.AddWithValue("p_batter", NpgsqlDbType.Text, entityMheader.Batter);
                }
                else
                {
                    command.Parameters.AddWithValue("p_batter", NpgsqlDbType.Text, DBNull.Value);
                }


                if (entityMheader.BetStatus != null) // string text
                {

                    command.Parameters.AddWithValue("p_bet_status", NpgsqlDbType.Text, entityMheader.Status);
                }
                else
                {
                    command.Parameters.AddWithValue("p_bet_status", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.BetStopReason != null) // string text
                {

                    command.Parameters.AddWithValue("p_bet_stop_reason", NpgsqlDbType.Text, entityMheader.BetStopReason);
                }
                else
                {
                    command.Parameters.AddWithValue("p_bet_stop_reason", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.Booked != null)
                {

                    command.Parameters.AddWithValue("p_booked", NpgsqlDbType.Boolean, entityMheader.Booked);
                }
                else
                {
                    command.Parameters.AddWithValue("p_booked", NpgsqlDbType.Boolean, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(entityMheader.ClearedScore))
                {

                    command.Parameters.AddWithValue("p_cleared_score", NpgsqlDbType.Text, entityMheader.ClearedScore);
                }
                else
                {
                    command.Parameters.AddWithValue("p_cleared_score", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.ClockStopped != null)
                {

                    command.Parameters.AddWithValue("p_ClockStopped", NpgsqlDbType.Boolean, entityMheader.ClockStopped);
                }
                else
                {
                    command.Parameters.AddWithValue("p_ClockStopped", NpgsqlDbType.Boolean, DBNull.Value);
                }

                if (entityMheader.Corners != null)
                {

                    command.Parameters.AddWithValue("p_corners", NpgsqlDbType.Text, entityMheader.Corners);
                }
                else
                {
                    command.Parameters.AddWithValue("p_corners", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.CurrentCtTeam != null)
                {

                    command.Parameters.AddWithValue("p_current_ct_team", NpgsqlDbType.Text, entityMheader.CurrentCtTeam);
                }
                else
                {
                    command.Parameters.AddWithValue("p_current_ct_team", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.CurrentEnd != null) // start 
                {

                    command.Parameters.AddWithValue("p_current_end", NpgsqlDbType.Bigint, entityMheader.CurrentCtTeam);
                }
                else
                {
                    command.Parameters.AddWithValue("p_current_end", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.Delivery != null)
                {

                    command.Parameters.AddWithValue("p_delivery", NpgsqlDbType.Text, entityMheader.Delivery);
                }
                else
                {
                    command.Parameters.AddWithValue("p_delivery", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.Dismissals != null)
                {

                    command.Parameters.AddWithValue("p_dismissals", NpgsqlDbType.Text, entityMheader.Dismissals);
                }
                else
                {
                    command.Parameters.AddWithValue("p_dismissals", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.EarlyBetStatus != null)
                {

                    command.Parameters.AddWithValue("p_early_bet_status", NpgsqlDbType.Text, entityMheader.EarlyBetStatus);
                }
                else
                {
                    command.Parameters.AddWithValue("p_early_bet_status", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.Expedite != null)
                {

                    command.Parameters.AddWithValue("p_expedite", NpgsqlDbType.Boolean, entityMheader.Expedite);
                }
                else
                {
                    command.Parameters.AddWithValue("p_expedite", NpgsqlDbType.Boolean, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(entityMheader.GameScore))
                {

                    command.Parameters.AddWithValue("p_game_score", NpgsqlDbType.Text, entityMheader.GameScore);
                }
                else
                {
                    command.Parameters.AddWithValue("p_game_score", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.Innings != null)
                {

                    command.Parameters.AddWithValue("p_innings", NpgsqlDbType.Bigint, entityMheader.Innings);
                }
                else
                {
                    command.Parameters.AddWithValue("p_innings", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.LegScore != null)
                {

                    command.Parameters.AddWithValue("p_leg_score", NpgsqlDbType.Text, entityMheader.LegScore);
                }
                else
                {
                    command.Parameters.AddWithValue("p_leg_score", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.MatchTime != null)
                {

                    command.Parameters.AddWithValue("p_match_time", NpgsqlDbType.Bigint, entityMheader.MatchTime);
                }
                else
                {
                    command.Parameters.AddWithValue("p_match_time", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.MatchTimeExtended != null)
                {

                    command.Parameters.AddWithValue("p_match_time_extended", NpgsqlDbType.Text, entityMheader.MatchTimeExtended);
                }
                else
                {
                    command.Parameters.AddWithValue("p_match_time_extended", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.Message != null)
                {

                    command.Parameters.AddWithValue("p_message", NpgsqlDbType.Text, entityMheader.Message);
                }
                else
                {
                    command.Parameters.AddWithValue("p_message", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.Msgnr != null)
                {

                    command.Parameters.AddWithValue("p_msgnr", NpgsqlDbType.Bigint, entityMheader.Msgnr);
                }
                else
                {
                    command.Parameters.AddWithValue("p_msgnr", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.Outs != null)
                {

                    command.Parameters.AddWithValue("p_outs", NpgsqlDbType.Bigint, entityMheader.Outs);
                }
                else
                {
                    command.Parameters.AddWithValue("p_outs", NpgsqlDbType.Bigint, DBNull.Value);
                }


                if (entityMheader.Over != null)
                {

                    command.Parameters.AddWithValue("p_mh_over", NpgsqlDbType.Bigint, entityMheader.Over);
                }
                else
                {
                    command.Parameters.AddWithValue("p_mh_over", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.PentaltyRuns != null)
                {

                    command.Parameters.AddWithValue("p_pentalty_runs", NpgsqlDbType.Text, entityMheader.PentaltyRuns);
                }
                else
                {
                    command.Parameters.AddWithValue("p_pentalty_runs", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.Position != null)
                {

                    command.Parameters.AddWithValue("p_mh_position", NpgsqlDbType.Bigint, entityMheader.Position);
                }
                else
                {
                    command.Parameters.AddWithValue("p_mh_position", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.Possession != null)
                {

                    command.Parameters.AddWithValue("p_possession", NpgsqlDbType.Text, entityMheader.Possession);
                }
                else
                {
                    command.Parameters.AddWithValue("p_possession", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.RedCards != null)
                {

                    command.Parameters.AddWithValue("p_red_cards", NpgsqlDbType.Text, entityMheader.RedCards);
                }
                else
                {
                    command.Parameters.AddWithValue("p_red_cards", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.RemainingBowls != null)
                {

                    command.Parameters.AddWithValue("p_remaining_bowls", NpgsqlDbType.Text, entityMheader.RemainingBowls);
                }
                else
                {
                    command.Parameters.AddWithValue("p_remaining_bowls", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.RemainingReds != null)
                {

                    command.Parameters.AddWithValue("p_remaining_reds", NpgsqlDbType.Bigint, entityMheader.RemainingReds);
                }
                else
                {
                    command.Parameters.AddWithValue("p_remaining_reds", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.RemainingTime != null)
                {

                    command.Parameters.AddWithValue("p_remaining_time", NpgsqlDbType.Text, entityMheader.RemainingTime);
                }
                else
                {
                    command.Parameters.AddWithValue("p_remaining_time", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(entityMheader.RemainingTimeInPeriod))
                {

                    command.Parameters.AddWithValue("p_remaining_time_in_period", NpgsqlDbType.Text, entityMheader.RemainingTimeInPeriod);
                }
                else
                {
                    command.Parameters.AddWithValue("p_remaining_time_in_period", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(entityMheader.Score))
                {

                    command.Parameters.AddWithValue("p_score", NpgsqlDbType.Text, entityMheader.Score);
                }
                else
                {
                    command.Parameters.AddWithValue("p_score", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.Server != null)
                {

                    command.Parameters.AddWithValue("p_mh_server", NpgsqlDbType.Bigint, entityMheader.Server);
                }
                else
                {
                    command.Parameters.AddWithValue("p_mh_server", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.SetScores != null)
                {

                    command.Parameters.AddWithValue("p_set_scores", NpgsqlDbType.Text, entityMheader.SetScores);
                }
                else
                {
                    command.Parameters.AddWithValue("p_set_scores", NpgsqlDbType.Text, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(entityMheader.SourceId))
                {

                    command.Parameters.AddWithValue("p_source_id", NpgsqlDbType.Text, entityMheader.SourceId);
                }
                else
                {
                    command.Parameters.AddWithValue("p_source_id", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.Status != null)
                {

                    command.Parameters.AddWithValue("p_status", NpgsqlDbType.Text, entityMheader.Status);
                }
                else
                {
                    command.Parameters.AddWithValue("p_status", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.Strikes != null)
                {

                    command.Parameters.AddWithValue("p_strikes", NpgsqlDbType.Bigint, entityMheader.Strikes);
                }
                else
                {
                    command.Parameters.AddWithValue("p_strikes", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.Suspend != null)
                {

                    command.Parameters.AddWithValue("p_suspend", NpgsqlDbType.Text, entityMheader.Suspend);
                }
                else
                {
                    command.Parameters.AddWithValue("p_suspend", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.Throw != null)
                {

                    command.Parameters.AddWithValue("p_throw", NpgsqlDbType.Bigint, entityMheader.Throw);
                }
                else
                {
                    command.Parameters.AddWithValue("p_throw", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.TieBreak != null)
                {

                    command.Parameters.AddWithValue("p_tie_break", NpgsqlDbType.Boolean, entityMheader.TieBreak);
                }
                else
                {
                    command.Parameters.AddWithValue("p_tie_break", NpgsqlDbType.Boolean, DBNull.Value);
                }

                if (entityMheader.Try != null)
                {

                    command.Parameters.AddWithValue("p_try", NpgsqlDbType.Bigint, entityMheader.Try);
                }
                else
                {
                    command.Parameters.AddWithValue("p_try", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.Visit != null)
                {

                    command.Parameters.AddWithValue("p_visit", NpgsqlDbType.Bigint, entityMheader.Visit);
                }
                else
                {
                    command.Parameters.AddWithValue("p_visit", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.Yards != null)
                {

                    command.Parameters.AddWithValue("p_yards", NpgsqlDbType.Bigint, entityMheader.Yards);
                }
                else
                {
                    command.Parameters.AddWithValue("p_yards", NpgsqlDbType.Bigint, DBNull.Value);
                }

                if (entityMheader.YellowCards != null)
                {

                    command.Parameters.AddWithValue("p_yellow_cards", NpgsqlDbType.Text, entityMheader.YellowCards);
                }
                else
                {
                    command.Parameters.AddWithValue("p_yellow_cards", NpgsqlDbType.Text, DBNull.Value);
                }

                if (entityMheader.YellowRedCards != null)
                {

                    command.Parameters.AddWithValue("p_yellow_red_cards", NpgsqlDbType.Text, entityMheader.YellowRedCards);
                }
                else
                {
                    command.Parameters.AddWithValue("p_yellow_red_cards", NpgsqlDbType.Text, DBNull.Value);
                }

                //if (entityMinfo.BetPal != null)
                //{

                //    command.Parameters.AddWithValue("p_bet_pal", NpgsqlDbType.Text, entityMinfo.BetPal);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_bet_pal", NpgsqlDbType.Text, DBNull.Value);
                //}

                //if (entityMinfo.CoverageInfo != null)
                //{

                //    command.Parameters.AddWithValue("p_coverage_info", NpgsqlDbType.Text, entityMinfo.CoverageInfo);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_coverage_info", NpgsqlDbType.Text, DBNull.Value);
                //}

                //if (entityMinfo.DateOfMatch != null)
                //{

                //    command.Parameters.AddWithValue("p_date_of_match", NpgsqlDbType.Timestamp, entityMinfo.DateOfMatch);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_date_of_match", NpgsqlDbType.Timestamp, DBNull.Value);
                //}

                //if (entityMinfo.ExtraInfo != null)
                //{

                //    command.Parameters.AddWithValue("p_extra_info", NpgsqlDbType.Text, entityMinfo.ExtraInfo);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_extra_info", NpgsqlDbType.Text, DBNull.Value);
                //}

                if (entityMinfo == null)
                {
                    command.Parameters.AddWithValue("p_home_team_id", NpgsqlDbType.Bigint, DBNull.Value);
                    command.Parameters.AddWithValue("p_away_team_id", NpgsqlDbType.Bigint, DBNull.Value);
                    command.Parameters.AddWithValue("p_home_team_name", NpgsqlDbType.Text, DBNull.Value);
                    command.Parameters.AddWithValue("p_away_team_name", NpgsqlDbType.Text, DBNull.Value);
                    command.Parameters.AddWithValue("p_sport_id", NpgsqlDbType.Bigint, DBNull.Value);
                    command.Parameters.AddWithValue("p_sport_name", NpgsqlDbType.Text, DBNull.Value);
                    command.Parameters.AddWithValue("p_tournament_name", NpgsqlDbType.Text, DBNull.Value);
                    command.Parameters.AddWithValue("p_country_iso", NpgsqlDbType.Text, DBNull.Value);
                    command.Parameters.AddWithValue("p_category_name", NpgsqlDbType.Text, DBNull.Value);
                    command.Parameters.AddWithValue("p_bet_pal", NpgsqlDbType.Text, DBNull.Value);
                    command.Parameters.AddWithValue("p_coverage_info", NpgsqlDbType.Text, DBNull.Value);
                    command.Parameters.AddWithValue("p_date_of_match", NpgsqlDbType.Timestamp, DBNull.Value);
                    command.Parameters.AddWithValue("p_extra_info", NpgsqlDbType.Text, DBNull.Value);
                    command.Parameters.AddWithValue("p_round", NpgsqlDbType.Text, DBNull.Value);
                    command.Parameters.AddWithValue("p_season", NpgsqlDbType.Text, DBNull.Value);
                    command.Parameters.AddWithValue("p_streaming_channels", NpgsqlDbType.Text, DBNull.Value);
                    command.Parameters.AddWithValue("p_tv_channels", NpgsqlDbType.Text, DBNull.Value);


                }

                //if (entityMinfo.Round != null)
                //{

                //    command.Parameters.AddWithValue("p_round", NpgsqlDbType.Text, entityMinfo.Round);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_round", NpgsqlDbType.Text, DBNull.Value);
                //}

                //if (entityMinfo.Season != null)
                //{

                //    command.Parameters.AddWithValue("p_season", NpgsqlDbType.Text, entityMinfo.Season);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_season", NpgsqlDbType.Text, DBNull.Value);
                //}

                //if (entityMinfo.StreamingChannels != null)
                //{

                //    command.Parameters.AddWithValue("p_streaming_channels", NpgsqlDbType.Text, entityMinfo.StreamingChannels);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_streaming_channels", NpgsqlDbType.Text, DBNull.Value);
                //}

                //if (entityMinfo.TvChannels != null)
                //{

                //    command.Parameters.AddWithValue("p_tv_channels", NpgsqlDbType.Text, entityMinfo.TvChannels);
                //}
                //else
                //{
                //    command.Parameters.AddWithValue("p_tv_channels", NpgsqlDbType.Text, DBNull.Value);
                //}

                //p_match_on
                if (p_match_on != null)
                {

                    command.Parameters.AddWithValue("p_match_on", NpgsqlDbType.Boolean, p_match_on);
                }
                else
                {
                    command.Parameters.AddWithValue("p_match_on", NpgsqlDbType.Boolean, DBNull.Value);
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
        //public static Task<List<List<string>>> duplicateJsonToArray(string json)
        //{
        //    json = json.Replace("[", "");
        //    json = json.Replace("]", "");
        //    json = json.Replace("{", "");
        //    json = json.Replace("}", "");
        //    json = json.Replace("\"", "");
        //    json = json.Replace("(", "");
        //    json = json.Replace(")", "");
        //    json = json.Replace(" ", "");

        //    List<List<string>> list = new List<List<string>>();
        //    List<string> sublist = new List<string>();
        //    String key = "";
        //    String value = "";
        //    bool key_door = true;
        //    bool value_door = false;
        //    foreach (var element in json)
        //    {

        //        if (Convert.ToInt64(element) != 58 && key_door == true && Convert.ToInt64(element) != 44)
        //        {
        //            key += element;
        //        }
        //        if (Convert.ToInt64(element) == 58)
        //        {
        //            key_door = false;
        //            value_door = true;
        //        }
        //        if (Convert.ToInt64(element) != 44 && value_door == true && Convert.ToInt64(element) != 58)
        //        {
        //            value += element;
        //        }
        //        if (Convert.ToInt64(element) == 44)
        //        {
        //            key_door = true;
        //            value_door = false;
        //            sublist.Add(key);
        //            sublist.Add(value);
        //            /*ı need to delete the content of the string right here but i could not because i got a error what can i do*/
        //            value = "";
        //            key = "";
        //        }


        //    }
        //    sublist.Add(key);
        //    sublist.Add(value);
        //    list.Add(sublist);

        //    return list;
        //}
        public  void WorkAlive(AliveEventArgs e)
        {
            foreach (var head in e.Alive.EventHeaders)
            {
                //Task.Factory.StartNew(() => insertMatchDataAllDetails((MatchHeader)head, null));
                 insertMatchDataAllDetails((MatchHeader)head, null);
            }
        }
        public  Merchants selectDyMerchants(long p_merchant_id, string p_username)
        {
            var dyMerchants = new Merchants();
            var command = new NpgsqlCommand( Globals.DB_Functions.selectDyMerchants.ToDescription().ToString());

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

            var ds =  select(command);

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
        public long insertDyMerchants(Merchants merch)
        {

            var command = new NpgsqlCommand(Globals.DB_Functions.InsertDyMerchants.ToDescription().ToString());

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

                var RetId =  insert(command);
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
        public  void insertMatch(MatchEntity m_entity, int feedtype)
        {
            var command = new NpgsqlCommand( Globals.DB_Functions.InsertMatchAllData.ToDescription().ToString());
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
                    var first = "";
                    var second = "";
                    if (match.Fixture.Competitors.Texts.Count > 0)
                    {
                        if (match.Fixture.Competitors.Texts[0].Texts.Count > 0 && match.Fixture.Competitors.Texts[0].Texts[0].Text.Count > 1)
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
                //command.Parameters.AddWithValue("p_feed_type", NpgsqlDbType.Integer, feedtype);
                command.Parameters.AddWithValue("p_feed_type", NpgsqlDbType.Integer, DBNull.Value);
                insert(command);

            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }
        public  void    insertMatchLocal(MatchLocal m_entity, int feedtype)
        {
            var match = m_entity;
            var command = new NpgsqlCommand( Globals.DB_Functions.InsertMatchAllData.ToDescription().ToString());
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
                //command.Parameters.AddWithValue("p_feed_type", NpgsqlDbType.Integer, feedtype);
                command.Parameters.AddWithValue("p_feed_type", NpgsqlDbType.Integer, DBNull.Value);
                 insert(command);
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }
        public  MatchLocal ConvertLiveMatchToLcooMatch(MatchHeader header, MatchInfo info)
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
                match.match_name = info.HomeTeam.Name.GetTranslation("BET") + @" | " + info.AwayTeam.Name.GetTranslation("BET");
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
            
            var ObjCommand = new NpgsqlCommand( Globals.DB_Functions.InsertDyMatchDataAllDetails.ToDescription().ToString());
            try
            {
                if (entityMinfo != null)
                {
                    insertMatchLocal( ConvertLiveMatchToLcooMatch(entityMheader, entityMinfo), 2);
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
                var ret = insert(ObjCommand);
                try
                {
                    var merchList = new List<string>();
                    merchList.Add(config.AppSettings.Get("ChannelsSecretPrefixLast_real"));
                    merchList.Add(config.AppSettings.Get("ChannelsSecretPrefixLast_real2"));
                    merchList.Add(config.AppSettings.Get("ChannelsSecretPrefixLast_real3"));
                    merchList.Add(config.AppSettings.Get("ChannelsSecretPrefixLast_real4"));
                    merchList.Add(config.AppSettings.Get("ChannelsSecretPrefixLast_real5"));
                    merchList.Add(config.AppSettings.Get("ChannelsSecretPrefixLast_real6"));
                    foreach (var merch in merchList)
                    {
                        var socket = new LiveOddSendClient();
                        var langList = new List<string>();
                        langList.Add("BET");
                        langList.Add("en");

                        if (entityMinfo != null)
                        {
                           // Task.Factory.StartNew(() =>
                                socket.SendToHybridgeLiveMenue(CreateLiveOddsChannelName("sports.live", merch),
                                    (entityMheader.Status == null) ? " " : entityMheader.Status.ToString(),
                                    (entityMheader.MatchTime == null) ? " " : entityMheader.MatchTime.ToString(),
                                    (entityMheader.BetStatus == null) ? " " : entityMheader.BetStatus.ToString(),
                                    (entityMheader.Id == null) ? " " : entityMheader.Id.ToString(),
                                    (entityMinfo.HomeTeam == null) ? " " : entityMinfo.HomeTeam.Id.ToString(), " ",
                                    (entityMinfo.AwayTeam == null)
                                        ? " "
                                        : LocalizedStringToJson(entityMinfo.AwayTeam.Name),
                                    (entityMinfo.DateOfMatch == null) ? " " : entityMinfo.DateOfMatch.ToString(),
                                    (entityMinfo.AwayTeam == null) ? " " : entityMinfo.AwayTeam.Id.ToString(),
                                    (entityMheader.Score == null) ? " " : entityMheader.Score,
                                    (entityMinfo.HomeTeam == null)
                                        ? " "
                                        : LocalizedStringToJson(entityMinfo.HomeTeam.Name),
                                    (entityMinfo.Tournament == null)
                                        ? " "
                                        : LocalizedStringToJson(entityMinfo.Tournament.Name),
                                    (entityMinfo.Category == null) ? " " : entityMinfo.Category.Id.ToString(),
                                    " ",
                                    (entityMinfo.Category == null)
                                        ? " "
                                        : LocalizedStringToJson(entityMinfo.Category.Name),
                                    (entityMinfo.Tournament == null) ? " " : entityMinfo.Tournament.Id.ToString(),
                                    (entityMinfo.Sport == null) ? " " : entityMinfo.Sport.Id.ToString(),
                                    (entityMinfo.Sport.Name == null)
                                        ? " "
                                        : LocalizedStringToJson(entityMinfo.Sport.Name));
                            // ).ConfigureAwait(false);
                        }
                        else
                        {


                            
                            //Task.Factory.StartNew(() =>
                            socket.SendToHybridgeLiveMenue(CreateLiveOddsChannelName("sports.live", merch),
                                entityMheader.Status.ToString(),
                                (entityMheader.MatchTime == null) ? " " : entityMheader.MatchTime.ToString(),
                                (entityMheader.BetStatus == null) ? " " : entityMheader.BetStatus.ToString(),
                                (entityMheader.Id == null) ? " " : entityMheader.Id.ToString(), " ", " ", " ", " ", " ",
                                (entityMheader.Score == null) ? " " : entityMheader.Score, " ", " ", " ", " ", " ", " ",
                                " ", " ");
                            // ).ConfigureAwait(false);
                        }
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////

                        socket = null;

                    }

                }
                catch (Exception ex)
                {
                    Logg.logger.Fatal(ex.Message);
                }
                return ret;
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
                var command = new NpgsqlCommand( Globals.DB_Functions.UpdateDmadYellowRedCard.ToDescription().ToString());
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
        public bool UpdateAllLiveOddsOutcomesActive(long match_id, EventOdds entity, bool Active)
        {
            try
            {
                var command = new NpgsqlCommand(Globals.DB_Functions.UpdateCpLiveOddsActive.ToDescription().ToString());
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
        public long insertLiveOdds(EventOdds entity, EventOddsField odd_field,
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


            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
            var command = new NpgsqlCommand(Globals.DB_Functions.InsertLiveOdds.ToDescription().ToString());
            try
            {

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
                if (p_odd_eventoddsfield_viewindex != null)
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

                return insert(command);
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
                return -1;
            }
        }

        public  long updateLiveOdds(string entity_Name, bool p_odd_active, bool? p_odd_changed, long p_odd_combination,
             string p_odd_for_the_rest, string p_odd_free_text, bool? p_odd_most_balanced, string p_odd_special_odds_value, long p_odd_type_id,
              bool p_odd_eventoddsfield_active, bool? p_odd_eventoddsfield_outcomestatus, string p_odd_type,
              long? p_odd_eventoddsfield_playerid, string p_odd_eventoddsfield_probability, string p_odd_eventoddsfield_type,
              string p_odd_eventoddsfield_value, int p_odd_eventoddsfield_viewindex, string p_odd_eventoddsfield_voidfactor,
              string p_odd_eventoddsfield_name, long row_id)
        {

            var queue = new Queue<Globals.Rollback>();
            var Updatecommand = new NpgsqlCommand( Globals.DB_Functions.UpdateLiveOdds.ToDescription().ToString());
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

                return update(Updatecommand);
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
                return -1;
            }
        }

        #region Coupons
        /// <summary>
        /// Insert Bet Clears for Lcoo
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="MatchID"></param>
        /// <returns></returns>
        public long insertCpLcooBetclearOdds(BetResultEntity entity, long MatchID, string mid_otid_ocid_sid)
        {

            var queue = new Queue<Globals.Rollback>();
            var ObjCommand = new NpgsqlCommand( Globals.DB_Functions.InsertCpLcooBetClearOdds.ToDescription().ToString());
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
                return insert(ObjCommand);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return -1;
            }
        }
        public long insertCpLcooBetclearOdds(BetResultEntity entity, long MatchID, string mid_otid_ocid_sid, bool status)
        {

            var queue = new Queue<Globals.Rollback>();
            var ObjCommand = new NpgsqlCommand(Globals.DB_Functions.InsertCpLcooBetClearOdds.ToDescription().ToString());
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
                return  insert(ObjCommand);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return -1;
            }
        }
        public long insertCpLcooBetclearOdds(long MatchID, long OddsType, string Outcome, string OutcomeId, string PlayerId, string Reason,
            string SpecialBetValue, string TeamId, string VoidFactor,
            string mid_otid_ocid_sid, bool status)
        {

            var ObjCommand = new NpgsqlCommand(Globals.DB_Functions.InsertCpLcooBetClearOdds.ToDescription().ToString());
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
                    ObjCommand.Parameters.AddWithValue("p_player_id", NpgsqlDbType.Text, PlayerId);
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
                return  insert(ObjCommand);


            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return -1;
            }
        }


        #endregion

        public string LocalizedStringToJson(LocalizedString tuple)
        {
            var Hdictionary = new Dictionary<string, string>();
            try
            {
                Hdictionary.Add("BET", tuple.International);
                foreach (var lang in tuple.AvailableTranslationLanguages)
                {
                    Hdictionary.Add(lang, tuple.GetTranslation(lang));
                }
                return new JavaScriptSerializer().Serialize(Hdictionary);
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
                return null;
            }
        }
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
        public  string TextsToJson(List<TextEntity> texts)
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
                if (matches.Count > 0)
                {
                    string csv = String.Join(",", matches.Select(x => x.ToString()).ToArray());
                    string query = "UPDATE dy_alive_matches SET matches_on = '" + csv + "', last_update = now() WHERE id = 1;  UPDATE dy_match_all_data SET feed_type = 2 WHERE match_id in (" + csv + ")";
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
       /* public Globals.Rollback SetRollback(long RowId, Globals.Tables Table, Globals.TransactionTypes Transaction)
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

        //public  NpgsqlConnection connection()
        //{
        //    var con = new NpgsqlConnection();
        //    try
        //    {
        //        if (con.State == ConnectionState.Closed)
        //        {

        //            var connectionBuilder = new NpgsqlConnectionStringBuilder();
        //            connectionBuilder.Host = config.AppSettings.Get("DB_Host");
        //            connectionBuilder.Port = int.Parse(config.AppSettings.Get("DB_Port"));
        //            connectionBuilder.Database = config.AppSettings.Get("DB_Database");
        //            connectionBuilder.Username = config.AppSettings.Get("DB_Username");
        //            connectionBuilder.Password = config.AppSettings.Get("DB_Password");
        //            connectionBuilder.Timeout = 300;
        //            connectionBuilder.Pooling = true;
        //            connectionBuilder.CommandTimeout = 300;
        //            con = new NpgsqlConnection(connectionBuilder.ConnectionString);
        //            return con;
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logg.logger.Fatal(ex.Message);
        //        return null;
        //    }

        //}
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
//        public long insert(NpgsqlCommand objCommand)
//        {
//            long errorNumber = -1;
//            long result = -20;
//            object one = null;
//            objCommand.CommandType = CommandType.StoredProcedure;
//            try
//            {

//                objCommand.Connection =  connection();
//                //objCommand.CommandTimeout = 5;
//                if (objCommand.Connection != null)  objCommand.Connection.Open();
//                one =  objCommand.ExecuteScalar();
//                bool successfullyParsed = long.TryParse(one.ToString(), out result);
//                long val = 0;
//                if (successfullyParsed)
//                {
//                    if (result != null && long.TryParse(result.ToString(), out val))
//                    {
//                        if (val > 0)
//                        {
//#if DEBUG
//                            WriteFullLine(objCommand.CommandText);
//#endif
//                            return val;
//                        }
//                        else
//                        {
//                            errorNumber = val;
//                            throw new DataException();
//                        }
//                    }
//                }

//                return -1;
//            }
//            catch (Exception ex)
//            {
//                StackTrace trace = new StackTrace(true);
//                var frame = trace.GetFrame(1);
//                var altMessage = "  Error#: " + errorNumber.ToString() + "  METHOD: " + frame.GetMethod().Name + "  LINE:  " + frame.GetFileLineNumber();
//                Logg.logger.Fatal(ex.Message + altMessage);
//                // Task.Factory.StartNew(() => Globals.Queue_Errors.Enqueue(objCommand));
//                return -1;
//            }
//            finally
//            {
//                if (objCommand.Connection != null) objCommand.Connection.Close();
//            }
//        }
        public long insertNonProc(NpgsqlCommand objCommand)
        {

            long errorNumber = -1;
            objCommand.CommandType = CommandType.Text;
            try
            {

                objCommand.Connection =  connection();
                objCommand.Connection.Open();
                objCommand.CommandTimeout = 100;
                var result =  objCommand.ExecuteNonQuery();
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
                objCommand.Connection =  connection();
                if (objCommand.Connection != null) objCommand.Connection.Open();
#if DEBUG
                WriteFullLine(objCommand.CommandText);
#endif
                var result =  objCommand.ExecuteScalar();
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
                if (objCommand.Connection != null) objCommand.Connection.Close();
            }
        }

        public DataSet selectReader(NpgsqlCommand objCommand)
        {
            var lbetclear = new LcooBetClear();
            var ldata = new List<LcooBetClear>();
            objCommand.Connection =  connection();
            objCommand.Connection.Open();
            objCommand.CommandType = CommandType.StoredProcedure;
            var ds = new DataSet();
            try
            {
                ds.Tables.Add();
                NpgsqlDataReader dr =  objCommand.ExecuteReader() as NpgsqlDataReader;
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
                if (objCommand.Connection != null) objCommand.Connection.Close();
            }
        }
        public DataSet select(NpgsqlCommand objCommand)
        {
            objCommand.Connection = connection();
            if (objCommand.Connection != null)
            {
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
            return null;
        }
        public long selectOne(NpgsqlCommand objCommand)
        {

            objCommand.CommandType = CommandType.Text;
            try
            {
                objCommand.Connection =  connection();
                if (objCommand.Connection != null)  objCommand.Connection.Open();
                var res =  objCommand.ExecuteReader(CommandBehavior.SingleResult);
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
        public  void TraceMessage(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Logg.logger.Fatal(message + "  |||  " + memberName + "  |||  " + sourceFilePath + "  |||  " + sourceLineNumber);
        }
        public  void SendToCoupon(BetClearQueueElement bet)
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
        public static  void WriteFullLine(string value)
        {

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(value.PadRight(Console.WindowWidth - 1));
            Console.ResetColor();
        }*/

    }

}
