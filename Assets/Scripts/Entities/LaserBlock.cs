using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private List<Transform> _canonPositions;

    public bool Used;

    private LevelDifficulty _difficulty;



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
        if (!skipLoad)
        {
            for (int i = 0; i < _clockLeds.Count; i++)
            {
                yield return new WaitForSeconds(_difficulty.ShotsTime / _clockLeds.Count);
                _clockLeds[i].SetActive(true);
            }
        }

        for (int i = 0; i < _clockLeds.Count; i++)
            _clockLeds[i].SetActive(false);

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

        Used = false;
    }


    private void Shot(GameObject laser, float angle)
    {
        Controller.Instance.PoolController.GiveObject(laser).GetComponent<Laser>().Initialize(angle, _canonPositions[_facing].position, _difficulty.ReactionTime);
    }
}