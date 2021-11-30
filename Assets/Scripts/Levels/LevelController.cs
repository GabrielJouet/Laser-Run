using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private List<GameObject> _levelAvailables;


    private void Start()
    {
        Controller.Instance.LoadScene(this);
        Controller.Instance.PoolController.GiveObject(_playerPrefab);
        Instantiate(_levelAvailables[Controller.Instance.ChoiceController.LevelIndex]).GetComponent<Level>().Initialize();
    }


    public void FinishLevel(GameObject level)
    {
        Controller.Instance.PoolController.RetrieveObject(level);
        Controller.Instance.PoolController.RetrieveObject(FindObjectOfType<Player>().gameObject);
        SceneManager.LoadScene("LevelSelection");
    }
}