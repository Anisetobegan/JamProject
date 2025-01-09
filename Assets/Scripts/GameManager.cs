using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int _level;

    IEnumerator _enumerator = null;

    public static bool _isPaused = false;

    [SerializeField] Player _player;

    public Player PlayerGet { get { return _player; } }

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
        _level = 1;

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
        
        LevelManager.Instance.EndLevel();
        UIManager.Instance.OpenLevelClearScreen();
        LevelManager.Instance.ChangeLevel(_level);

        if (LevelManager.Instance.CurrentLevel != null)
        {
            _enumerator = StartNewLevel();
            StartCoroutine(_enumerator);
        }
    }

    public void Lose()
    {
        UIManager.Instance.OpenGameOverScreen();
    }

    public IEnumerator StartNewLevel()
    {
        yield return new WaitForSeconds(3f);

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
