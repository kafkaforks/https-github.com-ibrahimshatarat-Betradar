using System;
using SharedLibrary;

namespace Betradar.Classes.DbInsert
{
    internal interface ILiveData
    {
        long fk_feed_type_id { get; set; }
        long fk_additional_data_id { get; set; }
        long fk_cards_id { get; set; }
        DateTime end_time { get; set; }
        long fk_event_header_id { get; set; }
        long event_id { get; set; }
        bool isoutof_band { get; set; }
        long fk_messages_id { get; set; }
        long msgnr { get; set; }
        long fx_odds_id { get; set; }
        string priority { get; set; }
        long reply_nr { get; set; }
        string odds_reply_type { get; set; }
        int server_type { get; set; }
        string server_version { get; set; }
        DateTime start_time { get; set; }
        string odds_status { get; set; }
        long time { get; set; }
        DateTime timestamp { get; set; }
        long virtual_game_id { get; set; }
        Globals.Rollback SetRollback(long RowId, Globals.Tables Table, Globals.TransactionTypes Transaction);
    }

    [Serializable]
    public class LiveData:Core, ILiveData
    {
        public long fk_feed_type_id { get; set; }
        public long fk_additional_data_id { get; set; }
        public long fk_cards_id { get; set; }
        public DateTime end_time { get; set; }
        public long fk_event_header_id { get; set; }
        public long event_id { get; set; }
        public bool isoutof_band { get; set; }
        public long fk_messages_id { get; set; }
        public long msgnr { get; set; }
        public long fx_odds_id { get; set; }
        public string priority { get; set; }
        public long reply_nr { get; set; }
        public string odds_reply_type { get; set; }
        public int server_type { get; set; }
        public string server_version { get; set; }
        public DateTime start_time { get; set; }
        public string odds_status { get; set; }
        public long time { get; set; }
        public DateTime timestamp { get; set; }
        public long virtual_game_id { get; set; }
    }
}
