using System;
using System.Runtime.Serialization;

namespace SharedLibrary
{
    public interface ISocketOdd
    {
        string odd_special_odds_value { get; set; }
        bool odd_eventoddsfield_active { get; set; }
        string odd_odd { get; set; }
        string odd_name { get; set; }
        string odd_probability { get; set; }
        DateTime last_update { get; set; }
    }

    [Serializable]
    [DataContract]
   public class SocketOdd : ISocketOdd
    {
        [DataMember]
        public string odd_special_odds_value { get; set; }
        [DataMember]
        public bool odd_eventoddsfield_active { get; set; }
        [DataMember]
        public string odd_odd { get; set; }
        [DataMember]
        public string odd_name { get; set; }
        [DataMember]
        public string odd_probability { get; set; }
        [DataMember]
        public DateTime last_update { get; set; }


    }
}
