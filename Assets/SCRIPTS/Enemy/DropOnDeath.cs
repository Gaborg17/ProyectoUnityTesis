using UnityEngine;

public class DropOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject prefabToDrop;

    public void DropLoot(int amount)
    {
        Instantiate(prefabToDrop, transform.position, Quaternion.identity);
    }


}
