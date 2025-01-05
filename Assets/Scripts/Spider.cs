using System.Collections;
using UnityEngine;

public class Spider : Enemies
{
    float jumpForce = 7f;
    bool isJumping = false;
    float minJumpTimerOffset = 2f;
    float maxJumpTimerOffset = 6f;

    enum State
    {
        Walking,
        Jumping,
        Dead
    }
    [SerializeField] State state;

    IEnumerator enumerator = null;

    protected override void Start()
    {
        base.Start();
        moveSpeed = 3f;
    }

    private void Awake()
    {
        state = State.Walking;
        timerGoal = Random.Range(minTimerOffset, maxTimerOffset);
    }

    protected override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timerGoal)
        {
            if (Random.Range(0, 2) == 0)
            {
                GetLeftMovementDirection();
                movingDirection = "Left";
                timer = 0;
                timerGoal = Random.Range(minTimerOffset, maxTimerOffset);
            }
            else
            {
                GetRightMovementDirection();
                movingDirection = "Right";
                timer = 0;
                timerGoal = Random.Range(minTimerOffset, maxTimerOffset);
            }
        }

        switch (state)
        {
            case State.Walking:

                Move();
                
                if (enumerator == null)
                {
                    PrepareJump();
                }

                break;

            case State.Jumping:

                Move();

                if (!isJumping)
                {
                    state = State.Walking;
                }

                break;

            case State.Dead:

                break;
        }
    }

    void PrepareJump()
    {
        enumerator = JumpTimer();
        StartCoroutine(enumerator);
    }

    IEnumerator JumpTimer()
    {        
        float jumpTimerOffset = Random.Range(minJumpTimerOffset, maxJumpTimerOffset);

        yield return new WaitForSeconds(jumpTimerOffset);
        Jump();
        enumerator = null;
    }
    
    void Jump()
    {
        rb.AddForce((gravityDirection * -1).normalized * jumpForce, ForceMode.Impulse);
        state = State.Jumping;
        isJumping = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            currentWall = collision.gameObject.GetComponent<Wall>();
            gravityDirection = currentWall.GravityDirection;
            constant.force = gravityDirection;
            isJumping = false;

            if (movingDirection == "Left")
            {
                GetLeftMovementDirection();
            }
            else
            {
                GetRightMovementDirection();
            }
        }
    }
}
