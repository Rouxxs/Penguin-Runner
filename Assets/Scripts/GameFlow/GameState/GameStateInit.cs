using Audio;
using GameFlow;
using TMPro;
using UnityEngine;

public class GameStateInit : GameState
{
    public GameObject menuUI;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI fishCountText;
    [SerializeField] private AudioClip menuLoopMusic;

    // public GameStateInit(GameObject menuUI, TextMeshProUGUI highScoreText, TextMeshProUGUI fishCountText, AudioClip menuLoopMusic)
    // {
    //     this.menuUI = menuUI;
    //     this.highScoreText = highScoreText;
    //     this.fishCountText = fishCountText;
    //     this.menuLoopMusic = menuLoopMusic;
    // }
    public override void Construct()
    {
         GameManager.Instance.ChangeCamera(GameCamera.Init);

         highScoreText.text = "HIGHSCORE: " + SaveManager.Instace.save.HighScore;
         fishCountText.text = "FISH: " + SaveManager.Instace.save.Fish;
         menuUI.SetActive(true);
         AudioManager.Instance.PlayMusicWithFade(menuLoopMusic, 0.5f);
    }

    public override void Destruct()
    {
        menuUI.SetActive(false);
    }

    public void OnPlayClick()
    {
        GameManager.ChangeState(GameManager.GameStatePlay);
        GameStats.Instance.ResetSession();
    }

    public void OnShopClick()
    {
        //Debug.Log("Shop Button clicked");
        GameManager.ChangeState(GameManager.GameStateShop);
    }
}
