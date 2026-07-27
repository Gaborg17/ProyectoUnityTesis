using UnityEngine;

public abstract class ItemToBuySO : ScriptableObject
{
    public string ItemName;

    public abstract string Description { get; }
    public int amountToBuy;

    public int price;

    public bool canBeRobbed;

    public virtual void OnBuy()
    {

    } 

    public virtual void OnRob()
    {
        if (canBeRobbed == true)
        {
            GameManager.Instance.bountyManager.ChangeBounty(Crime.Steal);
            OnBuy();
        }
    }


}
