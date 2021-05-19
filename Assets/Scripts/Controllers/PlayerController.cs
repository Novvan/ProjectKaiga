using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(PlayerHandler))]
public class PlayerController : MonoBehaviour, IController
{
    #region Variables
    [Header("Scripts")]
    [SerializeField] private Player _player;
    public event Action<float> OnHealthPrcChange = delegate { };
    private float _currentHealth;
    private float _currentHPrc;
    private Rigidbody _rb;
    private InputHandler _input;
    private PlayerHandler _ph;
    private WeaponHandler _wpH;

    [Header("Movement")]
    private float _baseSpeed = 1f;
    private float _hAxis;
    private float _vAxis;
    private Vector3 _moveDirection;
    private Vector3 _slopeMoveDirection;

    [Header("Camera")]
    [SerializeField] private GameObject _camPrefab;
    [SerializeField] private Transform _cameraDeadLocation;
    [SerializeField] private Transform _camHolder;
    [SerializeField] private Transform _weapon;
    private float _mouseY;
    private float _mouseX;
    private float _rotationY;
    private float _rotationX;

    [Header("Ground Detection")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundMask;
    private bool _isGrounded;
    private float _groundDistance = 0.4f;
    private RaycastHit _slopeHit;
    private bool _isAlive;
    public GameObject victoryCanvas;
    public GameObject loseCanvas;
    public GameObject hud;
    [SerializeField] private GameManager _gm;
    private bool _newCamAlive;
    #endregion
    public bool IsAlive => _isAlive;

    #region MonoBehaviour callbacks
    private void Awake()
    {
        _currentHealth = _player.maxhealth;
    }

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        // _wallRun = this.GetComponent<WallRun>();
        _input = this.GetComponent<InputHandler>();
        _ph = this.GetComponent<PlayerHandler>();
        _wpH = this.GetComponent<WeaponHandler>();

        _rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if (_isAlive && !_gm.bossDead)
        {
            _inputHandler();
            _wpH.checkForInputs();
            _ph.controlDrag(_isGrounded, _player.groundDrag, _player.airDrag);
            _controlSpeed();
        }
        if (_gm.bossDead)
        {
            _playerVictory();
        }
    }

    private void FixedUpdate()
    {
        if (_currentHealth <= 0)
        {
            _isAlive = false;
            if (!_newCamAlive)
            {
                Instantiate(_camPrefab, _cameraDeadLocation.position, Quaternion.identity);
                _newCamAlive = true;
            }
            _playerDead();
            Destroy(gameObject);
        }
        else
        {
            _isAlive = true;
            Move();
        }
    }

    #endregion

    #region Interface callbacks

    public void Move()
    {
        _moveDirection = _ph.getMoveDirection(_orientation, _vAxis, _hAxis);
        _slopeMoveDirection = _ph.getSlopeMoveDirection(_orientation, _vAxis, _hAxis, _slopeHit);


        if (_isGrounded && !_slopeHandling()) _ph.move(_moveDirection, _baseSpeed);
        else if (_isGrounded && _slopeHandling()) _ph.move(_slopeMoveDirection, _baseSpeed);
        else _ph.move(_moveDirection, _baseSpeed * _player.airMoveMultiplier);
    }
    #endregion

    #region Custom callbacks
    private void _inputHandler()
    {
        // Keyboard Inputs
        _hAxis = Input.GetAxisRaw("Horizontal");
        _vAxis = Input.GetAxisRaw("Vertical");

        //Mouse Inputs
        _mouseX = Input.GetAxisRaw("Mouse X");
        _mouseY = Input.GetAxisRaw("Mouse Y");

        //Add mouse rotation
        _rotationY += _ph.addMouseRotation(_mouseX);
        _rotationX -= _ph.addMouseRotation(_mouseY);

        _rotationX = Mathf.Clamp(_rotationX, -75f, 75f);

        _camHolder.transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
        _orientation.rotation = Quaternion.Euler(0, _rotationY, 0);

        _weapon.transform.rotation = _camHolder.transform.rotation;

        if (Input.GetKeyDown(_input.jumpKey))
        {
            if (_isGrounded)
            {
                _player.currentJumps = 0;
                _jump();
            }
            else if (_player.currentJumps < _player.maxJumps)
            {
                _jump();
            }
        }
    }

    private bool _slopeHandling()
    {
        if (_ph.checkSlope(_player.playerHeight, out _slopeHit))
        {
            if (_slopeHit.normal != Vector3.up) return true;
            else return false;
        }
        else return false;
    }

    private void _controlSpeed()
    {
        if (Input.GetKey(_input.sprintKey) && _isGrounded) _baseSpeed = _ph.calculateSpeed(_baseSpeed, _player.sprintSpeed, _player.acceleration);
        else _baseSpeed = _ph.calculateSpeed(_baseSpeed, _player.walkSpeed, _player.acceleration);
    }

    private void _jump()
    {
        if (_player.currentJumps == 0) _ph.jump(_player.jumpForce);
        else _ph.jump(_player.jumpForce / 2);

        _player.currentJumps += 1;
    }

    public void takeDamage(float damage)
    {
        _currentHealth = _ph.modifyHealth(-damage, _currentHealth);
        _currentHPrc = _currentHealth / _player.maxhealth;


        OnHealthPrcChange(_currentHPrc);
    }

    private void _playerDead()
    {
        loseCanvas.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void _playerVictory()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        hud.SetActive(false);
        victoryCanvas.SetActive(true);
    }
    #endregion
}
