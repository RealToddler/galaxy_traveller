using UnityEngine.SceneManagement;

public class ModeMenu : BasicMenu
{
    public void ToFirstLvl()
    {
        SceneManager.LoadScene("Lvl1");
    }
}