using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IController
{
    #region Variables
    public Enemy enemyData;
    public event Action<float> OnHealthPrcChange = delegate { };
    private float _currentHPrc;
    private float _currentHealth;
    private NavMeshAgent _agent;
    [HideInInspector] public bool alreadyAttacked;
    [HideInInspector] public bool walkPointSet;
    private Vector3 _newWalkPoint;
    private Transform _player;
    [SerializeField] private float _attackCD;
    #endregion
    public float currentHealth => _currentHealth;

    #region MonoBehaviour callbacks
    public virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _currentHealth = enemyData.maxhealth;
        _player = FindObjectOfType<PlayerController>().gameObject.transform;
    }

    public virtual void Update()
    {
        enemyData.playerInSightRange = Physics.CheckSphere(transform.position, enemyData.sightRange, enemyData.playerMask);
        enemyData.playerInAttackRange = Physics.CheckSphere(transform.position, enemyData.attackRange, enemyData.playerMask);

        if (_currentHealth <= 0) destroyCharacter();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyData.sightRange);
    }
    #endregion

    #region Interface callbacks
    public virtual void Move()
    {
        if (!walkPointSet) _searchWalkPoint();
        if (walkPointSet) _agent.SetDestination(_newWalkPoint);
        Vector3 distanceToWalkPoint = transform.position - _newWalkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    public virtual void ChasePlayer()
    {
        _agent.SetDestination(_player.position);
    }
    public virtual void AttackPlayer()
    {
        _agent.SetDestination(transform.position);
        transform.LookAt(_player);
        Invoke(nameof(resetAttack), _attackCD);
    }
    private void resetAttack()
    {
        alreadyAttacked = false;
    }
    public void takeDamage(float damage)
    {
        _modifyHealth(-damage);
    }
    private void _modifyHealth(float mod)
    {
        _currentHealth += mod;
        _currentHPrc = _currentHealth / enemyData.maxhealth;

        OnHealthPrcChange(_currentHPrc);
    }

    public virtual void destroyCharacter()
    {
        FindObjectOfType<GameManager>().KilledEnemy();
        Destroy(gameObject, 0.1f);
    }

    private void _searchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-enemyData.walkPointRange, enemyData.walkPointRange);
        float randomX = UnityEngine.Random.Range(-enemyData.walkPointRange, enemyData.walkPointRange);

        _newWalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_newWalkPoint, -transform.up, enemyData.walkPointRange, enemyData.groundMask)) walkPointSet = true;
    }
    #endregion
}
