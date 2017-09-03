using System;

namespace SharedLibrary
{
    [Serializable]
    public class Merchants
    {
        public long id { get; set; }
        public string username { get; set; }
        public string merchant_name { get; set; }
        public string prefix { get; set; }
        public int vendor_id { get; set; }
        public string domain_m { get; set; }
        public DateTime last_update { get; set; }
        public decimal profit_margin { get; set; }
        public string seamlessurl { get; set; }
        public string skin { get; set; }
    

    }
}
