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
    private Action<InputAction.CallbackContext> mAttackAction;
    private float mMoveInputVector;
    private Vector2 mMoveVector;
    private Rigidbody2D mMyRb;
    private Animator mAnimator;
    private SpriteRenderer mPlayerSprite;
    private float mPlayerAnimSpeed;
    private bool flipPlayer;

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
       /*check if we are at the start of the combo
        if we haven't then it is the beginning of said combo
        if it's the beginning we want to play an attack animation.
        while the anim plays we wait to hit animation events that will call some logic to do a few things
        -first event will call to enable a damage component found on a weapon. 
        -It will also call a timer that will detect whether or not the player pressed the attack button again.
            -If they did we transition into the next attack
            -If they didn't then we wait to hit the cancel anim event on an attack to reset all parameters
            -We also make sure the player can't start another attack if the have missed the timing window
        -The last attack will automatically cancel and reset all parameters.  
        */
        print("Attack Detected");
    }

}
