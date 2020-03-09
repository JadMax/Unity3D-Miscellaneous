using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreference : MonoBehaviour
{
    public int hearts = 3;
    public float health = 100.0f;
    public float armorShield = 100.0f;
    public float armorHelmet = 50.0f;

    public float walkSpeed = 5.0f;
    public float waterSpeed = 1.0f;
    public float rollSpeed = 10.0f;
    public float rollDuration = 0.5f;
    public float gravity = 20.0f;
    public bool isRolling = false;
    public bool isUnderWater = false;
    public bool isAttacking = false;
    private Vector3 movement = Vector3.zero;
    public Transform cameraPivotTrans;

    public Animator playerAnimator;

    public FromVirtualStick_PlayerModule playerJoystickModule;
    private Vector2 offset;

    private void Update()
    {
        PlayerControl();
    }

    private void PlayerControl()
    {
        CharacterController controller = GetComponent<CharacterController>();
        float speed = isUnderWater ? waterSpeed : (isRolling ? rollSpeed : walkSpeed);
        offset = playerJoystickModule.offset;
        Roll();
        Water();
        Attack();
        if (controller.isGrounded)
        {
            if (!isRolling)
                movement = new Vector3(offset.x, 0, offset.y).normalized;
            else
            {
                movement = transform.forward;
                playerAnimator.Play("roll");
            }
            movement *= speed;
            movement = cameraPivotTrans.TransformDirection(movement);
        }
        
        movement.y -= gravity * Time.deltaTime;
        controller.Move(movement * Time.deltaTime);

        if (Vector3.Scale(movement, new Vector3(1, 0, 1)).magnitude > 0 && !isRolling && !isAttacking)
        {
            transform.LookAt(transform.position + Vector3.Scale(movement, new Vector3(1, 0, 1)).normalized);
            playerAnimator.Play("walk");
        }
        if (Vector3.Scale(movement, new Vector3(1, 0, 1)).magnitude <= 0 && !isRolling)
        {
            playerAnimator.Play("rest");
        }
    }

    private float _rollDur = 0.0f;
    public void SetRoll()
    {
        if (!isUnderWater)
        {
            isRolling = true;
            _rollDur = Time.time + rollDuration;
        }
    }
    private void Roll()
    {
        if (isRolling && _rollDur <= Time.time)
        {
            isRolling = false;
        }
    }
    private void Water()
    {
        if (transform.position.y <= 0.5f)
        {
            isUnderWater = true;
        }
        else
        {
            isUnderWater = false;
        }
    }
    public void SetAttack()
    {
        isAttacking = true;
    }
    public void UnSetAttack()
    {
        isAttacking = false;
    }
    private void Attack()
    {
        // 优先朝向最近的敌人
    }
}
