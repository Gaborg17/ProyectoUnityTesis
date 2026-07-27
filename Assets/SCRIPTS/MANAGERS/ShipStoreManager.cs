using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipStoreManager : MonoBehaviour
{
    [SerializeField] private BarcoSO[] ships;

    [SerializeField] private TextMeshProUGUI shipName;
    [SerializeField] private TextMeshProUGUI shipCrewmates;
    [SerializeField] private TextMeshProUGUI shipLives;
    [SerializeField] private TextMeshProUGUI shipBonus;
    [SerializeField] private TextMeshProUGUI shipPrice;

    [SerializeField] private Button buyButton;

    private int shipInDisplay = 0;

    private GameManager gm; 

    private void Start()
    {
        gm = GameManager.Instance;
        ChangeShipData();
        CheckIfOwned();
    }

    public void BuyShip()
    {
        if(gm.oro >= ships[shipInDisplay].inStorePrice)
        {
            gm.actualBoat = ships[shipInDisplay];
            gm.oro -= ships[shipInDisplay].inStorePrice;
            gm.UpdateTripulationLimit();
            gm.maxshipHealth = ships[shipInDisplay].boatHealth;
            gm.shipHealth = gm.maxshipHealth;
        }

        
    }

    public void ChangeLeft()
    {
        shipInDisplay--;
        if (shipInDisplay < 0) shipInDisplay = ships.Length -1;
        ChangeShipData();

        CheckIfOwned();
    }

    public void ChangeRight()
    {
        shipInDisplay++;
        if (shipInDisplay > ships.Length -1) shipInDisplay = 0;
        ChangeShipData();

        CheckIfOwned();
    }

    private void CheckIfOwned()
    {
        if (ships[shipInDisplay].wasOwned == true || ships[shipInDisplay].boatNumber <= gm.actualBoat.boatNumber)
        {
            buyButton.enabled = false;
        }
        else
        {
            buyButton.enabled = true;
        }
    }

    private void ChangeShipData()
    {
        BarcoSO shipData = ships[shipInDisplay];

        shipName.text = shipData.boatName;
        shipCrewmates.text = $"Crewmate Slots: {shipData.maxAllies}";
        shipLives.text = $"Lives: {shipData.boatHealth}";
        shipBonus.text = "";
        shipPrice.text = $"${shipData.inStorePrice}";
    }


    [SerializeField] private GameObject cameraTab;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private GameObject canvasTab;
    [SerializeField] private GameObject playerHUD;
    public void CloseRecruitmentTab()
    {
        playerHUD.SetActive(true);
        cameraTab.SetActive(false);
        canvasTab.SetActive(false);
        playerCam.SetActive(true);
    }
}
