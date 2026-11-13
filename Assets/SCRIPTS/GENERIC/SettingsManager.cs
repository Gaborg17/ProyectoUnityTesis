using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public bool showFps = false;
    public bool windowed = false;
    public bool vsync = false;

    public int resolutionIndex;
    public int fpsIndex;
    public int targetFPS;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
