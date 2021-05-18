using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Scriptable Objects/Characters/Player")]
public class Player : Character
{
    [HideInInspector] public float playerHeight = 2;

    [Header("Air Movement")]
    public float airMoveMultiplier = 0.2f;

    [Header("Drag")]
    public float groundDrag = 6f;
    public float airDrag = 2f;

    [Header("Sprinting")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 6f;
    public float acceleration = 10f;

    [Header("Jumping")]
    public int maxJumps = 2;
    public float jumpForce;
    [HideInInspector] public int currentJumps = 0;

}
