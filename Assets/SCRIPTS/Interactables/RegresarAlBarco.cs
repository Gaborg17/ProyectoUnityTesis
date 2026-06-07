using UnityEngine;
using UnityEngine.SceneManagement;

public class RegresarAlBarco : MonoBehaviour, IInteractable
{
    private bool inRange;
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
        if (GameManager.Instance.comida < GameManager.Instance.CalculateFoodCost())
        {
            Debug.Log("No tienes suficiente comida");
            return;
        }
        SceneManager.LoadScene("MapaIslas");
    }

}
