using UnityEngine;

[CreateAssetMenu(fileName = "StoreItem", menuName = "Item/ Weapon")]
public class WeaponSO : ItemToBuySO
{
    public int WeaponLvl;
    public override string Description =>
        $"You can kill enemies with this weapon";
    public override void OnBuy()
    {

    }

    public override void OnRob()
    {
        base.OnRob();
    }
}
