using System.Collections;
using UnityEngine;

/// <summary>
/// Class used to handle every music behavior.
/// </summary>
public class MusicController : MonoBehaviour
{
    /// <summary>
    /// Title theme.
    /// </summary>
    [SerializeField]
    private AudioSource _title;

    /// <summary>
    /// Play theme.
    /// </summary>
    [SerializeField]
    private AudioSource _play;



    /// <summary>
    /// Coroutine used to start the object.
    /// </summary>
    protected IEnumerator Start()
    {
        SaveController saveController = GetComponent<SaveController>();
        yield return new WaitUntil(() => saveController.Initialized);

        UpdateVolume(saveController.SaveFile.MusicMuted ? 0 : saveController.SaveFile.Music);
    }


    /// <summary>
    /// Method used to update the volume.
    /// </summary>
    /// <param name="volume">The new volume</param>
    public void UpdateVolume(float volume)
    {
        _title.volume = volume;
        _play.volume = volume;
    }


    /// <summary>
    /// Method used to load the title scene or level selection.
    /// </summary>
    public void LoadTitle()
    {
        StartCoroutine(ChangeMusic(true));
    }


    /// <summary>
    /// Method used to load the play scene.
    /// </summary>
    public void LoadPlay()
    {
        StartCoroutine(ChangeMusic(false));
    }


    /// <summary>
    /// Coroutine used to change a music in a specific timing.
    /// </summary>
    /// <param name="title">What music changes?</param>
    private IEnumerator ChangeMusic(bool title)
    {
        (title ? _title : _play).Play();

        for(int i = 0; i < 50; i ++)
        {
            yield return new WaitForFixedUpdate();
            _title.volume += title ? 0.02f : -0.02f;
            _play.volume += title ? -0.02f : 0.02f;
        }

        (title ? _play : _title).Stop();
    }
}