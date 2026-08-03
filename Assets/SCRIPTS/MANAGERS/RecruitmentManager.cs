using System.Collections;
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
    [SerializeField] private TextMeshProUGUI Message;
    [SerializeField] private TextMeshProUGUI GoldCount;

    [SerializeField] private AlliesSO actualAlly;

    [SerializeField] private Transform allyDisplayPoint;


    [SerializeField] private AllyFamily[] allyTypes;

    private int currentAllyType = 0;
    private int currentLevel;

    private Coroutine messageCoroutine;
    private void OnEnable()
    {
        allyType.text = allyTypes[currentAllyType].name;
        GoldCount.text = $"Gold: {GameManager.Instance.oro}";
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
    [SerializeField] private GameObject playerHUD;
    public void CloseRecruitmentTab()
    {
        playerHUD.SetActive(true);
        cameraTab.SetActive(false);
        canvasTab.SetActive(false);
        playerCam.SetActive(true);
    }

    public void RecruitAlly()
    {
        if(GameManager.Instance.maxAllies == 0)
        {
            if (messageCoroutine == null)
            {
                messageCoroutine = StartCoroutine(ShowMessage($"You need to get a better boat to recruit someone"));
            }
            return;
        }

        if (GameManager.Instance.oro < actualAlly.recruitmentPrice)
        { 
            if(messageCoroutine == null)
            {
                messageCoroutine = StartCoroutine(ShowMessage($"You don't have enough money"));
            }
            return; 
        }

        if (actualAlly.allyLevel > 1 && !GameManager.Instance.allowLvl2Allies)
        {
            if (messageCoroutine == null)
            {
                messageCoroutine = StartCoroutine(ShowMessage($"You need a higher bounty to recruit a Level 2 or higher {actualAlly.allyName}"));
            }
            return;
        }

        if (actualAlly.allyLevel > 2 && !GameManager.Instance.allowLvl3Allies)
        {
            if (messageCoroutine == null)
            {
                messageCoroutine = StartCoroutine(ShowMessage($"You need a higher bounty to recruit a Level 3 {actualAlly.allyName}"));
            }
            return;
        }

        if (ContainsAllyType() == true)
        {
            if (messageCoroutine == null)
            {
                messageCoroutine = StartCoroutine(ShowMessage($"You already have a {actualAlly.allyName} in your crew"));
            }
            return;
        }


        GameManager.Instance.AddAlly(actualAlly);
        GoldCount.text = $"Gold: {GameManager.Instance.oro}";
        if (messageCoroutine == null && ContainsAllyType() == true)
        {
            messageCoroutine = StartCoroutine(ShowMessage($"{actualAlly.allyName} has been recruited!!"));
        }


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


    public IEnumerator ShowMessage(string message)
    {
        Message.text = message;
        Message.gameObject.SetActive(true);
        yield return new WaitForSeconds(.8f);
        Message.gameObject.SetActive(false);
        messageCoroutine = null;
    }


}
