using UnityEngine;

public class AlliesManager : MonoBehaviour
{
    [SerializeField] private int maxActiveAllies;

    [SerializeField] private GameObject[] AlliesInGame;
    [SerializeField] private GameObject allyPrefab;
    [SerializeField] private Vector2[] allyParameters;
    void Start()
    {
        for (int i = 0; i < maxActiveAllies; i++)
        {
            AlliesInGame[i] = Instantiate(allyPrefab, this.transform);
            AlliesInGame[i].GetComponent<AllyStateMachine>().playerDistance = allyParameters[i].x;
            AlliesInGame[i].GetComponent<AllyStateMachine>().playerSideStep = allyParameters[i].y;
            AlliesInGame[i].SetActive(false);
        }
        UpdateActiveAllies();
        GameManager.Instance.ActiveAllies += UpdateActiveAllies;
    }

    void Update()
    {

    }

    private void OnDisable()
    {
        GameManager.Instance.ActiveAllies -= UpdateActiveAllies;
    }
    private void OnDestroy()
    {
        GameManager.Instance.ActiveAllies -= UpdateActiveAllies;
    }
    public void UpdateActiveAllies()
    {
        for (int i = 0; i < maxActiveAllies; i++)
        {
            if(GameManager.Instance.allies[i] != null)
            {
                AlliesInGame[i].SetActive(true);
            }
        }
    }
}

