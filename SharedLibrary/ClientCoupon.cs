namespace SharedLibrary
{


public class ClientCoupon
    {
        public Match[] matches { get; set; }
        public Meta meta { get; set; }
    }

    public class Meta
    {
        public string type { get; set; }
        public int rate_amount { get; set; }
        public bool change_odd_accept { get; set; }
        public Systems systems { get; set; }
        public int total { get; set; }
    }

    public class Systems
    {
        public Own[] owns { get; set; }
        public Upper upper { get; set; }
    }

    public class Upper
    {
        public int type { get; set; }
        public int amount { get; set; }
    }

    public class Own
    {
        public int type { get; set; }
        public int amount { get; set; }
    }

    public class Match
    {
        public int match_id { get; set; }
        public string tournament_name { get; set; }
        public int tournament_id { get; set; }
        public string sport_name { get; set; }
        public int sport_id { get; set; }
        public int match_start_date { get; set; }
        public string away_team_name { get; set; }
        public int away_team_id { get; set; }
        public string home_team_name { get; set; }
        public int home_team_id { get; set; }
        public Selected_Odd selected_odd { get; set; }
    }

    public class Selected_Odd
    {
        public int odd_outcome_id { get; set; }
        public bool odd_visible { get; set; }
        public string mid_otid_ocid_sid { get; set; }
        public string odd_probability { get; set; }
        public string odd_outcome { get; set; }
        public object odd_type_name { get; set; }
        public int odd_type_id { get; set; }
        public object odd_id { get; set; }
        public object odd_special { get; set; }
        public object last_update { get; set; }
        public string odd_odd { get; set; }
    }

}
