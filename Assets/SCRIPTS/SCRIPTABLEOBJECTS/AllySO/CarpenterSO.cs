using UnityEngine;
[CreateAssetMenu(fileName = "Carpenter", menuName = "NPCs/Carpenter")]

public class CarpenterSO : AlliesSO
{
    public int minBoatLvlRepair;
    public int maxBoatLvlRepair;
    public override string Description =>
        $"Permite reparar los barcos de nivel {minBoatLvlRepair} hasta los de nivel {maxBoatLvlRepair}";
}
