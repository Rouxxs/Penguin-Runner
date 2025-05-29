
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Material material;
    public float offsetSpeed = 0.25f;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.forward * player.transform.position.z;
        material.SetVector("snowOffset", new Vector2(0, -transform.position.z * offsetSpeed));
    }
}
