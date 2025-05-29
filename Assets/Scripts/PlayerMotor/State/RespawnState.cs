using GameFlow;
using UnityEngine;

public class RespawnState : BaseState
{
    [SerializeField] private float verticalDistance = 25.0f;
    [SerializeField] private float immunityTime = 1.0f;
    private float startTime;
    public override void Construct()
    {
        startTime = Time.time;
        
        Manager.controller.enabled = false;
        Manager.transform.position = new Vector3(0, verticalDistance, Manager.transform.position.z);
        Manager.controller.enabled = true;
        
        Manager.animator?.SetTrigger("Respawn");
        Manager.verticalVelocity = 0;
        Manager.currentLane = 0;
    }

    public override void Destruct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Play);
    }

    public override Vector3 ProcessMotion()
    {
        Manager.ApplyPhysics();
        Vector3 moveVector = Vector3.zero;
        moveVector.x = Manager.SnapToLane();
        moveVector.y = Manager.verticalVelocity;
        moveVector.z = Manager.baseRunSpeed;

        return moveVector;
    }

    public override void Transition()
    {
        if (Manager.isGrounded && Time.time - startTime > immunityTime)
        {
            Manager.ChangeState(Manager.RunningState);
        }

        if (InputManager.Instance.SwipeLeft)
        {
            Manager.ChangeLane(-1);
        }
        if (InputManager.Instance.SwipeRight)
        {
            Manager.ChangeLane(1);
        }
    }
    
}
