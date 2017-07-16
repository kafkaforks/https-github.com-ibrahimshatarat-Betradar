using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Npgsql;


namespace SharedLibrary
{
    public static class Globals
    {
        public static QueueBetClear<BetClearQueueElement> QueueLcooBetClear;
        //public static BetQueue<ScoreCardSummaryEventArgs> Queue_ScoreCardSummary;
        //public static BetQueue<MatchEventOdds> Queue_MatchEventOdds;
        //public static BetQueue<OutrightEventOdds> Queue_OutRightEventOdds;
        //public static BetQueue<ThreeBallEventArgs> Queue_ThreeBallEvent;
        //public static BetQueue<BetCancelEventArgs> Queue_BetCancel;
        //public static BetQueue<BetCancelUndoEventArgs> Queue_BetCancelUndo;
        //public static BetQueue<BetClearEventArgs> Queue_BetClear;
        //public static BetQueue<BetClearRollbackEventArgs> Queue_BetClearRollBack;
        //public static BetQueue<BetStartEventArgs> Queue_BetStart;
        //public static BetQueue<BetStopEventArgs> Queue_BetStop;
        //public static BetQueue<OddsChangeEventArgs> Queue_OddsChange;
        //public static BetQueue<RaceResultEventArgs> Queue_RaceResult;
        //public static BetQueue<OutrightBetClearEventArgs> Queue_OutrightBetClear;
        //public static BetQueue<OutrightBetStartEventArgs> Queue_OutrightBetStart;
        //public static BetQueue<OutrightBetStopEventArgs> Queue_OutrightBetStop;
        public static BetQueue<string> Queue_Feed;
        public static BetQueue<NpgsqlCommand> Queue_Errors;
        public static BetQueue<OddChangeQueue> Queue_Odd_Change;
        public static BetQueue<BetClearQueueElementLive> Queue_BetClearQueueElementLive;
        public static volatile bool timerOnOff = false;
        public static object lockObject = new object();
        public static DataSet MerchantsDs;
        //public static void insertenum(string membername, int value, string description, int enums)
        //{
        //    try
        //    {
        //        Common qw = new Common();
        //        var dictionaryObjCommand =
        //            new NpgsqlCommand(Globals.DB_Functions.InsertEnum.ToDescription());
        //        dictionaryObjCommand.Parameters.AddWithValue("member_name", membername);
        //        dictionaryObjCommand.Parameters.AddWithValue("value", value);
        //        dictionaryObjCommand.Parameters.AddWithValue("description", description);
        //        dictionaryObjCommand.Parameters.AddWithValue("fk_enums_id", enums);
        //        qw.insert(dictionaryObjCommand);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public enum BetFeedTypes
        {
            [Description("PreMatch")]
            PreMatch = 1,
            [Description("LiveOdds")]
            LiveOdds = 2
        }

        public enum FeedTypes
        {
            //[Description("live_ScoreCardSummary")]
            //live_ScoreCardSummary = 1,
            [Description("BetCancel")]
            BetCancel = 1,
            [Description("BetCanselUndo")]
            BetCanselUndo = 2,
            [Description("BetClear")]
            BetClear = 3,
            [Description("BetClearRollBack")]
            BetClearRollBack = 4,
            [Description("BatStart")]
            BatStart = 5,
            [Description("BetStop")]
            BetStop = 6,
            [Description("MatchEventOdds")]
            MatchEventOdds = 7,
            [Description("OutrightEventOdds")]
            OutrightEventOdds = 8,
            [Description("RawFeed")]
            RawFeed = 9,
            [Description("ScoreCardSummary")]
            ScoreCardSummary = 10,
            [Description("OddsCreatorModule")]
            OddsCreatorModule = 11,
            [Description("OddsChange")]
            OddsChange = 12,
            [Description("OutrightBetClear")]
            OutrightBetClear = 13,
            [Description("OutrightBetStart")]
            OutrightBetStart = 14,
            [Description("OutrightBetStop")]
            OutrightBetStop = 15,
            [Description("ThreeBallEvent")]
            ThreeBallEvent = 16,
            [Description("BetStart")]
            BetStart = 17


        }

        public enum CouponMove:long
        {
            [Description("MoveToFinalized")]
            MoveToFinalized = 2,
            [Description("MoveToCancelled")]
            MoveToCancelled = 3
        }

