using System.Collections;
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
    [SerializeField] private TextMeshProUGUI messageDisplayed;

    private Coroutine messageToShow;

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
            if (messageToShow == null)
            {
                messageToShow = StartCoroutine(DisplayMessage($"{inStoreItems[itemInDisplay].name} was bought!"));
            }
        }
        else
        {
            if (messageToShow == null)
            {
                messageToShow = StartCoroutine(DisplayMessage($"You don't have enough money"));
            }
        }


    }

    public void RobResources()
    {
        if(inStoreItems[itemInDisplay].canBeRobbed == true)
        {
            inStoreItems[itemInDisplay].OnRob();
            GameManager.Instance.bountyManager.UpdateBounty();
            if (messageToShow == null)
            {
                messageToShow = StartCoroutine(DisplayMessage($"You took {inStoreItems[itemInDisplay].name}, your bounty has raised to {GameManager.Instance.bountyManager.TotalBounty}"));
            }
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


    public IEnumerator DisplayMessage(string messageText)
    {
        messageDisplayed.gameObject.SetActive(true);
        messageDisplayed.text = messageText;
        yield return new WaitForSeconds(0.8f);
        messageDisplayed.gameObject.SetActive(false);
        messageToShow = null;
    }
}
