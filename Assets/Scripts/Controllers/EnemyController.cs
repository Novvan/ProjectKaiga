using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IController
{
    #region Variables
    public Enemy enemyData;
    private float _currentHealth;
    private NavMeshAgent _agent;
    public bool alreadyAttacked;
    public bool walkPointSet;
    private Vector3 _newWalkPoint;
    private Transform _player;
    #endregion

    #region MonoBehaviour callbacks
    public virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _currentHealth = enemyData.maxhealth;
        GetComponent<ExplosiveScript>().explosionRadius = enemyData.attackRange;
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

    private void OnDestroy()
    {
        FindObjectOfType<GameManager>().KilledEnemy();
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
    }
    private void resetAttack()
    {
        alreadyAttacked = false;
    }
    public void takeDamage(float damage)
    {
        _currentHealth -= damage;
    }

    public virtual void destroyCharacter()
    {
        Destroy(gameObject);
    }

    private void _searchWalkPoint()
    {
        float randomZ = Random.Range(-enemyData.walkPointRange, enemyData.walkPointRange);
        float randomX = Random.Range(-enemyData.walkPointRange, enemyData.walkPointRange);

        _newWalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_newWalkPoint, -transform.up, enemyData.walkPointRange, enemyData.groundMask)) walkPointSet = true;
    }
    #endregion
}
