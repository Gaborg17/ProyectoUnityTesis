using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoverBArco : MonoBehaviour
{
    [SerializeField]private Transform barco;
    [SerializeField] private float velocidad;//Velocidad a la que se va a mover el barco

    public Transform destino { get; set; }//Lugar al que se va a mover le barco

    public string islaActual;

    private Vector3 midpoint;
    private bool tryEvent;

    public bool continueMovement = false;

    [SerializeField] private Camera cam;
    [SerializeField] private bool isMoving = false;

    private Coroutine moveCoroutine;

    void Start()
    {
        GameManager.Instance.travelEventHandler.Treasure += TreasureEvent;
        GameManager.Instance.travelEventHandler.marineAttack += MarineAttack;
        GameManager.Instance.travelEventHandler.pirateAttack += PirateAttack;
        GameManager.Instance.travelEventHandler.findCrewmate += FindCrewmateEvent;
        GameManager.Instance.travelEventHandler.noEvent += HideEventDisplay;

        barco.position = GameManager.Instance.boatPosition;
        barco.rotation = GameManager.Instance.boatRotation;
        cam.transform.position = GameManager.Instance.CamPosition;
    }

    
    void Update()
    {
        if (InputManager.Instance.LeftMouseClicked() && isMoving == false)
        {
            Clicked();
        }
    }
    private void OnDisable()
    {
        GameManager.Instance.travelEventHandler.Treasure -= TreasureEvent;
        GameManager.Instance.travelEventHandler.marineAttack -= MarineAttack;
        GameManager.Instance.travelEventHandler.pirateAttack -= PirateAttack;
        GameManager.Instance.travelEventHandler.findCrewmate -= FindCrewmateEvent;
        GameManager.Instance.travelEventHandler.noEvent -= HideEventDisplay;

    }

    private void OnDestroy()
    {
        GameManager.Instance.travelEventHandler.Treasure -= TreasureEvent;
        GameManager.Instance.travelEventHandler.marineAttack -= MarineAttack;
        GameManager.Instance.travelEventHandler.pirateAttack -= PirateAttack;
        GameManager.Instance.travelEventHandler.findCrewmate -= FindCrewmateEvent;
        GameManager.Instance.travelEventHandler.noEvent -= HideEventDisplay;

    }
    private void Clicked()
    {
        
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            IClickable clickable = hit.collider.GetComponent<IClickable>();
            if (clickable != null)
            {
                clickable.OnClick();
                islaActual = destino.parent.gameObject.name;
                GameManager.Instance.islaActual = islaActual;
                midpoint = (barco.position + destino.position) / 2;
                if(moveCoroutine == null)
                {
                    moveCoroutine = StartCoroutine(MoverAIsla());
                }
            }
        }
    }

    public IEnumerator MoverAIsla()
    {
        GameManager.Instance.comida -= GameManager.Instance.CalculateFoodCost();

        while (Vector3.Distance(barco.position, destino.position) > 0.01f)
        {
            if (Vector3.Distance(barco.position, midpoint) < 0.1f && !tryEvent)
            {
                GameManager.Instance.travelEventHandler.CheckForEvent();
                Debug.Log("Intento de Evento");
                yield return new WaitUntil(() => continueMovement == true);
                tryEvent = true;
            }
            isMoving = true;
            barco.LookAt(destino);
            barco.position = Vector3.MoveTowards(barco.position, destino.position, velocidad * Time.deltaTime);

            yield return null;
        }
        barco.position = destino.position;
        GameManager.Instance.boatPosition = destino.position;
        barco.rotation = destino.rotation;
        GameManager.Instance.boatRotation = destino.rotation;
        
        yield return new WaitForSeconds(.3f);
        moveCoroutine = null;
        isMoving = false;
        tryEvent = false;
        continueMovement = false;
        SceneManager.LoadScene("Islas");
    }


    [SerializeField] private GameObject eventDisplay;
    [SerializeField] private TextMeshProUGUI eventName;
    [SerializeField] private TextMeshProUGUI eventDescription;
    [SerializeField] private TextMeshProUGUI onError;
    [SerializeField] private Button leaveButton;
    [SerializeField] private Button acceptButton;

    public void TreasureEvent()
    {
        eventDisplay.SetActive(true);
        eventName.text = "Treasure Found";
        int treasurelvl =  GameManager.Instance.travelEventHandler.GetRandomChest();
        eventDescription.text = $"Tresure of level {treasurelvl} was found";
        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(() =>
        {
            GameManager.Instance.travelEventHandler.OpenChest();
            HideEventDisplay();
        });
        leaveButton.gameObject.SetActive(false);

    }

    public void FindCrewmateEvent()
    {
        eventDisplay.SetActive(true);
        eventName.text = "Castaway Found";
        AlliesSO ally = GameManager.Instance.travelEventHandler.GetRandomCrewmate();
        eventDescription.text = $"You found {ally.name}, their level is {ally.allyLevel}";
        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(() =>
        {
            GameManager.Instance.travelEventHandler.RecruitRandomAlly();
        });
        leaveButton.gameObject.SetActive(true);
        leaveButton.onClick.RemoveAllListeners();
        leaveButton.onClick.AddListener(() =>
        {
            HideEventDisplay();
        });
    }

    public void PirateAttack()
    {
        eventDisplay.SetActive(true);
        eventName.text = "Pirate Attack";
        eventDescription.text = "You are under attack, defend your ship";
        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(() =>
        {
            HideEventDisplay();
        });
        leaveButton.gameObject.SetActive(false);

    }

    public void MarineAttack()
    {
        eventDisplay.SetActive(true);
        eventName.text = "Marine Attack";
        eventDescription.text = "Marines have found you, fight them to survive";
        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(() =>
        {
            HideEventDisplay();

        });
        leaveButton.gameObject.SetActive(false);

    }

    private void HideEventDisplay()
    {
        eventDisplay.SetActive(false);
        continueMovement = true;
    }
}