        public enum DB_Functions
        {
            [Description("insert_dictionary")]
            InserDictionary = 1,
            [Description("insert_dictionaries")]
            InsertDictionaries = 2,
            [Description("insert_live_alive")]
            InsertLive = 3,
            [Description("insert_cards")]
            InsertCards = 4,
            [Description("insert_card")]
            InsertCard = 5,
            [Description("insert_enum")]
            InsertEnum = 6,
            [Description("insert_enums")]
            InsertEnums = 7,
            [Description("insert_message")]
            InsertMessage = 8,
            [Description("insert_messages")]
            InsertMessages = 9,
            [Description("insert_match_header")]
            InserMatchHeader = 10,
            [Description("insert_home_away")]
            InsertHomeAway = 11,
            [Description("select_enum_by_names")]
            SelectEnumsByNames = 12,
            [Description("insert_bet_stop_reason")]
            InsertBetStopReason = 13,
            [Description("insert_set_scores")]
            InsertSetScores = 14,
            [Description("insert_set_score")]
            InsertSetScore = 15,
            [Description("insert_transaction")]
            InsertTransaction = 16,
            [Description("select_tableid_by_tablenames")]
            SelectTableIdByTableName = 17,
            [Description("insert_betfair_id")]
            InsertBetfairId = 18,
            [Description("insert_bet_results")]
            InsertBetResults = 19,
            [Description("insert_bet_result")]
            InsertBetResult = 20,
            [Description("insert_player")]
            InsertPlayer = 21,
            [Description("insert_texts")]
            InsertTexts = 22,
            [Description("insert_text")]
            InsertText = 23,
            [Description("insert_category")]
            InsertCategory = 24,
            [Description("insert_corners")]
            InsertCorners = 25,
            [Description("insert_corner")]
            InsertCorner = 26,
            [Description("insert_fixture")]
            InsertFixture = 27,
            [Description("insert_aggregate_score")]
            InsertAggregateScore = 28,
            [Description("insert_competitors")]
            InsertCompetitors = 29,
            [Description("insert_players")]
            InsertPlayers = 30,
            [Description("select_tablename_by_tableid")]
            SelectTablenameByTableId = 31,
            [Description("rollback")]
            Rollback = 32,
            [Description("insert_comment")]
            InsertComment = 33,
            [Description("insert_date_info")]
            InsertDateinfo = 34,
            [Description("insert_type_value_tuple")]
            InsertTypeValueTuple = 35,
            [Description("insert_tournament")]
            InsertTournament = 36,
            [Description("insert_event_info")]
            InsertEventInfo = 37,
            [Description("insert_id_name_tuple")]
            InsertIdNameTuple = 38,
            [Description("insert_languages")]
            InsertLanguages = 39,
            [Description("insert_available_translation_languages")]
            InsertAvailableTrasnlationLanguages = 40,
            [Description("insert_localized_string")]
            InsertLocalizedString = 41,
            [Description("select_last_errors")]
            SelectLastErrors = 42,
            [Description("select_last_transactions")]
            SelectLastTransactions = 43,
            [Description("insert_round_info")]
            InsertRountInfo = 44,
            [Description("insert_series_result")]
            InsertSeriesResult = 45,
            [Description("insert_status_info")]
            InsertStatusInfo = 46,
            [Description("insert_coordinates")]
            InsertCoordinates = 47,
            [Description("insert_venue")]
            InsertVenue = 48,
            [Description("insert_goals")]
            InsertGoals = 49,
            [Description("insert_goal")]
            InsertGoal = 50,
            [Description("insert_team")]
            InsertTeam = 51,
            [Description("insert_bet")]
            InsertBet = 52,
            [Description("insert_odds")]
            InsertOdds = 53,
            [Description("insert_bet_odds")]
            InsertBetOdds = 54,
            [Description("insert_bets")]
            InsertBets = 55,
            [Description("insert_pitcher")]
            InsertPitcher = 56,
            [Description("insert_probabilities")]
            InsertProbabilities = 57,
            [Description("insert_odds_probabilities")]
            InsertOddsProbabilities = 58,
            [Description("insert_odds_probability")]
            InsertOddsProbability = 59,
            [Description("insert_probability")]
            InsertProbability = 60,
            [Description("insert_scores_info")]
            InsertScoresInfo = 61,
            [Description("insert_winning_outcome")]
            InsertWinningOutcome = 62,
            [Description("insert_result")]
            InsertResult = 63,
            [Description("insert_scores")]
            InsertScores = 64,
            [Description("insert_sport")]
            InsertSport = 65,
            [Description("insert_tv_channel")]
            InsertTvChannel = 66,
            [Description("insert_tv_channels")]
            InsertTvChannels = 67,
            [Description("insert_event_header")]
            InserEventHeader = 68,
            [Description("insert_event_odds_field")]
            InsertEventOddsField = 69,
            [Description("insert_event_odds")]
            InsertEventOdds = 70,
            [Description("insert_event_odds_fields")]
            InsertEventOddsFields = 71,
            [Description("insert_event_odds_fields_dictionary")]
            InsertEventOddsFieldsDictionary = 72,
            [Description("insert_raw_feed")]
            InsertRawFeed = 73,
            [Description("insert_outright")]
            InsertOutright = 74,
            [Description("insert_outright_result")]
            InsertOutrightResult = 75,
            [Description("insert_outright_results")]
            InsertOutrightResults = 76,
            [Description("insert_three_ball")]
            InsertThreeBall = 77,
            [Description("insert_event_odds_many")]
            InsertEventOddsMany = 78,
            [Description("insert_managers")]
            InsertManagers = 79,
            [Description("insert_manager")]
            InsertManager = 80,
            [Description("insert_team_offical")]
            InsertTeamOffical = 81,
            [Description("insert_team_officals")]
            InsertTeamOfficals = 82,
            [Description("insert_lineups")]
            InsertLineups = 83,
            [Description("insert_attribute")]
            InsertAttribute = 84,
            [Description("insert_attributes")]
            InsertAttributes = 85,
            [Description("insert_player_entity")]
            InsertPlayerEntity = 86,
            [Description("insert_match_role")]
            InsertMatchRole = 87,
            [Description("insert_match_roles")]
            InsertMatchRoles = 88,
            [Description("insert_match")]
            InsertMatch = 89,
            [Description("insert_translations")]
            InsertTranslations = 90,
            [Description("insert_ls_match_data")]
            InsertLiveScoutMatchData = 91,
            [Description("insert_ls_match_booking")]
            InsertLiveScoutMatchBooking = 92,
            [Description("update_texts_text_ids")]
            UpdateTextsIds = 93,
            [Description("insert_match_all_data")]
            InsertMatchAllData = 94,
            [Description("insert_cp_coupon_odds")]
            InsertCouponOdds = 95,
            [Description("update_dy_match_all_data_two")]
            UpdateDyMatchAllData = 96,
            [Description("insert_cp_lcoo_odds")]
            InsertCpLcooOdds = 97,
            [Description("update_cp_lcoo_odds")]
            UpdateCpLcooOdds = 98,
            [Description("insert_cp_coupon_odds")]
            InsertCpCouponOdds = 99,
            [Description("insert_cp_lcoo_betclear_odds")]
            InsertCpLcooBetClearOdds = 100,
            [Description("update_cp_lcoo_odds_off")]
            UpdateCpLcooOddsOff = 101,
            [Description("cp_calculate_system_combinations")]
            CpCalculateSystemCombination = 102,
            [Description("select_coupon_odds_temp")]
            SelectCouponOddsTemp = 103,
            [Description("cp_finalize_odd")]
            FinalizeOdd = 104,
            [Description("select_finalized_odd_coupons")]
            FinalizeOddCoupons = 105,
            [Description("select_coupon_systems")]
            GetCouponSystems = 106,
            [Description("cp_get_full_finalised_coupons")]
            GetFullFinalisedCoupons = 107,
            [Description("cp_update_sent_coupons")]
            UpdateSentCounpons = 108,
            [Description("cp_coupon_winning_odds")]
            CouponWinningOdds = 109,
            [Description("insert_dy_match_data_all_details")]
            InsertDyMatchDataAllDetails = 110,
            [Description("insert_cp_live_odds")]
            InsertLiveOdds = 111,
            [Description("insert_cp_tournament")]
            InsertCpTournament = 112,
            [Description("update_cp_live_odds")]
            UpdateLiveOdds = 113,
            [Description("update_cp_live_odds_active")]
            UpdateCpLiveOddsActive = 114,
            [Description("fe_update_live_matches_show")]
            UpdateLiveMatchesShow = 115,
            [Description("select_dy_merchants")]
            selectDyMerchants = 116,
            [Description("insert_dy_merchants")]
            InsertDyMerchants = 117,
            [Description("update_dy_match_data_all_details_yellow_red_card")]
            UpdateDmadYellowRedCard = 118,
            [Description("insert_cp_team")]
            InsertCpTeam = 119,
            [Description("insert_cp_send_seamles")]
            InsertSendSeamless = 120,
            [Description("cp_update_sent_coupons_once")]
            UpdateSentCouponsOnce= 121,
            [Description("update_cp_send_seamless")]
            UpdateSendSeamless= 122,
            [Description("select_merchants")]
            SelectMerchants = 123,
            [Description("cp_cancel_odd")]
            CancelOdd = 124
        }
        public enum Tables
        {
            [Description("dy_series_result")]
            Series_Result = 1,
            [Description("st_venue")]
            Venue = 2,
            [Description("dy_aggregate_score")]
            Aggregate_Score = 3,
            [Description("dy_bet_fair_id")]
            Bet_Fair_Id = 4,
            [Description("dy_fixture")]
            Fixture = 5,
            [Description("dy_match")]
            Match = 6,
            [Description("dy_winning_outcome")]
            Winning_Outcome = 7,
            [Description("dy_result")]
            Result = 8,
            [Description("dy_card")]
            Card = 9,
            [Description("dy_text")]
            Text = 10,
            [Description("live_common_feed")]
            Live_Common_Feed = 11,
            [Description("dy_bet")]
            Bet = 12,
            [Description("dy_bet_result")]
            Bet_Result = 13,
            [Description("dy_match_odds")]
            Match_Odds = 14,
            [Description("st_pitcher")]
            Pitcher = 15,
            [Description("dy_round_info")]
            Round_Info = 16,
            [Description("dy_sports")]
            Sports = 17,
            [Description("st_team")]
            Team = 18,
            [Description("dy_goal")]
            Goal = 19,
            [Description("dy_odds")]
            Odds = 20,
            [Description("dy_status_info")]
            Status_Info = 21,
            [Description("enm_odds_server_type")]
            Odds_Server_Type = 22,
            [Description("enm_odds_status")]
            Odds_Status = 23,
            [Description("st_player")]
            Player = 24,
            [Description("dy_odds_probability")]
            Odds_Probability = 25,
            [Description("dy_scores")]
            Scores = 26,
            [Description("dy_texts")]
            Texts = 27,
            [Description("enm_odds_reply_type")]
            Odds_Reply_Type = 28,
            [Description("dy_comment")]
            Comment = 29,
            [Description("dy_competitors")]
            Competitors = 30,
            [Description("lcoo_bet_data")]
            Lcoo_Bet_Data = 31,
            [Description("st_players")]
            Players = 32,
            [Description("dy_category")]
            Category = 33,
            [Description("st_coordinates")]
            Coordinates = 34,
            [Description("dy_corner")]
            Corner = 35,
            [Description("dy_corners")]
            Corners = 36,
            [Description("dy_date_info")]
            Date_Info = 37,
            [Description("dy_event_info")]
            Event_Info = 38,
            [Description("dy_outright")]
            Outright = 39,
            [Description("dy_scores_info")]
            Scores_Info = 40,
            [Description("dy_message")]
            Message = 41,
            [Description("dy_messages")]
            Messages = 42,
            [Description("dy_event_headers")]
            Event_Headers = 43,
            [Description("logg_errors")]
            Logg_Errors = 44,
            [Description("dy_event_header")]
            Event_Header = 45,
            [Description("dy_cards")]
            Cards = 46,
            [Description("st_countries")]
            Countries = 47,
            [Description("dy_dictionaries")]
            Dictionaries = 48,
            [Description("dy_dictionary")]
            Dictionary = 49,
            [Description("dy_goals")]
            Goals = 50,
            [Description("dy_id_name_tuple")]
            Id_Name_Tuple = 51,
            [Description("st_tournament")]
            Tournament = 52,
            [Description("enm_tables")]
            Tables = 53,
            [Description("enums")]
            Enums = 54,
            [Description("enum")]
            Enum = 55,
            [Description("dy_outright_event_odds")]
            OutrightEventOdds = 56,
            [Description("dy_probability")]
            Probability = 57,
            [Description("dy_set_scores")]
            SetScores = 58,
            [Description("dy_home_away")]
            HomeAway = 59,
            [Description("enm_feed_type")]
            FeedType = 60,
            [Description("dy_set_score")]
            SetScore = 61,
            [Description("dy_bet_stop_reason")]
            BetStopReason = 62,
            [Description("dy_match_header")]
            MatchHeader = 63,
            [Description("enm_transaction_types")]
            TransactionTypes = 64,
            [Description("logg_transactions")]
            Transactions = 65,
            [Description("dy_bet_results")]
            BetResults = 66,
            [Description("dy_type_value_tuple")]
            TypeValueTuple = 67,
            [Description("st_languages")]
            Languages = 68,
            [Description("dy_available_translation_languages")]
            AvailableTranslationLanguages = 69,
            [Description("dy_localized_string")]
            LocalizedString = 70,
            [Description("dy_bet_odds")]
            BetOdds = 71,
            [Description("dy_bets")]
            Bets = 72,
            [Description("dy_probabilities")]
            Probabilities = 73,
            [Description("dy_odds_probabilities")]
            OddsProbabilities = 74,
            [Description("dy_sport")]
            Sport = 75,
            [Description("st_tv_channels")]
            TvChannels = 76,
            [Description("st_tv_channel")]
            TvChannel = 77,
            [Description("dy_event_odds_field")]
            EventOddsField = 78,
            [Description("dy_event_odds")]
            EventOdds = 79,
            [Description("dy_event_odds_fields")]
            EventOddsFields = 80,
            [Description("dy_event_odds_fields_dictionary")]
            EventOddsFieldsDictionary = 81,
            [Description("dy_raw_feed")]
            RawFeed = 82,
            [Description("dy_outright_result")]
            OutrightResult = 83,
            [Description("dy_outright_results")]
            OutrightResults = 84,
            [Description("dy_three_ball")]
            ThreeBall = 85,
            [Description("dy_event_odds_many")]
            EventOddsMany = 86,
            [Description("st_managers")]
            Managers = 87,
            [Description("st_manager")]
            Manager = 88,
            [Description("st_team_offical")]
            TeamOfficial = 89,
            [Description("st_team_officals")]
            TeamOfficals = 90,
            //[Description("dy_lineups")]
            //LineUps = 91,
            [Description("dy_attribute")]
            Attribute = 92,
            [Description("dy_attributes")]
            Attributes = 93,
            [Description("dy_match_roles")]
            MatchRoles = 94,
            [Description("dy_match_role")]
            MatchRole = 95,
            [Description("dy_translations")]
            Translations = 96,
            [Description("ls_match_booking")]
            LiveScoutMatchBooking = 97,
            [Description("ls_lineups")]
            LiveScoutLineUps = 98,
            [Description("ls_match_data")]
            LiveScoutMatchData = 99,
            //[Description("dy_coupon")]
            //Coupon = 100,
            [Description("dy_coupon_odds")]
            CouponOdds = 101,
            [Description("dy_coupon_result_queue")]
            CouponResultQueue = 102,
            [Description("dy_coupon_results")]
            CouponResults = 103,
            [Description("dy_margin_types")]
            MarginTypes = 104,
            [Description("dy_profit_margin")]
            ProfitMargin = 105,
            [Description("dy_merchants")]
            Merchants = 106,
            [Description("limitations_roles")]
            LimitationsRoles = 107,
            [Description("limitations_roles_model_types")]
            LimitationsRolesModelTypes = 108,
            [Description("limitations_roles_rules")]
            LimitationsRolesRules = 109,
            [Description("dy_match_all_data")]
            MatchAllData = 110,
            [Description("cp_coupon_cancelled")]
            CouponCancelled = 111,
            [Description("cp_coupon_finalised")]
            CouponFinalised = 112,
            [Description("cp_coupon_temp")]
            CouponTemp = 113,
            [Description("cp_coupon_odds")]
            CouponOddsNew = 114,
            [Description("country")]
            Country = 115,
            [Description("cp_popular_leagues")]
            PopularLeagues = 116,
            [Description("cp_lcoo_betclear_odds")]
            LcooBetclearOdds = 117
        }

