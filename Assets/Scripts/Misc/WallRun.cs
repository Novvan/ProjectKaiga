using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputHandler))]
public class WallRun : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField] private Transform _orientation;

    [Header("Detection")]
    [SerializeField] private float _wallDistance = 0.6f;
    [SerializeField] private float _minimumJumpHeight = 1.5f;

    [Header("Wall Run")]
    [SerializeField] private float _wallRunGravity = 1f;
    [SerializeField] private float _wallJumpForce = 6f;

    [Header("Camera")]
    [SerializeField] private Camera _cam;
    [SerializeField] private float _camTilt;
    [SerializeField] private float _camTiltTime;
    private float _fov;
    [SerializeField] private float _wallRunFov;
    [SerializeField] private float _wallRunFovTime;

    public float tilt { get; private set; }

    private bool _wallRight = false;
    private bool _wallLeft = false;

    private RaycastHit _leftRaycastHit;
    private RaycastHit _rightRaycastHit;
    private Rigidbody _rb;
    private InputHandler _input;
    #endregion

    #region MonoBehaviour callbacks
    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        _input = this.GetComponent<InputHandler>();
        _fov = _cam.fieldOfView;
        _wallRunFov += _fov;
    }

    private void Update()
    {
        _checkWall();
        if (_canWallRun())
        {
            if (_wallLeft)
            {
                _startWallRun();
                Debug.Log("Wall Running on the left");
            }
            else if (_wallRight)
            {
                _startWallRun();
                Debug.Log("Wall Running on the right");
            }
            else
            {
                _stopWallRun();
            }
        }
        else
        {
            _stopWallRun();
        }
    }
    #endregion

    #region Custom callbacks
    private bool _canWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, _minimumJumpHeight);
    }

    private void _checkWall()
    {
        _wallLeft = Physics.Raycast(transform.position, -_orientation.right, out _leftRaycastHit, _wallDistance);
        _wallRight = Physics.Raycast(transform.position, _orientation.right, out _rightRaycastHit, _wallDistance);
    }

    private void _startWallRun()
    {
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _wallRunFov, _wallRunFovTime * Time.deltaTime);

        _rb.useGravity = false;
        _rb.AddForce(Vector3.down * _wallRunGravity, ForceMode.Force);

        if (_wallLeft) tilt = Mathf.Lerp(tilt, -_camTilt, _camTiltTime * Time.deltaTime);
        else if (_wallRight) tilt = Mathf.Lerp(tilt, _camTilt, _camTiltTime * Time.deltaTime);


        if (Input.GetKeyDown(_input.jumpKey))
        {
            if (_wallLeft)
            {
                Vector3 wallJumpDirection = transform.up + _leftRaycastHit.normal;
                _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
                _rb.AddForce(wallJumpDirection * _wallJumpForce * 100, ForceMode.Force);

            }
            if (_wallRight)
            {
                Vector3 wallJumpDirection = transform.up + _rightRaycastHit.normal;
                _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
                _rb.AddForce(wallJumpDirection * _wallJumpForce * 100, ForceMode.Force);

            }
        }
    }
    private void _stopWallRun()
    {
        tilt = Mathf.Lerp(tilt, 0, _camTiltTime * Time.deltaTime);
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _fov, _wallRunFovTime * Time.deltaTime);
        _rb.useGravity = true;
    }
    #endregion

}
