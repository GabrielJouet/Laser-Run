using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that will handle every level information.
/// </summary>
public class Level : MonoBehaviour
{
    [Header("Level Data")]

    /// <summary>
    /// Name of this level.
    /// </summary>
    [SerializeField]
    private string _name;

    /// <summary>
    /// Name of this level, accessor.
    /// </summary>
    public string Name { get => _name; }


    /// <summary>
    /// Does this level is required to continue?
    /// </summary>
    [SerializeField]
    private bool _required;

    /// <summary>
    /// Does this level is required to continue? Accessor.
    /// </summary>
    public bool Required { get => _required; }


    [Header("Level complexity")]
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
    /// Last level, only accessible on hard mode.
    /// </summary>
    [SerializeField]
    private LevelDifficulty _hardDifficultyBonus;

    /// <summary>
    /// Level goal time.
    /// </summary>
    [SerializeField]
    [Range(25f, 500f)]
    private float _timeToLive;
    public float NeededTime { get => _timeToLive; }


    [Header("Misc")]

    /// <summary>
    /// Detritus prefab.
    /// </summary>
    [SerializeField]
    private GameObject _thingPrefab;

    /// <summary>
    /// All detritus sprites available.
    /// </summary>
    [SerializeField]
    private List<Sprite> _thingSprites;


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
    public void Initialize()
    {
        foreach (LaserBlock block in _blocks)
            block.ResetObject();

        _uiController = Controller.Instance.UIController;

        if (Controller.Instance.SaveController.Hard)
            _difficulties.Add(_hardDifficultyBonus);

        LoadDifficulty(_difficulties[0]);

        _index = 0;
        TimeElapsed = 0;

        for (int i = 0; i < Random.Range(3, 15) * (Controller.Instance.SaveController.Hard ? 3 : 1); i++)
        {
            GameObject thingBuffer = Controller.Instance.PoolController.Out(_thingPrefab);
            thingBuffer.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            thingBuffer.transform.localPosition = new Vector2(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f));
            thingBuffer.GetComponent<SpriteRenderer>().sprite = _thingSprites[Random.Range(0, _thingSprites.Count)];
        }

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
            for(int i = 0; i < _loadedDifficulty.ActivationCount; i ++)
                FindOneBlock()?.WarmUp(_loadedDifficulty);

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


    /// <summary>
    /// Method used to load a new difficulty into the level.
    /// </summary>
    /// <param name="level">The new difficulty</param>
    private void LoadDifficulty(LevelDifficulty level)
    {
        _loadedDifficulty = level;
        _uiController.UpdateThreatLevel(_loadedDifficulty.ThreatDescription, _loadedDifficulty.Warning);
    }


    /// <summary>
    /// Method called when the level stops.
    /// </summary>
    public void StopLevel()
    {
        StopAllCoroutines();
    }
}