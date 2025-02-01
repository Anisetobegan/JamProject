using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Bat : Enemies
{
    [SerializeField] SphereCollider trigger;

    enum State
    {
        Flying,
        Dead
    }
    [SerializeField] State state;

    protected override void Start()
    {
        gravityDirection = currentWall.GravityDirection;
        moveDirection = transform.rotation.eulerAngles.y;
    }

    private void Awake()
    {
        moveSpeed = 3f;
        state = State.Flying;
    }

    protected override void Update()
    {
        switch (state)
        {
            case State.Flying:

                Move();

                break;

            case State.Dead:

                break;
        }
    }

    void MantainWallDistance(Vector3 contactPoint)
    {
        float currentDistance = Vector3.Distance(transform.position, contactPoint);
        if (currentDistance < trigger.radius)
        {
            float distanceToMove = (trigger.radius - currentDistance) + transform.localPosition.y;
            transform.DOLocalMoveY(distanceToMove, 0.2f).SetEase(Ease.Linear);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (collision.gameObject.GetComponent<Wall>() != currentWall)
            {
                ChangeDirection();
            }
        }
    }

        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (other.gameObject.GetComponent<Wall>() != currentWall && canChangeWalls)
            {
                canChangeWalls = false;

                Vector3 lastForwardDirection = transform.forward;

                currentWall = other.gameObject.GetComponent<Wall>();
                gravityDirection = currentWall.GravityDirection;

                float signedAngle = Vector3.SignedAngle(transform.up, currentWall.GravityDirection.normalized * -1, Vector3.forward);
                                
                Quaternion rot = Quaternion.AngleAxis(signedAngle, Vector3.forward) * transform.rotation;
                //transform.rotation = rot;
                transform.DORotateQuaternion(rot, 0.2f).SetEase(Ease.Linear).OnComplete(() => canChangeWalls = true);

                float dot = Vector3.Dot(lastForwardDirection, transform.forward);

                if (dot <= -0.999f)
                {
                    ChangeDirection();
                }
                
                MantainWallDistance(other.ClosestPointOnBounds(transform.position));
            }            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (collision.gameObject.GetComponent<Wall>() != currentWall)
            {
                ChangeDirection();
            }
        }
    }
}
