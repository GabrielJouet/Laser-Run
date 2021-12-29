using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessCamera : MonoBehaviour
{
    protected IEnumerator Start()
    {
        SaveController saveController = Controller.Instance.SaveController;
        yield return new WaitUntil(() => saveController.Initialized);
        GetComponent<Volume>().profile.TryGet(out ChromaticAberration chroma);
        GetComponent<Volume>().profile.TryGet(out FilmGrain filmGrain);
        GetComponent<Volume>().profile.TryGet(out Bloom bloom);

        chroma.active = saveController.SaveFile.ChromaticAberration;
        filmGrain.active = saveController.SaveFile.FilmGrain;
        bloom.active = saveController.SaveFile.Bloom;
    }
}