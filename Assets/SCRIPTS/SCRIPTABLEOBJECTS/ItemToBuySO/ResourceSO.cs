using UnityEngine;

[CreateAssetMenu(fileName = "StoreItem", menuName = "Item/ Resource")]

public class ResourceSO : ItemToBuySO
{
    public TypeOfCollectible type;
    public override string Description =>
        $"Buy {amountToBuy} of {ItemName}";
    public override void OnBuy()
    {
        switch (type)
        {
            case TypeOfCollectible.Gold:
                GameManager.Instance.oro += amountToBuy;
                break;
            case TypeOfCollectible.Food:
                GameManager.Instance.comida += amountToBuy;
                break;
            case TypeOfCollectible.Wood:
                GameManager.Instance.madera += amountToBuy;
                break;
        }
    }

    public override void OnRob()
    {
        base.OnRob();
    }
}
