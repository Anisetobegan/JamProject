using DG.Tweening;
using UnityEngine;

public class Bat : Enemies
{
    float sinCenter;
    float sinAmplitude;
    float sinSpeed;

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

        if (Random.Range(0, 2) == 0)
        {
            //GetLeftMovementDirection();
            direction = Direction.Left;
        }
        else
        {
            //GetRightMovementDirection();
            direction = Direction.Right;
        }
        moveSpeed = 3f;
    }

    private void Awake()
    {
        state = State.Flying;
        timerGoal = Random.Range(minTimerOffset, maxTimerOffset);
        sinCenter = transform.position.y;
    }

    protected override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timerGoal)
        {
            if (Random.Range(0, 2) == 0)
            {
                GetLeftMovementDirection();
                direction = Direction.Left;
                timer = 0;
                timerGoal = Random.Range(minTimerOffset, maxTimerOffset);
            }
            else
            {
                GetRightMovementDirection();
                direction = Direction.Right;
                timer = 0;
                timerGoal = Random.Range(minTimerOffset, maxTimerOffset);
            }
        }

        switch (state)
        {
            case State.Flying:

                Move();

                break;

            case State.Dead:

                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (other.gameObject.GetComponent<Wall>() != currentWall)
            {
                currentWall = other.gameObject.GetComponent<Wall>();
                gravityDirection = currentWall.GravityDirection;
                
                if (direction == Direction.Left)
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
}
