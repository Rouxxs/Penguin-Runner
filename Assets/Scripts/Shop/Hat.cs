using UnityEngine;

[CreateAssetMenu(fileName = "Hat")]
public class Hat : ScriptableObject
{
    public string ItemName;
    public int Price;
    public Sprite Image;
    public GameObject Model;
}
