using System.Collections;
using TMPro;
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
    [Header("Main components")]

    /// <summary>
    /// Main menu.
    /// </summary>
    [SerializeField]
    private GameObject _menuObject;

    /// <summary>
    /// Title game object.
    /// </summary>
    [SerializeField]
    private GameObject _title;


    [Header("Confirmation")]

    /// <summary>
    /// Confirmation game object.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _confirmationText;

    /// <summary>
    /// Confirmation button object.
    /// </summary>
    [SerializeField]
    private Button _confirmationButton;


    [Header("Post processing values")]

    /// <summary>
    /// Bloom component.
    /// </summary>
    [SerializeField]
    private Image _bloomStatus;

    /// <summary>
    /// Chromatic aberration status component.
    /// </summary>
    [SerializeField]
    private Image _chromaticStatus;

    /// <summary>
    /// Film grain status component.
    /// </summary>
    [SerializeField]
    private Image _filmGrainStatus;

    /// <summary>
    /// Fullscreen status component.
    /// </summary>
    [SerializeField]
    private Image _fullScreenStatus;


    [Header("Sound and Music")]

    /// <summary>
    /// Sound status component.
    /// </summary>
    [SerializeField]
    private Image _soundStatus;

    /// <summary>
    /// Sound slider value component.
    /// </summary>
    [SerializeField]
    private Slider _soundSlider;


    /// <summary>
    /// Music status component.
    /// </summary>
    [SerializeField]
    private Image _musicStatus;

    /// <summary>
    /// Music slider value component.
    /// </summary>
    [SerializeField]
    private Slider _musicSlider;


    [Header("UI Sprites")]

    /// <summary>
    /// True sprite.
    /// </summary>
    [SerializeField]
    private Sprite _yes;

    /// <summary>
    /// False sprite.
    /// </summary>
    [SerializeField]
    private Sprite _no;


    /// <summary>
    /// Components shortcuts.
    /// </summary>
    private Volume _postProcessingVolume;
    private SaveController _saveController;


    /// <summary>
    /// Coroutine used at start.
    /// </summary>
    protected IEnumerator Start()
    {
        _postProcessingVolume = GetComponent<Volume>();
        _saveController = Controller.Instance.SaveController;
        yield return new WaitUntil(() => _saveController.Initialized);

        Screen.fullScreen = _saveController.SaveFile.FullScreen;

        _chromaticStatus.sprite = _saveController.SaveFile.ChromaticAberration ? _yes : _no;
        _filmGrainStatus.sprite = _saveController.SaveFile.FilmGrain ? _yes : _no;
        _bloomStatus.sprite = _saveController.SaveFile.Bloom ? _yes : _no;

        _soundStatus.sprite = !_saveController.SaveFile.SoundMuted ? _yes : _no;
        _soundSlider.value = _saveController.SaveFile.Sound;

        _musicStatus.sprite = !_saveController.SaveFile.MusicMuted ? _yes : _no;
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


    /// <summary>
    /// Method called to invert chromatic aberration.
    /// </summary>
    public void ChangeCromathicAberration()
    {
        _postProcessingVolume.profile.TryGet(out ChromaticAberration chroma);
        chroma.active = !chroma.active;

        _chromaticStatus.sprite = chroma.active ? _yes : _no;

        _saveController.SaveChromatic(chroma.active);
    }


    /// <summary>
    /// Method called to invert film grain.
    /// </summary>
    public void ChangeFilmGrain()
    {
        _postProcessingVolume.profile.TryGet(out FilmGrain filmGrain);
        filmGrain.active = !filmGrain.active;

        _filmGrainStatus.sprite = filmGrain.active ? _yes : _no;

        _saveController.SaveGrain(filmGrain.active);
    }


    /// <summary>
    /// Method called to invert bloom.
    /// </summary>
    public void ChangeBloom()
    {
        _postProcessingVolume.profile.TryGet(out Bloom bloom);
        bloom.active = !bloom.active;

        _bloomStatus.sprite = bloom.active ? _yes : _no;

        _saveController.SaveBloom(bloom.active);
    }


    /// <summary>
    /// Method called to invert sound.
    /// </summary>
    public void ChangeSoundStatus()
    {
        bool state = !_saveController.SaveFile.SoundMuted;
        _soundStatus.sprite = state ? _no : _yes;

        _saveController.SaveSoundMute(state);
    }


    /// <summary>
    /// Method called to change sound value.
    /// </summary>
    public void UpdateSoundValue()
    {
        _saveController.SaveSoundLevel(_soundSlider.value);
    }


    /// <summary>
    /// Method called to invert music status.
    /// </summary>
    public void ChangeMusicStatus()
    {
        bool state = !_saveController.SaveFile.MusicMuted;
        _musicStatus.sprite = state ? _no : _yes;

        _saveController.SaveMusicMute(state);
        Controller.Instance.MusicController.UpdateVolume(state ? 0 : _saveController.SaveFile.Music);
    }


    /// <summary>
    /// Method called to change music value.
    /// </summary>
    public void UpdateMusicValue()
    {
        _saveController.SaveMusicLevel(_musicSlider.value);
        Controller.Instance.MusicController.UpdateVolume(_saveController.SaveFile.MusicMuted ? 0 : _musicSlider.value);
    }


    /// <summary>
    /// Method called to invert fullscreen.
    /// </summary>
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

        Screen.fullScreen = _saveController.SaveFile.FullScreen;

        _chromaticStatus.sprite = _saveController.SaveFile.ChromaticAberration ? _yes : _no;
        _filmGrainStatus.sprite = _saveController.SaveFile.FilmGrain ? _yes : _no;
        _bloomStatus.sprite = _saveController.SaveFile.Bloom ? _yes : _no;

        _soundStatus.sprite = !_saveController.SaveFile.SoundMuted ? _yes : _no;
        _soundSlider.value = _saveController.SaveFile.Sound;

        _musicStatus.sprite = !_saveController.SaveFile.MusicMuted ? _yes : _no;
        _musicSlider.value = _saveController.SaveFile.Music;

        _fullScreenStatus.sprite = _saveController.SaveFile.FullScreen ? _yes : _no;

        Controller.Instance.MusicController.UpdateVolume(1);
    }
}