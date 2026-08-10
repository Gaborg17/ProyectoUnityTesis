using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{

    public MissionData assignedMission;


    public bool requiresInteraction;
    public bool destroyOnCompleted;

    private bool playerinRange = false;

    void OnEnable()
    {
        if (InputManager.Instance == null) return;
        InputManager.Instance.OnInteractPerformed += TryForInteraction;
        TutorialManager.Instance.OnChangedMission += CheckForRay;
    }

    void OnDisable()
    {
        InputManager.Instance.OnInteractPerformed -= TryForInteraction;
        TutorialManager.Instance.OnChangedMission -= CheckForRay;
    }

    void Start()
    {
        InputManager.Instance.OnInteractPerformed += TryForInteraction;
        TutorialManager.Instance.OnChangedMission += CheckForRay;
        if (TutorialManager.Instance != null)
        {
            CheckForRay();
        }
    }

    void CheckForRay()
    {
        if (TutorialManager.Instance.actualMission == assignedMission && assignedMission.useLightRay)
        {
            GuideRay.Instance.ShowRay(transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || assignedMission == null) return;

        playerinRange = true;

        if (!requiresInteraction)
        {
            Complete();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerinRange = false;
    }

    private void TryForInteraction()
    {
        if (requiresInteraction && playerinRange)
        {
            Complete();
        }
    }

    private void Complete()
    {
        if (TutorialManager.Instance.actualMission == assignedMission)
        {
            GuideRay.Instance.HideRay();
            TutorialManager.Instance.CompleteActiveMission(assignedMission.idMission);

            if (destroyOnCompleted) Destroy(gameObject);
            else this.enabled = false;
        }
    }
}
