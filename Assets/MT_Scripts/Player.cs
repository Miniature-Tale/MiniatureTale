using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float mMoveSpeed = 6.0f;
    [SerializeField]
    private float mJumpHeight = 2.0f;
    [SerializeField]
    private float mLastComboEndDelay = 0.5f;
    [SerializeField]
    private float mLastHitDelay = 0.2f;
    private MT_InputAsset mPlayerInput;
    private Action<InputAction.CallbackContext> mMoveAction;
    private Action<InputAction.CallbackContext> mCancelMoveAction;
    private Action<InputAction.CallbackContext> mJumpAction;
    private Action<InputAction.CallbackContext> mAttackAction;
    private float mMoveInputVector;
    private Vector2 mMoveVector;
    private Rigidbody2D mMyRb;
    private Animator mAnimator;
    private SpriteRenderer mPlayerSprite;
    private float mPlayerAnimSpeed;
    private bool flipPlayer;
    private int mComboLength = 3;
    private int mComboCounter = 0;
    private float mLastComboEndTime;
    private float mLastHitTime;
    private bool mCanInvokeEndCombo;

    private void Awake()
    {
        mPlayerInput = new MT_InputAsset();
        mMyRb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponentInChildren<Animator>();
        mPlayerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        mMoveAction = (context) =>
        {
            mMoveInputVector = context.ReadValue<float>();
            flipPlayer = mMoveInputVector < 0.0f ? true : false;
        };
        mCancelMoveAction = (context) =>
        {
            mMoveInputVector = 0.0f;
        };
        mJumpAction = (context) =>
        {
            Jump();
        };
        mAttackAction = (context) =>
        {
            Attack();
        };
        mPlayerInput.GamePlay.Run.performed += mMoveAction;
        mPlayerInput.GamePlay.Run.canceled += mCancelMoveAction;
        mPlayerInput.GamePlay.Jump.performed += mJumpAction;
        mPlayerInput.GamePlay.StandardAttack.performed += mAttackAction;
        mPlayerInput.GamePlay.Enable();
    }


    private void OnDisable()
    {
        mPlayerInput.GamePlay.Run.performed -= mMoveAction;
        mPlayerInput.GamePlay.Run.canceled -= mCancelMoveAction;
        mPlayerInput.GamePlay.Jump.performed -= mJumpAction;
        mPlayerInput.GamePlay.StandardAttack.performed -= mAttackAction;
        mPlayerInput.GamePlay.Disable();
    }

    private void Update()
    {
        mAnimator.SetBool("IsMoving", mMyRb.velocity.x != 0.0f);
        mPlayerSprite.flipX = flipPlayer;
        TryToExitAttack();
    }

    private void FixedUpdate()
    {
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

    private void Attack()
    {
        //check if can combo
        if (Time.time - mLastComboEndTime > mLastComboEndDelay && mComboCounter < mComboLength)
        {
            if (Time.time - mLastHitTime >= mLastHitDelay)
            {
                mAnimator.SetTrigger("Attack"+mComboCounter);
                //other Attack properties are set here
                mComboCounter++;
                mLastHitTime = Time.time;
                mCanInvokeEndCombo = true;
            }
        }
    }

    private void TryToExitAttack()
    {
        if (mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && mAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && mCanInvokeEndCombo)
        {
            EndCombo();
            mCanInvokeEndCombo = false;
        }
    }

    private void EndCombo()
    {
        mComboCounter = 0;
        mLastComboEndTime = Time.time;
    }

}
