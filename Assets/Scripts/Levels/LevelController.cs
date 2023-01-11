using System.Collections;
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
    /// Tutorial screen, used the first time.
    /// </summary>
    [SerializeField]
    private GameObject _tutorialScreen;


    /// <summary>
    /// Spawned player.
    /// </summary>
    private Player _player;

    /// <summary>
    /// Spawned level.
    /// </summary>
    private Level _level;


    /// <summary>
    /// How much time the player dies in a row?
    /// </summary>
    private int _deathInARow = 0;



    /// <summary>
    /// Start method, called after Awake.
    /// </summary>
    private void Start()
    {
        Controller.Instance.AddReferencesWhenLoaded(this, GetComponent<UIController>());

        if (!Controller.Instance.SaveController.SaveFile.Tutorial)
            _tutorialScreen.SetActive(true);
        else
        {
            _player = Controller.Instance.PoolController.Out(_playerPrefab).GetComponent<Player>();
            _player.Initialize(Vector2.zero, Controller.Instance.SaveController.Hard);

            _level = Instantiate(Controller.Instance.SaveController.CurrentLevel);
            _level.Initialize();
        }
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
        else
        {
            _deathInARow++;

            if (_deathInARow >= 10)
                Controller.Instance.AchievementController.TriggerAchievement("A-2");

            if (_level.NeededTime - _level.TimeElapsed <= 1 && Controller.Instance.SaveController.Hard)
                Controller.Instance.AchievementController.TriggerAchievement("A-9");
            else if (_level.NeededTime - _level.TimeElapsed <= 3 && !Controller.Instance.SaveController.Hard)
                Controller.Instance.AchievementController.TriggerAchievement("A-8");
        }

        Controller.Instance.SaveController.SaveLevelData(win ? _level.NeededTime : _level.TimeElapsed, win);
        Controller.Instance.UIController.DisplayGameOverScreen(win);

        if (!win)
            StartCoroutine(RestartLoadedLevel());
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
    /// Method called when the tutorial is set to done.
    /// </summary>
    public void SetTutorialDone()
    {
        _tutorialScreen.SetActive(false);
        Controller.Instance.SaveController.SaveTutorial();
        Controller.Instance.AchievementController.TriggerAchievement("A-12");

        _player = Controller.Instance.PoolController.Out(_playerPrefab).GetComponent<Player>();
        _player.Initialize(Vector2.zero, Controller.Instance.SaveController.Hard);

        _level = Instantiate(Controller.Instance.SaveController.CurrentLevel);
        _level.Initialize();
    }


    /// <summary>
    /// Method called when we want to restart a level.
    /// </summary>
    private IEnumerator RestartLoadedLevel()
    {
        yield return new WaitForSeconds(0.7f);
        Controller.Instance.PoolController.RetrieveAllPools();
        Controller.Instance.UIController.HideGameOverScreen();
        bool hard = Controller.Instance.SaveController.Hard;

        _player = Controller.Instance.PoolController.Out(_playerPrefab).GetComponent<Player>();
        _player.Initialize(Vector2.zero, hard);

        _level.Initialize();
    }
}