using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI woodCounter;
    [SerializeField] private TextMeshProUGUI goldCounter;
    [SerializeField] private TextMeshProUGUI foodCounter;
    [SerializeField] private TextMeshProUGUI woodDisplay;
    [SerializeField] private TextMeshProUGUI goldDisplay;
    [SerializeField] private TextMeshProUGUI foodDisplay;
    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            ResourceUpdate();
        }
    }
    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            ResourceUpdate();
        }
    }


    private void ResourceUpdate()
    {
        woodCounter.text = GameManager.Instance.madera.ToString();
        goldCounter.text = GameManager.Instance.oro.ToString();
        foodCounter.text = GameManager.Instance.comida.ToString();

        woodDisplay.text = $"Wood: {GameManager.Instance.madera.ToString()}";
        goldDisplay.text = $"Coins: {GameManager.Instance.oro.ToString()}";
        foodDisplay.text = $"Food: {GameManager.Instance.comida.ToString()}";
    }
}
