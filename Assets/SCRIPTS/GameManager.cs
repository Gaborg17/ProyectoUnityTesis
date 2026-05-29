using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Vector3 boatPosition = new Vector3(-1.4f,0f,0f);
    public Vector3 CamPosition = new Vector3(35f,42.5f,0f);
    public Quaternion boatRotation = Quaternion.identity;

    public IslasSO islaSeleccionada;
    public string islaActual;

    public List<GameObject> todasLasIslas = new List<GameObject>();

    public IslasSO[] islasDesiertas;
    public IslasSO[] islasTesoro;
    public IslasSO[] islasPueblo;

    private IslasSO isle;

    [Header("Rercursos")]

    public int comida;
    public int oro;
    public int madera;
    
    public int nTripulantes;

    [Header("EstadisticasActuales")]
    public int valorDeViaje;
    public int recompensa;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this.gameObject);
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

        if(isla != null && !todasLasIslas.Contains(isla))
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
        List<float> isleTypeWeights = new List<float> { 33f, 33f, 33f };

        int index = ProbabilityManager.GetRandomIndex(isleTypeWeights);

        switch (index)
        {
            case 0:
                Debug.Log("TipoDeIsla: Desierta");
                isle =  RandomIsleTypeSelector(islasDesiertas);
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
}
