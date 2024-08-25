using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEncryptCompress
{

    public class PriorityQueue<T> where T : IComparable<T>
    {
        public List<T> data;

        // Implementing Priority Queue using an inbuilt available List in C#
        public PriorityQueue()
        {
            this.data = new List<T>();
        }

        // Element Inserting function
        public void Enqueue(T item)
        {
            // Item insertion
            data.Add(item);
            int ci = data.Count - 1;

            // Re-structure heap (Min Heap) so that after addition, the min element will lie on top of pq
            while (ci > 0)
            {
                int pi = (ci - 1) / 2;
                if (data[ci].CompareTo(data[pi]) >= 0) // Change comparison to >= for min heap
                    break;
                T tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
                ci = pi;
            }
        }

        public T Dequeue()
        {
            // Deleting the top element of pq
            int li = data.Count - 1;
            T frontItem = data[0];
            data[0] = data[li];
            data.RemoveAt(li);

            --li;
            int pi = 0;

            // Re-structure heap (Min Heap) so that after deletion, the min element will lie on top of pq
            while (true)
            {
                int ci = pi * 2 + 1;
                if (ci > li) break;
                int rc = ci + 1;
                if (rc <= li && data[rc].CompareTo(data[ci]) < 0)
                    ci = rc;
                if (data[pi].CompareTo(data[ci]) <= 0) // Change comparison to <= for min heap
                    break;
                T tmp = data[pi]; data[pi] = data[ci]; data[ci] = tmp;
                pi = ci;
            }
            return frontItem;
        }

        // Function which returns peek element
        public T Peek()
        {
            T frontItem = data[0];
            return frontItem;
        }

        public int Count()
        {
            return data.Count;
        }
    }
}

