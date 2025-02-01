using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<Level> levels;
    Level currentLevel;
    Level nextLevel;

    public int LevelCount { get { return levels.Count; } }

    public static LevelManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Level CurrentLevel { get { return currentLevel; } }

    void Start()
    {
        int level = GameManager.Instance.Level;
        currentLevel = InstantiateLevel(level);
        nextLevel = InstantiateNextLevel(level + 1);
        GameManager.Instance.PlayerGet.AssignCurrentWall(currentLevel.StartingWall);
        GameManager.Instance.CameraGet.SetCameraBehaviour(currentLevel.CameraPosition, currentLevel.PlayerStartingPosition.position);
    }

    void Update()
    {
        
    }

    public void StartLevel()
    {
        currentLevel.EnableWallsColliders();
        currentLevel.EnableEnemies();
        GameManager.Instance.PlayerGet.MakeRigidBodyKinematic();
    }

    public void EndLevel()
    {
        currentLevel.DisableWallsColliders();
    }

    public void ChangeLevel(int level)
    {
        Level completedLevel = currentLevel;
        currentLevel = nextLevel;
        nextLevel = InstantiateNextLevel(level + 1);

        /*if (nextLevel != null)
        {
            GameManager.Instance.PlayerGet.PlayerTransition(currentLevel.PlayerStartingPosition, completedLevel);
            GameManager.Instance.PlayerGet.AssignCurrentWall(currentLevel.StartingWall);
            GameManager.Instance.CameraGet.SetCameraBehaviour(currentLevel.CameraPosition, currentLevel.PlayerStartingPosition.position);
        }
        else if (currentLevel != null)
        {
            GameManager.Instance.PlayerGet.PlayerTransition(currentLevel.PlayerStartingPosition, completedLevel);
            GameManager.Instance.PlayerGet.AssignCurrentWall(currentLevel.StartingWall);
            GameManager.Instance.CameraGet.SetCameraBehaviour(currentLevel.CameraPosition, currentLevel.PlayerStartingPosition.position);
        }*/
        if (currentLevel != null)
        {
            GameManager.Instance.PlayerGet.PlayerTransition(currentLevel.PlayerStartingPosition, completedLevel);
            GameManager.Instance.PlayerGet.AssignCurrentWall(currentLevel.StartingWall);
            GameManager.Instance.CameraGet.SetCameraBehaviour(currentLevel.CameraPosition, currentLevel.PlayerStartingPosition.position);
        }
    }

    public Level InstantiateLevel(int level)
    {
        Level newLevel = null;
        if (levels.Count >= level)
        {
            newLevel = Instantiate(levels[level - 1], transform.position, transform.rotation);
            return newLevel;
        }
        return newLevel;
    }

    public Level InstantiateNextLevel(int level)
    {
        Level newLevel = null;
        if (levels.Count >= level)
        {
            Vector3 levelPos = currentLevel.transform.position;
            float offset = (currentLevel.EndChain.position - currentLevel.transform.position).magnitude;
            levelPos.x += offset;

            newLevel = Instantiate(levels[level - 1], levelPos, transform.rotation);
            offset = (newLevel.StartChain.position - newLevel.transform.position).magnitude;
            levelPos.x += offset;
            newLevel.transform.position = levelPos;

            newLevel.DisableWallsColliders();
            newLevel.DisableEnemies();
            return newLevel;
        }
        return newLevel;
    }    
}
