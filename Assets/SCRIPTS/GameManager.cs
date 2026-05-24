using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Vector3 boatPosition = new Vector3(-1.4f,0f,0f);
    public Quaternion boatRotation = Quaternion.identity;

    public IslasSO islaSeleccionada;
    public string islaActual;

    public List<GameObject> todasLasIslas = new List<GameObject>();

    public IslasSO[] islasDesiertas;
    public IslasSO[] islasTesoro;
    public IslasSO[] islasPueblo;


    

    private void Awake()
    {
        if(Instance == null)
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
}
