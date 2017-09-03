using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using ServiceStack;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace Betradar.Classes.DbInsert
{
    public class BetStartHandle : Core
    {
        public BetStartHandle(BetStartEventArgs args)
        {
            try
            {
                var common = new Common();
                var merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real");
                var channel = CreateLiveOddsChannelName(args.BetStart.EventHeader.Id, "global", merch);
                merch = config.AppSettings.Get("ChannelsSecretPrefixLast_real2");
                var channel2 = CreateLiveOddsChannelName(args.BetStart.EventHeader.Id, "global", merch);


                Task.Factory.StartNew(
                              () =>
                              {
                                  SendToHybridgeSocketMessages(args.BetStart.Status.ToString(), channel);
                                  SendToHybridgeSocketMessages(args.BetStart.Status.ToString(), channel2);

                              }
                              , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);




                //Task.Factory.StartNew(() =>
                //{
                //    SendToHybridgeSocketMessages(args.BetStart.Status.ToString(), channel);
                //    SendToHybridgeSocketMessages(args.BetStart.Status.ToString(), channel2);
                //});


                Task.Factory.StartNew(
                    () =>
                    {
                        common.insertMatchDataAllDetails((MatchHeader) args.BetStart.EventHeader, null);
                    }
                    , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);



                //Task.Factory.StartNew(() => common.insertMatchDataAllDetails((MatchHeader)args.BetStart.EventHeader, null));

            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }

    }
}
