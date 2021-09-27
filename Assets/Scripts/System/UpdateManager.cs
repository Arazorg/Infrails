using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance;

    private HashSet<IUpdateable> _table = new HashSet<IUpdateable>();

    public void Register(IUpdateable obj)
    {
        _table.Add(obj);
    }

    public void Unregister(IUpdateable obj)
    {
        _table.Remove(obj);
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Update()
    {
        var e = _table.GetEnumerator();
        while (e.MoveNext() && e.Current != null)
            e.Current.Tick();
    }
}
