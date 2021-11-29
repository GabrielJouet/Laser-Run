using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private List<LaserBlock> _blocks;

    [Header("Level difficulty")]
    [SerializeField]
    private List<LevelDifficulty> _difficulties;

    [SerializeField]
    [Range(50f, 500f)]
    private float _timeToLive;

    private bool _finished = false;
    private LevelDifficulty _loadedDifficulty;
    private int _index = 0;

    private float _timeElapsed = 0;
    private UIController _uiController;



    private void Start()
    {
        Initialize();
    }


    public void Initialize()
    {
        _uiController = Controller.Instance.UIController;
        _loadedDifficulty = new LevelDifficulty(_difficulties[0]);
        _uiController.SetTimeMax(_timeToLive);

        StartCoroutine(StartBlocks());
        StartCoroutine(FinishLevel());
    }


    private void Update()
    {
        _timeElapsed += Time.deltaTime;
        _uiController.UpdateTimeLeft(_timeElapsed);
    }


    private IEnumerator StartBlocks()
    {
        while (!_finished)
        {
            _blocks[Random.Range(0, _blocks.Count)].WarmUp(_loadedDifficulty);
            yield return new WaitForSeconds(_loadedDifficulty.ActivationTime);
        }
    }

    private IEnumerator FinishLevel()
    {
        float timeLoaded = _timeToLive / _difficulties.Count;
        for (int i = 0; i < _difficulties.Count - 1; i ++)
        {
            yield return new WaitForSeconds(timeLoaded);
            _index++;
            _loadedDifficulty = new LevelDifficulty(_difficulties[_index]);
        }

        _finished = true;
        enabled = false;
    }
}