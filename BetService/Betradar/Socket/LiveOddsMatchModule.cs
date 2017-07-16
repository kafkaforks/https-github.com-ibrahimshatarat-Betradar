using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BetService.Classes.DbInsert;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace Betradar.Classes.Socket
{
    public class LiveOddsMatchModule : LiveOddsCommonModule
    {
        private readonly ILiveOdds m_live_odds;

        public LiveOddsMatchModule(ILiveOdds live_odds, string feed_name, TimeSpan meta_interval)
            : base(live_odds, feed_name, meta_interval)
        {
            m_live_odds = live_odds;
            m_live_odds.OnScoreCardSummary += ScoreCardSummaryHandler;
            m_live_odds.OnMetaInfo += MetaInfoHandler;
        }

        protected override void ConnectionStableHandler(object sender, EventArgs e)
        {
            base.ConnectionStableHandler(sender, e);
            if (m_live_odds.TestManager != null)
            {
                m_live_odds.TestManager.StartAuto();
                try
                {
                    if (config.AppSettings.Get("AutoReplay") != null)
                    {
                        if (bool.Parse(config.AppSettings.Get("AutoReplay")))
                        {
                            m_live_odds.TestManager.StartAuto();
                        }
                    }

                }
                catch (Exception ex)
                {
                    SharedLibrary.Logg.logger.Fatal(ex.Message);
                }
            }
        }

        private void MetaInfoHandler(object sender, MetaInfoEventArgs e)
        {

            var meta_data = e.MetaInfo.MetaInfoDataContainer as LiveOddsMetaData;
            if (meta_data != null)
            {
                var common = new Common();

                foreach (var match_info in meta_data.MatchHeaderInfos)
                {
                    //common.insertMatchDataAllDetails(match_info.MatchHeader, match_info.MatchInfo);
                    Tournament_Upsert Tup = new Tournament_Upsert();
                    Tup.tournament_id = match_info.MatchInfo.Tournament.Id;
                    if (match_info.MatchInfo.Tournament.Name != null)
                    {
                        var dic = new Dictionary<string, string>();
                        dic.Add("BET", match_info.MatchInfo.Tournament.Name.International);
                        dic.Add("en", match_info.MatchInfo.Tournament.Name.International);
                        foreach (var language in match_info.MatchInfo.Tournament.Name.AvailableTranslationLanguages)
                        {
                            dic.Add(language, match_info.MatchInfo.Tournament.Name.GetTranslation(language));
                        }
                        Tup.tournament = new JavaScriptSerializer().Serialize(dic);
                        if (match_info.MatchInfo.Tournament.UniqueId != null)
                        {
                            Tup.unique_tournament_id = match_info.MatchInfo.Tournament.UniqueId;
                            Tup.unique_tournament_name = new JavaScriptSerializer().Serialize(dic);
                        }
                    }
                    if (match_info.MatchInfo.Category != null)
                    {
                        Tup.category_id = match_info.MatchInfo.Category.Id;
                        var dic = new Dictionary<string, string>();
                        dic.Add("BET", match_info.MatchInfo.Category.Name.International);
                        dic.Add("en", match_info.MatchInfo.Category.Name.International);
                        foreach (var language in match_info.MatchInfo.Category.Name.AvailableTranslationLanguages)
                        {
                            dic.Add(language, match_info.MatchInfo.Category.Name.GetTranslation(language));
                        }
                        Tup.category = new JavaScriptSerializer().Serialize(dic);
                    }
                    if (match_info.MatchInfo.Sport != null)
                    {
                        Tup.sport_id = match_info.MatchInfo.Sport.Id;
                        var dic = new Dictionary<string, string>();
                        dic.Add("BET", match_info.MatchInfo.Sport.Name.International);
                        dic.Add("en", match_info.MatchInfo.Sport.Name.International);
                        foreach (var language in match_info.MatchInfo.Sport.Name.AvailableTranslationLanguages)
                        {
                            dic.Add(language, match_info.MatchInfo.Sport.Name.GetTranslation(language));
                        }
                        Tup.sport = new JavaScriptSerializer().Serialize(dic);
                    }
                    Tup.insertCpTournament();
                    Task.Factory.StartNew(
                        () =>
                        {
                            common.insertMatchDataAllDetails(match_info.MatchHeader, match_info.MatchInfo);
                        }
                        , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);



                    // Task.Factory.StartNew(() => common.insertMatchDataAllDetails(match_info.MatchHeader,match_info.MatchInfo));
                }

                Logg.logger.Info("{0}: Received MetaInfo with {1} matches", m_feed_name,
                    meta_data.MatchHeaderInfos.Count);
            }
        }

        private void ScoreCardSummaryHandler(object sender, ScoreCardSummaryEventArgs e)
        {
            Task.Factory.StartNew(
                () =>
                {
                    new ScoreCardSummaryHandle(e);
                }
                , CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);


            // Task.Factory.StartNew(() => new ScoreCardSummaryHandle(e));
#if DEBUG
            InCount += 1;
            Logg.logger.Warn("InCount Count = " + InCount);
#endif
            Logg.logger.Info("{0}: Received ScoreCardSummary for event {1}", m_feed_name,
                e.ScoreCardSummary.EventHeader.Id);
        }
    }
}