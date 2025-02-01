using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemies : MonoBehaviour
{
    protected float moveSpeed;
    protected float moveDirection;
    protected Vector3 gravityDirection;
    protected bool canChangeWalls = true;
    protected bool isDead = false;

    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected ConstantForce constant;
    [SerializeField] protected Wall currentWall;
    [SerializeField] protected GameObject deadVFX;

    public bool IsDead {  get { return isDead; } }

    private void OnEnable()
    {

    }

    virtual protected void Start()
    {
        gravityDirection = currentWall.GravityDirection;
        constant.force = gravityDirection;
        moveDirection = transform.rotation.eulerAngles.y;
    }

    virtual protected void Update()
    {
        
    }

    virtual protected void Move()
    {
        rb.position = (transform.forward * moveSpeed) * Time.deltaTime + rb.position;
    }

    protected void ChangeDirection()
    {
        moveDirection *= -1;
        transform.Rotate(0, moveDirection * 2, 0, Space.Self);
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

    protected Wall CheckContactsForCurrentWall(ContactPoint[] contacts)
    {
        Wall contactWall = null;

        foreach (ContactPoint contact in contacts)
        {
            if (contact.otherCollider.gameObject.GetComponent<Wall>() != currentWall)
            {
                contactWall = contact.otherCollider.gameObject.GetComponent<Wall>();
                return contactWall;
            }
        }
        return currentWall;
    }
}
