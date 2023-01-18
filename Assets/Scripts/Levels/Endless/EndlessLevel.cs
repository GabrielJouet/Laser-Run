using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class EndlessLevel : BaseLevel
{
    /// <summary>
    /// 
    /// </summary>
    private EndlessController _endlessController;



    /// <summary>
    /// Method called to initialize the object.
    /// </summary>
    public override void Initialize(int detritusCount)
    {
        base.Initialize(detritusCount);

        _endlessController = FindObjectOfType<EndlessController>();

        _loadedDifficulty = new LevelDifficulty();

        foreach (Emitter block in _allBlocks)
            block.Initialize(_loadedDifficulty);

        StartCoroutine(LoadTraps());
        StartCoroutine(ChangeDifficulty());
    }


    /// <summary>
    /// Coroutine that will handle the end of the level.
    /// </summary>
    private IEnumerator ChangeDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(8f, 15f));

            SelectAttributeToModify();
            SelectAttributeToModify();

            _endlessController.UpdateThreatLevel("Harder", false);

            foreach (Emitter emitter in _allBlocks)
                emitter.UpdateDifficulty(_loadedDifficulty);
        }
    }


    protected void SelectAttributeToModify()
    {
        int attribute = Random.Range(0, 4);

        float change = 0;
        float attributeChange;
        switch (attribute)
        {
            case 0:
                attributeChange = Random.Range(0.05f, 0.15f);
                change += attributeChange * 1.5f;

                _loadedDifficulty.ActivationTime = Mathf.Clamp(_loadedDifficulty.ActivationTime + attributeChange, 0.5f, 2.5f);


                attributeChange = Random.Range(0.05f, 0.1f);
                change += attributeChange * 1.75f;

                _loadedDifficulty.LoadTime = Mathf.Clamp(_loadedDifficulty.LoadTime + attributeChange, 0.5f, 1.5f);


                attributeChange = Random.Range(0, 10) > 6 ? 1 : 0;
                change += attributeChange * 15;

                _loadedDifficulty.ActivationCount = Mathf.Clamp(_loadedDifficulty.ActivationCount + Mathf.FloorToInt(attributeChange), 1, 4);
                break;

            case 1:
                attributeChange = Random.Range(0.02f, 0.05f);
                change += attributeChange * 2.5f;

                _loadedDifficulty.ReactionTime = Mathf.Clamp(_loadedDifficulty.ReactionTime + attributeChange, 0.15f, 0.8f);


                attributeChange = Random.Range(0.1f, 0.25f);
                change += attributeChange * 2.75f;
                _loadedDifficulty.DisplayTime = Mathf.Clamp(_loadedDifficulty.DisplayTime + attributeChange, 0.1f, 2.5f);


                attributeChange = Random.Range(0, 10) > 5 ? 1 : 0;
                change += attributeChange * 20;
                _loadedDifficulty.NumberOfShots = Mathf.Clamp(_loadedDifficulty.NumberOfShots + Mathf.FloorToInt(attributeChange), 1, 6);
                break;

            case 2:
                attributeChange = Random.Range(0.5f, 2);
                change += attributeChange;

                _loadedDifficulty.MinDispersion = Mathf.Clamp(_loadedDifficulty.MinDispersion + attributeChange, 0, 45);
                _loadedDifficulty.MaxDispersion = Mathf.Clamp(_loadedDifficulty.MaxDispersion + attributeChange, 0, 45);
                break;

            case 3:
                attributeChange = Random.Range(0.02f, 0.1f);
                change += attributeChange * 1.5f;

                _loadedDifficulty.Speed = Mathf.Clamp(_loadedDifficulty.Speed + attributeChange, 0, 0.35f);


                attributeChange = Random.Range(0.02f, 0.1f);
                change += attributeChange;
                _loadedDifficulty.TimeBeforeDirectionChange = Mathf.Clamp(_loadedDifficulty.TimeBeforeDirectionChange + attributeChange, 0, 0.5f);
                break;

            case 4:
                attributeChange = Random.Range(0.5f, 1f);
                change += attributeChange * 2.5f;

                _loadedDifficulty.RotationSpeed = Mathf.Clamp(_loadedDifficulty.RotationSpeed + attributeChange, 0.5f, 15f);


                attributeChange = Random.Range(0.5f, 0.75f);
                change += attributeChange;

                _loadedDifficulty.TimeBeforeRotationChange = Mathf.Clamp(_loadedDifficulty.TimeBeforeRotationChange + attributeChange, 0, 5);


                attributeChange = Random.Range(0.5f, 5);
                change += attributeChange * 1.5f;
                _loadedDifficulty.MinusAngle = Mathf.Clamp(_loadedDifficulty.MinusAngle + attributeChange, -85, 0);
                _loadedDifficulty.PositiveAngle = Mathf.Clamp(_loadedDifficulty.PositiveAngle + 25, -60, 10);

                attributeChange = Random.Range(0.5f, 0.75f);
                change += attributeChange * 2.5f;

                _loadedDifficulty.BlockRotationSpeed = Mathf.Clamp(_loadedDifficulty.BlockRotationSpeed + attributeChange, 0.5f, 5);
                break;
        }

        _endlessController.UpdateFactor(change);
    }
}