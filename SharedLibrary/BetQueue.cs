using System;
using System.Collections.Generic;

namespace SharedLibrary
{
    public class BetQueue<T>:Core
    {
        private readonly Queue<T> queue = new Queue<T>();
        public event EventHandler Changed;
        public virtual void OnChanged()
        {
            if (Changed != null)
            {
                Changed(this, EventArgs.Empty);
            }
        }
        public virtual void Enqueue(T item)
        {
            queue.Enqueue(item);
            OnChanged();
        }
        public int Count { get { return queue.Count; } }

        public virtual T Dequeue()
        {
            T item = queue.Dequeue();
            OnChanged();
            return item;
        }
    }
}
