using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeMenu : BasicMenu
{
    public void StartSinglePlayer()
    {
        GameMode.Instance.IsMultiPlayer = false;
        SceneManager.LoadScene("Loading");
    }
    public void StartMultiPlayer()
    {
        GameMode.Instance.IsMultiPlayer = true;
        SceneManager.LoadScene("Loading");
    }
}