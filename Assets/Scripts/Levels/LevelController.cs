using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class used to handle level and player inside a level.
/// </summary>
[RequireComponent(typeof(UIController))]
public class LevelController : MonoBehaviour
{
    /// <summary>
    /// Player prefab.
    /// </summary>
    [SerializeField]
    private GameObject _playerPrefab;

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
    /// Spawned player.
    /// </summary>
    private Player _player;

    /// <summary>
    /// Spawned level.
    /// </summary>
    private Level _level;



    /// <summary>
    /// Start method, called after Awake.
    /// </summary>
    private void Start()
    {
        Controller.Instance.AddReferencesWhenLoaded(this, GetComponent<UIController>());

        _player = Controller.Instance.PoolController.Out(_playerPrefab).GetComponent<Player>();
        _player.Initialize(Vector2.zero, Controller.Instance.SaveController.Hard);

        _level = Instantiate(Controller.Instance.SaveController.CurrentLevel).GetComponent<Level>();

        for(int i = 0; i < Random.Range(3, 15); i ++)
        {
            GameObject thingBuffer = Controller.Instance.PoolController.Out(_thingPrefab);
            thingBuffer.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            thingBuffer.transform.localPosition = new Vector2(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f));
            thingBuffer.GetComponent<SpriteRenderer>().sprite = _thingSprites[Random.Range(0, _thingSprites.Count)];
        }

        _level.Initialize();
    }


    /// <summary>
    /// Method called when a level is finished.
    /// </summary>
    /// <param name="win">Does the player wins the level?</param>
    public void FinishLevel(bool win)
    {
        _level.StopLevel();
        _player.BecameInvicible();

        if (_level.TimeElapsed >= _level.NeededTime)
            win = true;

        Controller.Instance.SaveController.SaveLevelData(win ? _level.NeededTime : _level.TimeElapsed, win);
        Controller.Instance.UIController.DisplayGameOverScreen(win);
    }


    /// <summary>
    /// Method used to get back to level selection screen.
    /// </summary>
    public void GoBackToSelection()
    {
        Controller.Instance.PoolController.RetrieveAllPools();
        Controller.Instance.MusicController.LoadTitle();
        SceneManager.LoadScene("LevelSelection");
    }


    /// <summary>
    /// Method called when we want to restart a level.
    /// </summary>
    public void RestartLoadedLevel()
    {
        Controller.Instance.PoolController.RetrieveAllPools();
        Controller.Instance.UIController.HideGameOverScreen();
        bool hard = Controller.Instance.SaveController.Hard;

        _player = Controller.Instance.PoolController.Out(_playerPrefab).GetComponent<Player>();
        _player.Initialize(Vector2.zero, hard);

        for (int i = 0; i < Random.Range(3, 15); i++)
        {
            GameObject thingBuffer = Controller.Instance.PoolController.Out(_thingPrefab);
            thingBuffer.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            thingBuffer.transform.localPosition = new Vector2(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f));
            thingBuffer.GetComponent<SpriteRenderer>().sprite = _thingSprites[Random.Range(0, _thingSprites.Count)];
        }

        _level.Initialize();
    }
}