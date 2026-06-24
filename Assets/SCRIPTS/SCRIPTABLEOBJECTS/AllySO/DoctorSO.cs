using UnityEngine;
[CreateAssetMenu(fileName = "Doctor", menuName = "NPCs/Doc")]

public class DoctorSO : AlliesSO
{
    public bool canHeallPlayer;
    public bool canHealAllies;
    public bool canRevive;
    public override string Description =>
        $"Puede curar a los aliados";
    public override void AllyAbility()
    {
        
    }

    public override void OnAddedToTeam()
    {
        GameManager.Instance.doctor = this;
    }

    public override void OnRemovedOfTeam()
    {
        GameManager.Instance.doctor = null;
    }
}
