using UnityEngine;

public class SlidingState : BaseState
{
    public float slideDuration = 1.0f;

    // Collider logic
    private Vector3 initialCenter;
    private float initialSize;
    private float slideStart;

    public override void Construct()
    {
        Manager.animator?.SetTrigger("Slide");
        slideStart = Time.time;

        initialSize = Manager.controller.height;
        initialCenter = Manager.controller.center;

        Manager.controller.height = initialSize * 0.5f;
        Manager.controller.center = initialCenter * 0.5f;
    }

    public override void Destruct()
    {
        Manager.controller.height = initialSize;
        Manager.controller.center = initialCenter;
        Manager.animator?.SetTrigger("Running");
    }

    public override void Transition()
    {
        if (InputManager.Instance.SwipeLeft)
            Manager.ChangeLane(-1);

        if (InputManager.Instance.SwipeRight)
            Manager.ChangeLane(1);

        if (!Manager.isGrounded)
            Manager.ChangeState(Manager.FallingState);

        if (InputManager.Instance.SwipeUp)
            Manager.ChangeState(Manager.JumpingState);

        if (Time.time - slideStart > slideDuration)
            Manager.ChangeState(Manager.RunningState);
    }

    public override Vector3 ProcessMotion()
    {
        Vector3 m = Vector3.zero;

        m.x = Manager.SnapToLane();
        m.y = -1.0f;
        m.z = Manager.baseRunSpeed;

        return m;
    }

}
