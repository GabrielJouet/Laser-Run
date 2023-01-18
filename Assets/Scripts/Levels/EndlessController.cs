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
    /// Start method, called after Awake, changed into a coroutine.
    /// </summary>
    private IEnumerator Start()
    {
        _player = Controller.Instance.PoolController.Out(_playerPrefab).GetComponent<Player>();
        _player.Initialize(Vector2.zero, Controller.Instance.SaveController.Hard);

        yield return new WaitUntil(() => Controller.Instance);

        _previousScore.text = Controller.Instance.SaveController.SaveFile.EndlessScore + " pts";

        _level = Instantiate(_endlessLevel);
        _level.Initialize(Random.Range(5, 15));
    }


    /// <summary>
    /// Update method, called every frame.
    /// </summary>
    private void Update()
    {
        _score += Time.deltaTime * _factor;
        _timeLeft.text = Mathf.FloorToInt(_score) + " pts";
    }


    /// <summary>
    /// Method called when a level is finished.
    /// </summary>
    public void FinishLevel()
    {
        _player.BecameInvicible();
        _level.StopLevel();

        bool highScore = _score >= Controller.Instance.SaveController.SaveFile.EndlessScore;

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
        Controller.Instance.PoolController.RetrieveAllPools();
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
        if (!skip)
            yield return new WaitForSeconds(0.7f);

        Controller.Instance.PoolController.RetrieveAllPools();

        _player = Controller.Instance.PoolController.Out(_playerPrefab).GetComponent<Player>();
        _player.Initialize(Vector2.zero, false);

        _level.Initialize(Random.Range(5, 15));
        _factor = 0.5f;
        _score = 0;
        _previousScore.text = Controller.Instance.SaveController.SaveFile.EndlessScore + " pts";
    }


    public void UpdateFactor(float factor)
    {
        _factor = Mathf.Clamp(factor + _factor, 0, 99999);
    }
}