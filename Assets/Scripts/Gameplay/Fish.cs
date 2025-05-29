using UnityEngine;

public class Fish : MonoBehaviour
{
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickupFish();
        }
    }

    private void PickupFish()
    {
        _animator?.SetTrigger("Fish_Pickup");
        GameStats.Instance.CollectFish();
    }

    public void OnShowChunk()
    {
        _animator?.SetTrigger("Fish_Idle");
    }
}
