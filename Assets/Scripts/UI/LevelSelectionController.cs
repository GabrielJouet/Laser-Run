using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class that will handle level selection inputs.
/// </summary>
public class LevelSelectionController : MonoBehaviour
{
    [SerializeField]
    private List<Button> _levels;



    private void Start()
    {
        List<LevelSave> saves = Controller.Instance.SaveController.SaveFile.LevelsProgression;

        for(int i = 0; i < _levels.Count; i ++)
            if (!saves[i].Locked)
                _levels[i].enabled = true;
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