using System;
using System.Collections.Generic;

public class ObservableHashSet<T>
{
    private HashSet<T> hashSet = new HashSet<T>();

    // Eventos para añadir y eliminar
    public event Action<T> OnElementAdded;
    public event Action<T> OnElementRemoved;

    // Constructor vacío
    public ObservableHashSet() { }

    // Constructor con colección inicial
    public ObservableHashSet(IEnumerable<T> coleccionInicial)
    {
        New(coleccionInicial);
    }

    public void New(IEnumerable<T> coleccionInicial) 
    {
        foreach (var item in coleccionInicial)
        {
            hashSet.Add(item); 
            OnElementAdded?.Invoke(item); 
        }
    }

    public bool Add(T item)
    {
        bool added = hashSet.Add(item);
        if (added)
        {
            OnElementAdded?.Invoke(item); // Disparar el evento al añadir
        }
        return added;
    }

    public bool Remove(T item)
    {
        bool removed = hashSet.Remove(item);
        if (removed)
        {
            OnElementRemoved?.Invoke(item); // Disparar el evento al eliminar
        }
        return removed;
    }

    public void Clear()
    {
        foreach (var item in hashSet)
        {
            OnElementRemoved?.Invoke(item); // Disparar el evento por cada elemento eliminado
        }
        hashSet.Clear();
    }

    public int Count => hashSet.Count;
    public bool Contains(T item) => hashSet.Contains(item);
    public HashSet<T>.Enumerator GetEnumerator() => hashSet.GetEnumerator();
}