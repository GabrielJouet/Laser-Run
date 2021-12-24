using System;

[Serializable]
public class LevelSave
{
    public float Time;

    public bool Locked;

    public bool Hard;

    public bool Win;



    public LevelSave(bool locked)
    {
        Time = 0;
        Locked = locked;
        Hard = false;
        Win = false;
    }
}