        public enum MasterEnum
        {
            [Description("EventStatus")]
            EventStatus = 1,
            [Description("Team")]
            Team = 2,
            [Description("OddsMatchEarlyBetStatus")]
            OddsMatchEarlyBetStatus = 3,
            [Description("EventBetStatus")]
            EventBetStatus = 4,
            [Description("FeedType")]
            FeedType = 5
        }
        public enum TransactionTypes
        {
            [Description("insert")]
            Insert = 1,
            [Description("update")]
            Update = 2,
            [Description("delete")]
            Delete = 3
        }
        public partial class MessagesString
        {
            public string[] messages { get; set; }
        }
        public partial class ReturnQueueLong
        {
            public ReturnQueueLong()
            {

            }

            public ReturnQueueLong(Queue<Rollback> iQueue, long iId)
            {
                queue = iQueue;
                id = iId;
            }

            public Queue<Rollback> queue { get; set; }
            public long id { get; set; }
        }
        public partial class enum_grouping
        {
            public int master { get; set; }
            public int slave { get; set; }
        }
        public partial class Rollback
        {
            public long RowId { get; set; }
            public int TableId { get; set; }
            public int TransactionType { get; set; }
        }
        public partial class return_nonquery
        {
            public int f1 { get; set; }
            public string f2 { get; set; }
        }
        public partial class insert_type
        {
            public return_nonquery notes { get; set; }
            public long sequence { get; set; }
        }
        public partial class Serialization
        {
            public static string XmlSerialize(object item)
            {
                //try
                //{
                //string serializedString = string.Empty;
                //XmlSerializer xmlSerializer = new XmlSerializer(item.GetType());
                //using (StringWriter stringWriter = new StringWriter())
                //{
                //    xmlSerializer.Serialize(stringWriter, item);
                //    serializedString = stringWriter.ToString();
                //    stringWriter.Flush();
                //}
                //return serializedString;
                //}
                //catch (Exception ex)
                //{
                //    return null;

                //}
                return "{MatchHeader[Active=True,BetStatus=STOPPED,Id=10761580,Message=null,Msgnr=null,Status=UNDEFINED,Booked=null,Active=True,AutoTraded=null,Balls=null,Bases=null,Batter=(:),BetStopReason=null,ClearedScore=null,ClockStopped=null,Corners=(:),CurrentCtTeam=null,CurrentEnd=null,Delivery=null,Dismissals=null,EarlyBetStatus=null,GameScore=null,Innings=null,LegScore=null,MatchTime=null,MatchTimeExtended=null,Outs=null,Over=null,PentaltyRuns=null,Position=null,Possession=null,RedCards=(:),RemainingBowls=(:),RemainingReds=null,RemainingTime=null,RemainingTimeInPeriod=null,Score=null,Server=null,SetScores=[],SourceId=null,Strikes=null,Suspend=(:),Throw=null,TieBreak=null,Try=null,Visit=null,Yards=null,YellowCards=(:),YellowRedCards=(:)]}";
            }

