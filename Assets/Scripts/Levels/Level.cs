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
    /// Which category is this level?
    /// </summary>
    [SerializeField]
    private LevelCategory _category;

    /// <summary>
    /// Which category is this level? Accesor.
    /// </summary>
    public LevelCategory Category { get => _category; }


    [Header("Level complexity")]
    /// <summary>
    /// Available laser blocks in this level.
    /// </summary>
    [SerializeField]
    private List<Emitter> _blocks;

    /// <summary>
    /// Additionnal laser blocks in this level.
    /// </summary>
    [SerializeField]
    private List<Emitter> _additionnalBlocks;

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
    /// All blocks in this level.
    /// </summary>
    private List<Emitter> _allBlocks = new List<Emitter>();



    /// <summary>
    /// Method called to initialize the object.
    /// </summary>
    public void Initialize()
    {
        _allBlocks.AddRange(_blocks);
        _allBlocks.AddRange(_additionnalBlocks);

        bool hard = Controller.Instance.SaveController.Hard;

        foreach (Emitter block in _allBlocks)
            block.Initialize(hard);

        if (hard)
            _difficulties.Add(_hardDifficultyBonus);

        _uiController = Controller.Instance.UIController;

        _index = 0;
        TimeElapsed = 0;

        LoadDifficulty();

        for (int i = 0; i < Random.Range(3, 15) * (hard ? 3 : 1); i++)
        {
            GameObject thingBuffer = Controller.Instance.PoolController.Out(_thingPrefab);
            thingBuffer.transform.Rotate(Vector3.forward * Random.Range(0, 360));
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
                FindOneBlock()?.WarmUp();

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
                LoadDifficulty();
        }

        Controller.Instance.LevelController.FinishLevel(true);
    }


    /// <summary>
    /// Method used to load a new difficulty into the level.
    /// </summary>
    private void LoadDifficulty()
    {
        _loadedDifficulty = _difficulties[_index];
        _uiController.UpdateThreatLevel(_loadedDifficulty.ThreatDescription, _loadedDifficulty.Warning);

        foreach (Emitter emitter in _allBlocks)
            emitter.UpdateDifficulty(_index);
    }


    /// <summary>
    /// Method called when the level stops.
    /// </summary>
    public void StopLevel()
    {
        StopAllCoroutines();
    }
}