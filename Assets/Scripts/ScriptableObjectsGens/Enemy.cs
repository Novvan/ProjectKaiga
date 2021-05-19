using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Scriptable Objects/Characters/Enemy")]
public class Enemy : Character
{
    public LayerMask groundMask, playerMask;
    public Vector3 walkPoint;
    public float sightRange, attackRange, damage;
    public bool playerInSightRange, playerInAttackRange;
    public float walkPointRange;
    public float timeBetweenAttacks;
}
