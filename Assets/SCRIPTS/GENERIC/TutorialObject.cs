using UnityEngine;

public class TutorialObject : MonoBehaviour
{
    private Tutorial tutorial;
    [Header("OnKill")]
    [SerializeField] private bool isKillable;

    [Header("OnInteract")]
    [SerializeField] private bool isInteractable;


    private void Start()
    {
        tutorial = FindAnyObjectByType<Tutorial>();

        if (isInteractable)
        {
            InputManager.Instance.OnInteractPerformed += WhenInteracted;
        }
    }
    private void OnEnable()
    {
        if (isInteractable)
        {
            InputManager.Instance.OnInteractPerformed += WhenInteracted;
        }
    }
    private void OnDisable()
    {
        if (isInteractable)
        {
            InputManager.Instance.OnInteractPerformed -= WhenInteracted;
        }
        if (isKillable)
        {
            WhenKilled();
        }
    }
    private void OnDestroy()
    {
        if (isInteractable)
        {
            InputManager.Instance.OnInteractPerformed -= WhenInteracted;
        }
        if (isKillable)
        {
            WhenKilled();
        }
    }

    private void WhenInteracted()
    {
        tutorial.CompleteInteraction();
        InputManager.Instance.OnInteractPerformed -= WhenInteracted;
    }

    private void WhenKilled()
    {
        tutorial.AddKill();
    }
}
