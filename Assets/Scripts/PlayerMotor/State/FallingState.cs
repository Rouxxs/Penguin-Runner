using UnityEngine;

public class FallingState : BaseState
{
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
        if (Manager.isGrounded)
        {
            Manager.ChangeState(Manager.RunningState);
        }
    }
}
