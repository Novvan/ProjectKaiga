using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private float _movementMultiplier = 10;
    private InputHandler _input;
    private Rigidbody _rb;

    private void Start()
    {
        _input = this.GetComponent<InputHandler>();
        _rb = this.GetComponent<Rigidbody>();
    }
    public float addMouseRotation(float mouseAxis)
    {
        float multiplier = 0.1f;
        return mouseAxis * multiplier * _input.sensitivityX;
    }
    public void controlDrag(bool isGrounded, float groundDrag, float airDrag)
    {
        if (isGrounded) _rb.drag = groundDrag;
        else _rb.drag = airDrag;
    }
    public bool checkSlope(float playerHeight, out RaycastHit _hit)
    {
        return Physics.Raycast(transform.position, Vector3.down, out _hit, playerHeight / 2 + 0.5f);
    }
    public float calculateSpeed(float baseSpeed, float finalSpeed, float acceleration)
    {
        return Mathf.Lerp(baseSpeed, finalSpeed, acceleration * Time.deltaTime);
    }
    public Vector3 getMoveDirection(Transform orientation, float vAxis, float hAxis)
    {
        return orientation.forward * vAxis + orientation.right * hAxis;
    }
    public Vector3 getSlopeMoveDirection(Transform orientation, float vAxis, float hAxis, RaycastHit slope)
    {
        return Vector3.ProjectOnPlane(getMoveDirection(orientation, vAxis, hAxis), slope.normal);
    }
    public void move(Vector3 direction, float speed)
    {
        _rb.AddForce(direction.normalized * speed * _movementMultiplier, ForceMode.Acceleration);
    }
    public void jump(float jumpForce)
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}
