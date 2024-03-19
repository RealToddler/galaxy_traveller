using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public void LoadServer() {
        SceneManager.LoadScene("Loading");
    }
}
