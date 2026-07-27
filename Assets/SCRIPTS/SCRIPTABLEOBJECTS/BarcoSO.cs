using UnityEngine;
[CreateAssetMenu(fileName = "Barco", menuName = "Barco/ BData")]
public class BarcoSO : ScriptableObject
{
    public string boatName;
    public int boatNumber;

    public int boatHealth;

    public int maxAllies;

    public bool canBeAttacked;
    public bool wasOwned;

    public int inStorePrice;

    public int repairCost;




}
