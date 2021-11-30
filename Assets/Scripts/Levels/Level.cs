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

    private LevelDifficulty _loadedDifficulty;
    private int _index = 0;

    private float _timeElapsed = 0;
    private UIController _uiController;


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
        while (true)
        {
            LaserBlock block = FindOneBlock();

            if (block != null)
                block.WarmUp(_loadedDifficulty);

            yield return new WaitForSeconds(_loadedDifficulty.ActivationTime);
        }
    }


    private LaserBlock FindOneBlock()
    {
        LaserBlock found = null;
        foreach(LaserBlock block in _blocks)
        {
            if (!block.Used)
            {
                found = block;
                break;
            }
        }

        return found;
    }


    private IEnumerator FinishLevel()
    {
        float timeLoaded = _timeToLive / _difficulties.Count;
        for (int i = 0; i < _difficulties.Count; i ++)
        {
            yield return new WaitForSeconds(timeLoaded);
            _index++;

            if (_index < _difficulties.Count)
                _loadedDifficulty = new LevelDifficulty(_difficulties[_index]);
        }

        Controller.Instance.LevelController.FinishLevel();
    }
}