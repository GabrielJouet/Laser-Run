using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used to handle achievement popup.
/// </summary>
public class AchievementPopup : MonoBehaviour
{
    /// <summary>
    /// Image component used to display achievement icon.
    /// </summary>
    [SerializeField]
    private Image _icon;

    /// <summary>
    /// Text mesh pto component used to display achievement name.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _title;


    /// <summary>
    /// Animator component used to display achievement movement.
    /// </summary>
    private Animator _animator;


    /// <summary>
    /// Audio Source component used to add emphasis on achievement unlock.
    /// </summary>
    private AudioSource _audioSource;



    /// <summary>
    /// Awake method, called at initialization before Start.
    /// </summary>
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _audioSource.volume = !Controller.Instance.SaveController.SaveFile.SoundMuted ? Controller.Instance.SaveController.SaveFile.Sound : 0;
    }


    /// <summary>
    /// Method called to trigger the achievement popup.
    /// </summary>
    /// <param name="achievement">The achievement triggered</param>
    public void PopupAchievement(Achievement achievement)
    {
        _icon.sprite = achievement.Icon;
        _title.text = achievement.Name;

        _animator.SetTrigger("popup");
        _audioSource.Play();

        StartCoroutine(PopOutAchievement());
    }


    /// <summary>
    /// Coroutine used to hide the achievement popup.
    /// </summary>
    private IEnumerator PopOutAchievement()
    {
        yield return new WaitForSeconds(2.5f);
        _animator.SetTrigger("popout");
    }
}