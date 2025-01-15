using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Spider : Enemies
{
    [SerializeField] float jumpForce = 9f;
    [SerializeField] bool isJumping = false;
    float timebetweenJumps = 0.5f;

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
        moveSpeed = 4f;
    }

    private void Awake()
    {
        state = State.Walking;
        timerGoal = Random.Range(minTimerOffset, maxTimerOffset);
    }

    protected override void Update()
    {
        /*timer += Time.deltaTime;

        if (timer >= timerGoal)
        {
            ChangeDirection();
            timer = 0;
            timerGoal = Random.Range(minTimerOffset, maxTimerOffset);
            if (Random.Range(0, 2) == 0)
            {
                direction = Direction.Left;
                timer = 0;
                timerGoal = Random.Range(minTimerOffset, maxTimerOffset);
            }
            else
            {
                direction = Direction.Right;
                timer = 0;
                timerGoal = Random.Range(minTimerOffset, maxTimerOffset);
            }
        }*/

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
        yield return new WaitForSeconds(timebetweenJumps);
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
            if (isJumping)
            {
                /*timer = 0;
                timerGoal = Random.Range(minTimerOffset, maxTimerOffset);*/
                //Debug.Log("Before: " + transform.eulerAngles);

                Vector3 lastForwardDirection = transform.forward;

                Vector3 newPos = collision.GetContact(0).point;
                newPos.z = 0;
                //transform.position = newPos;

                float currentYRotation = transform.rotation.eulerAngles.y;

                //currentWall = collision.gameObject.GetComponent<Wall>();
                currentWall = CheckContactsForCurrentWall(collision.contacts);
                gravityDirection = currentWall.GravityDirection;
                isJumping = false;
                constant.force = gravityDirection;
                //transform.localRotation = Quaternion.Euler(Vector3.SignedAngle(transform.up, currentWall.GravityDirection.normalized * -1, Vector3.right), transform.localRotation.y, 0);
                float signedAngle = Vector3.SignedAngle(transform.up, currentWall.GravityDirection.normalized * -1, Vector3.forward);
                Debug.Log(signedAngle);
                //transform.Rotate(signedAngle, 0f, 0f, Space.Self);

                if (signedAngle > 0)
                {
                    Quaternion rot = Quaternion.AngleAxis(signedAngle, Vector3.forward) * transform.rotation;
                    //Debug.Log(rot.eulerAngles);
                    //Vector3 rot = new Vector3(signedAngle, transform.rotation.eulerAngles.y, 0);
                    transform.rotation = rot;

                    float dot = Vector3.Dot(lastForwardDirection, transform.forward);


                    if (dot <= -0.999f)
                    {
                        ChangeDirection();
                    }

                    transform.position = newPos;
                    
                    //Debug.Log("After: " + transform.eulerAngles);
                }

                

                /*if (movingDirection == "Left")
                {
                    GetLeftMovementDirection();
                }
                else
                {
                    GetRightMovementDirection();
                }*/
            }
            else
            {
                if (collision.gameObject.GetComponent<Wall>() != currentWall)
                {
                    //timer = 0;
                    //timerGoal = Random.Range(minTimerOffset, maxTimerOffset);

                    ChangeDirection();
                    /*if (direction == Direction.Left)
                    {
                        direction = Direction.Right;
                    }
                    else
                    {
                        direction = Direction.Left;
                    }*/
                }
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ramp") && canChangeWalls)
        {
            timer = 0;
            timerGoal = Random.Range(minTimerOffset, maxTimerOffset);
            currentWall = collision.gameObject.GetComponent<Ramp>().GetAdjacentWall(currentWall);
            gravityDirection = currentWall.GravityDirection;
            constant.force = gravityDirection;
            canChangeWalls = false;

            StartCoroutine(StartWallChangeTimer());

            /*if (movingDirection == "Left")
            {
                GetLeftMovementDirection();
            }
            else
            {
                GetRightMovementDirection();
            }*/
        }
    }

    IEnumerator StartWallChangeTimer()
    {
        yield return new WaitForSeconds(2f);
        canChangeWalls = true;
    }

    Wall CheckContactsForCurrentWall(ContactPoint[] contacts)
    {
        Wall contactWall = null;

        foreach (ContactPoint contact in contacts)
        {
            if (contact.otherCollider.gameObject.GetComponent<Wall>() != currentWall)
            {
                contactWall = contact.otherCollider.gameObject.GetComponent<Wall>();
                return contactWall;
            }
        }
        return currentWall;
    }
}
