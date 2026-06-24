using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "Warrior", menuName = "NPCs/Warrior")]

public class WarriorSO : AlliesSO
{
    [Range(0f,2f)]
    public float damageMultiplier;
    public override string Description =>
        $"Tienen mas fuerza que los otros tripulantes";
    public override void OnAddedToTeam()
    {
        GameManager.Instance.warriors.Add(this);
    }

    public override void OnRemovedOfTeam()
    {
        GameManager.Instance.warriors.Remove(this);
    }
}
