using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that will handle every level information.
/// </summary>
public class Level : BaseLevel
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
    /// All difficulties in the level.
    /// </summary>
    [SerializeField]
    protected List<LevelDifficulty> _difficulties;

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
    public override void Initialize(int detritusCount)
    {
        bool hard = Controller.Instance.SaveController.Hard;

        base.Initialize(detritusCount * (hard ? 3 : 1));

        if (hard && !_difficulties.Contains(_hardDifficultyBonus))
            _difficulties.Add(_hardDifficultyBonus);

        foreach (Emitter block in _allBlocks)
            block.Initialize(_difficulties);

        _uiController = Controller.Instance.UIController;

        _index = 0;
        TimeElapsed = 0;

        StartCoroutine(LevelCountDown());

        LoadDifficulty();

        StartCoroutine(LoadTraps());
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
    protected void LoadDifficulty()
    {
        _loadedDifficulty = _difficulties[_index];
        _uiController.UpdateThreatLevel(_loadedDifficulty.ThreatDescription, _loadedDifficulty.Warning);

        foreach (Emitter emitter in _allBlocks)
            emitter.UpdateDifficulty(_index);
    }
}