using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlock : MonoBehaviour
{
    [SerializeField]
    private float _timeBetweenShots;

    [SerializeField]
    private float _dispersion;

    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    private int _facing;

    [SerializeField]
    private List<Transform> _canonpositions;


    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeBetweenShots);
            Controller.Instance.PoolController.GiveObject(_prefab).GetComponent<Laser>().Initialize(_facing * 90 + Random.Range(-_dispersion, _dispersion), _canonpositions[_facing].position);
        }
    }
}