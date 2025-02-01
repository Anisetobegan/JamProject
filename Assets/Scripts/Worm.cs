using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Worm : Enemies
{
    enum State
    {
        Walking,
        Dead
    }
    [SerializeField] State state;

    bool isFalling = false;

    private void Awake()
    {
        moveSpeed = 5f;
        state = State.Walking;
    }

    protected override void Update()
    {
        switch (state)
        {
            case State.Walking:

                Move();

                break;

            case State.Dead:

                break;
        }
    }    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (isFalling)
            {
                isFalling = false;
                currentWall = CheckContactsForCurrentWall(collision.contacts);
                gravityDirection = currentWall.GravityDirection;
                constant.force = gravityDirection;

                Vector3 lastForwardDirection = transform.forward;
                Vector3 newPos = collision.GetContact(0).point;
                newPos.z = 0;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ramp") && canChangeWalls)
        {
            if (!isFalling)
            {
                Ramp ramp = other.GetComponent<Ramp>();
                canChangeWalls = false;
                rb.isKinematic = false;
                currentWall = ramp.GetAdjacentWall(currentWall);
                gravityDirection = currentWall.GravityDirection;
                constant.force = gravityDirection;

                float signedAngle = Vector3.SignedAngle(transform.up, currentWall.GravityDirection.normalized * -1, Vector3.forward);

                Quaternion rot = Quaternion.AngleAxis(signedAngle, Vector3.forward) * transform.rotation;
                //transform.rotation = rot;
                transform.DORotateQuaternion(rot, ramp.RotateTime).SetEase(Ease.Linear).OnComplete(() => canChangeWalls = true);
            }
        }
    }

    /*private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (collision.gameObject.GetComponent<Wall>() != currentWall && canChangeWalls)
            {
                ChangeDirection();
            }
        }
    }*/

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (collision.gameObject.GetComponent<Wall>() == currentWall)
            {
                isFalling = true;
            }
        }
    }
}
