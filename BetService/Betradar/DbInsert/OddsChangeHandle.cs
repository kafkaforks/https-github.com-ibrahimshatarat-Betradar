using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BetService.Classes.DbInsert;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace BetService.Classes.DbInsert
{
    public class OddsChangeHandle : Core
    {
        public async Task OddsChangeHandler(OddsChangeEventArgs args)
        {

            var common = new Common();
            bool active;


            foreach (var l_odds in args.OddsChange.Odds)
            {

            }


            foreach (var odd in args.OddsChange.Odds)
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
                            await common.insertLiveOdds(odd, val, active, val.Outcome, val.PlayerId,
                                val.Probability.ToString() ?? "", val.Type, val.Value.ToString() ?? "",
                                val.ViewIndex,
                                val.VoidFactor.ToString() ?? "", field.Key, args.OddsChange.EventHeader.Id,
                                val.TypeId ?? 0, args.OddsChange.Status.ToString(), args.OddsChange.Timestamp.ToString());

                            var NameDictionary = new Dictionary<string, string>();
                            if (odd.Name != null)
                            {
                                NameDictionary.Add("BET", odd.Name.International);
                                NameDictionary.Add("en", odd.Name.International);
                                foreach (var language in odd.Name.AvailableTranslationLanguages)
                                {
                                    NameDictionary.Add(language, odd.Name.GetTranslation(language));
                                }
                            }

                            //TODO OPEN
                            var csr = new CancellationTokenSource();
                            foreach (var lang in NameDictionary)
                            {
                                var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real");
                                await SendToHybridgeSocket(args.OddsChange.EventHeader.Id, odd.Id, val.TypeId, "", odd.SpecialOddsValue,
                                    val, CreateLiveOddsChannelName(args.OddsChange.EventHeader.Id, lang.Key, last_prefix), "ODDCHANGE");
                            }

                            foreach (var lang in NameDictionary)
                            {
                                var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real2");
                                await SendToHybridgeSocket(args.OddsChange.EventHeader.Id, odd.Id, val.TypeId, "", odd.SpecialOddsValue,
                                     val, CreateLiveOddsChannelName(args.OddsChange.EventHeader.Id, lang.Key, last_prefix), "ODDCHANGE");
                            }
                            foreach (var lang in NameDictionary)
                            {
                                var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real3");
                                await SendToHybridgeSocket(args.OddsChange.EventHeader.Id, odd.Id, val.TypeId, "", odd.SpecialOddsValue,
                                    val, CreateLiveOddsChannelName(args.OddsChange.EventHeader.Id, lang.Key, last_prefix), "ODDCHANGE");
                            }
                            NameDictionary = null;

                        }
                    }
                    else
                    {
                        await common.UpdateAllLiveOddsOutcomesActive(args.OddsChange.EventHeader.Id, odd, odd.Active);
                    }
                }
            }
        }
    }
}
