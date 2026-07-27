using UnityEngine;
[CreateAssetMenu(fileName = "Carpenter", menuName = "NPCs/Carpenter")]

public class CarpenterSO : AlliesSO
{
    public int minBoatLvlRepair;
    public int maxBoatLvlRepair;
    public override string Description =>
        $"Permite reparar los barcos de nivel {minBoatLvlRepair} hasta los de nivel {maxBoatLvlRepair}";

    public override void AllyAbility()
    {
        GameManager gm = GameManager.Instance;
        if (gm.shipHealth == gm.maxshipHealth) return;

        if(gm.actualBoat.boatNumber <= maxBoatLvlRepair && gm.madera >= gm.actualBoat.repairCost)
        {
            gm.madera -= gm.actualBoat.repairCost;
            GameManager.Instance.UpdateResourcesUI();
            gm.shipHealth++;
            GameManager.Instance.UpdateTripulationTab();
        }
    }

    public override void OnAddedToTeam()
    {
        GameManager.Instance.carpenter = this;
    }

    public override void OnRemovedOfTeam()
    {
        GameManager.Instance.carpenter = null;
    }
}
