using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    public string nextSceneName = "LEVEL2-DIJKSTRA";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Level Complete! Loading next level...");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}