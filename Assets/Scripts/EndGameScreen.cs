using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScreen : MonoBehaviour
{
    // Start is called before the first frame update    
    public void PlayGame() {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("OurHouse");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
