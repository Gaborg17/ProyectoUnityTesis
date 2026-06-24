using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void Activate(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void Deactivate(GameObject panel)
    {
        panel.SetActive(false);
    }

}
