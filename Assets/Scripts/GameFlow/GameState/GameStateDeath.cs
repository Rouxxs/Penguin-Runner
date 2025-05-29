using GameFlow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateDeath : GameState
{
    public GameObject deathUI;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI fishTotal;
    [SerializeField] private TextMeshProUGUI currentFish;
    
    [SerializeField] private Image completionCircle;
    public float countdown = 2.5f;
    private float _deathTime;
    public override void Construct()
    {
        if (SaveManager.Instace.save.HighScore < (int) GameStats.Instance.score)
        {
            SaveManager.Instace.save.HighScore = (int) GameStats.Instance.score;
            currentScore.color = Color.green;
        }
        else
        {
            currentScore.color = Color.white;
        }
        
        SaveManager.Instace.save.Fish += GameStats.Instance.fishCollectedThisSession;
        SaveManager.Instace.Save();
        
        // base.Construct();
        GameManager.Instance.playerManager.PausePlayer();
        deathUI.SetActive(true);
        highScore.text = "Highscore: " + SaveManager.Instace.save.HighScore;
        currentScore.text = GameStats.Instance.ScoreToString();
        fishTotal.text = "Total Fish: " + SaveManager.Instace.save.Fish;
        currentFish.text = GameStats.Instance.FishToString();
        _deathTime = Time.time;
        completionCircle.gameObject.SetActive(true);
    }
    
    public override void Destruct()
    {
        deathUI.SetActive(false);
    }
    public override void UpdateState()
    {
        float ration = (Time.time - _deathTime) / countdown;
        completionCircle.color = Color.Lerp(Color.green, Color.red, ration);
        completionCircle.fillAmount = 1 - ration;
        if (ration > 1)
        {
            completionCircle.gameObject.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        GameManager.ChangeState(GameManager.GameStatePlay);
        GameManager.Instance.playerManager.RespawnPlayer();
    }

    public void ToMenu()
    {
        GameManager.ChangeState(GameManager.GameStateInit);
        
        GameManager.Instance.playerManager.ResetPlayer();
        GameManager.Instance.worldGeneration.ResetWorld();
        GameManager.Instance.sceneChunkGeneration.ResetWorld();
    }
}
