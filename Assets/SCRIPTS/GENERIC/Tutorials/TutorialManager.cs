using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    public event Action OnChangedMission;

    [SerializeField] private TextMeshProUGUI missionName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject tutorialPanel;

    private List<MissionData> activeMissions = new List<MissionData>();
    private int actualindex;
    private bool activeTutorial;

    public MissionData actualMission => (activeTutorial && actualindex < activeMissions.Count)? activeMissions[actualindex]: null;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        tutorialPanel.SetActive(false);
    }

    public void StartMissionSet(List<MissionData> newSet)
    {
        if (newSet == null || newSet.Count == 0) return;

        activeMissions = newSet;
        actualindex = 0;
        activeTutorial = true;
        tutorialPanel.SetActive(true);

        UpdatePanel();
        OnChangedMission?.Invoke();
    }

    public void CompleteActiveMission(string missionIDVerification)
    {
        if (!activeTutorial || actualMission == null) return;

        if(actualMission.idMission == missionIDVerification)
        {
            NextStep();
        }
    }

    private void NextStep()
    {
        actualindex++;

        if(actualindex >= activeMissions.Count)
        {
            EndActualTutorial();
        }
        else
        {
            UpdatePanel();
            OnChangedMission?.Invoke();
        }
    }

    private void UpdatePanel()
    {
        if(actualMission != null)
        {
            //missionName.text = actualMission.name;
            description.text = actualMission.Description;

            if(actualMission.missionType == MissionData.MissionType.DefeatNpc || actualMission.missionType == MissionData.MissionType.DefeatNpc)
            {
                description.text = string.Format(actualMission.Description, actualKills);
            }

            if (string.IsNullOrEmpty(actualMission.keyNeeded)) return;
            System.Reflection.FieldInfo field = typeof(InputManager)?.GetField(actualMission.keyNeeded);
            description.text = string.Format(actualMission.Description, field.GetValue(InputManager.Instance));
        }
    }

    private void EndActualTutorial()
    {
        activeTutorial = false;
        tutorialPanel.SetActive(false);
        activeMissions.Clear();
        if (GuideRay.Instance != null) GuideRay.Instance.HideRay();
    }

    private int actualKills = 0;
    
    public void DefeatMissionValidate()
    {
        if(actualMission.missionType == MissionData.MissionType.DefeatNpc || actualMission.missionType == MissionData.MissionType.DefeatNpc)
        {
            actualKills++;
            UpdatePanel();

            if(actualKills == actualMission.amountNeeded)
            {
                CompleteActiveMission(actualMission.idMission);
                actualKills = 0;
            }
        }
    }
}
