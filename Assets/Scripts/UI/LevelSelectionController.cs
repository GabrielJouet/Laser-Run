using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that will handle level selection inputs.
/// </summary>
public class LevelSelectionController : MonoBehaviour
{
    /// <summary>
    /// Every levels.
    /// </summary>
    [SerializeField]
    private Transform _levelPanel;



    /// <summary>
    /// Start method, used after Awake.
    /// </summary>
    private void Start()
    {
        LevelMenu[] levels = _levelPanel.GetComponentsInChildren<LevelMenu>();
        List<LevelSave> saves = Controller.Instance.SaveController.SaveFile.LevelsProgression;

        for(int i = 0; i < levels.Length; i ++)
            levels[i].Initialize(saves[i].Locked, saves[i].Time, saves[i].Hard, saves[i].Win);
    }


    /// <summary>
    /// Method called by level buttons when chosen.
    /// </summary>
    /// <param name="index">The index of the level</param>
    public void LoadLevel(int index)
    {
        Controller.Instance.SaveController.LevelIndex = index;
        Controller.Instance.SaveController.Hard = false;
        SceneManager.LoadScene("PlayScene");
    }


    /// <summary>
    /// Method called by level buttons when chosen.
    /// </summary>
    /// <param name="index">The index of the level</param>
    public void LoadHardLevel(int index)
    {
        Controller.Instance.SaveController.LevelIndex = index;
        Controller.Instance.SaveController.Hard = true;
        SceneManager.LoadScene("PlayScene");
    }


    /// <summary>
    /// Method called to go back to main menu.
    /// </summary>
    public void GoBackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}