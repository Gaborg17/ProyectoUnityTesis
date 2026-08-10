using UnityEngine;

public class TutorialInputsListener : MonoBehaviour
{
    void OnEnable()
    {
        if (InputManager.Instance == null) return;
        InputManager.Instance.OnJumpPerformed += () => ValidateInput(MissionData.MissionType.Jump);
        InputManager.Instance.OnAttackPerformed += () => ValidateInput(MissionData.MissionType.Attack);
        InputManager.Instance.OnInteractPerformed += () => ValidateInput(MissionData.MissionType.Interact);
        InputManager.Instance.OnMovementPerformed += () => ValidateMovement();
    }
    private void Start()
    {
        InputManager.Instance.OnJumpPerformed += () => ValidateInput(MissionData.MissionType.Jump);
        InputManager.Instance.OnAttackPerformed += () => ValidateInput(MissionData.MissionType.Attack);
        InputManager.Instance.OnInteractPerformed += () => ValidateInput(MissionData.MissionType.Interact);
        InputManager.Instance.OnMovementPerformed += () => ValidateMovement();
    }
    void OnDisable()
    {
        InputManager.Instance.OnJumpPerformed -= () => ValidateInput(MissionData.MissionType.Jump);
        InputManager.Instance.OnAttackPerformed -= () => ValidateInput(MissionData.MissionType.Attack);
        InputManager.Instance.OnInteractPerformed -= () => ValidateInput(MissionData.MissionType.Interact);
        InputManager.Instance.OnMovementPerformed -= () => ValidateMovement();

    }

    private void ValidateInput(MissionData.MissionType pressedKey)
    {
        MissionData actualMission = TutorialManager.Instance.actualMission;
        if (actualMission == null) return;
        if (actualMission != null && actualMission.missionType == pressedKey)
        {
            TutorialManager.Instance.CompleteActiveMission(actualMission.idMission);
        }
    }

    private void ValidateMovement()
    {
        MissionData actualMission = TutorialManager.Instance.actualMission;

        if (actualMission == null) return;
        switch (actualMission.missionType)
        {
            case MissionData.MissionType.WalkFwd:
                if (InputManager.Instance.MoveDirection().y > 0.1f)
                    TutorialManager.Instance.CompleteActiveMission(actualMission.idMission);
                break;
            case MissionData.MissionType.WalkBck:
                if (InputManager.Instance.MoveDirection().y < -0.1f)
                    TutorialManager.Instance.CompleteActiveMission(actualMission.idMission);
                break;
            case MissionData.MissionType.WalkR:
                if (InputManager.Instance.MoveDirection().x > 0.1f)
                    TutorialManager.Instance.CompleteActiveMission(actualMission.idMission);
                break;
            case MissionData.MissionType.WalkL:
                if (InputManager.Instance.MoveDirection().x < -0.1f)
                    TutorialManager.Instance.CompleteActiveMission(actualMission.idMission);
                break;
        }
    }
}
