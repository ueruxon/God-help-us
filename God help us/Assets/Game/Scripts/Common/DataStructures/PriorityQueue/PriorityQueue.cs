using System;
using System.Collections.Generic;
using Game.Scripts.Common.DataStructures.Heap;

namespace Game.Scripts.Common.DataStructures.PriorityQueue
{
    public class PriorityQueue<T> where T : IPriorityItem
    {
        private PriorityHeap<T> _mHeap;
        private object _lockObj;
        
        public int Count => _mHeap.Count;
        
        public PriorityQueue(int size = 2048)
        {
            _mHeap = new PriorityHeap<T>(size);
            _lockObj = new object();
        }
        
        public void Enqueue(T item)
        {
            object obj = _lockObj;
            lock (obj)
            {
                bool enqueued = item.Enqueued;
                if (enqueued)
                {
                    throw new ArgumentException("Element already in a priority queue");
                }

                _mHeap.PushObj(item);
                item.Enqueued = true;
            }
        }
        
        public void EnqueueRange(IEnumerable<T> range)
        {
            object obj = _lockObj;
            lock (obj)
            {
                foreach (T item in range)
                {
                    bool enqueued = item.Enqueued;
                    if (enqueued)
                    {
                        throw new ArgumentException("Element already in a priority queue");
                    }

                    _mHeap.PushObj(item);
                    item.Enqueued = true;
                }
            }
        }
        
        public T Dequeue()
        {
            object obj = _lockObj;
            T result;
            lock (obj)
            {
                bool flag2 = _mHeap.Count == 0;
                if (flag2)
                {
                    result = default(T);
                }
                else
                {
                    T item = _mHeap.PopObj();
                    bool flag3 = item != null;
                    if (flag3)
                    {
                        item.Enqueued = false;
                    }

                    result = item;
                }
            }

            return result;
        }
        
        public T Peek()
        {
            object obj = _lockObj;
            T result;
            lock (obj)
            {
                result = ((_mHeap.Count > 0) ? _mHeap.HeadHeapObject : default(T));
            }

            return result;
        }
        
        public void Clear()
        {
            object obj = _lockObj;
            lock (obj)
            {
                _mHeap.Clear();
            }
        }
        
        public List<T> ToList()
        {
            object obj = _lockObj;
            List<T> result;
            lock (obj)
            {
                List<T> list = new List<T>();
                foreach (T item in _mHeap.Objs)
                {
                    bool flag2 = item != null;
                    if (flag2)
                    {
                        list.Add(item);
                    }
                }

                result = list;
            }

            return result;
        }
    }
}