using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace Betradar.Classes.DbInsert
{
    public class OddsChangeHandle : Core
    {
        public OddsChangeHandle(OddsChangeEventArgs args)
        {
            //var queue = new OddChangeQueue();
            //queue.arg = args;
            //queue.time = args.OddsChange.Timestamp;
            //Globals.Queue_Odd_Change.Enqueue(queue);
            RunTask(args);

        }

        public void RunTask(OddsChangeEventArgs queueElement)
        {
            var entity = queueElement.OddsChange;
            var common = new Common();
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
                            // common.insertMatchDataAllDetails((MatchHeader)entity.EventHeader, null);


                            Task.Factory.StartNew(
                                () =>
                                {
                                    foreach (var lang in NameDictionary)
                                    {
                                        var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real");
                                        SendToHybridgeSocket(entity.EventHeader.Id, odd.Id, val.TypeId, "", odd.SpecialOddsValue,
                                            val, CreateLiveOddsChannelName(entity.EventHeader.Id, lang.Key, last_prefix));
                                    }

                                    foreach (var lang in NameDictionary)
                                    {
                                        var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real2");
                                        SendToHybridgeSocket(entity.EventHeader.Id, odd.Id, val.TypeId, "", odd.SpecialOddsValue,
                                            val, CreateLiveOddsChannelName(entity.EventHeader.Id, lang.Key, last_prefix));
                                    }

                                }
                                , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);


                            //Task.Factory.StartNew(() =>
                            //{

                            //    foreach (var lang in NameDictionary)
                            //    {
                            //        var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real");
                            //        SendToHybridgeSocket(entity.EventHeader.Id, odd.Id, val.TypeId, "", odd.SpecialOddsValue,
                            //            val, CreateLiveOddsChannelName(entity.EventHeader.Id, lang.Key, last_prefix));
                            //    }

                            //    foreach (var lang in NameDictionary)
                            //    {
                            //        var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real2");
                            //        SendToHybridgeSocket(entity.EventHeader.Id, odd.Id, val.TypeId, "", odd.SpecialOddsValue,
                            //            val, CreateLiveOddsChannelName(entity.EventHeader.Id, lang.Key, last_prefix));
                            //    }
                            //}
                            // )
                            //;
                        }
                    }
                    else
                    {
                        common.UpdateAllLiveOddsOutcomesActive(entity.EventHeader.Id, odd, odd.Active);
                    }
                }
            }

            //common.insertMatchDataAllDetails((MatchHeader)entity.EventHeader, null);

            Task.Factory.StartNew(
                     () =>
                     {
                         common.insertMatchDataAllDetails((MatchHeader)entity.EventHeader, null);
                     }
                     , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

            //Task.Factory.StartNew(
            //    () => common.insertMatchDataAllDetails((MatchHeader)entity.EventHeader, null));
            //Globals.Queue_Odd_Change.Remove(l);
        }
    }
}
