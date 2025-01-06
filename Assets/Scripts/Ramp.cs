using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Ramp : MonoBehaviour
{
    [SerializeField] List<Wall> _adjacentWalls;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Wall GetAdjacentWall(Wall wall)
    {
        Wall currentWall = new Wall();
        Wall adjacentWall = new Wall();

        for (int i = 0; i < _adjacentWalls.Count; i++)
        {
            if (_adjacentWalls[i] == wall)
            {
                currentWall = _adjacentWalls[i];
            }
            else
            {
                adjacentWall = _adjacentWalls[i];
            }
        }

        return adjacentWall;
    }
}
