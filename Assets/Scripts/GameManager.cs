using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static int _level = 1;

    IEnumerator _enumerator = null;

    public static bool _isPaused = false;

    public float TransitionTime = 3f;
    public bool IsStartOfGame = true;

    [SerializeField] Player _player;
    [SerializeField] CameraFollow _cam;

    public int Level {  get { return _level; } }

    public Player PlayerGet { get { return _player; } }
    public CameraFollow CameraGet { get { return _cam; } }

    public IEnumerator EnumeratorGet { get { return _enumerator; } }

    public static GameManager Instance
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

    void Start()
    {
        UIManager.Instance.OpenLevelStartScreen(_level);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && _isPaused)
        {
            Resume();
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && !_isPaused)
        {
            Pause();
        }
    }

    public void LevelWon()
    {
        _level += 1;
        IsStartOfGame = false;
        _player.MakeRigidBodyKinematic();

        if (_level <= LevelManager.Instance.LevelCount)
        {

            LevelManager.Instance.EndLevel();
            UIManager.Instance.OpenLevelClearScreen();
            LevelManager.Instance.ChangeLevel(_level);

            if (LevelManager.Instance.CurrentLevel != null)
            {
                _enumerator = StartNewLevel();
                StartCoroutine(_enumerator);
            }
        }
        else
        {
            GameWon();
        }
    }

    public void Lose()
    {
        UIManager.Instance.OpenGameOverScreen();
    }

    public IEnumerator StartNewLevel()
    {
        yield return new WaitForSeconds(TransitionTime);

        UIManager.Instance.OpenLevelStartScreen(_level);

        LevelManager.Instance.StartLevel();

        _enumerator = null;
    }

    public void Pause()
    {
        UIManager.Instance.OpenPauseScreen();
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void Resume()
    {
        UIManager.Instance.ClosePauseScreen();
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {
        _level = 1;
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameWon()
    {
        UIManager.Instance.OpenGameWonScreen();
    }
}
