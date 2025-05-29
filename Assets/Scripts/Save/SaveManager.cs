using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instace
    {
        get { return instace; }
    }
    private static SaveManager instace;
    
    private const string saveFileName = "New_Data.prss";
    public SaveState save;
    private BinaryFormatter formatter;
    
    // Action
    public event Action<SaveState> OnLoad;
    public event Action<SaveState> OnSave;

    private void Awake()
    {
        if (instace == null)
        {
            instace = this;
        }
        formatter = new BinaryFormatter();
        Load();
    }

    public void Load()
    {
        try
        {
            FileStream file = new FileStream(Application.persistentDataPath + saveFileName, FileMode.Open, FileAccess.Read);
            save = formatter.Deserialize(file) as SaveState;
            //Debug.Log(Application.persistentDataPath + saveFileName);
            file.Close();
            OnLoad?.Invoke(save);
            
        }
        catch
        {
            Debug.Log("Save file not found, creating new save file.");
            Save();
        }
    }

    public void Save()
    {
        // If save is null, create a new SaveState
        if (save == null)
        {
            save = new SaveState();
        }
        
        // Set the last save time to the current time
        save.LastSaveTime = DateTime.Now;
        
        FileStream file = new FileStream(Application.persistentDataPath+ saveFileName, FileMode.OpenOrCreate, FileAccess.Write);
        formatter.Serialize(file, save);
        //Debug.Log(file.ToString());
        file.Close();
        
        OnSave?.Invoke(save);
    }
}
