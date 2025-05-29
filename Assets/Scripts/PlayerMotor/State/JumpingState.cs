using UnityEngine;

public class JumpingState : BaseState
{
    public float jumpForce = 5.0f;
    public override void Construct()
    {
        Manager.animator?.SetTrigger("Jump");
        Manager.verticalVelocity = jumpForce;
    }

    public override void Transition()
    {
        if (InputManager.Instance.SwipeLeft)
        {
            // Change lane to the left
            Manager.ChangeLane(-1);
        }
      
        if (InputManager.Instance.SwipeRight)
        {
            // Change lane to the right
            Manager.ChangeLane(1);
        }
        if (Manager.verticalVelocity <= 0.0f)
        {
            Manager.ChangeState(Manager.FallingState);
        }
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
}
