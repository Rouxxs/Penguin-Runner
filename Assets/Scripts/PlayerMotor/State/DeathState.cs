using GameFlow;
using UnityEngine;

public class DeathState : BaseState
{
    [SerializeField] private Vector3 knockbackForce = new Vector3(0, 4, -3);
    private Vector3 currentKockback;

    public override void Construct()
    {
        Manager.animator?.SetTrigger("Death");
        currentKockback = knockbackForce;
    }

    public override Vector3 ProcessMotion()
    {
        Vector3 m = currentKockback;

        currentKockback = new Vector3(
            0,
            currentKockback.y -= Manager.gravity * Time.deltaTime,
            currentKockback.z += 2.0f * Time.deltaTime);

        if (currentKockback.z > 0)
        {
            currentKockback.z = 0;
            GameManager.Instance.ChangeState(GameManager.Instance.GameStateDeath);
        }

        return currentKockback;
    }
}