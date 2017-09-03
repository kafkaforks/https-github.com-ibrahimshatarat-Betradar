using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using NpgsqlTypes;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using SharedLibrary;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BetService.Classes.DbInsert;
using RestSharp.Extensions;
using SharedLibrary.RPC;
using Sportradar.SDK.FeedProviders.Lcoo;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace BetService.Classes.DbInsert
{
    class BetClearHandle : Core
    {
        public void BetClearHandler(BetClearEventArgs args)
        {
             RunTask(args);
        }

        private void RunTask(BetClearEventArgs queueElement)
        {
            //var client = new Client();
            // var proxy = client.ServerproxyLive();
            var common = new Common();
            try
            {
                foreach (var Odd in queueElement.BetClear.Odds)
                {
                    var NameDictionary = new Dictionary<string, string>();
                    NameDictionary.Add("BET", Odd.Name.International);
                    NameDictionary.Add("en", Odd.Name.International);
                    foreach (var language in Odd.Name.AvailableTranslationLanguages)
                    {
                        NameDictionary.Add(language, Odd.Name.GetTranslation(language));
                    }
                    foreach (var odd in Odd.OddsFields.Values)
                    {
                        var oddUnique = new BetClearQueueElementLive();
                        oddUnique.MatchId = queueElement.BetClear.EventHeader.Id;
                        oddUnique.OddId = Odd.Id;
                        if (odd.TypeId != null)
                        {
                            oddUnique.TypeId = odd.TypeId;
                        }
                        else
                        {
                            oddUnique.TypeId = null;
                        }

                         common.insertLiveOdds(Odd, odd, Odd.Active, odd.Outcome, odd.PlayerId,
                            odd.Probability.ToString() ?? "", odd.Type, odd.Value.ToString() ?? "",
                            odd.ViewIndex,
                            odd.VoidFactor.ToString() ?? "", new JavaScriptSerializer().Serialize(NameDictionary), queueElement.BetClear.EventHeader.Id, odd.TypeId ?? 0, queueElement.BetClear.Status.ToString(), queueElement.BetClear.Timestamp.ToString());

                        NameDictionary = null;
                        try
                        {
                            var coupon = new Coupons();
                             coupon.MatchFinalize(EncodeUnifiedBetClearQueueElementLive(oddUnique));
                        }
                        catch (Exception ex)
                        {
                            SharedLibrary.Logg.logger.Fatal("SEND TO PROXY ERROR: " + ex.Message);
                        }
                    }
                    Task.Factory.StartNew(() => common.insertMatchDataAllDetails((MatchHeader)queueElement.BetClear.EventHeader, null)).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }
    }
}
