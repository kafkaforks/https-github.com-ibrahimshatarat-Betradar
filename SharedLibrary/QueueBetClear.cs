using System;
using System.Collections;
using System.Collections.Generic;

namespace SharedLibrary
{
   public class QueueBetClear<T> : Core, IEnumerable
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
            lock (queue)
            {
                queue.Enqueue(item);
                OnChanged();
            }
           
        }
        public int Count { get { return queue.Count; } }

        public virtual T Dequeue()
        {
            lock (queue)
            {
                T item = queue.Dequeue();
                OnChanged();
                return item;
            }
           
        }

       public IEnumerator GetEnumerator()
       {
           throw new NotImplementedException();
       }
   }
}
