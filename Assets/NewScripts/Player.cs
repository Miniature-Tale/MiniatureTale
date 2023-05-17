using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float mMoveSpeed = 6.0f;
    [SerializeField]
    private float mJumpHeight = 2.0f;
    private MT_InputAsset mPlayerInput;
    private Action<InputAction.CallbackContext> mMoveAction;
    private Action<InputAction.CallbackContext> mCancelMoveAction;
    private Action<InputAction.CallbackContext> mJumpAction;
    private float mMoveInputVector;
    private Vector2 mMoveVector;
    private Rigidbody2D mMyRb;

    private void Awake()
    {
        mPlayerInput = new MT_InputAsset();
        mMyRb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        mMoveAction = (context) =>
        {
            mMoveInputVector = context.ReadValue<float>();
        };
        mCancelMoveAction = (context) =>
        {
            mMoveInputVector = 0.0f;
        };
        mJumpAction = (context) =>
        {
            Jump();
        };
        mPlayerInput.GamePlay.Run.performed += mMoveAction;
        mPlayerInput.GamePlay.Run.canceled += mCancelMoveAction;
        mPlayerInput.GamePlay.Jump.performed += mJumpAction;
        mPlayerInput.GamePlay.Enable();
    }

    private void OnDisable()
    {
        mPlayerInput.GamePlay.Run.performed -= mMoveAction;
        mPlayerInput.GamePlay.Run.canceled -= mCancelMoveAction;
        mPlayerInput.GamePlay.Jump.performed -= mJumpAction;
        mPlayerInput.GamePlay.Disable();
    }

    private void FixedUpdate()
    {
        // mMyRb.velocity.x < mMoveSpeed || mMyRb.velocity.x  > -mMoveSpeed
        _handleHorizontalMovement();
    }

    private void _handleHorizontalMovement()
    {
        if (mMoveInputVector != 0.0f)
        {
            mMyRb.AddForce(transform.right * mMoveInputVector * mMoveSpeed * mMyRb.mass, ForceMode2D.Impulse);
        }

        if (mMoveInputVector == 0.0f)
        {
            mMyRb.AddForce(transform.right * -mMyRb.velocity.x * mMyRb.mass, ForceMode2D.Impulse);
        }

        if (mMyRb.velocity.x > mMoveSpeed)
        {
            mMyRb.AddForce(transform.right * -(mMyRb.velocity.x - mMoveSpeed) * mMyRb.mass, ForceMode2D.Impulse);
        }
        if (mMyRb.velocity.x < -mMoveSpeed)
        {
            mMyRb.AddForce(transform.right * -(mMyRb.velocity.x + mMoveSpeed) * mMyRb.mass, ForceMode2D.Impulse);
        }
    }

    private void _cancelVerticalVelocity()
    {
        mMyRb.AddForce(transform.up * -mMyRb.velocity.y * mMyRb.mass, ForceMode2D.Impulse);
    }

    private void Jump()
    {
        _cancelVerticalVelocity();
        mMyRb.AddForce(transform.up * mJumpHeight * mMyRb.mass, ForceMode2D.Impulse);
    }

}
