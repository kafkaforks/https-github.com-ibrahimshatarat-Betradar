using System;
using System.Collections.Generic;

namespace SharedLibrary
{
    [Serializable]
    public class SeamlessResponse
    {
        public IList<int> error_codes { get; set; }
    }
}
