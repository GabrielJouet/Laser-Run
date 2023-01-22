using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class used to handle endless mode behavior.
/// </summary>
public class EndlessController : UIController
{
    /// <summary>
    /// Previous score component.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _previousScore;

    /// <summary>
    /// Player prefab.
    /// </summary>
    [SerializeField]
    private GameObject _playerPrefab;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private EndlessLevel _endlessLevel;


    /// <summary>
    /// Spawned player.
    /// </summary>
    private Player _player;

    /// <summary>
    /// Actual score.
    /// </summary>
    private float _score = 0;

    /// <summary>
    /// Actual factor.
    /// </summary>
    private float _factor = 0.5f;

    /// <summary>
    /// Actual current level.
    /// </summary>
    private EndlessLevel _level;

    /// <summary>
    /// 
    /// </summary>
    private bool _counting = false;



    /// <summary>
    /// Start method, called after Awake, changed into a coroutine.
    /// </summary>
    private IEnumerator Start()
    {
        _player = Instantiate(_playerPrefab).GetComponent<Player>();
        _player.Initialize(Vector2.zero, Controller.Instance.SaveController.Hard);

        yield return new WaitUntil(() => Controller.Instance);

        _previousScore.text = Controller.Instance.SaveController.SaveFile.EndlessScore + " pts";

        _counting = true;
        _level = Instantiate(_endlessLevel);
        _level.Initialize(Random.Range(5, 15));
    }


    /// <summary>
    /// Update method, called every frame.
    /// </summary>
    private void Update()
    {
        if (_counting)
        {
            _score += Time.deltaTime * _factor;
            _timeLeft.text = Mathf.FloorToInt(_score) + " pts";
        }
    }


    /// <summary>
    /// Method called when a level is finished.
    /// </summary>
    public void FinishLevel()
    {
        _player.BecameInvicible();
        _level.StopLevel();
        _counting = false;

        bool highScore = _score >= Controller.Instance.SaveController.SaveFile.EndlessScore;

        if (_score > 1000)
        {
            Controller.Instance.AchievementController.TriggerAchievement("A-17");

            if (_score > 2500)
                Controller.Instance.AchievementController.TriggerAchievement("A-18");
        }

        Controller.Instance.SaveController.SaveAchievementProgress("A-16", 1, true);

        Controller.Instance.SaveController.SaveEndlessModeScore(Mathf.FloorToInt(_score));

        if (highScore)
            StartCoroutine(DelayScreenDisplay("New high score!"));
        else
        {
            _deathCutOut.SetTrigger("die");
            StartCoroutine(RestartLoadedLevel(false));
        }
    }


    /// <summary>
    /// Method used to get back to level selection screen.
    /// </summary>
    public void GoBackToSelection()
    {
        Controller.Instance.MusicController.LoadTitle();
        SceneManager.LoadScene("MainMenu");
    }


    /// <summary>
    /// Method used to hide the game over screen.
    /// </summary>
    public override void HideGameOverScreen()
    {
        base.HideGameOverScreen();

        StartCoroutine(RestartLoadedLevel(true));
    }


    /// <summary>
    /// Method called when we want to restart a level.
    /// </summary>
    private IEnumerator RestartLoadedLevel(bool skip)
    {
        yield return new WaitForSeconds(0.25f);

        if (skip)
            _deathCutOut.SetTrigger("die");

        _endlessLevel.CleanUpLevel();

        yield return new WaitForSeconds(skip ? 0.7f : 0.45f);

        _player = Instantiate(_playerPrefab).GetComponent<Player>();
        _player.Initialize(_level.PlayerPostion, false);

        _level.Initialize(Random.Range(5, 15));
        _counting = true;
        _score = 0;
        _factor = 0.5f;
    }


    public void UpdateFactor(float factor)
    {
        _factor = Mathf.Clamp(factor + _factor, 0, 99999);
    }
}