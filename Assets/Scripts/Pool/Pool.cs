using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to store items and give it back.
/// </summary>
public class Pool : MonoBehaviour
{
    /// <summary>
    /// Prefab of reference.
    /// </summary>
    public GameObject Class { get; set; }

    /// <summary>
    /// The current stack of item.
    /// </summary>
    private readonly Stack<GameObject> _itemPool = new Stack<GameObject>();



    /// <summary>
    /// Method used to get an item out of the stack.
    /// </summary>
    /// <returns>The item found or instantiated</returns>
    public GameObject Out()
    {
        if (_itemPool.Count > 0)
        {
            GameObject buffer = _itemPool.Pop();
            buffer.SetActive(true);
            return buffer;
        }
        else
            return Instantiate(Class, transform);
    }


    /// <summary>
    /// Method used to get an item in the stack.
    /// </summary>
    /// <param name="newObject">The item wanted</param>
    public void In(GameObject newObject)
    {
        newObject.SetActive(false);
        newObject.transform.parent = transform;
        _itemPool.Push(newObject);
    }
}