using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private List<GameObject> _levelAvailables;

    private GameObject _player;

    private GameObject _level;


    private void Start()
    {
        Controller.Instance.LoadScene(this);

        _player = Controller.Instance.PoolController.GiveObject(_playerPrefab);
        _player.GetComponent<Player>().Initialize();

        _level = Instantiate(_levelAvailables[Controller.Instance.ChoiceController.LevelIndex]);
        _level.GetComponent<Level>().Initialize();
    }


    public void FinishLevel()
    {
        Controller.Instance.PoolController.RetrieveAllPools();
        SceneManager.LoadScene("LevelSelection");
    }
}