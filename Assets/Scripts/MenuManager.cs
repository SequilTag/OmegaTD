using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "MenuScene")
        {
            SceneManager.LoadScene("MenuScene");
        } 
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
