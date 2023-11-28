using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    [Header("移動の速さ"), SerializeField]
    private float _speed = 3;

    [Header("ジャンプする瞬間の速さ"), SerializeField]
    private float _jumpSpeed = 7;

    [Header("重力加速度"), SerializeField]
    private float _gravity = 15;

    [Header("落下時の速さ制限（Infinityで無制限）"), SerializeField]
    private float _fallSpeed = 10;

    [Header("落下の初速"), SerializeField]
    private float _initFallSpeed = 2;

    private Transform _transform;
    private CharacterController _characterController;

    private Vector2 _inputMove;
    private float _verticalVelocity;
    private float _turnVelocity;
    private bool _isGroundedPrev;
    private bool isRunning = false;
    private bool isJumping = false;
    public Transform playerCamera;
    Vector3 rotatedMovement;
    public float rotationSpeed = 15f;
    float mDesiredRotation = 0f;
    public Animator anim;

    PauseManager PauseManager;
    public void OnMove(InputAction.CallbackContext context)
    {
        _inputMove = context.ReadValue<Vector2>();
        
        if (context.performed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;  
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || !_characterController.isGrounded) return;

        _verticalVelocity = _jumpSpeed;
    }

    private void Awake()
    {
        PauseManager = GameObject.Find("Manager").GetComponent<PauseManager>();
        Cursor.visible = false; 
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(PauseManager.isPaused)
        {
            return;
        }

        var isGrounded = _characterController.isGrounded;

        if (isGrounded && !_isGroundedPrev)
        {
            _verticalVelocity = -_initFallSpeed;
            isJumping = false;

        }
        else if (!isGrounded)
        {
            isJumping = true;

            _verticalVelocity -= _gravity * Time.deltaTime;


            if (_verticalVelocity < -_fallSpeed)
                _verticalVelocity = -_fallSpeed;
        }

        HandleAnimation();

        _isGroundedPrev = isGrounded;

        var moveVelocity = new Vector3(
            _inputMove.x * _speed,
            _verticalVelocity,
            _inputMove.y * _speed
        );

        var moveDelta = moveVelocity * Time.deltaTime;

        rotatedMovement = Quaternion.Euler(0, playerCamera.transform.rotation.eulerAngles.y, 0) * moveDelta;
        _characterController.Move(rotatedMovement);



        if (_inputMove != Vector2.zero)
        {
            var targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x)
                * Mathf.Rad2Deg + 90;

            var angleY = Mathf.SmoothDampAngle(
                _transform.eulerAngles.y,
                targetAngleY,
                ref _turnVelocity,
                0.1f
            );

            if (rotatedMovement.magnitude > 0)
            {
                mDesiredRotation = Mathf.Atan2(rotatedMovement.x, rotatedMovement.z) * Mathf.Rad2Deg;
            }

            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0, mDesiredRotation, 0);
            _transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        



    }

    void HandleAnimation()
    {
        if (isRunning)
        {
            anim.SetBool("run", true);
            _speed = 10;
        }
        else
        {
            anim.SetBool("run", false);
            _speed = 7;
        }

        if (isJumping)
        {
            anim.SetBool("jump", true);
        }
        else
        {
            anim.SetBool("jump", false);
        }
    }
}

