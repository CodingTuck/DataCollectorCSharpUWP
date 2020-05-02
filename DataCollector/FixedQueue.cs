using System.Collections.Concurrent;


namespace DataCollector
{
    //creates the limit to hold in the history display in the user interface
    //throws out the oldest number displayed that would make the total count greater than 10
    public class FixedQueue<T>
    {
        public ConcurrentQueue<T> q = new ConcurrentQueue<T>();
        private readonly object lockObject = new object();

        public int Limit { get; set; }

        public void Enqueue(T obj)
        {
            q.Enqueue(obj);
            lock (lockObject)
            {
                T overflow;
                while (q.Count > Limit && q.TryDequeue(out overflow)) ;
            }
        }
    }
}
