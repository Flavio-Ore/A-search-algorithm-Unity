using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadLevel1()
    {
        SceneManager.LoadScene("LEVEL1-A-STAR");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("LEVEL2-DIJKSTRA");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}