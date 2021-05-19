using EZCameraShake;
using UnityEngine;

public class TposerController : EnemyController, IController
{
    #region Variables
    [SerializeField] private float _pushForce;
    [SerializeField] private bool _isboss;
    private GameManager _gm;
    #endregion

    #region MonoBehaviour callbacks
    public override void Start()
    {
        base.Start();
        _gm = FindObjectOfType<GameManager>();
    }

    public override void Update()
    {
        base.Update();

        if (currentHealth > 0)
        {
            if (!enemyData.playerInSightRange && !enemyData.playerInAttackRange) Move();
            else if (enemyData.playerInSightRange && !enemyData.playerInAttackRange) ChasePlayer();
            else if (enemyData.playerInSightRange && enemyData.playerInAttackRange) AttackPlayer();
        }

    }
    #endregion

    #region Overrides && custom callbacks

    public override void destroyCharacter()
    {

        if (_gm != null && _isboss)
        {
            _gm.bossDead = true;
        }
        base.destroyCharacter();
    }



    public override void AttackPlayer()
    {
        base.AttackPlayer();
        if (!alreadyAttacked)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemyData.attackRange, enemyData.playerMask);

            foreach (var collider in hitColliders)
            {
                if (collider.transform.root.GetComponent<Rigidbody>() != null)
                {
                    if (collider.tag == "Player")
                    {
                        collider.transform.root.GetComponent<Rigidbody>().AddForce((collider.transform.root.position - this.transform.position) * _pushForce, ForceMode.Impulse);
                        CameraShaker.Instance.ShakeOnce(2.5f, 2.5f, 0.1f, 1f);
                        collider.transform.root.GetComponent<PlayerController>().takeDamage(enemyData.damage);
                        alreadyAttacked = true;
                    }
                }
            }
        }
    }

    #endregion
}