            public static T XmlDeserialize<T>(string item)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    using (StringReader stringReader = new StringReader(item))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        return (T)serializer.Deserialize(stringReader);
                    }
                }
                return default(T);
            }

            public static string XmlSerializeToPureXML(object item)
            {
                string serializedXml = string.Empty;

                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.OmitXmlDeclaration = true;
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.CloseOutput = true;
                xmlWriterSettings.Encoding = Encoding.UTF8;

                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(item.GetType());
                        XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                        xmlSerializerNamespaces.Add(string.Empty, string.Empty);
                        xmlSerializer.Serialize(xmlWriter, item, xmlSerializerNamespaces);
                        serializedXml = stringWriter.ToString();
                        xmlWriter.Flush();
                    }
                    stringWriter.Flush();
                }
                return serializedXml;
            }

            public static string XmlSerializeWithNoXmlDeclaration(Object item)
            {
                string serializedXml = string.Empty;

                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.OmitXmlDeclaration = true;
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.CloseOutput = true;
                xmlWriterSettings.Encoding = Encoding.UTF8;

                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(item.GetType());
                        xmlSerializer.Serialize(xmlWriter, item);
                        serializedXml = stringWriter.ToString();
                        xmlWriter.Flush();
                    }
                    stringWriter.Flush();
                }
                //
                return serializedXml;
            }
        }
    }
}
