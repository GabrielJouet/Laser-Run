using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pool COntroller used to handle multiple pools.
/// </summary>
public class PoolController : MonoBehaviour
{
    /// <summary>
    /// Pool prefab used in storage.
    /// </summary>
    [SerializeField]
    private GameObject _poolPrefab;

    /// <summary>
    /// Pools used, each type of item has its own pool.
    /// </summary>
    private readonly List<Pool> _pools = new List<Pool>();



    /// <summary>
    /// Give object from pools.
    /// </summary>
    /// <param name="wanted">Wanted item</param>
    /// 
    /// <returns>The found item (or instantiated)</returns>
    public GameObject GiveObject(GameObject wanted)
    {
        GameObject newObject = null;
        foreach (Pool buffer in _pools)
            if (buffer.Class == wanted)
                newObject = buffer.Out();

        if (newObject == null)
        {
            Pool buffer = Instantiate(_poolPrefab, transform).GetComponent<Pool>();
            buffer.Class = wanted;
            buffer.name = wanted.name + "Pool";
            _pools.Add(buffer);

            newObject = buffer.Out();
        }

        return newObject;
    }


    /// <summary>
    /// Retrieve an object to pools.
    /// </summary>
    /// <param name="given">The object given back</param>
    public void RetrieveObject(GameObject given)
    {
        foreach (Pool buffer in _pools)
            if (buffer.Class.name == given.name.Substring(0, given.name.Length - 7))
                buffer.In(given);
    }


    public void RetrieveAllPools()
    {
        foreach(Pool buffer in _pools)
            for (int i = 0; i < buffer.transform.childCount; i ++)
                RetrieveObject(buffer.transform.GetChild(i).gameObject);
    }
}