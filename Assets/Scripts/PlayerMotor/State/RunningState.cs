using UnityEngine;

public class RunningState : BaseState
{
   public override void Construct()
   {
      Manager.verticalVelocity = 0;
   }

   public override Vector3 ProcessMotion()
   {
      Vector3 moveVector = Vector3.zero;
      moveVector.x = Manager.SnapToLane();
      moveVector.y = -1f;
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
      
      if (InputManager.Instance.SwipeUp && Manager.isGrounded)
      {
         // Change to jumping state
         Manager.ChangeState(Manager.JumpingState);
      }

      if (InputManager.Instance.SwipeDown)
      {
         Manager.ChangeState(Manager.SlidingState);
      }
      
      if (Manager.isGrounded == false)
      {
         // Change to falling state
         Manager.ChangeState(Manager.FallingState);
      }
   }
}
