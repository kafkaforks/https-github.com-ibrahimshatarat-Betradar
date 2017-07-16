namespace SharedLibrary.RPC
{
  public  interface IBetClearingQueue
    {
        void AddQueue(BetClearQueueElement element);
        void AddStringQueue(string element);
        string GetStringQueue();
        BetClearQueueElement GetQueue();
    }
    //public interface IBetClearingQueueLive
    //{
    //    void AddQueueLive(BetClearQueueElementLive element);
    //    ErrorReturn AddStringQueueLive(string element);
    //    string GetStringQueueLive();
    //    BetClearQueueElementLive GetQueueLive();
    //}
}
