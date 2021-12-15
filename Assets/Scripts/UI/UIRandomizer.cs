using System.Collections.Generic;
using UnityEngine;

public class UIRandomizer : MonoBehaviour
{
    [SerializeField]
    private List<UIHover> _objects;


    [SerializeField]
    [Range(0, 10)]
    private float _maxAngle;


    private void Start()
    {
        foreach (UIHover newObject in _objects)
        {
            float angle = Random.Range(-_maxAngle, _maxAngle);
            newObject.Initialize(angle, -angle);
        }
    }
}