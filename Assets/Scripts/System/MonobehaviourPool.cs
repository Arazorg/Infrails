using System;
using System.Collections.Generic;
using UnityEngine;

public class MonobehaviourPool<T> where T : MonoBehaviour
{
    private Transform _container;
    private List<T> _pool;
    private GenericFactory<T> _currentFactory;
    private T _prefab;
    private bool _isAutoExpand;

    public List<T> Pool => _pool;

    public MonobehaviourPool(int number, bool isAutoExpand, GenericFactory<T> factory, T prefab)
    {
        _currentFactory = factory;
        _isAutoExpand = isAutoExpand;
        _prefab = prefab;
        CreatePool(number, prefab);
    }

    public MonobehaviourPool(int number, bool isAutoExpand, GenericFactory<T> factory, T prefab, Transform container)
    {
        _currentFactory = factory;
        _isAutoExpand = isAutoExpand;
        _prefab = prefab;
        _container = container;
        CreatePool(number, prefab);
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var monoObject in _pool)
        {
            if (!monoObject.gameObject.activeInHierarchy)
            {
                element = monoObject;
                monoObject.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out var element))
            return element;

        if (_isAutoExpand)
            return CreateObject(_prefab, false);

        throw new System.Exception($"No free elements in pool of type {typeof(T)}");
    }

    private void CreatePool(int number, T prefab)
    {
        _pool = new List<T>();
        for (int i = 0; i < number; i++)
            CreateObject(prefab);
    }

    private T CreateObject(T prefab, bool isActiveByDefault = false)
    {
        T createdObject = _currentFactory.GetNewInstanceByPosition(prefab, Vector3.zero);
        createdObject.gameObject.SetActive(isActiveByDefault);
        _pool.Add(createdObject);
        return createdObject;
    }
}
