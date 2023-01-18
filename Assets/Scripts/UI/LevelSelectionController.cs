using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    /// Level menu prefab used to display available levels.
    /// </summary>
    [SerializeField]
    private LevelMenu _levelMenuPrefab;

    /// <summary>
    /// Slider used to move levels.
    /// </summary>
    [SerializeField]
    private Slider _slider;


    /// <summary>
    /// What is the maximum size of the panel?
    /// </summary>
    private int _maxLevelPanelSize;



    /// <summary>
    /// Start method, used after Awake.
    /// </summary>
    private void Start()
    {
        List<LevelSave> saves = Controller.Instance.SaveController.SaveFile.LevelsProgression;
        List<Level> levels = Controller.Instance.SaveController.Levels;

        bool locked = false;
        int spawnedLevels = 0;
        for (int i = 0; i < levels.Count; i ++)
        {
            if (locked)
                break;

            spawnedLevels += 1;
            Instantiate(_levelMenuPrefab, _levelPanel).Initialize(saves[i], levels[i].Name, this, i, levels[i].Category);

            if (saves[i].State == LevelState.LOCKED)
                locked = true;
        }

        _maxLevelPanelSize = spawnedLevels * 300 + (spawnedLevels - 1) * 35 + 125 - Screen.width;

        _slider.gameObject.SetActive(spawnedLevels > 5);
    }


    /// <summary>
    /// Method called by level buttons when chosen.
    /// </summary>
    /// <param name="index">The index of the level</param>
    public void LoadLevel(int index)
    {
        Controller.Instance.SaveController.LevelIndex = index;
        Controller.Instance.SaveController.Hard = false;
        Controller.Instance.MusicController.LoadPlay();
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
        Controller.Instance.MusicController.LoadPlay();
        SceneManager.LoadScene("PlayScene");
    }


    /// <summary>
    /// Method called to go back to main menu.
    /// </summary>
    public void GoBackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    /// <summary>
    /// Method called when a change is made to the slider.
    /// </summary>
    public void MoveSlider()
    {
        _levelPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(_maxLevelPanelSize * -_slider.value, 0);
    }
}