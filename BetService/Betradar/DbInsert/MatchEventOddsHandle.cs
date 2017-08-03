using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BetService.Classes.DbInsert;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using SharedLibrary.RPC;
using Sportradar.SDK.FeedProviders.Lcoo;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace BetService.Classes.DbInsert
{
    public class MatchEventOddsHandle : Core
    {

        public async 
        Task
MatchEventOddsHandler(MatchEventOdds args)
        {
            await MatchEventOdds_Queue_WatchQueueMatches(args);
        }
        public object GetPropertyValue(object obj, string propertyName)
        {
            var _propertyNames = propertyName.Split('.');

            for (var i = 0; i < _propertyNames.Length; i++)
            {
                if (obj != null)
                {
                    var _propertyInfo = obj.GetType().GetProperty(_propertyNames[i]);
                    if (_propertyInfo != null)
                        obj = _propertyInfo.GetValue(obj);
                    else
                        obj = null;
                }
            }

            return obj;
        }
        public async Task MatchEventOdds_Queue_WatchQueueMatches(MatchEventOdds queueElement)
        {

            var common = new Common();
            var match = queueElement.MatchEntity;
            try
            {
                if (match.BetResults != null)
                {
                    foreach (var bet_result in match.BetResults)
                    {
                        var bet = new BetClearQueueElement();
                        bet.MatchId = match.MatchId;
                        bet.OddsId = bet_result.OddsType;
                        if (bet_result.OutcomeId != null)
                        {
                            bet.OutcomeId = int.Parse(bet_result.OutcomeId);
                        }
                        bet.SpecialBetValue = bet_result.SpecialBetValue;
                        bet.PlayerId = bet_result.PlayerId;
                        bet.TeamId = bet_result.TeamId;
                        var mid = EncodeUnifiedBetClearQueueElement(bet);
                        await common.insertCpLcooBetclearOdds(bet_result, match.MatchId, mid);
                    }
                }
                common.insertMatch(match, 1);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
            try
            {
                // Here we add the odds of the match

                int[] visible_odd_types = new int[] { 10, 46, 60, 42, 20, 225, 52 };
                if (match.Odds != null)
                {
                    foreach (var Odds in match.Odds)
                    {
                        foreach (var odd in Odds.Odds)
                        {
                            if (odd.Value != "OFF")
                            {
                                // New Pre match Odds Insert to new Table.

                                var probability = "";
                                if (match.Probabilities != null)
                                {

                                    foreach (var prob in match.Probabilities)
                                    {
                                        if (prob.OddsType == Odds.OddsType)
                                        {
                                            foreach (var odd_prob in prob.OddsProbabilities)
                                            {
                                                if (odd_prob.Outcome == odd.OutCome)
                                                {
                                                    if (odd_prob.Value != null)
                                                    {
                                                        probability = odd_prob.Value;
                                                    }
                                                    else
                                                    {
                                                        probability = String.Empty;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (visible_odd_types.Contains(Odds.OddsType))
                                {
                                    await common.insertCpLcooOdds(odd, match.MatchId, Odds.OddsType, "", "", true, probability);
                                }
                                else
                                {
                                    await common.insertCpLcooOdds(odd, match.MatchId, Odds.OddsType, "", "", false, probability);
                                }

                            }
                            else
                            {
                                await common.updateCpLcooOdds(match.MatchId, Odds.OddsType);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
            }


#if DEBUG
            OutCount += 1;
            Logg.logger.Error("InCount Count = " + InCount + "  ||||| OutCount = " + OutCount);
#endif
        }
    }
}
