using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenTabern : MonoBehaviour, IInteractable
{
    private bool inRange;

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject cameraTaberna;
    [SerializeField] private GameObject playerCam;

    private void Start()
    {
        InputManager.Instance.OnInteractPerformed += OnInteract;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnInteractPerformed -= OnInteract;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    public void OnInteract()
    {
        if (inRange == false) return;


        cameraTaberna.SetActive(true);
        canvas.SetActive(true);
        playerCam.SetActive(false);
    }
}
