using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    private int score;
    private PlayerController _player;
    private GameManager _instance;
    private Camera _mainCamera;
    #endregion

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
        _player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    { }
    private void LateUpdate()
    { }
    #endregion

    #region Custom callbacks
    public void KilledEnemy()
    {
        score++;
    }
    #endregion
}
