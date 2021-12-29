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
        bool hard = Controller.Instance.SaveController.Hard;

        _player = Controller.Instance.PoolController.GiveObject(_playerPrefab).GetComponent<Player>();
        _player.Initialize(Vector2.zero, hard);

        _level = Instantiate(Controller.Instance.SaveController.CurrentLevel).GetComponent<Level>();
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

        _player = Controller.Instance.PoolController.GiveObject(_playerPrefab).GetComponent<Player>();
        _player.Initialize(Vector2.zero, hard);

        _level.Initialize();
    }
}