using System;
using System.Collections.Generic;
using UnityEngine;

public class ObservableHashSet<T>
{
    private HashSet<T> hashSet = new HashSet<T>();

    // Eventos para añadir y eliminar, con referencia al conjunto
    public event Action<T, ObservableHashSet<T>> OnElementAdded;
    public event Action<T, ObservableHashSet<T>> OnElementRemoved;

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
            OnElementAdded?.Invoke(item, this); // Incluir referencia al conjunto
        }
    }

    public bool Add(T item)
    {
        bool added = hashSet.Add(item);
        if (added)
        {
            OnElementAdded?.Invoke(item, this); // Incluir referencia al conjunto
        }
        return added;
    }

    public bool Remove(T item)
    {
        bool removed = hashSet.Remove(item);
        if (removed)
        {
            OnElementRemoved?.Invoke(item, this); // Incluir referencia al conjunto
        }
        return removed;
    }

    public void Clear()
    {
        foreach (var item in hashSet)
        {
            OnElementRemoved?.Invoke(item, this); // Incluir referencia al conjunto
        }
        hashSet.Clear();
    }


    public int Count => hashSet.Count;
    public bool Contains(T item) => hashSet.Contains(item);
    public List<T> ToList() => new List<T>(hashSet);
    public HashSet<T>.Enumerator GetEnumerator() => hashSet.GetEnumerator();
}
