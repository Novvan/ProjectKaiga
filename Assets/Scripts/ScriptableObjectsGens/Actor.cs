using UnityEngine;

[CreateAssetMenu(fileName = "New Actor", menuName = "Scriptable Objects/Actor")]
public class Actor : ScriptableObject
{
    public bool destructible = true;
    public float maxhealth;

}
