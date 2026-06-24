using UnityEngine;
[CreateAssetMenu(fileName = "Carpenter", menuName = "NPCs/Carpenter")]

public class CarpenterSO : AlliesSO
{
    public int minBoatLvlRepair;
    public int maxBoatLvlRepair;
    public override string Description =>
        $"Permite reparar los barcos de nivel {minBoatLvlRepair} hasta los de nivel {maxBoatLvlRepair}";


    public override void OnAddedToTeam()
    {
        GameManager.Instance.carpenter = this;
    }

    public override void OnRemovedOfTeam()
    {
        GameManager.Instance.carpenter = null;
    }
}
