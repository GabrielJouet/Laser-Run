using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for Level and Endless level.
/// </summary>
public abstract class BaseLevel : MonoBehaviour
{
    [Header("Level complexity")]

    /// <summary>
    /// Available laser blocks in this level.
    /// </summary>
    [SerializeField]
    protected List<Emitter> _blocks;

    /// <summary>
    /// Additionnal laser blocks in this level.
    /// </summary>
    [SerializeField]
    protected List<Emitter> _additionnalBlocks;


    [Header("Misc")]

    /// <summary>
    /// Detritus prefab.
    /// </summary>
    [SerializeField]
    protected GameObject _thingPrefab;

    /// <summary>
    /// All detritus sprites available.
    /// </summary>
    [SerializeField]
    protected List<Sprite> _thingSprites;


    /// <summary>
    /// Player position when spawning.
    /// </summary>
    [SerializeField]
    protected Vector2 _playerPosition;

    /// <summary>
    /// Player position when spawning.
    /// </summary>
    public Vector2 PlayerPostion { get => _playerPosition; }


    /// <summary>
    /// Current difficulty loaded.
    /// </summary>
    protected LevelDifficulty _loadedDifficulty;

    /// <summary>
    /// All blocks in this level.
    /// </summary>
    protected readonly List<Emitter> _allBlocks = new List<Emitter>();



    /// <summary>
    /// Method called to initialize the object.
    /// </summary>
    public virtual void Initialize(int detritusCount)
    {
        _allBlocks.AddRange(_blocks);
        _allBlocks.AddRange(_additionnalBlocks);

        for (int i = 0; i < detritusCount; i++)
        {
            GameObject thingBuffer = Controller.Instance.PoolController.Out(_thingPrefab);
            thingBuffer.transform.Rotate(Vector3.forward * Random.Range(0, 360));
            thingBuffer.transform.localPosition = new Vector2(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f));
            thingBuffer.GetComponent<SpriteRenderer>().sprite = _thingSprites[Random.Range(0, _thingSprites.Count)];
        }
    }



    /// <summary>
    /// Coroutine used to start and warm up laser blocks.
    /// </summary>
    protected IEnumerator LoadTraps()
    {
        while (true)
        {
            for (int i = 0; i < _loadedDifficulty.ActivationCount; i++)
                FindOneBlock()?.WarmUp();

            yield return new WaitForSeconds(_loadedDifficulty.ActivationTime);
        }
    }


    /// <summary>
    /// Method ysed to find an available block.
    /// </summary>
    /// <returns>A non-used block</returns>
    protected LaserBlock FindOneBlock()
    {
        _blocks.Shuffle();

        LaserBlock found = null;
        foreach (LaserBlock block in _blocks)
        {
            if (!block.Used)
            {
                found = block;
                break;
            }
        }

        return found;
    }


    /// <summary>
    /// Method called when the level stops.
    /// </summary>
    public void StopLevel()
    {
        StopAllCoroutines();
    }
}