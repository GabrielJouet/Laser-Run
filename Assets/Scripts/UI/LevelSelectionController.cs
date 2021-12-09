using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that will handle level selection inputs.
/// </summary>
public class LevelSelectionController : MonoBehaviour
{
    [SerializeField]
    private Transform _levelPanel;



    private void Start()
    {
        LevelMenu[] levels = _levelPanel.GetComponentsInChildren<LevelMenu>();
        List<LevelSave> saves = Controller.Instance.SaveController.SaveFile.LevelsProgression;

        for(int i = 0; i < levels.Length; i ++)
            levels[i].Initialize(saves[i].Locked, saves[i].Time);
    }


    /// <summary>
    /// Method called by level buttons when chosen.
    /// </summary>
    /// <param name="index">The index of the level</param>
    public void LoadLevel(int index)
    {
        Controller.Instance.ChoiceController.LevelIndex = index;
        SceneManager.LoadScene("PlayScene");
    }
}