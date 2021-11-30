using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private List<GameObject> _levelAvailables;


    private void Start()
    {
        Controller.Instance.LoadScene();
        Controller.Instance.PoolController.GiveObject(_playerPrefab);
        Controller.Instance.PoolController.GiveObject(_levelAvailables[Controller.Instance.ChoiceController.LevelIndex]);
    }
}