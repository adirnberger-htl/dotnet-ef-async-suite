using System;
using System.Collections.Generic;

namespace MyQuery
{
    public class MyList<T>
    {
        private readonly List<T> _internalList = new List<T>();

        public event EventHandler<ListContentChangedEventArgs<T>> ContentChanged;

        public void Add(T item)
        {
            _internalList.Add(item);
            OnContentChanged(new ListContentChangedEventArgs<T>(true, item));
        }

        public void Remove(T item)
        {
            // Info: _internalList.Remove(item) gibt 'true' zurück, wenn es das Element gefunden und gelöscht hat.
            // Es gibt 'false' zurück, wenn das Element gar nicht in der Liste war.
            
            // TODO: Versuche das Element aus _internalList zu entfernen.
            var success = _internalList.Remove(item);
            
            // TODO: Wenn es erfolgreich entfernt wurde, rufe OnContentChanged auf.
            // Achtung: Welchen bool-Wert (Added) musst du diesmal übergeben?
        }
        
        public void RemoveAt(int index)
        {
            // TODO: Bevor du das Element löschst, musst du es dir in einer Variable merken!
            // Du brauchst es nämlich gleich für das Event (T elementToRemove = _internalList[index];)
            
            // TODO: Entferne das Element an dem Index aus der _internalList
            
            // TODO: Löse das Event mit dem gemerkten Element aus (wieder mit false für Added).
        }

        protected virtual void OnContentChanged(ListContentChangedEventArgs<T> e)
        {
            // Das Fragezeichen prüft, ob überhaupt jemand das Event abonniert hat.
            // Wenn ja (nicht null), wird es aufgerufen (Invoke).
            ContentChanged?.Invoke(this, e);
        }
    }
}