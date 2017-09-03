using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetService.DbInsert.Betradar
{
   public class BetResultEntity_test
    {
        public long OddsType { get; set; }
        public string Outcome { get; set; }
        public string OutcomeId { get; set; }
        public string PlayerId { get; set; }
        public string Reason { get; set; }
        public string SpecialBetValue { get; set; }
        public Nullable<bool> Status { get; set; }
        public string TeamId { get; set; }
        public string VoidFactor { get; set; }

        public BetResultEntity_test()
        {
            OddsType = 10;
            Outcome = "1";
            OutcomeId = "1";
            PlayerId = null;
            Reason = null;
            SpecialBetValue = null;
            Status = false;
            TeamId = null;
            VoidFactor = null;
        }
    }
}
