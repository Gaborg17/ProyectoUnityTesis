using System.Collections.Generic;
using UnityEngine;

public class TutorialLauncher : MonoBehaviour
{
    public List<MissionData> startingMissions;

    public List<MissionData> advancedMissions;


    private void Start()
    {
        TutorialManager.Instance.StartMissionSet(startingMissions);
    }

    public void StartNewMissionSet()
    {
        TutorialManager.Instance.StartMissionSet(advancedMissions);
    }
}
