using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    float _moveSpeed = 6f;
    float _jumpForce = 10f;
    bool _isJumping = false;
    bool _isVertical = false;
    float _moveDirection;
    bool _canKillEnemy = false;

    //Vectors
    Vector3 _gravityDirection;
    Vector3 _direction;

    //Assets
    [SerializeField] Rigidbody _rb;
    [SerializeField] ConstantForce _constant;
    [SerializeField] Wall _currentWall;
    [SerializeField] LayerMask _enemyLayerMask;

    [SerializeField] Animator animator;

    [SerializeField] GameObject _deadVFX;

    enum State
    {
        Running,
        Jumping,
        Transitioning,
        Dead
    }
    [SerializeField] State _state;

    void Start()
    {
        transform.position = LevelManager.Instance.CurrentLevel.PlayerStartingPosition.position;
        transform.rotation = LevelManager.Instance.CurrentLevel.PlayerStartingPosition.rotation;
        _gravityDirection = _currentWall.GravityDirection;
        _constant.force = _gravityDirection;
        _direction = transform.forward;
        _state = State.Running;
        _moveDirection = transform.rotation.eulerAngles.y;

        if (_currentWall.direction == Wall.Direction.Up || _currentWall.direction == Wall.Direction.Down)
        {
            _isVertical = false;
        }
        else
        {
            _isVertical = true;
        }
    }

    void Update()
    {
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
                if (_direction != Vector3.left)
                {
                    ChangeDirection();
                }
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
                if (_direction != Vector3.right)
                {
                    ChangeDirection();
                }
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
                if (_direction != Vector3.up)
                {
                    ChangeDirection();
                }
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
                if (_direction != Vector3.down)
                {
                    ChangeDirection();
                }
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

                if (_isJumping)
                {
                    _state = State.Jumping;
                }

                break;

            case State.Jumping:

                Move();

                if (!_isJumping)
                {
                    Jump();
                }

                break;

            case State.Transitioning:

                if (GameManager.Instance.EnumeratorGet == null)
                {
                    //_rb.isKinematic = false;
                    _state = State.Running;
                }
                break;

            case State.Dead:

                PlayDeadVFX();
                GameManager.Instance.Lose();
                gameObject.SetActive(false);

                break;
        }
        animator.SetBool("Jumping", _isJumping);

    }

    void Move()
    {
        _rb.position = (transform.forward * _moveSpeed) * Time.deltaTime + _rb.position;
    }

    void Jump()
    {
        _rb.AddForce((_gravityDirection * -1).normalized * _jumpForce, ForceMode.Impulse);
        _isJumping = true;
    }

    void ChangeDirection()
    {
        _moveDirection *= -1;
        transform.Rotate(0, _moveDirection * 2, 0, Space.Self);
        _direction = transform.forward;
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

    public void AssignCurrentWall(Wall wallToAsign)
    {
        _currentWall = wallToAsign;
    }

    public void PlayerTransition(Transform startingPos, Level levelToDestroy)
    {
        _state = State.Transitioning;
        _constant.force = Vector3.zero;
        _rb.linearVelocity = Vector3.zero;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        //_rb.isKinematic = true;
        transform.DOMove(startingPos.position, GameManager.Instance.TransitionTime).SetEase(Ease.InOutSine).OnComplete(() => levelToDestroy.DestroyCompletedLevel());
        transform.DORotate(startingPos.rotation.eulerAngles, 3f).OnComplete(() => _direction = transform.forward);
    }

    void PlayDeadVFX()
    {
        Instantiate(_deadVFX, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (_isJumping)
            {
                _rb.Sleep();

                Vector3 lastForwardDirection = transform.forward;

                ContactPoint contactPoint = collision.GetContact(0);

                Wall lastWall = _currentWall;
                _currentWall = collision.gameObject.GetComponent<Wall>();
                _gravityDirection = _currentWall.GravityDirection;
                _isJumping = false;
                _state = State.Running;
                _constant.force = _gravityDirection;
                _isVertical = _currentWall.direction == Wall.Direction.Left || _currentWall.direction == Wall.Direction.Right;

                float signedAngle = Vector3.SignedAngle(transform.up, _currentWall.GravityDirection.normalized * -1, Vector3.forward);

                if (signedAngle != 0)
                {
                    Quaternion rot = Quaternion.AngleAxis(signedAngle, Vector3.forward) * transform.rotation;
                    transform.rotation = rot;
                    _direction = transform.forward;

                    float dot = Vector3.Dot(lastForwardDirection, transform.forward);

                    if (dot <= -0.999f)
                    {
                        ChangeDirection();
                    }
                    transform.position = contactPoint.point;
                }
            }
            else
            {
                if (collision.gameObject.GetComponent<Wall>() != _currentWall)
                {
                    ChangeDirection();
                }
            }
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //checks if the Enemy is below the Player
            //if (CalculateDotProduct(collision.gameObject))
            //if (CheckIfEnemyIsBelow(collision.gameObject))
            if (_canKillEnemy)
            {
                //if true, kills the Enemy
                _rb.Sleep();
                collision.gameObject.GetComponent<Enemies>().Die();
                Jump();
                Debug.Log("enemy killed");
                _canKillEnemy = false;
            }  
            else
            {
                //if false, the Enemy klls the Player
                Debug.Log("you are dead");
                _state = State.Dead;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (collision.gameObject.GetComponent<Wall>() == _currentWall)
            {
                _isJumping = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _canKillEnemy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _canKillEnemy = false;
        }
    }

    bool CheckIfEnemyIsBelow(GameObject enemy)
    {
        Ray ray = new Ray(transform.position, transform.up * -1);
        RaycastHit hit;

        return Physics.Raycast(ray, out hit, 100f, _enemyLayerMask);
    }

    public void MakeRigidBodyKinematic()
    {
        _rb.isKinematic = _rb.isKinematic == false;
    }
}
