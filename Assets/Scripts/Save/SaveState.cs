using System;

[System.Serializable]
public class SaveState 
{   
    [NonSerialized] private const int HAT_COUNT = 21;
    public int HighScore { get; set; }
    public int Fish { get; set; }
    public DateTime LastSaveTime { get; set; }
    public int CurrentHat { get; set; }
    public byte[] UnlockedHatFlag { get; set; }
    public SaveState()
    {
        HighScore = 0;
        Fish = 10;
        LastSaveTime = DateTime.Now;
        CurrentHat = 0;
        UnlockedHatFlag = new byte[HAT_COUNT];
        UnlockedHatFlag[0] = 1;
    }
}
