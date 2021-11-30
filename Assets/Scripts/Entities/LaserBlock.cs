using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LaserBlock : MonoBehaviour
{
    [SerializeField]
    private GameObject _laser;

    [SerializeField]
    private GameObject _semiLaser;

    [SerializeField]
    private List<GameObject> _clockLeds;

    [SerializeField]
    private int _facing;

    [SerializeField]
    private Light2D _canonPosition;

    public bool Used;

    private LevelDifficulty _difficulty;


    private void Awake()
    {
        switch(_facing)
        {
            case 0:
                _canonPosition.transform.position = transform.position + new Vector3(0, 0.09f);
                break;

            case 1:
                _canonPosition.transform.position = transform.position + new Vector3(-0.09f, 0);
                break;

            case 2:
                _canonPosition.transform.position = transform.position + new Vector3(0, -0.09f);
                break;

            case 3:
                _canonPosition.transform.position = transform.position + new Vector3(0.09f, 0);
                break;
        }
    }


    public void WarmUp(LevelDifficulty difficulty)
    {
        _difficulty = difficulty;

        StartCoroutine(DelayShot(false));
    }


    public void ActivateEarly()
    {
        StopAllCoroutines();
        StartCoroutine(DelayShot(true));
    }


    private IEnumerator DelayShot(bool skipLoad)
    {
        Used = true;
        _canonPosition.enabled = true;
        _canonPosition.intensity = 0.25f;
        if (!skipLoad)
        {
            for (int i = 0; i < _clockLeds.Count; i++)
            {
                yield return new WaitForSeconds(_difficulty.ShotsTime / _clockLeds.Count);
                _canonPosition.intensity = 0.25f + i * 0.1f;
                _clockLeds[i].SetActive(true);
            }
        }

        for (int i = 0; i < _difficulty.NumberOfShots; i ++)
        {
            float angle = Random.Range(-_difficulty.Dispersion, _difficulty.Dispersion);

            if (_difficulty.RandomShots)
                angle += _facing * 90;
            else
            {
                Transform buffer = FindObjectOfType<Player>().transform;
                Vector3 vectorToTarget = new Vector3(transform.position.x - buffer.position.x, transform.position.y - buffer.position.y, 0);
                angle += Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
            }

            Shot(_semiLaser, angle);
            yield return new WaitForSeconds(_difficulty.ReactionTime);
            Shot(_laser, angle);

            yield return new WaitForSeconds(_difficulty.ReactionTime);
        }

        for (int i = 0; i < _clockLeds.Count; i++)
            _clockLeds[i].SetActive(false);

        Used = false;
        _canonPosition.enabled = false;
    }


    private void Shot(GameObject laser, float angle)
    {
        Controller.Instance.PoolController.GiveObject(laser).GetComponent<Laser>().Initialize(angle, _canonPosition.transform.position, _difficulty.ReactionTime);
    }
}