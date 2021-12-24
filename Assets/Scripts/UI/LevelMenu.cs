using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField]
    private Text _timeText;

    [SerializeField]
    private Button _startButton;

    [SerializeField]
    private GameObject _lockedImage;

    [SerializeField]
    private GameObject _finishedImage;

    [SerializeField]
    private GameObject _finishedHardImage;

    [SerializeField]
    private GameObject _hardButton;


    public void Initialize(bool locked, float maxTime, bool hard, bool win)
    {
        _timeText.text = string.Format("{0:#.00 sec}", maxTime);

        _startButton.enabled = !locked;
        _lockedImage.SetActive(locked);

        _finishedImage.SetActive(win);
        _finishedHardImage.SetActive(hard);

        _hardButton.SetActive(win);
    }
}