using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float _moveSpeed = 6f;
    float _jumpForce = 10f;
    float _minimumJumpForce = 3f;
    bool _isJumping = false;
    bool _isVertical = false;

    //Vectors
    Vector3 _gravityDirection;

    //Assets
    [SerializeField] Rigidbody _rb;
    [SerializeField] ConstantForce _constant;
    [SerializeField] Wall _currentWall;

    [SerializeField] Animator animator;

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
        _state = State.Running;        
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.A) && !_isVertical)
        {
            _direction = Direction.Left;
        }*/
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (_isVertical) 
            {
                if (_currentWall.direction == Wall.Direction.Left && _isJumping)
                {
                    _rb.AddForce((_gravityDirection).normalized * (_jumpForce * 2), ForceMode.Impulse);
                }
            }
            else
            {
                _direction = Direction.Left;
            }            
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (_isVertical)
            {
                if (_currentWall.direction == Wall.Direction.Right && _isJumping)
                {
                    _rb.AddForce((_gravityDirection).normalized * (_jumpForce * 2), ForceMode.Impulse);
                }
            }
            else
            {
                _direction = Direction.Right;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!_isVertical)
            {
                if (_currentWall.direction == Wall.Direction.Up && _isJumping)
                {
                    _rb.AddForce((_gravityDirection).normalized * (_jumpForce * 2), ForceMode.Impulse);
                }
            }
            else
            {
                _direction = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!_isVertical)
            {
                if (_currentWall.direction == Wall.Direction.Down && _isJumping)
                {
                    _rb.AddForce((_gravityDirection).normalized * (_jumpForce * 2), ForceMode.Impulse);
                }
            }
            else
            {
                _direction = Direction.Down;
            }
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
        animator.SetBool("Jumping", _isJumping);

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

    bool CalculateDotProduct(GameObject target)
    {
        Vector3 targetDirection = (target.transform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(_gravityDirection.normalized, targetDirection);        

        return dotProduct > 0.5;
    }

    bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if (Mathf.Abs(planarFactor) < 0.0001f
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2)
                    / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (_isJumping)
            {
                Wall lastWall = _currentWall;
                _currentWall = collision.gameObject.GetComponent<Wall>();
                _gravityDirection = _currentWall.GravityDirection;
                _isJumping = false;
                _state = State.Running;
                _constant.force = _gravityDirection;

                //switch to mantain the direction the Player is moving when changing walls
                switch (_currentWall.direction) 
                {
                    case Wall.Direction.Up:                        
                        if (lastWall.direction == Wall.Direction.Left)
                        {
                            _isVertical = false;
                            _direction = Direction.Right;
                        }
                        else if (lastWall.direction == Wall.Direction.Right)
                        {
                            _isVertical = false;
                            _direction = Direction.Left;
                        }
                        break;

                    case Wall.Direction.Down:
                        if (lastWall.direction == Wall.Direction.Left)
                        {
                            _isVertical = false;
                            _direction = Direction.Right;
                        }
                        else if (lastWall.direction == Wall.Direction.Right)
                        {
                            _isVertical = false;
                            _direction = Direction.Left;
                        }
                        break;

                    case Wall.Direction.Left:
                        if (lastWall.direction == Wall.Direction.Up)
                        {
                            _isVertical = true;
                            _direction = Direction.Down;
                        }
                        else if (lastWall.direction == Wall.Direction.Down)
                        {
                            _isVertical = true;
                            _direction = Direction.Up;
                        }
                        break;

                    case Wall.Direction.Right:
                        if (lastWall.direction == Wall.Direction.Up)
                        {
                            _isVertical = true;
                            _direction = Direction.Down;
                        }
                        else if (lastWall.direction == Wall.Direction.Down)
                        {
                            _isVertical = true;
                            _direction = Direction.Up;
                        }
                        break;
                }
            }
            else
            {
                if (collision.gameObject.GetComponent<Wall>() != _currentWall)
                {
                    //inverts the direction when the Player collides with a wall
                    switch (_direction)
                    {
                        case Direction.Left:
                            _direction = Direction.Right;
                            break;
                        case Direction.Up:
                            _direction = Direction.Down;
                            break;
                        case Direction.Down:
                            _direction = Direction.Up;
                            break;
                        case Direction.Right:
                            _direction = Direction.Left;
                            break;
                    }
                }
            }
        }

        else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //checks if the Enemy is below the Player
            if (CalculateDotProduct(collision.gameObject))
            {
                //if true, kills the Enemy
                collision.gameObject.GetComponent<Enemies>().Die();
                Jump();
                Debug.Log("enemy killed");
            }  
            else
            {
                //if false, the Enemy klls the Player
                Debug.Log("you are dead");
                _state = State.Dead;
            }
        }
    }
}
