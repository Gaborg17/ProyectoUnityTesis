using TMPro;
using UnityEngine;

[System.Serializable]
public class AllyFamily
{
    public string name;
    public AlliesSO[] levels;
}

public class RecruitmentManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI allyType;
    [SerializeField] private TextMeshProUGUI allyDescription;
    [SerializeField] private TextMeshProUGUI allyPrice;

    [SerializeField] private AlliesSO actualAlly;

    [SerializeField] private Transform allyDisplayPoint;


    [SerializeField] private AllyFamily[] allyTypes;

    private int currentAllyType = 0;
    private int currentLevel;

    private void OnEnable()
    {
        allyType.text = allyTypes[currentAllyType].name;
        SetInfo(0);
    }

    public void ChangeAllyTypeToDisplay(int typeMod)
    {
        currentAllyType += typeMod;

        if(currentAllyType > 5)
        {
            currentAllyType = 0;
        }
        else if(currentAllyType < 0)
        {
            currentAllyType = 5;
        }


        allyType.text = allyTypes[currentAllyType].name;
        SetInfo(currentLevel);
    }

    public void ChangeLevelToDisplay(int level)
    {
        currentLevel = level;
        SetInfo(level);
    }


    public void SetInfo(int level)
    {
        if(allyTypes[currentAllyType].levels[level] == null)
        {
            actualAlly = null;
            Debug.Log("Fuera del Rango");
            return;
        }

        allyDescription.text = allyTypes[currentAllyType].levels[level].Description;
        allyPrice.text = allyTypes[currentAllyType].levels[level].recruitmentPrice.ToString();
        actualAlly = allyTypes[currentAllyType].levels[level];
        //Change 3D model
    }

    [SerializeField] private GameObject cameraTab;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private GameObject canvasTab;
    public void CloseRecruitmentTab()
    {
        cameraTab.SetActive(false);
        canvasTab.SetActive(false);
        playerCam.SetActive(true);
    }

    public void RecruitAlly()
    {
        if (GameManager.Instance.oro < actualAlly.recruitmentPrice) return;

        if(ContainsAllyType() == true) return;

        GameManager.Instance.AddAlly(actualAlly);


    }

    public bool ContainsAllyType()
    {
        if (actualAlly == null) return false;

        string ally = actualAlly.allyName;
        int limit = actualAlly.maxInTeam;
        int count = 0;

        for(int i = 0; i < GameManager.Instance.allies.Count; i++)
        {
            if(GameManager.Instance.allies[i] != null && GameManager.Instance.allies[i].allyName == ally)
            {
                count++;
                if(count >= limit) return true;
                
            }
        }
        return false;
    }

}
