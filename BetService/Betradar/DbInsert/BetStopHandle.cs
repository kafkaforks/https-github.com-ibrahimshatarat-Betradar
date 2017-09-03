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
        public void BetStopHandler(BetStopEventArgs args)
        {
            try
            {

                var common = new Common();
                common.insertMatchDataAllDetails((MatchHeader)args.BetStop.EventHeader, null);
                var merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real");
                string channel = CreateLiveOddsChannelName(args.BetStop.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real2");
                var channel2 = CreateLiveOddsChannelName(args.BetStop.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real3");
                var channel3 = CreateLiveOddsChannelName(args.BetStop.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real4");
                var channel4 = CreateLiveOddsChannelName(args.BetStop.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real5");
                var channel5 = CreateLiveOddsChannelName(args.BetStop.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real6");
                var channel6 = CreateLiveOddsChannelName(args.BetStop.EventHeader.Id, "global", merch);

                var socket = new LiveOddSendClient();
                socket.SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel, "BETSTOP");
                socket.SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel2, "BETSTOP");
                socket.SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel3, "BETSTOP");
                socket.SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel4, "BETSTOP");
                socket.SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel5, "BETSTOP");
                socket.SendToHybridgeSocketMessages(args.BetStop.Status.ToString(), channel6, "BETSTOP");
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }


    }
}
