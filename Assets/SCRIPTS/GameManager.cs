using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action UpdateResources;
    public event Action<int> UpdatePlayerHealth;
    public event Action Allies;
    public event Action Tripulation;

    public Vector3 boatPosition = new Vector3(-1.4f, 0f, 0f);
    public Vector3 CamPosition = new Vector3(35f, 42.5f, 0f);
    public Quaternion boatRotation = Quaternion.identity;

    public IslasSO islaSeleccionada;
    public string islaActual;

    public List<GameObject> todasLasIslas = new List<GameObject>();

    public IslasSO[] islasDesiertas;
    public IslasSO[] islasTesoro;
    public IslasSO[] islasPueblo;

    private IslasSO isle;


    [Header("ShipData")]
    public BarcoSO actualBoat;
    public int shipHealth;
    public int maxshipHealth;

    [Header("Rercursos")]

    public int comida;
    public int oro;
    public int madera;

    public int nTripulantes;

    public int mapFragments = 0;

    [Header("EstadisticasActuales")]
    public int valorDeViaje;
    public int recompensa;

    [Header("Aliados")]
    public int maxAllies;
    public int baseAllies;

    public bool allowLvl2Allies = false;
    public bool allowLvl3Allies = false;

    public List<AlliesSO> allies;

    public ChefSO chef;
    public NavigatorSO navigator;
    public ArqueologistSO arqueologist;
    public CarpenterSO carpenter;
    public DoctorSO doctor;
    public List<WarriorSO> warriors;

    public BountyManager bountyManager { get; private set; }
    public TravelEventHandler travelEventHandler { get; private set; }

    public bool tutorialCompleted = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this.gameObject);

            bountyManager = GetComponentInChildren<BountyManager>();
            travelEventHandler = GetComponentInChildren<TravelEventHandler>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        todasLasIslas.Clear();
    }
    public void DeactivateIsles(Material material)
    {
        foreach (GameObject obj in todasLasIslas)
        {
            if (obj != null && obj.name != islaActual)
            {
                obj.GetComponent<Collider>().enabled = false;
                obj.GetComponent<MeshRenderer>().material = material;
            }
        }
    }

    public void AddToArray(GameObject isla)
    {

        if (isla != null && !todasLasIslas.Contains(isla))
        {
            todasLasIslas.Add(isla);
        }

    }
    public void RemoveFromArray(GameObject isla)
    {
        todasLasIslas.Remove(isla);
    }

    [ContextMenu("islarandom")]
    public IslasSO RandomIsleSelector()
    {
        int index;
        if (navigator == null)
        {
            List<float> isleTypeWeights = new List<float> { 33f, 33f, 33f };
            index = ProbabilityManager.GetRandomIndex(isleTypeWeights);

        }
        else
        {
            index = ProbabilityManager.GetRandomIndex(navigator.probabilities);
        }


        switch (index)
        {
            case 0:
                Debug.Log("TipoDeIsla: Desierta");
                isle = RandomIsleTypeSelector(islasDesiertas);
                break;
            case 1:
                Debug.Log("TipoDeIsla: Pueblo");
                isle = RandomIsleTypeSelector(islasPueblo);
                break;
            case 2:
                Debug.Log("TipoDeIsla: Tesoro");
                isle = RandomIsleTypeSelector(islasTesoro);
                break;
        }
        return isle;
    }

    public IslasSO RandomIsleTypeSelector(IslasSO[] islasDeTipo)
    {
        int randomTipoDeIsla = Random.Range(0, islasDeTipo.Length);

        return islasDeTipo[randomTipoDeIsla];
    }

    public int CalculateFoodCost()
    {
        if (chef == null)
        {
            return valorDeViaje;
        }
        float total = valorDeViaje - (valorDeViaje * chef.foodCostModifier);

        return (int)total;
    }

    [ContextMenu("UpdateLimit")]
    public void UpdateTripulationLimit()
    {
        maxAllies = baseAllies + actualBoat.maxAllies;
        if (maxAllies < 8)
        {
            for (int i = maxAllies; i < 8 && i < allies.Count; i++)
            {
                if (allies[i] != null)
                {
                    allies[i].OnRemovedOfTeam();
                    allies[i] = null;
                }
            }
        }
    }


    public bool AddAlly(AlliesSO allyToAdd)
    {
        if (allyToAdd == null) return false;
        for (int i = 0; i < maxAllies && i < allies.Count; i++)
        {
            if (allies[i] == null)
            {
                if (oro < allyToAdd.recruitmentPrice) return false;
                oro -= allyToAdd.recruitmentPrice;
                allies[i] = allyToAdd;
                allyToAdd.OnAddedToTeam();
                return true;
            }
        }
        return false;
    }

    public void RemoveAlly(AlliesSO allyToRemove)
    {
        if (allyToRemove == null) return;
        int index = allies.IndexOf(allyToRemove);
        if (index != -1 && index < maxAllies)
        {
            allies[index] = null;
            allyToRemove.OnRemovedOfTeam();

        }
    }


    public void UpdateHealthUI(int health)
    {
        UpdatePlayerHealth?.Invoke(health);
    }
    public void UpdateResourcesUI()
    {
        UpdateResources?.Invoke();
    }
    public void UpdateAlliesUI()
    {
        Allies?.Invoke();
    }


    public void UpdateTripulationTab()
    {
        Tripulation?.Invoke();
    }
}
