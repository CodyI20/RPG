using System;
using System.Collections.Generic;
using System.Collections;

[Serializable]
public class ObservableList<T> : IList<T>, IObservableList<T>
{
    private readonly IList<T> list;
    public event Action<IList<T>> AnyValueChanged;

    public ObservableList(IList<T> initialList = null)
    {
        list = initialList ?? new List<T>();
    }

    public T this[int index]
    {
        get => list[index];
        set
        {
            list[index] = value;
            Invoke();
        }
    }

    public void Invoke() => AnyValueChanged?.Invoke(list);

    public int Count => list.Count;

    public bool IsReadOnly => list.IsReadOnly;

    public void Add(T item)
    {
        list.Add(item);
        Invoke();
    }

    public void Clear()
    {
        list.Clear();
        Invoke();
    }

    public bool Contains(T item) => list.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

    public bool Remove(T item)
    {
        var result = list.Remove(item);
        if (result)
        {
            Invoke();
        }

        return result;
    }

    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

    public int IndexOf(T item) => list.IndexOf(item);

    public void Insert(int index, T item)
    {
        list.Insert(index, item);
        Invoke();
    }

    public void RemoveAt(int index)
    {
        T item = list[index];
        list.RemoveAt(index);
        Invoke();
    }
}