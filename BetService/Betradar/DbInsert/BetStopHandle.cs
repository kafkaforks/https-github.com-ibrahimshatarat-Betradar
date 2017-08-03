using System;
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
    public class BetStopHandle : Core
    {
        public async Task BetStopHandler(BetStopEventArgs args)
        {
            try
            {

                var common = new Common();
                await common.insertMatchDataAllDetails((MatchHeader)args.BetStop.EventHeader, null);
                var merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real");
                var channel = await CreateLiveOddsChannelName(args.BetStop.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real2");
                var channel2 = await CreateLiveOddsChannelName(args.BetStop.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real3");
                var channel3 = await CreateLiveOddsChannelName(args.BetStop.EventHeader.Id, "global", merch);

                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel2, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel3, "BETSTOP");



                //// Benchmark  Testing
                /// 
                /// 
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel2, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel3, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel2, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel3, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel2, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel3, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel2, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel3, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel2, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel3, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel2, "BETSTOP");
                await SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel3, "BETSTOP");

                /// 
                /// 
                /// 
                /// END
                
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }


    }
}
