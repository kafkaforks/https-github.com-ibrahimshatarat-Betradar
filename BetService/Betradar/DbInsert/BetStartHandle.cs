using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BetService.Classes.DbInsert;
using Npgsql;
using NpgsqlTypes;
using ServiceStack;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace BetService.Classes.DbInsert
{
    public class BetStartHandle : Core
    {
        public void BetStartHandler(BetStartEventArgs args)
        {
            try
            {
                var common = new Common();
                common.insertMatchDataAllDetails((MatchHeader)args.BetStart.EventHeader, null);

                var merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real");
                var channel = CreateLiveOddsChannelName(args.BetStart.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real2");
                var channel2 = CreateLiveOddsChannelName(args.BetStart.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real3");
                var channel3 = CreateLiveOddsChannelName(args.BetStart.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real4");
                var channel4 = CreateLiveOddsChannelName(args.BetStart.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real5");
                var channel5 = CreateLiveOddsChannelName(args.BetStart.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real6");
                var channel6 = CreateLiveOddsChannelName(args.BetStart.EventHeader.Id, "global", merch);
                var socket = new LiveOddSendClient();
                socket.SendToHybridgeSocketMessages(args.BetStart.Status.ToString(), channel, "BETSTART");
                socket.SendToHybridgeSocketMessages(args.BetStart.Status.ToString(), channel2, "BETSTART");
                socket.SendToHybridgeSocketMessages(args.BetStart.Status.ToString(), channel3, "BETSTART");
                socket.SendToHybridgeSocketMessages(args.BetStart.Status.ToString(), channel4, "BETSTART");
                socket.SendToHybridgeSocketMessages(args.BetStart.Status.ToString(), channel5, "BETSTART");
                socket.SendToHybridgeSocketMessages(args.BetStart.Status.ToString(), channel6, "BETSTART");
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }

    }
}
