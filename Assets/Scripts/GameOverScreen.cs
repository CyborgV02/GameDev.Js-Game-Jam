using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void TryAgain()
    {
        SceneManager.LoadScene("Area1");
    }
}
