using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to randomize every UI Hover of the scene.
/// </summary>
public class UIRandomizer : MonoBehaviour
{
    /// <summary>
    /// All objects in the scene.
    /// </summary>
    [SerializeField]
    private List<UIHover> _objects;


    /// <summary>
    /// Maximum angle of the object allowed.
    /// </summary>
    [SerializeField]
    [Range(0, 10)]
    private float _maxAngle;



    /// <summary>
    /// Start method, called after Awake.
    /// </summary>
    private void Start()
    {
        foreach (UIHover newObject in _objects)
            newObject.Initialize(Random.Range(-_maxAngle, _maxAngle));
    }
}