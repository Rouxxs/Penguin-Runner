using Audio;
using GameFlow;
using TMPro;
using UnityEngine;
public class GameStatePlay : GameState
{
    public GameObject gameUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI fishCountText;
    [SerializeField] private AudioClip gameMusic;
    public override void Construct()
    {
        GameManager.Instance.playerManager.ResumePlayer();
        GameManager.Instance.ChangeCamera(GameCamera.Play);

        GameStats.Instance.OnCollectFish += OnFishCollected;
        GameStats.Instance.OnScoreChange += OnScoreChange;
        gameUI.SetActive(true);
        
        AudioManager.Instance.PlayMusicWithFade(gameMusic, 0.5f);
    }
    
    private void OnFishCollected(int fishCount)
    {
        fishCountText.text = GameStats.Instance.FishToString();
    }
    
    private void OnScoreChange(float score)
    {
        scoreText.text = GameStats.Instance.ScoreToString();
    }
    public override void Destruct()
    {
        gameUI.SetActive(false);
        
        GameStats.Instance.OnCollectFish -= OnFishCollected;
        GameStats.Instance.OnScoreChange -= OnScoreChange;
    }

    public override void UpdateState()
    {
        GameManager.Instance.worldGeneration.ScanPosition();
        GameManager.Instance.sceneChunkGeneration.ScanPosition();
    }
    
    
}
