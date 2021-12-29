using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class that will handle every inputs in main menu.
/// </summary>
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


    protected IEnumerator Start()
    {
        SaveController saveController = Controller.Instance.SaveController;
        yield return new WaitUntil(() => saveController.Initialized);

        _chromaticStatus.sprite = saveController.SaveFile.ChromaticAberration ? _yes : _no;

        _filmGrainStatus.sprite = saveController.SaveFile.FilmGrain ? _yes : _no;

        _bloomStatus.sprite = saveController.SaveFile.Bloom ? _yes : _no;
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
        GetComponent<Volume>().profile.TryGet(out ChromaticAberration chroma);
        chroma.active = !chroma.active;

        _chromaticStatus.sprite = chroma.active ? _yes : _no;

        Controller.Instance.SaveController.SaveChromatic(chroma.active);
    }


    public void ChangeFilmGrain()
    {
        GetComponent<Volume>().profile.TryGet(out FilmGrain filmGrain);
        filmGrain.active = !filmGrain.active;

        _filmGrainStatus.sprite = filmGrain.active ? _yes : _no;

        Controller.Instance.SaveController.SaveGrain(filmGrain.active);
    }


    public void ChangeBloom()
    {
        GetComponent<Volume>().profile.TryGet(out Bloom bloom);
        bloom.active = !bloom.active;

        _bloomStatus.sprite = bloom.active ? _yes : _no;

        Controller.Instance.SaveController.SaveBloom(bloom.active);
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