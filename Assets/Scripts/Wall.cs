using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Wall : MonoBehaviour
{
    [SerializeField] Vector3 gravityDirection;
    [SerializeField] Collider wallCollider;

    public enum Direction
    {
        Up,
        Down, 
        Left, 
        Right
    }

    public Direction direction;

    public Vector3 GravityDirection { get { return gravityDirection; } }

    public Collider WallCollider { get { return wallCollider; } }

    void Start()
    {
        
    }
}
