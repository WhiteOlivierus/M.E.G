using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject homeScreen;
    public GameObject pauseScreen;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseScreen.SetActive(!pauseScreen.activeSelf);
        }
    }

    public void LoadScene(int index)
    {
        homeScreen.SetActive(false);
        pauseScreen.SetActive(false);
        SceneManager.LoadScene(index);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
