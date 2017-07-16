using System;
using System.Runtime.Serialization;

namespace SharedLibrary
{
    public interface IBetClearQueueElement
    {
        long MatchId { get; set; }
        long OddsId { get; set; }
        int OutcomeId { get; set; }
        string PlayerId { get; set; }
        string SpecialBetValue { get; set; }
        string TeamId { get; set; }
    }
    [Serializable]
    [DataContract]
    public class BetClearQueueElement : IBetClearQueueElement
    {
        [DataMember]
        public long MatchId { get; set; }
        [DataMember]
        public long OddsId { get; set; }
        [DataMember]
        public int OutcomeId { get; set; }
        [DataMember]
        public string SpecialBetValue { get; set; }
        [DataMember]
        public string PlayerId { get; set; }
        [DataMember]
        public string TeamId { get; set; }
    }
}
