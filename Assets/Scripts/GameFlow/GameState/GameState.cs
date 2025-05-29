using GameFlow;
using UnityEngine;

public class GameState : MonoBehaviour
{
    protected GameManager GameManager;

    protected virtual void Awake()
    {
        GameManager = GetComponent<GameManager>();
    }

    public virtual void Construct()
    {
       // Debug.Log("Constructing: " + this.ToString());
    }
    
    public virtual void Destruct()
    {
        
    }

    public virtual void UpdateState()
    {
        
    }
}
