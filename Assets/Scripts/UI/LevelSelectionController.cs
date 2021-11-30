using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionController : MonoBehaviour
{
    public void LoadLevel(int index)
    {
        Controller.Instance.ChoiceController.LevelIndex = index;
        SceneManager.LoadScene("PlayScene");
    }
}