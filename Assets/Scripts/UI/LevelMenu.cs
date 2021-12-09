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


    public void Initialize(bool locked, float maxTime)
    {
        _timeText.text = string.Format("{0:#.00 sec}", maxTime);

        _startButton.enabled = !locked;
        _lockedImage.SetActive(locked);
    }
}