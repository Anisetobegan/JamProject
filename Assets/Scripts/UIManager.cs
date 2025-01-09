using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _pauseScreen;
    [SerializeField] GameObject _gameWonScreen;
    [SerializeField] GameObject _gameOverScreen;
    [SerializeField] GameObject _levelStartScreen;
    [SerializeField] TextMeshProUGUI _levelTMP;
    [SerializeField] GameObject _levelClearScreen;

    public static UIManager Instance
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
        
    }

    void Update()
    {
        
    }

    //Pause Screen
    public void OpenPauseScreen()
    {
        _pauseScreen.SetActive(true);
    }

    public void ClosePauseScreen()
    {
        _pauseScreen.SetActive(false);
    }

    //Game Won Screen
    public void OpenGameWonScreen()
    {
        _gameWonScreen.SetActive(true);
    }

    public void CloseGameWonScreen()
    {
        _gameWonScreen.SetActive(false);
    }

    //Game Over Screen
    public void OpenGameOverScreen()
    {
        _gameOverScreen.SetActive(true);
    }

    public void CloseGameOverScreen()
    {
        _gameOverScreen.SetActive(false);
    }

    //Level Start Screen
    public void OpenLevelStartScreen(int level)
    {
        _levelStartScreen.SetActive(true);
        _levelTMP.text = $"Level {level}";
        StartCoroutine(LevelStartScreenTimer());
    }

    public void CloseLevelStartScreen()
    {
        _levelStartScreen.SetActive(false);
    }

    IEnumerator LevelStartScreenTimer()
    {
        yield return new WaitForSeconds(1.5f);
        CloseLevelStartScreen();
    }

    //Level Clear Screen
    public void OpenLevelClearScreen()
    {
        _levelClearScreen.SetActive(true);
        StartCoroutine(LevelClearScreenTimer());
    }

    public void CloseLevelClearScreen()
    {
        _levelClearScreen.SetActive(false);
    }

    IEnumerator LevelClearScreenTimer()
    {
        yield return new WaitForSeconds(1.5f);
        CloseLevelClearScreen();
    }
}
