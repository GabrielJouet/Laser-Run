using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Slider _scoreSlider;

    [SerializeField]
    private Text _timeLeft;

    [SerializeField]
    private GameObject _gameOverScreen;

    private float _timeMax;


    public void SetTimeMax(float timeMax)
    {
        _timeMax = timeMax;
    }


    public void UpdateTimeLeft(float timeElapsed)
    {
        _scoreSlider.value = timeElapsed / _timeMax;
        _timeLeft.text = (_timeMax - timeElapsed).ToString();
    }


    public void DisplayGameOverScreen()
    {
        StartCoroutine(DelayGameOverScreen());
    }


    private IEnumerator DelayGameOverScreen()
    {
        yield return new WaitForSeconds(0.5f);
        _gameOverScreen.SetActive(true);
    }
}