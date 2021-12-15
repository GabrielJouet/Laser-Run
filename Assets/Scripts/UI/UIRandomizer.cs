using System.Collections.Generic;
using UnityEngine;

public class UIRandomizer : MonoBehaviour
{
    [SerializeField]
    private List<RectTransform> _objects;


    [SerializeField]
    [Range(0, 10)]
    private float _maxAngle;


    private void Start()
    {
        foreach (RectTransform newObject in _objects)
            newObject.localRotation = Quaternion.Euler(0, 0, Random.Range(-_maxAngle, _maxAngle));
    }
}