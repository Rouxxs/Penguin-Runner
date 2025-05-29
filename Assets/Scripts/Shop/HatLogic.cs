using System.Collections.Generic;
using UnityEngine;

public class HatLogic : MonoBehaviour
{
    [SerializeField] private Transform hatContainer;
    private List<GameObject> hatModels = new List<GameObject>();
    private Hat[] hats;

    private void Start()
    {
        hats = Resources.LoadAll<Hat>("Hats");
        SpawnHats();
        SelectHat(SaveManager.Instace.save.CurrentHat);
    }
    
    private void SpawnHats()
    {
        for (int i = 0; i < hats.Length; i++)
        {
            int index = i;
            hatModels.Add(Instantiate(hats[index].Model, hatContainer));
        }
    }

    public void DisableAllHats()
    {
        for (int i = 0; i < hats.Length; i++)
        {
            hatModels[i].SetActive(false);
        }
    }

    public void SelectHat(int index)
    {
        DisableAllHats();
        hatModels[index].SetActive(true);
    }
}
