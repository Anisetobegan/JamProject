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
            if (collision.gameObject.GetComponent<Wall>() != currentWall)
            {
                ChangeDirection();
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ramp") && canChangeWalls)
        {
            canChangeWalls = false;
            rb.isKinematic = false;
            currentWall = collision.gameObject.GetComponent<Ramp>().GetAdjacentWall(currentWall);
            gravityDirection = currentWall.GravityDirection;
            constant.force = gravityDirection;

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
        yield return new WaitForSeconds(1f);
        rb.isKinematic = true;
        canChangeWalls = true;
        constant.force = Vector3.zero;
    }
}
