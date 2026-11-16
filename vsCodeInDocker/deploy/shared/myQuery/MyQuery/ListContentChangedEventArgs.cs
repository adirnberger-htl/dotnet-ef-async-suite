using System;

namespace MyQuery
{
    public class ListContentChangedEventArgs<T> : EventArgs
    {
        public bool Added { get; }
        public T Element { get; }

        public ListContentChangedEventArgs(bool added, T element)
        {
            Added = added;
            Element = element;
        }
    }
}