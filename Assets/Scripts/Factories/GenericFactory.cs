using UnityEngine;

public class GenericFactory<T> : MonoBehaviour where T : MonoBehaviour
{
    public T GetNewInstanceToParent(T prefab, Transform pointToSpawn)
    {
        return Instantiate(prefab, pointToSpawn);
    }

    public T GetNewInstanceByPosition(T prefab, Vector3 spawnPosition)
    {
        return Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}
