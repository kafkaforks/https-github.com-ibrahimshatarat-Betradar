using System;

namespace SharedLibrary
{
    public interface IMatchLocal
    {
        long match_id { get; set; }
        string match_name { get; set; }
        long home_team_id { get; set; }
        long away_team_id { get; set; }
        string home_team_name { get; set; }
        string away_team_name { get; set; }
        long sport_id { get; set; }
        string sport_name { get; set; }
        long tournament_id { get; set; }
        string tournament_name { get; set; }
        bool is_active { get; set; }
        string country_iso { get; set; }
        DateTime match_start_date { get; set; }
    }

    public class MatchLocal : IMatchLocal
    {
        public long match_id { get; set; }
        public string match_name { get; set; }
        public long home_team_id { get; set; }
        public long away_team_id { get; set; }
        public string home_team_name { get; set; }
        public string away_team_name { get; set; }
        public long sport_id { get; set; }
        public string sport_name { get; set; }
        public long tournament_id { get; set; }
        public string tournament_name { get; set; }
        public bool is_active { get; set; }
        public string country_iso { get; set; }
        public DateTime match_start_date { get; set; }
    }
}
