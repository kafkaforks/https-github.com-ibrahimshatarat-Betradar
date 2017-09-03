using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace BetService.Classes.DbInsert
{

    public class BetCancelHandle : Core
    {

        public void BetCancelHandler(BetCancelEventArgs args)
        {
             RunTask(args);
            Task.Factory.StartNew(() => HandleSendOdds(args)).ConfigureAwait(false);
        }

        private void RunTask(BetCancelEventArgs args)
        {

            var common = new Common();
            var queue = new Queue<Globals.Rollback>();
            try
            {
                if (args.BetCancel.Odds != null && args.BetCancel.Odds.Count > 0)
                {
                     insertOdds(args);
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
            try
            {
                
                //TODO: REDISSSSSS
                 common.insertMatchDataAllDetails((MatchHeader)args.BetCancel.EventHeader, null);
              
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        private void HandleSendOdds(BetCancelEventArgs args)
        {
            try
            {
                foreach (var odd in args.BetCancel.Odds)
                {
                        if (odd.OddsFields.Count > 0)
                        {
                            foreach (var field in odd.OddsFields)
                            {
                                var val = field.Value;
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
                                var socket = new LiveOddSendClient();
                                foreach (var lang in NameDictionary)
                                {
                                    var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real");

                                    socket.SendToHybridgeSocket(args.BetCancel.EventHeader.Id, odd.Id, val.TypeId, "",
                                        odd.SpecialOddsValue,
                                        val,
                                        CreateLiveOddsChannelName(args.BetCancel.EventHeader.Id, lang.Key, last_prefix),
                                        "ODDCANCEL");

                                }

                                foreach (var lang in NameDictionary)
                                {
                                    var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real2");

                                    socket.SendToHybridgeSocket(args.BetCancel.EventHeader.Id, odd.Id, val.TypeId, "",
                                        odd.SpecialOddsValue,
                                        val,
                                        CreateLiveOddsChannelName(args.BetCancel.EventHeader.Id, lang.Key, last_prefix),
                                        "ODDCANCEL");

                                }
                                foreach (var lang in NameDictionary)
                                {
                                    var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real3");

                                    socket.SendToHybridgeSocket(args.BetCancel.EventHeader.Id, odd.Id, val.TypeId, "",
                                        odd.SpecialOddsValue,
                                        val,
                                        CreateLiveOddsChannelName(args.BetCancel.EventHeader.Id, lang.Key, last_prefix),
                                        "ODDCANCEL");

                                }
                                foreach (var lang in NameDictionary)
                                {
                                    var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real4");

                                    socket.SendToHybridgeSocket(args.BetCancel.EventHeader.Id, odd.Id, val.TypeId, "",
                                        odd.SpecialOddsValue,
                                        val,
                                        CreateLiveOddsChannelName(args.BetCancel.EventHeader.Id, lang.Key, last_prefix),
                                        "ODDCANCEL");

                                }
                                foreach (var lang in NameDictionary)
                                {
                                    var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real5");

                                    socket.SendToHybridgeSocket(args.BetCancel.EventHeader.Id, odd.Id, val.TypeId, "",
                                        odd.SpecialOddsValue,
                                        val,
                                        CreateLiveOddsChannelName(args.BetCancel.EventHeader.Id, lang.Key, last_prefix),
                                        "ODDCANCEL");

                                }
                                foreach (var lang in NameDictionary)
                                {
                                    var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real6");

                                    socket.SendToHybridgeSocket(args.BetCancel.EventHeader.Id, odd.Id, val.TypeId, "",
                                        odd.SpecialOddsValue,
                                        val,
                                        CreateLiveOddsChannelName(args.BetCancel.EventHeader.Id, lang.Key, last_prefix),
                                        "ODDCANCEL");

                                }
                                NameDictionary = null;
                                socket = null;
                            }
                        }
                }

            }
            catch (Exception ex)
            {
                    
                
            }
        }

        private void insertOdds(BetCancelEventArgs args)
        {
            var common = new Common();
            try
            {
               // var entity = args.BetCancel;
                bool active = true;
                foreach(var odd in args.BetCancel.Odds)
                    {
                    if (odd.Active != null)
                    {
                        active = odd.Active;
                    }

                     common.insertLiveOdds(odd, null, active, null, null,
                        "", null, "", null,
                        "", null, args.BetCancel.EventHeader.Id, 0, args.BetCancel.Status.ToString(), args.BetCancel.Timestamp.ToString());

                    foreach (var field in odd.OddsFields)
                    {
                        var oddUnique = new BetClearQueueElementLive();
                        oddUnique.MatchId = args.BetCancel.EventHeader.Id;
                        oddUnique.OddId = odd.Id;
                        if (odd.TypeId != null)
                        {
                            oddUnique.TypeId = int.Parse(odd.TypeId.ToString());
                        }
                        else
                        {
                            oddUnique.TypeId = null;
                        }
                        try
                        {
                            // sendToRpc(EncodeUnifiedBetClearQueueElementLive(oddUnique));
                            // Task.Factory.StartNew(() => sendToRpc(EncodeUnifiedBetClearQueueElementLive(oddUnique)));

                            var coupon = new Coupons();
                             coupon.BetCancelDB( EncodeUnifiedBetClearQueueElementLive(oddUnique));

                        }
                        catch (Exception ex)
                        {
                            SharedLibrary.Logg.logger.Fatal("SEND TO PROXY ERROR: " + ex.Message);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }
    }
}
