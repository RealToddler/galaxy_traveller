using UnityEngine;

public class GameMode : MonoBehaviour
{
    public bool IsMultiPlayer { get; set; }
    public static GameMode Instance;

    private void Start()
    {
        Instance = this;
    }
}
