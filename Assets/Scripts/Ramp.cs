using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Ramp : MonoBehaviour
{
    [SerializeField] List<Wall> _adjacentWalls;
    [SerializeField] float _rotateTime = 0.8f;

    public float RotateTime { get { return _rotateTime; } }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Wall GetAdjacentWall(Wall wall)
    {
        Wall adjacentWall = null;
        
        if (_adjacentWalls[0] == wall)
        {
            adjacentWall = _adjacentWalls[1];
        }
        else
        {
            adjacentWall = _adjacentWalls[0];
        }
        return adjacentWall;
    }
}
