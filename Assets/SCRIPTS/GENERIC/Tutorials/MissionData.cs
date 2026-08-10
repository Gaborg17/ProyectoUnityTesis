using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Tutorial/Mission")]
public class MissionData : ScriptableObject
{

    public enum MissionType
    {
        Jump, WalkFwd, WalkBck, WalkL, WalkR, Attack, Interact, DefeatEnemy, DefeatNpc, ReachPlace, Collect
    }

    public string idMission;
    public MissionType missionType;
    public bool useLightRay;
    public string keyNeeded;

    public int amountNeeded;

    [TextArea(2,5)]
    public string Description;

}
