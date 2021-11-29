using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private List<LaserBlock> _blocks;

    [Header("Level difficulty")]

    [SerializeField]
    [Range(0.5f, 10f)]
    private float _timeBetweenEachActivation;

    [SerializeField]
    private float _timeBetweenShots;

    [SerializeField]
    private float _reactionTime;

    [SerializeField]
    private float _dispersion;

    [SerializeField]
    private int _numberOfShots;

    [SerializeField]
    private bool _randomShots;



    private void Start()
    {
        Initialize();
    }


    public void Initialize()
    {
        StartCoroutine(StartBlocks());
    }


    private IEnumerator StartBlocks()
    {
        while (true)
        {
            _blocks[Random.Range(0, _blocks.Count)].WarmUp(_timeBetweenShots, _dispersion, _reactionTime, _numberOfShots, _randomShots);
            yield return new WaitForSeconds(_timeBetweenEachActivation);
        }
    }
}