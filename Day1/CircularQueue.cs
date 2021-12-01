using System;
using System.Collections.Generic;

namespace AdventOfCode2021
{
    public class CircularQueue<T>
    {
        private int _capacity { get; }
        private Queue<T> _queue { get; }
        
        public int Count => this._queue.Count;

        public CircularQueue(int capacity)
        {
            this._capacity = capacity;
            this._queue = new Queue<T>(capacity+1);
        }

        public void Enqueue(T value, Action<Queue<T>>? preDequeueAction)
        {
            if (this.Count == this._capacity)
            {
                preDequeueAction?.Invoke(_queue); 
                this.Dequeue();
            }

            this._queue.Enqueue(value);
        }

        public T Dequeue()
        {
            return this._queue.Dequeue();
        }
    }
}