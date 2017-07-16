using System;

namespace SharedLibrary
{
    public interface IErrorReturn
    {
        bool success { get; set; }
        string message { get; set; }
    }

    [Serializable]
   public class ErrorReturn : IErrorReturn
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}
