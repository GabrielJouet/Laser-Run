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
    /// The current stack of items.
    /// </summary>
    private readonly Stack<GameObject> _itemPool = new Stack<GameObject>();

    /// <summary>
    /// The current stack of activated items.
    /// </summary>
    private readonly List<GameObject> _activePool = new List<GameObject>();



    /// <summary>
    /// Method used to get an item out of the stack.
    /// </summary>
    /// <returns>The item found or instantiated</returns>
    public GameObject Out()
    {
        GameObject buffer;

        if (_itemPool.Count > 0)
        {
            buffer = _itemPool.Pop();
            buffer.SetActive(true);
        }
        else
        {
            buffer = Instantiate(Class, transform);
            buffer.name = Class.name;
        }

        _activePool.Add(buffer);
        return buffer;
    }


    /// <summary>
    /// Method used to get an item in the stack.
    /// </summary>
    /// <param name="newObject">The item wanted</param>
    /// <param name="reset">Does the object is resetted or not?</param>
    public void In(GameObject newObject, bool reset)
    {
        if (!reset)
            _activePool.Remove(newObject);

        newObject.SetActive(false);
        newObject.transform.parent = transform;
        _itemPool.Push(newObject);
    }


    /// <summary>
    /// Method called to retrieve every active object.
    /// </summary>
    public void RetrieveObjects()
    {
        foreach(GameObject active in _activePool)
            In(active, true);

        _activePool.Clear();
    }
}