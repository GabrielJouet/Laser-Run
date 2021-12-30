using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class that will handle every inputs in main menu.
/// </summary>
[RequireComponent(typeof(Volume))]
public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// Confirmation game object.
    /// </summary>
    [SerializeField]
    private Text _confirmationText;

    /// <summary>
    /// Confirmation game object.
    /// </summary>
    [SerializeField]
    private Button _confirmationButton;

    /// <summary>
    /// Confirmation game object.
    /// </summary>
    [SerializeField]
    private GameObject _menuObject;

    /// <summary>
    /// Title game object.
    /// </summary>
    [SerializeField]
    private GameObject _title;

    [SerializeField]
    private Image _bloomStatus;

    [SerializeField]
    private Image _chromaticStatus;

    [SerializeField]
    private Image _filmGrainStatus;


    [SerializeField]
    private Sprite _yes;

    [SerializeField]
    private Sprite _no;


    [SerializeField]
    private Image _soundStatus;

    [SerializeField]
    private Slider _soundSlider;


    [SerializeField]
    private Image _musicStatus;

    [SerializeField]
    private Slider _musicSlider;


    [SerializeField]
    private Image _fullScreenStatus;


    private Volume _postProcessingVolume;
    private SaveController _saveController;


    protected IEnumerator Start()
    {
        _postProcessingVolume = GetComponent<Volume>();
        _saveController = Controller.Instance.SaveController;
        yield return new WaitUntil(() => _saveController.Initialized);

        Screen.fullScreen = _saveController.SaveFile.FullScreen;

        _chromaticStatus.sprite = _saveController.SaveFile.ChromaticAberration ? _yes : _no;
        _filmGrainStatus.sprite = _saveController.SaveFile.FilmGrain ? _yes : _no;
        _bloomStatus.sprite = _saveController.SaveFile.Bloom ? _yes : _no;

        _soundStatus.sprite = _saveController.SaveFile.SoundMuted ? _yes : _no;
        _soundSlider.value = _saveController.SaveFile.Sound;

        _musicStatus.sprite = _saveController.SaveFile.MusicMuted ? _yes : _no;
        _musicSlider.value = _saveController.SaveFile.Music;

        _fullScreenStatus.sprite = _saveController.SaveFile.FullScreen ? _yes : _no;
    }


    /// <summary>
    /// Method called to open level selection screen.
    /// </summary>
    public void LoadLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }


    /// <summary>
    /// Method called when we want to open the menu.
    /// </summary>
    public void OpenMenu()
    {
        _title.SetActive(false);
        _menuObject.SetActive(true);
    }


    /// <summary>
    /// Method called when we want to open the menu.
    /// </summary>
    public void CloseMenu()
    {
        _title.SetActive(true);
        _menuObject.SetActive(false);
    }


    public void ChangeCromathicAberration()
    {
        _postProcessingVolume.profile.TryGet(out ChromaticAberration chroma);
        chroma.active = !chroma.active;

        _chromaticStatus.sprite = chroma.active ? _yes : _no;

        _saveController.SaveChromatic(chroma.active);
    }


    public void ChangeFilmGrain()
    {
        _postProcessingVolume.profile.TryGet(out FilmGrain filmGrain);
        filmGrain.active = !filmGrain.active;

        _filmGrainStatus.sprite = filmGrain.active ? _yes : _no;

        _saveController.SaveGrain(filmGrain.active);
    }


    public void ChangeBloom()
    {
        _postProcessingVolume.profile.TryGet(out Bloom bloom);
        bloom.active = !bloom.active;

        _bloomStatus.sprite = bloom.active ? _yes : _no;

        _saveController.SaveBloom(bloom.active);
    }


    public void ChangeSoundStatus()
    {
        bool state = !_saveController.SaveFile.SoundMuted;
        _soundStatus.sprite = state ? _yes : _no;

        _saveController.SaveSoundMute(state);
    }


    public void UpdateSoundValue()
    {
        _saveController.SaveSoundLevel(_soundSlider.value);
    }


    public void ChangeMusicStatus()
    {
        bool state = !_saveController.SaveFile.MusicMuted;
        _musicStatus.sprite = state ? _yes : _no;

        _saveController.SaveMusicMute(state);
    }


    public void UpdateMusicValue()
    {
        _saveController.SaveMusicLevel(_musicSlider.value);
    }


    public void ChangeFullScreen()
    {
        bool state = !_saveController.SaveFile.FullScreen;
        _fullScreenStatus.sprite = state ? _yes : _no;

        _saveController.SaveFullScreen(state);
        Screen.fullScreen = state;
    }


    /// <summary>
    /// Method called when we want to close the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }


    /// <summary>
    /// Method called when we want to reset game save, this is the first confirmation.
    /// </summary>
    public void FirstReset()
    {
        _confirmationText.text = "Sure?";
        _confirmationButton.onClick.RemoveAllListeners();
        _confirmationButton.onClick.AddListener(() => SecondReset());
    }


    /// <summary>
    /// Method called when we want to reset game save, this is the second confirmation.
    /// </summary>
    public void SecondReset()
    {
        _confirmationText.text = "Confirm";
        _confirmationButton.onClick.RemoveAllListeners();
        _confirmationButton.onClick.AddListener(() => ResetSave());
    }


    /// <summary>
    /// Method called when we want to reset game save, this is the last confirmation.
    /// </summary>
    public void ResetSave()
    {
        Controller.Instance.SaveController.ResetData();
        _confirmationText.text = "Done";
        _confirmationButton.enabled = false;
    }
}