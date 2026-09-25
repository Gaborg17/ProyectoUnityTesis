using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject activeMenu;
    public GameObject activeSettingsMenu;


    private void Start()
    {
        GameManager.Instance.OnPause += ActivatePauseMenu;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnPause -= ActivatePauseMenu;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnPause -= ActivatePauseMenu;
    }
    public void ActivatePauseMenu()
    {
        PauseMenu.SetActive(GameManager.Instance.IsPaused);
    }
    public void Continue()
    {
        PauseMenu.SetActive(false);
        GameManager.Instance.Pause();
    }

    public void ActivateMenu(GameObject menuToActivate)
    {
        DeactivateMenu();
        menuToActivate.SetActive(true);
        activeMenu = menuToActivate;
    }

    public void DeactivateMenu()
    {
        activeMenu.SetActive(false);
    }

    public void ActivateSubMenu(GameObject menuToActivate)
    {
        DeactivateSubMenu();
        menuToActivate.SetActive(true);
        activeSettingsMenu = menuToActivate;
    }

    public void DeactivateSubMenu()
    {
        activeSettingsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.IsPaused = false;
        GameManager.Instance.showCursor = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
