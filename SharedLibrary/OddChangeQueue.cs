using System;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;

namespace SharedLibrary
{
    public interface IOddChangeQueue
    {
        DateTime time { get; set; }
        OddsChangeEventArgs arg { get; set; }
    }

    public class OddChangeQueue : IOddChangeQueue
    {
        public DateTime time { get; set; }
        public OddsChangeEventArgs arg { get; set; }
    }
}
