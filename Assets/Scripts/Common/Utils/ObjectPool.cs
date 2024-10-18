using System.Collections.Generic;

namespace TTT
{
    public class ObjectPool<T> where T : class, new()
    {
        private HashSet<T> itemHash = new HashSet<T>();

        public T GetObject()
        {
            var result = _GetFirstItem();
            if (result != null)
            {
                RemoveObject(result);
            }
            else
            {
                result = new T();
            }
            return result;
        }

        public void ReleaseObject(T item)
        {
            if (item == null)
            {
                return;
            }

            if (itemHash.Contains(item))
            {
                return;
            }
            itemHash.Add(item);
        }

        private T _GetFirstItem()
        {
            T result = null;
            foreach (var item in itemHash)
            {
                result = item;
                break;
            }
            return result;
        }

        private void RemoveObject(T item)
        {
            if (item == null)
            {
                return;
            }

            if (!itemHash.Contains(item))
            {
                return;
            }
            itemHash.Remove(item);
        }
    }
}