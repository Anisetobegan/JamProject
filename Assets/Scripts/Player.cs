using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    string _directionKey;
    float _moveSpeed = 3f;
    float _jumpForce = 10f;
    float _minimumJumpForce = 3f;
    bool _isJumping = false;

    //Vectors
    Vector3 _gravityDirection;

    //Assets
    [SerializeField] Rigidbody _rb;
    [SerializeField] ConstantForce _constant;
    [SerializeField] Wall _currentWall;

    enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    Direction _direction;

    enum State
    {
        Running,
        Jumping,
        Hit,
        Dead
    }
    State _state;

    void Start()
    {
        _gravityDirection = _currentWall.GravityDirection;
        _constant.force = _gravityDirection;
        _direction = Direction.Right;
        _directionKey = "D";
        _state = State.Running;        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _directionKey = "A";
            GetLeftMovementDirection();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _directionKey = "D";
            GetRightMovementDirection();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _state = State.Jumping;
        }

        switch (_state)
        {
            case State.Running:

                Move();

                break;

            case State.Jumping:

                Move();

                if (!_isJumping)
                {
                    Jump();
                }

                break;

            case State.Hit:

                break;

            case State.Dead:

                break;
        }
    }

    void Move()
    {
        switch( _direction)
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

    void MoveRight()
    {
        Vector3 moveDirection = Vector3.right;
        _rb.position = (moveDirection * _moveSpeed) * Time.deltaTime + _rb.position;
        transform.rotation = _currentWall.transform.rotation;
        transform.rotation = Quaternion.LookRotation(moveDirection, _gravityDirection * -1);
    }
    void MoveLeft()
    {
        Vector3 moveDirection = Vector3.left;        
        _rb.position = (moveDirection * _moveSpeed) * Time.deltaTime + _rb.position;
        transform.rotation = _currentWall.transform.rotation;
        transform.rotation = Quaternion.LookRotation(moveDirection, _gravityDirection * -1);
    }

    void MoveUp()
    {
        Vector3 moveDirection = Vector3.up;
        _rb.position = (moveDirection * _moveSpeed) * Time.deltaTime + _rb.position;
        transform.rotation = _currentWall.transform.rotation;
        transform.rotation = Quaternion.LookRotation(moveDirection, _gravityDirection * -1);
    }

    void MoveDown()
    {
        Vector3 moveDirection = Vector3.down;
        _rb.position = (moveDirection * _moveSpeed) * Time.deltaTime + _rb.position;
        transform.rotation = _currentWall.transform.rotation;
        transform.rotation = Quaternion.LookRotation(moveDirection, _gravityDirection * -1);
    }

    void Jump()
    {
        _rb.AddForce((_gravityDirection * -1).normalized * _jumpForce, ForceMode.Impulse);
        _isJumping = true;
    }

    void GetLeftMovementDirection()
    {
        switch (_currentWall.direction)
        {
            case Wall.Direction.Left:
                _direction = Direction.Up;
                break;

            case Wall.Direction.Right:
                _direction = Direction.Down;
                break;

            case Wall.Direction.Up:
                _direction = Direction.Right;
                break;

            case Wall.Direction.Down:
                _direction = Direction.Left;
                break;
        }
    }

    void GetRightMovementDirection()
    {
        switch (_currentWall.direction)
        {
            case Wall.Direction.Left:
                _direction = Direction.Down;
                break;

            case Wall.Direction.Right:
                _direction = Direction.Up;
                break;

            case Wall.Direction.Up:
                _direction = Direction.Left;
                break;

            case Wall.Direction.Down:
                _direction = Direction.Right;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (_isJumping)
            {
                _currentWall = collision.gameObject.GetComponent<Wall>();
                _gravityDirection = _currentWall.GravityDirection;
                _isJumping = false;
                _state = State.Running;
                _constant.force = _gravityDirection;

                if (_directionKey == "A")
                {
                    GetLeftMovementDirection();
                }
                else
                {
                    GetRightMovementDirection();
                }
            }
            else
            {
                if (collision.gameObject.GetComponent<Wall>() != _currentWall)
                {
                    if (_directionKey == "A")
                    {
                        GetRightMovementDirection();
                    }
                    else
                    {
                        GetLeftMovementDirection();
                    }
                }
            }
        }
    }
}
