using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject activeMenu;
    [SerializeField] private GameObject activeSettingMenu;

    public void ActivateMenu(GameObject menu)
    {
        menu.SetActive(true);
        activeMenu.SetActive(false);
        activeMenu = menu;
    }

    public void ActivateSettingMenu(GameObject menu)
    {
        menu.SetActive(true);
        activeSettingMenu.SetActive(false);
        activeSettingMenu = menu;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartNewGame()
    {

        SceneManager.LoadScene("Islas");
    }

    public void LoadGame()
    {

    }

}
