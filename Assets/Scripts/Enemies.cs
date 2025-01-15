using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemies : MonoBehaviour
{
    protected float moveSpeed;
    protected float moveDirection;
    protected Vector3 gravityDirection;
    protected float minTimerOffset = 2f;
    protected float maxTimerOffset = 5f;
    protected float timer = 0;
    protected float timerGoal;
    protected bool canChangeWalls = true;
    protected bool isDead = false;

    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected ConstantForce constant;
    [SerializeField] protected Wall currentWall;
    [SerializeField] protected GameObject deadVFX;

    protected enum Direction
    {
        Left,
        Right
    }
    protected Direction direction;

    public bool IsDead {  get { return isDead; } }

    private void OnEnable()
    {

    }

    virtual protected void Start()
    {
        gravityDirection = currentWall.GravityDirection;
        constant.force = gravityDirection;

        
            //GetLeftMovementDirection();
            direction = Direction.Left;
            moveDirection = transform.rotation.eulerAngles.y;
            //transform.rotation = Quaternion.LookRotation(currentWall.transform.right * -1, gravityDirection * -1);
            Debug.Log(transform.forward);
    }

    virtual protected void Update()
    {
        
    }

    virtual protected void Move()
    {
        /*switch (direction)
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

        if (direction == Direction.Left)
        {
            moveDirection = transform.forward;
            transform.rotation = currentWall.transform.rotation;
            rb.position = (moveDirection * moveSpeed) * Time.deltaTime + rb.position;
            transform.rotation = Quaternion.LookRotation(moveDirection, gravityDirection * -1);
        }
        else
        {
            moveDirection = transform.forward;
            transform.rotation = currentWall.transform.rotation;
            rb.position = (moveDirection * moveSpeed) * Time.deltaTime + rb.position;            
            transform.rotation = Quaternion.LookRotation(moveDirection, gravityDirection * -1);
        }*/

        //transform.up = currentWall.transform.up;
        
        rb.position = (transform.forward * moveSpeed) * Time.deltaTime + rb.position;
        //transform.rotation = Quaternion.LookRotation(moveDirection, gravityDirection * -1);
        //moveDirection = transform.forward;
    }

    protected void ChangeDirection()
    {
        //transform.forward *= -1;
        //transform.rotation = Quaternion.FromToRotation(transform.forward, transform.forward * -1);
        moveDirection *= -1;
        transform.Rotate(0, moveDirection * 2, 0, Space.Self);
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

    protected Vector3 GetLeftMovementDirection()
    {
        direction = Direction.Left;
        return currentWall.transform.right * -1;

        /*switch (currentWall.direction)
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
        }*/
    }

    protected Vector3 GetRightMovementDirection()
    {
        direction = Direction.Right;
        return currentWall.transform.right;

        /*switch (currentWall.direction)
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
        }*/
    }

    virtual public void Die()
    {
        if (!IsDead)
        {
            isDead = true;
            Actions.OnEnemyKilled(this);
            GameObject newDeadVFX = Instantiate(deadVFX, transform.position, transform.rotation);
            Destroy(newDeadVFX, 1f);
            Destroy(gameObject);
        }
    }
}
