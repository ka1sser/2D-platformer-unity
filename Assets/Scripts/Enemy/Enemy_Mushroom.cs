using UnityEngine;

public class Enemy_Mushroom : Enemy
{

    protected override void Update()
    {

        base.Update();

        anim.SetFloat("xVelocity", rb.linearVelocityX);

        HandleMovement();
        HandleCollision();

        if (isGrounded)
            HandleTurnAround();
    }

    private void HandleTurnAround()
    {
        if (!isGroundInfrontDetected || isWallDetected)
        {
            if (isGrounded == false)
                return;
            Flip();
            idleTimer = idleDuration;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void HandleMovement()
    {
        if (idleTimer > 0)
            return;

        if (isGroundInfrontDetected)
            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);
    }
}
