using UnityEngine;

public enum TypeOfCollectible
{
    Gold, Food, Wood
}

public class Collectible : MonoBehaviour
{
    [SerializeField] private TypeOfCollectible type;

    public int amountToAdd;

    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {
                case TypeOfCollectible.Gold:
                    gameManager.oro += amountToAdd;
                    break;
                case TypeOfCollectible.Food:
                    gameManager.comida += amountToAdd;
                    break;
                case TypeOfCollectible.Wood:
                    gameManager.madera += amountToAdd;
                    break;
            }

            GameManager.Instance.UpdateResourcesUI();
            Destroy(this.gameObject);
        }
    }
}
