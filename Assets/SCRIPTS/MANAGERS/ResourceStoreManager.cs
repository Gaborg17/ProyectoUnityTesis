using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceStoreManager : MonoBehaviour
{
    [SerializeField] private ItemToBuySO[] inStoreItems;

    private int itemInDisplay;

    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemPrice;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
        ChangeResourceData();
    }

    public void BuyResources()
    {
        if (gm.oro >= inStoreItems[itemInDisplay].price)
        {
           
            gm.oro -= inStoreItems[itemInDisplay].price;
            inStoreItems[itemInDisplay].OnBuy();

        }


    }

    public void RobResources()
    {
        if(inStoreItems[itemInDisplay].canBeRobbed == true)
        {
            inStoreItems[itemInDisplay].OnRob();
            GameManager.Instance.bountyManager.UpdateBounty();
        }
    }

    public void ChangeLeft()
    {
        itemInDisplay--;
        if (itemInDisplay < 0) itemInDisplay = inStoreItems.Length - 1;
        ChangeResourceData();

    }

    public void ChangeRight()
    {
        itemInDisplay++;
        if (itemInDisplay > inStoreItems.Length - 1) itemInDisplay = 0;
        ChangeResourceData();

    }


    private void ChangeResourceData()
    {
        ItemToBuySO itemData = inStoreItems[itemInDisplay];

        itemName.text = itemData.name;
        itemDescription.text = itemData.Description;
        itemPrice.text = $"${itemData.price}";


    }


    [SerializeField] private GameObject cameraTab;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private GameObject canvasTab;
    [SerializeField] private GameObject playerHUD;
    public void CloseResourcesTab()
    {
        playerHUD.SetActive(true);
        cameraTab.SetActive(false);
        canvasTab.SetActive(false);
        playerCam.SetActive(true);
    }
}
