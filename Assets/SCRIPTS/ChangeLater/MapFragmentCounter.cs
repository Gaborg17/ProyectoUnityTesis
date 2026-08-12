using TMPro;
using UnityEngine;

public class MapFragmentCounter : MonoBehaviour
{
    private TextMeshProUGUI count;

    private void Start()
    {
        count = GetComponent<TextMeshProUGUI>();
        count.text = $"Map fragments {GameManager.Instance.mapFragments}/3";
    }

}
