using System;

[Serializable]
public class LevelSave
{
    public float Time;

    public bool Locked;



    public LevelSave(bool locked)
    {
        Time = 0;
        Locked = locked;
    }
}