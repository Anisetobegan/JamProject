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

    protected override void Start()
    {
        gravityDirection = currentWall.GravityDirection;

        if (Random.Range(0, 2) == 0)
        {
            GetLeftMovementDirection();
            movingDirection = "Left";
        }
        else
        {
            GetRightMovementDirection();
            movingDirection = "Right";
        }
        moveSpeed = 5f;
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

                break;

            case State.Dead:

                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") && canChangeWalls)
        {
            if (collision.gameObject.GetComponent<Wall>() != currentWall)
            {
                /*currentWall = collision.gameObject.GetComponent<Wall>();
                gravityDirection = currentWall.GravityDirection;*/
                timer = 0;
                timerGoal = Random.Range(minTimerOffset, maxTimerOffset);

                if (movingDirection == "Left")
                {
                    GetRightMovementDirection();
                }
                else
                {
                    GetLeftMovementDirection();
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

    IEnumerator StartWallChangeTimer()
    {
        yield return new WaitForSeconds(1f);
        canChangeWalls = true;
        constant.force = Vector3.zero;
    }
}
