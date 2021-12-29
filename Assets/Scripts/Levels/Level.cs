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

    [SerializeField]
    private List<ThreatLights> _lights;

    /// <summary>
    /// All difficulties in the level.
    /// </summary>
    [SerializeField]
    private List<LevelDifficulty> _difficulties;

    /// <summary>
    /// Level goal time.
    /// </summary>
    [SerializeField]
    [Range(25f, 500f)]
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
    public float TimeElapsed { get; private set; } = 0;

    /// <summary>
    /// UI Controller shortcut.
    /// </summary>
    private UIController _uiController;



    /// <summary>
    /// Method called to initialize the object.
    /// </summary>
    public void Initialize(bool hard)
    {
        foreach (LaserBlock block in _blocks)
            block.ResetObject();

        enabled = true;

        _uiController = Controller.Instance.UIController;

        LoadDifficulty(_difficulties[0]);

         _index = 0;
        TimeElapsed = 0;

        StartCoroutine(LoadTraps());
        StartCoroutine(LevelCountDown());
    }


    /// <summary>
    /// Update method, called each frame.
    /// </summary>
    private void Update()
    {
        TimeElapsed += Time.deltaTime;
        _uiController.UpdateTimeLeft(_timeToLive - TimeElapsed >= 0 ? _timeToLive - TimeElapsed : 0);
    }


    /// <summary>
    /// Coroutine used to start and warm up laser blocks.
    /// </summary>
    private IEnumerator LoadTraps()
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
    private IEnumerator LevelCountDown()
    {
        float timeLoaded = _timeToLive / _difficulties.Count;
        for (int i = 0; i < _difficulties.Count; i ++)
        {
            yield return new WaitForSeconds(timeLoaded);
            _index++;

            if (_index < _difficulties.Count)
                LoadDifficulty(_difficulties[_index]);
        }

        Controller.Instance.LevelController.FinishLevel(true);
    }


    private void LoadDifficulty(LevelDifficulty level)
    {
        _loadedDifficulty = level;
        _uiController.UpdateThreatLevel(_loadedDifficulty.ThreatDescription);

        if (_loadedDifficulty.Warning)
            StartCoroutine(LightWarning());
    }


    private IEnumerator LightWarning()
    {
        foreach (ThreatLights light in _lights)
            light.LightUp();

        yield return new WaitForSeconds(5f);

        foreach (ThreatLights light in _lights)
            light.CutOff();
    }


    /// <summary>
    /// Method called when the level stops.
    /// </summary>
    public void StopLevel()
    {
        StopAllCoroutines();

        enabled = false;
    }
}