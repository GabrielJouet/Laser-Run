using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pool COntroller used to handle multiple pools.
/// </summary>
public class PoolController : MonoBehaviour
{
    /// <summary>
    /// All pools available.
    /// </summary>
    private readonly Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();



    /// <summary>
    /// Give object from pools.
    /// </summary>
    /// <param name="wanted">Wanted item</param>
    /// 
    /// <returns>The found item (or instantiated)</returns>
    public GameObject Out(GameObject wanted)
    {
        if (_pools.ContainsKey(wanted.name))
            return _pools[wanted.name].Out();

        Pool newPool = new GameObject(wanted.name + " Pool").AddComponent<Pool>();
        newPool.transform.parent = transform;
        newPool.Class = wanted;
        _pools.Add(wanted.name, newPool);

        return newPool.Out();
    }


    /// <summary>
    /// Retrieve an object to pools.
    /// </summary>
    /// <param name="given">The object given back</param>
    public void In(GameObject given)
    {
        _pools[given.name].In(given, false);
    }


    /// <summary>
    /// Method called to retrieve every object inside pools at the end of the level.
    /// </summary>
    public void RetrieveAllPools()
    {
        foreach (Pool buffer in _pools.Values)
            buffer.RetrieveObjects();
    }
}