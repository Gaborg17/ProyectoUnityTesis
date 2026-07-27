using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [Header("PlayerHealth")]
    [SerializeField] private Slider healthBar;

    [Header("Resources")]
    [SerializeField] private TextMeshProUGUI woodCounter;
    [SerializeField] private TextMeshProUGUI goldCounter;
    [SerializeField] private TextMeshProUGUI foodCounter;

    [Header("Allies")]
    [SerializeField] private Image ally1;
    [SerializeField] private Image ally2;
    [SerializeField] private Image ally3;
    [SerializeField] private Image ally4;


    [Header("Bounty")]
    [SerializeField] private TextMeshProUGUI bountyCount;


    private void Start()
    {
        GameManager.Instance.UpdatePlayerHealth += HealthUpdate;
        GameManager.Instance.UpdateResources += ResourceUpdate;
        GameManager.Instance.Allies += AlliesUpdate;
        GameManager.Instance.bountyManager.bountyChanged += BountyUpdate;


        GameManager.Instance.UpdateResourcesUI();
        GameManager.Instance.bountyManager.UpdateBounty();

    }
    private void OnEnable()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.UpdatePlayerHealth += HealthUpdate;
            GameManager.Instance.UpdateResources += ResourceUpdate;
            GameManager.Instance.Allies += AlliesUpdate;
            GameManager.Instance.bountyManager.bountyChanged += BountyUpdate;


            GameManager.Instance.UpdateResourcesUI();
            GameManager.Instance.bountyManager.UpdateBounty();

        }
    }
    private void OnDisable()
    {
        GameManager.Instance.UpdatePlayerHealth -= HealthUpdate;
        GameManager.Instance.UpdateResources -= ResourceUpdate;
        GameManager.Instance.Allies -= AlliesUpdate;
        GameManager.Instance.bountyManager.bountyChanged -= BountyUpdate;
    }

    private void OnDestroy()
    {
        GameManager.Instance.UpdatePlayerHealth -= HealthUpdate;
        GameManager.Instance.UpdateResources -= ResourceUpdate;
        GameManager.Instance.Allies -= AlliesUpdate;
        GameManager.Instance.bountyManager.bountyChanged -= BountyUpdate;
    }

    private void ResourceUpdate()
    {
        woodCounter.text = GameManager.Instance.madera.ToString();
        goldCounter.text = GameManager.Instance.oro.ToString();
        foodCounter.text = GameManager.Instance.comida.ToString();
    }


    private void HealthUpdate(int newHealth)
    {
        healthBar.value = newHealth;
    }

    private void BountyUpdate()
    {
        bountyCount.text = GameManager.Instance.bountyManager.TotalBounty.ToString();
        Debug.Log("New Bounty");
    }

    private void AlliesUpdate()
    {

    }

}
