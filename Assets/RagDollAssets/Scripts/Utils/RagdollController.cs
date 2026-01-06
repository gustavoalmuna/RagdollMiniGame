using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class RagdollController : MonoBehaviour
{
    [Header("--- References ---")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ConfigurableJoint _mainJoint;
    [SerializeField] private Animator _animator;

    [Header("--- Settings ---")]
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _rotationSmoothTime = 5f;
    [SerializeField] private float _jumpForce = 25f;
    [SerializeField] private LayerMask _groundLayer;

    // Animator Parameters:
    private int _speedAnimation = Animator.StringToHash("speed");

    private SyncPhysicsObject[] _syncPhysicsObject;

    private Vector2 direction;
    private bool _isJumping;

    private void Awake()
    {
        _syncPhysicsObject = GetComponentsInChildren<SyncPhysicsObject>();
    }

    private void FixedUpdate()
    {
        Rotate();
        UpdateJointsRotation();

        if (_rb != null)
        {
            Move();
            
            if( _isJumping)
            {
                Jump();
            }
        }
        else
        {
            Debug.LogWarning("Rigidbody isnt Asigned!...");
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if(ctx.performed && IsGrounded())
        {
            _isJumping = true;
        }
    }

    public void OnAttackLeft(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && IsGrounded())
        {
            _isJumping = false;
            // Attack code
        }
    }

    

    private void Move()
    {
        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        _rb.AddForce(moveDirection * _movementSpeed, ForceMode.Acceleration);
        _animator.SetFloat(_speedAnimation, moveDirection.magnitude);
    }

    private void Jump()
    {
        
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _isJumping = false;
    }

    private void Rotate()
    {
        if(direction != Vector2.zero)
        {
            Vector3 inputDirection = new Vector3(-direction.x, 0, direction.y).normalized;
            
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);

            _mainJoint.targetRotation = Quaternion.RotateTowards(_mainJoint.targetRotation, targetRotation, _rotationSmoothTime * Time.fixedDeltaTime);
        }
    }

    private void UpdateJointsRotation()
    {
        for (int i = 0; i < _syncPhysicsObject.Length; i++)
        {
            _syncPhysicsObject[i].UpdateJointFromAnimation();
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(_rb.position, Vector3.down, 1f, _groundLayer);
    }

    // agregar ataque:

}
