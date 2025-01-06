using UnityEngine;

public class Enemies : MonoBehaviour
{
    protected float moveSpeed;
    protected Vector3 gravityDirection;
    protected string movingDirection;
    protected float minTimerOffset = 2f;
    protected float maxTimerOffset = 5f;
    protected float timer = 0;
    protected float timerGoal;
    protected bool canChangeWalls = true;

    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected ConstantForce constant;
    [SerializeField] protected Wall currentWall;

    protected enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    protected Direction direction;

    private void OnEnable()
    {

    }

    virtual protected void Start()
    {
        gravityDirection = currentWall.GravityDirection;
        constant.force = gravityDirection;

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
    }

    virtual protected void Update()
    {
        
    }

    virtual protected void Move()
    {
        switch (direction)
        {
            case Direction.Right:
                MoveRight();
                break;

            case Direction.Left:
                MoveLeft();
                break;
            case Direction.Up:
                MoveUp();
                break;
            case Direction.Down:
                MoveDown();
                break;
        }
    }

    protected void MoveRight()
    {
        Vector3 moveDirection = Vector3.right;
        rb.position = (moveDirection * moveSpeed) * Time.deltaTime + rb.position;
        transform.rotation = currentWall.transform.rotation;
        transform.rotation = Quaternion.LookRotation(moveDirection, gravityDirection * -1);
    }
    protected void MoveLeft()
    {
        Vector3 moveDirection = Vector3.left;
        rb.position = (moveDirection * moveSpeed) * Time.deltaTime + rb.position;
        transform.rotation = currentWall.transform.rotation;
        transform.rotation = Quaternion.LookRotation(moveDirection, gravityDirection * -1);
    }

    protected void MoveUp()
    {
        Vector3 moveDirection = Vector3.up;
        rb.position = (moveDirection * moveSpeed) * Time.deltaTime + rb.position;
        transform.rotation = currentWall.transform.rotation;
        transform.rotation = Quaternion.LookRotation(moveDirection, gravityDirection * -1);
    }

    protected void MoveDown()
    {
        Vector3 moveDirection = Vector3.down;
        rb.position = (moveDirection * moveSpeed) * Time.deltaTime + rb.position;
        transform.rotation = currentWall.transform.rotation;
        transform.rotation = Quaternion.LookRotation(moveDirection, gravityDirection * -1);
    }

    protected void GetLeftMovementDirection()
    {
        switch (currentWall.direction)
        {
            case Wall.Direction.Left:
                direction = Direction.Up;
                break;

            case Wall.Direction.Right:
                direction = Direction.Down;
                break;

            case Wall.Direction.Up:
                direction = Direction.Right;
                break;

            case Wall.Direction.Down:
                direction = Direction.Left;
                break;
        }
    }

    protected void GetRightMovementDirection()
    {
        switch (currentWall.direction)
        {
            case Wall.Direction.Left:
                direction = Direction.Down;
                break;

            case Wall.Direction.Right:
                direction = Direction.Up;
                break;

            case Wall.Direction.Up:
                direction = Direction.Left;
                break;

            case Wall.Direction.Down:
                direction = Direction.Right;
                break;
        }
    }

    virtual public void Die()
    {
        Destroy(gameObject);
    }
}
