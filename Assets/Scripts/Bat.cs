using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Bat : Enemies
{
    [SerializeField] Collider trigger;

    enum State
    {
        Flying,
        Dead
    }
    [SerializeField] State state;

    protected override void Start()
    {
        gravityDirection = currentWall.GravityDirection;
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
                rb.Sleep();

                Vector3 lastForwardDirection = transform.forward;

                currentWall = other.gameObject.GetComponent<Wall>();
                gravityDirection = currentWall.GravityDirection;

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
                }
                StartCoroutine(StartWallChangeTimer());
            }            
        }
    }

    IEnumerator StartWallChangeTimer()
    {
        yield return new WaitForSeconds(1f);
        canChangeWalls = true;
    }
}
