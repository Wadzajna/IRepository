using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoschAspApps.Utils.IDAL
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Co chceme ukladat do bufferu</typeparam>
    /// <typeparam name="Key">Jaky typ PK to je? (vetsinou int)</typeparam>
    public abstract class BufferRepozitar<T, Key>
    {

        private Dictionary<Key, T> hashTable;


        public BufferRepozitar()
        {
            hashTable = new Dictionary<Key,T>();
        }


        public void ClearBuffer()
        {
            hashTable.Clear();
            return;
        }


        public T GetElement(Key elementKey)
        {
           
            T element;
            bool OK = hashTable.TryGetValue(elementKey, out element);
            if (OK) return element;

            element = GetMissingElement(elementKey);
            if (element == null) return element;

            hashTable.Add(elementKey, element);
            return element;
        }


        public abstract T GetMissingElement(Key elementKey);


    }
}
