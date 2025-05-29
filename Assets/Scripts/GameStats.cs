using System;
using Audio;
using GameFlow;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    private static GameStats instance;
    public static GameStats Instance { get { return instance; } }
    
    // Score
    public float score;
    public float highScore;
    public float distancecPoint = 1.5f;
    
    // Fish
    public int totalFish;
    public int fishCollectedThisSession;
    public float pointPerFish = 10.0f;
    
    // Internal Cooldown
    private float lastScoreUpdate;
    private float scoreUpdateDelta = 0.2f; // update score every 0.2 seconds
    
    // Action
    public Action<int> OnCollectFish;
    public Action<float> OnScoreChange;

    public AudioClip fishCollectSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        float s = GameManager.Instance.playerManager.transform.position.z * distancecPoint;
        s += fishCollectedThisSession * pointPerFish;
        
        if (s > score)
        {
            score = s;
            if (Time.time - lastScoreUpdate > scoreUpdateDelta)
            {
                lastScoreUpdate = Time.time;
                OnScoreChange?.Invoke(score);
            }
        }
    }

    public void CollectFish()
    {
        fishCollectedThisSession++;
        OnCollectFish?.Invoke(fishCollectedThisSession);
        AudioManager.Instance.PlaySfx(fishCollectSound, 0.5f, true);
    }

    public void ResetSession()
    {
        score = 0;
        fishCollectedThisSession = 0;
        
        OnScoreChange?.Invoke(score);
        OnCollectFish?.Invoke(fishCollectedThisSession);
    }

    public string ScoreToString()
    {
        return score.ToString("0000000");
    }

    public string FishToString()
    {
        return fishCollectedThisSession.ToString("000");
    }
}
