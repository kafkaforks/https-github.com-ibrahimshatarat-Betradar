using System;
using System.Runtime.Serialization;

namespace SharedLibrary
{
    public interface IBetClearQueueElementLive
    {
        long MatchId { get; set; }
        long OddId { get; set; }
        int? TypeId { get; set; }
    }

    [Serializable]
    [DataContract]
    public class BetClearQueueElementLive : IBetClearQueueElementLive
    {
        [DataMember]
        public long MatchId { get; set; }
        [DataMember]
        public long OddId { get; set; }
        [DataMember]
        public int? TypeId { get; set; }
       
    }
}
