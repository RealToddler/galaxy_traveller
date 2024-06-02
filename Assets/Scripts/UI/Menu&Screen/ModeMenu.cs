using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeMenu : BasicMenu
{
    private GameMode _gameMode;
    private void Start()
    {
        _gameMode = GameObject.Find("GameMode").GetComponent<GameMode>();
    }

    public void StartSinglePlayer()
    {
        _gameMode.IsMultiPlayer = false;
        SceneManager.LoadScene("Loading");
    }
    public void StartMultiPlayer()
    {
        _gameMode.IsMultiPlayer = true;
        SceneManager.LoadScene("Loading");
    }
}