using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IController
{
    #region Variables

    [SerializeField] private Enemy _enemyData;

    #endregion

    #region MonoBehaviour callbacks

    public virtual void Start()
    { }

    public virtual void Update()
    { }

    #endregion

    #region Interface callbacks

    public virtual void Move()
    { }

    #endregion
}
