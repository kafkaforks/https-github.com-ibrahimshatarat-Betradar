using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SharedLibrary
{
    [Serializable]
    [DataContract]
    public class Coupon
    {

        [DataMember(Name = "BetOdds")]
        public IList<string> BetOdds { get; set; }

        [DataMember(Name = "State")]
        public int State { get; set; }

        [DataMember(Name = "BetId")]
        public int BetId { get; set; }

        [DataMember(Name = "Stake")]
        public int Stake { get; set; }

        [DataMember(Name = "VoidFactor")]
        public IList<string> VoidFactor { get; set; }
    }

    [DataContract]
    public class Example
    {
        [DataMember(Name = "Coupons")]
        public IList<Coupon> Coupons { get; set; }
    }

}
