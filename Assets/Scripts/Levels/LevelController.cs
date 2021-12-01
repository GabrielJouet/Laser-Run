using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class used to handle level and player inside a level.
/// </summary>
public class LevelController : MonoBehaviour
{
    /// <summary>
    /// Player prefab.
    /// </summary>
    [SerializeField]
    private GameObject _playerPrefab;

    /// <summary>
    /// All levels availables in the game.
    /// </summary>
    [SerializeField]
    private List<GameObject> _levelAvailables;


    /// <summary>
    /// Spawned player.
    /// </summary>
    private GameObject _player;

    /// <summary>
    /// Spawned level.
    /// </summary>
    private GameObject _level;



    /// <summary>
    /// Start method, called after Awake.
    /// </summary>
    private void Start()
    {
        Controller.Instance.LoadScene(this);

        _player = Controller.Instance.PoolController.GiveObject(_playerPrefab);
        _player.GetComponent<Player>().Initialize();

        _level = Instantiate(_levelAvailables[Controller.Instance.ChoiceController.LevelIndex]);
        _level.GetComponent<Level>().Initialize();
    }


    /// <summary>
    /// Method called when a level is finished.
    /// </summary>
    public void FinishLevel()
    {
        Controller.Instance.PoolController.RetrieveAllPools();
        SceneManager.LoadScene("LevelSelection");
    }
}