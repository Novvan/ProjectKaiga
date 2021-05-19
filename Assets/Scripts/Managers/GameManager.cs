using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    private int _score;
    private GameManager _instance;
    [SerializeField] private PlayerController _pc;
    public bool bossDead = false;
    public GameObject victoryCanvas;
    public GameObject hud;
    #endregion
    public int Score => _score;

    #region MonoBehaviour callbacks
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("Theme");
    }
    private void LateUpdate()
    { }
    #endregion

    #region Custom callbacks
    public void KilledEnemy()
    {
        _score++;
    }
    #endregion
}
