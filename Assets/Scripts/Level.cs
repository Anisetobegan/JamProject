using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class Level : MonoBehaviour
{
    [SerializeField] int levelNumber;
    [SerializeField] List<Enemies> enemiesAlive;
    [SerializeField] List<Wall> walls;
    [SerializeField] Wall startingWall;
    [SerializeField] Transform playerStartingPos;
    [SerializeField] Transform startChain;
    [SerializeField] Transform endChain;
    [SerializeField] Transform cameraPos;

    public Wall StartingWall { get { return startingWall; } }
    public Transform PlayerStartingPosition { get { return playerStartingPos; } }
    public Transform StartChain { get { return startChain; } }
    public Transform EndChain { get { return endChain; } }
    public Transform CameraPosition { get { return cameraPos; } }

    private void OnEnable()
    {
        Actions.OnEnemyKilled += CheckEnemiesAlive;
    }

    private void OnDisable()
    {
        Actions.OnEnemyKilled -= CheckEnemiesAlive;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void CheckEnemiesAlive(Enemies enemyKilled)
    {
        if (enemiesAlive.Count > 0 && enemyKilled.IsDead)
        {
            enemiesAlive.Remove(enemyKilled);
            if (enemiesAlive.Count == 0)
            {
                GameManager.Instance.LevelWon();
            }
        }        
    }

    public void DestroyCompletedLevel()
    {
        Destroy(gameObject);
    }

    public void DisableWallsColliders()
    {
        foreach (Wall wall in walls)
        {
            wall.WallCollider.enabled = false;
        }
    }

    public void EnableWallsColliders()
    {
        foreach (Wall wall in walls)
        {
            wall.WallCollider.enabled = true;
        }
    }

    public void DisableEnemies()
    {
        foreach (Enemies enemies in enemiesAlive)
        {
            enemies.enabled = false;
        }
    }

    public void EnableEnemies()
    {
        foreach (Enemies enemies in enemiesAlive)
        {
            enemies.enabled = true;
        }
    }
}
