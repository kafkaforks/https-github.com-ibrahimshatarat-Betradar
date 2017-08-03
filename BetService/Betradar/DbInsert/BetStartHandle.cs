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
        public async Task BetStartHandler(BetStartEventArgs args)
        {
            try
            {
                var common = new Common();
                await common.insertMatchDataAllDetails((MatchHeader)args.BetStart.EventHeader, null);

                var merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real");
                var channel = await CreateLiveOddsChannelName(args.BetStart.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real2");
                var channel2 = await CreateLiveOddsChannelName(args.BetStart.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real3");
                var channel3 = await CreateLiveOddsChannelName(args.BetStart.EventHeader.Id, "global", merch);
                var socket = new SocketClient();
                await socket.SendToHybridgeSocketMessages(args.BetStart.Status.ToString(), channel, "BETSTART");
                await socket.SendToHybridgeSocketMessages(args.BetStart.Status.ToString(), channel2, "BETSTART");
                await socket.SendToHybridgeSocketMessages(args.BetStart.Status.ToString(), channel3, "BETSTART");
                
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }

    }
}
