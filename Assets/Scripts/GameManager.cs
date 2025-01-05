using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _wave;

    private int _enemiesToAddPerWave = 5;

    private float _timeBetweenWaves = 3;

    IEnumerator _enumerator = null;

    public static bool _isPaused = false;

    [SerializeField] private GameObject _waveScreen;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _pauseScreen;

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
        _wave = 1;
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

        /*if (_enemySpawner.EnemiesKilled == _enemySpawner.MaxEnemies) //checks if player killed the amount of max enemies per wave
        {
            _enemySpawner.EnemiesKilled = 0;

            _enemySpawner.EnemiesSpawned = 0;

            WaveWon();

            _enumerator = StartNewWave();
            StartCoroutine(_enumerator);
        }*/
    }

    public void WaveWon()
    {
        //waveScreen.SetActive(true);

        //spawner.gameObject.SetActive(false);

        _wave += 1;
    }

    public void Lose()
    {
        _gameOverScreen.SetActive(true);
    }

    public IEnumerator StartNewWave()
    {
        yield return new WaitForSeconds(_timeBetweenWaves);

        _waveScreen.SetActive(false);

        //Actions.OnWaveWon?.Invoke();

        //_enemySpawner.MaxEnemies += _enemiesToAddPerWave;

        _enumerator = null;
    }

    public void Pause()
    {
        _pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void Resume()
    {
        _pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }
}
