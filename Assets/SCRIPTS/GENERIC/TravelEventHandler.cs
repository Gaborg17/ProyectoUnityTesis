using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EventType
{
    FindCrewmate, PirateAttack, MarineAttack, Treasure, None
}

public class TravelEventHandler : MonoBehaviour
{
    private List<float> probabilities;


    public event Action findCrewmate;
    public event Action pirateAttack;
    public event Action marineAttack;
    public event Action Treasure;
    public event Action noEvent;


    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
    }


    public void CheckForEvent()
    {

        if (gm.actualBoat.boatName == "Bote")
        {
            probabilities = new List<float> { 0f, 0f, 0f, 10f, 90f };

        }
        else
        {
            probabilities = new List<float> { 10f, 20f, gm.bountyManager.eventProbability, 10f, 60f - gm.bountyManager.eventProbability };
        }


        EventType selectedEvent = (EventType)ProbabilityManager.GetRandomIndex(probabilities);

        switch (selectedEvent)
        {
            case EventType.None:
                noEvent?.Invoke();
                break;
            case EventType.FindCrewmate:
                findCrewmate?.Invoke();
                break;
            case EventType.PirateAttack:
                pirateAttack?.Invoke();
                break;
            case EventType.MarineAttack:
                marineAttack?.Invoke();
                break;
            case EventType.Treasure:
                Treasure?.Invoke();
                break;
        }

        Debug.Log(selectedEvent);
    }


    private List<float> possibleTreasures;
    [Header("Chest")]
    [SerializeField] private CofresSO[] nivelCofre;
    private CofresSO cofre;

    public int GetRandomChest()
    {
        possibleTreasures = new List<float> { 70f, 20f, 6f, 3f, 1f };

        int tresureLvl = ProbabilityManager.GetRandomIndex(possibleTreasures);
        cofre = nivelCofre[tresureLvl];

        return tresureLvl;
    }
    public void OpenChest()
    {
        GetTreasureRewards(cofre);

    }


    public void GetTreasureRewards(CofresSO cofreSeleccionado)
    {
        int comida = Random.Range(cofreSeleccionado.minComida, cofreSeleccionado.maxComida + 1);
        int oro = Random.Range(cofreSeleccionado.minOro, cofreSeleccionado.maxOro + 1);
        int madera = Random.Range(cofreSeleccionado.minMadera, cofreSeleccionado.maxMadera + 1);

        Debug.Log($"Comida {comida}, Oro {oro}, Madera {madera}");


        gm.comida += comida;
        gm.oro += oro;
        gm.madera += madera;

        GameManager.Instance.UpdateResourcesUI();
    }

    private List<float> possibleCrewmate;
    private List<float> crewmateLvl;

    private AlliesSO randomAlly;

    [SerializeField] private AllyFamily[] allyTypes;
    public AlliesSO GetRandomCrewmate()
    {
        possibleCrewmate = new List<float> { 16.67f, 16.67f, 16.67f, 16.67f/*, 16.67f, 16.67f*/ };
        crewmateLvl = new List<float> { 60f, 30f, 10f };

        int ally = ProbabilityManager.GetRandomIndex(possibleCrewmate);
        int lvl = ProbabilityManager.GetRandomIndex(crewmateLvl);

        randomAlly = allyTypes[ally].levels[lvl];
        return randomAlly;
    }


    public void RecruitRandomAlly()
    {

        if (randomAlly.allyLevel > 1 && !GameManager.Instance.allowLvl2Allies) return;

        if (randomAlly.allyLevel > 2 && !GameManager.Instance.allowLvl3Allies) return;

        if (ContainsAllyType() == true) return;

        GameManager.Instance.AddAlly(randomAlly);
    }


    public bool ContainsAllyType()
    {
        if (randomAlly == null) return false;

        string ally = randomAlly.allyName;
        int limit = randomAlly.maxInTeam;
        int count = 0;

        for (int i = 0; i < GameManager.Instance.allies.Count; i++)
        {
            if (GameManager.Instance.allies[i] != null && GameManager.Instance.allies[i].allyName == ally)
            {
                count++;
                if (count >= limit) return true;

            }
        }
        return false;
    }
}
