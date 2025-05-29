using GameFlow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateShop : GameState
{
    public GameObject shopUI;
    public TextMeshProUGUI totalFish;
    public TextMeshProUGUI currentHat;
    
    //Hats
    public GameObject hatPrefab;
    public Transform hatContainer;
    private Hat[] hats;
    
    public HatLogic hatLogic;
    private bool initialize = false;
    private int _countHat;
    private int _unlockedHatCount;
    
    // Completion circle
    public Image completionCircle;
    public TextMeshProUGUI completionText;

    private void ResetCompletionCircle()
    {
        _countHat = hats.Length - 1;
        int currUnlocked = _unlockedHatCount - 1;
        completionCircle.fillAmount = (float)currUnlocked / (float)_countHat;
        completionText.text = currUnlocked + "/" + _countHat;
    }
    private void PopulateShop()
    {
        for (int i = 0; i < hats.Length; i++)
        {
            int index = i;
            GameObject hat = Instantiate(hatPrefab, hatContainer);
            
            hat.GetComponent<Button>().onClick.AddListener(() => OnHatClicked(index));
            //Name
            hat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = hats[index].ItemName;
            //Imgae
            hat.transform.GetChild(1).GetComponent<Image>().sprite = hats[index].Image;
            //Price
            if (SaveManager.Instace.save.UnlockedHatFlag[index] == 0)
            {
                hat.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = hats[index].Price.ToString();
            }
            else
            {
                hat.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                _unlockedHatCount++;
            }
        }
    }

    private void OnHatClicked(int i)
    {
        //Debug.Log("Hat clicked: " + i);
        if (SaveManager.Instace.save.UnlockedHatFlag[i] == 1)
        {
            SaveManager.Instace.save.CurrentHat = i;
            currentHat.text = hats[i].ItemName;
            hatLogic.SelectHat(i);
            SaveManager.Instace.Save();
        }
        else if (hats[i].Price <= SaveManager.Instace.save.Fish)
        {
            SaveManager.Instace.save.Fish -= hats[i].Price;
            SaveManager.Instace.save.UnlockedHatFlag[i] = 1;
            SaveManager.Instace.save.CurrentHat = i;
            currentHat.text = hats[i].ItemName;
            totalFish.text = SaveManager.Instace.save.Fish.ToString();
            hatLogic.SelectHat(i);
            SaveManager.Instace.Save();
            hatContainer.GetChild(i).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
            _unlockedHatCount++;
            ResetCompletionCircle();
        }
        else
        {
            Debug.Log("Not enough fish");
        }
    }
    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Shop);
        totalFish.text = SaveManager.Instace.save.Fish.ToString();
        shopUI.SetActive(true);
        if (initialize == false)
        {
            hats = Resources.LoadAll<Hat>("Hats");
            Debug.Log("Hats loaded: " + hats.Length);
            PopulateShop();
            initialize = true;
        }

        ResetCompletionCircle();
    }

    public override void Destruct()
    {
        shopUI.SetActive(false);
    }
    
    public void OnHomeClick()
    {
        GameManager.ChangeState(GameManager.GameStateInit);
    }
}
