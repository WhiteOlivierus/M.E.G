using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject homeScreen;
    public GameObject pauseScreen;
    private SmoothMouseLook sml;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        ShowPauseMenu();
    }

    private void ShowPauseMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1) { return; }

        sml = FindObjectOfType<SmoothMouseLook>();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.activeSelf)
            {
                pauseScreen.SetActive(!pauseScreen.activeSelf);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
                sml.enabled = true;
            }
            else
            {
                pauseScreen.SetActive(!pauseScreen.activeSelf);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                sml.enabled = false;
            }
        }
    }

    public void LoadScene(int index)
    {
        if (index == 0)
            Destroy(gameObject);
        homeScreen.SetActive(false);
        pauseScreen.SetActive(false);
        SceneManager.LoadScene(index);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
