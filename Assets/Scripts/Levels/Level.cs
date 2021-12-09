using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that will handle every level information.
/// </summary>
public class Level : MonoBehaviour
{
    /// <summary>
    /// Available laser blocks in this level.
    /// </summary>
    [SerializeField]
    private List<LaserBlock> _blocks;

    /// <summary>
    /// All difficulties in the level.
    /// </summary>
    [SerializeField]
    private List<LevelDifficulty> _difficulties;

    /// <summary>
    /// Level goal time.
    /// </summary>
    [SerializeField]
    [Range(50f, 500f)]
    private float _timeToLive;
    public float NeededTime { get => _timeToLive; }


    /// <summary>
    /// Current difficulty loaded.
    /// </summary>
    private LevelDifficulty _loadedDifficulty;

    /// <summary>
    /// Difficulty index.
    /// </summary>
    private int _index = 0;

    /// <summary>
    /// How much time did elapsed from the start of the level?
    /// </summary>
    private float _timeElapsed = 0;

    /// <summary>
    /// UI Controller shortcut.
    /// </summary>
    private UIController _uiController;



    /// <summary>
    /// Method called to initialize the object.
    /// </summary>
    public void Initialize()
    {
        enabled = true;

        _uiController = Controller.Instance.UIController;
        _loadedDifficulty = _difficulties[0];
        _index = 0;
        _timeElapsed = 0;

        StartCoroutine(StartBlocks());
        StartCoroutine(FinishLevel());
    }


    /// <summary>
    /// Update method, called each frame.
    /// </summary>
    private void Update()
    {
        _timeElapsed += Time.deltaTime;
        _uiController.UpdateTimeLeft(_timeToLive - _timeElapsed);
    }


    /// <summary>
    /// Coroutine used to start and warm up laser blocks.
    /// </summary>
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


    /// <summary>
    /// Method ysed to find an available block.
    /// </summary>
    /// <returns>A non-used block</returns>
    private LaserBlock FindOneBlock()
    {
        _blocks.Shuffle();

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


    /// <summary>
    /// Coroutine that will handle the end of the level.
    /// </summary>
    private IEnumerator FinishLevel()
    {
        float timeLoaded = _timeToLive / _difficulties.Count;
        for (int i = 0; i < _difficulties.Count; i ++)
        {
            yield return new WaitForSeconds(timeLoaded);
            _index++;

            if (_index < _difficulties.Count)
                _loadedDifficulty = _difficulties[_index];
        }

        Controller.Instance.LevelController.FinishLevel(true);
    }


    /// <summary>
    /// Method called when the level stops.
    /// </summary>
    public void StopLevel()
    {
        StopAllCoroutines();
        StartCoroutine(DelayLevelStop(_uiController.ScreenDelayTime));
    }


    /// <summary>
    /// Coroutine used to delay the level stops.
    /// </summary>
    /// <param name="delayTime">How much time before desactivation?</param>
    private IEnumerator DelayLevelStop(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        foreach (LaserBlock block in _blocks)
            block.ResetObject();

        enabled = false;
    }
}