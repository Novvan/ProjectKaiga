using System.Collections;
using UnityEngine;

public class DroneController : EnemyController, IController
{
    #region Variables
    private bool _startedCorroutine;
    #endregion

    #region MonoBehaviour callbacks

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (!enemyData.playerInSightRange && !enemyData.playerInAttackRange) Move();
        else if (enemyData.playerInSightRange && !enemyData.playerInAttackRange) ChasePlayer();
        else if (enemyData.playerInSightRange && enemyData.playerInAttackRange) AttackPlayer();

    }

    #endregion

    #region Interface callbacks

    public override void Move()
    {
        if (_startedCorroutine)
        {
            StopCoroutine("startAttack");
            _startedCorroutine = false;
        }
        base.Move();
    }

    public override void ChasePlayer()
    {
        if (_startedCorroutine)
        {
            StopCoroutine("startAttack");
            _startedCorroutine = false;
        }
        base.ChasePlayer();
    }

    public override void AttackPlayer()
    {
        base.AttackPlayer();
        if (!_startedCorroutine)
        {
            StartCoroutine("startAttack");
            _startedCorroutine = true;
        }
    }

    IEnumerator startAttack()
    {
        yield return new WaitForSeconds(0.75f);
        this.GetComponent<ExplosiveScript>().explode();
    }

    public override void destroyCharacter()
    {
        StartCoroutine("startAttack");
    }

    #endregion
}
