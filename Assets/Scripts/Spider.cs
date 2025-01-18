using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        moveSpeed = 4f;
        state = State.Walking;
    }

    protected override void Update()
    {
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
                rb.Sleep();

                Vector3 lastForwardDirection = transform.forward;

                Vector3 newPos = collision.GetContact(0).point;
                newPos.z = 0;

                float currentYRotation = transform.rotation.eulerAngles.y;

                currentWall = CheckContactsForCurrentWall(collision.contacts);
                gravityDirection = currentWall.GravityDirection;
                isJumping = false;
                constant.force = gravityDirection;
                float signedAngle = Vector3.SignedAngle(transform.up, currentWall.GravityDirection.normalized * -1, Vector3.forward);

                if (signedAngle != 0)
                {
                    Quaternion rot = Quaternion.AngleAxis(signedAngle, Vector3.forward) * transform.rotation;
                    transform.rotation = rot;

                    float dot = Vector3.Dot(lastForwardDirection, transform.forward);

                    if (dot <= -0.999f)
                    {
                        ChangeDirection();
                    }

                    transform.position = newPos;
                }
            }
            else
            {
                if (collision.gameObject.GetComponent<Wall>() != currentWall)
                {
                    ChangeDirection();
                }
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ramp") && canChangeWalls)
        {
            rb.Sleep();

            Vector3 lastForwardDirection = transform.forward;

            currentWall = collision.gameObject.GetComponent<Ramp>().GetAdjacentWall(currentWall);
            gravityDirection = currentWall.GravityDirection;
            constant.force = gravityDirection;
            canChangeWalls = false;

            float signedAngle = Vector3.SignedAngle(transform.up, currentWall.GravityDirection.normalized * -1, Vector3.forward);

            if (signedAngle != 0)
            {
                Quaternion rot = Quaternion.AngleAxis(signedAngle, Vector3.forward) * transform.rotation;
                transform.rotation = rot;
            }

            StartCoroutine(StartWallChangeTimer());
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
