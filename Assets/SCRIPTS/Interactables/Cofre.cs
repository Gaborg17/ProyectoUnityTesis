using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour, IInteractable
{
    private List<float> possibleTreasures;

    private bool inRange = false;

    [SerializeField]private CofresSO[] nivelCofre;
    private CofresSO cofre;
    private GameManager gameManager;

    private void Start()
    {
        InputManager.Instance.OnInteractPerformed += OnInteract;
        gameManager = GameManager.Instance;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnInteractPerformed -= OnInteract;
    }

    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    public void OnInteract()
    {
        if (inRange == false) return;
        if(GameManager.Instance.arqueologist == null)
        {
            possibleTreasures = new List<float> { 70f, 20f, 6f, 3f, 1f };
        }
        else
        {
            possibleTreasures = GameManager.Instance.arqueologist.probabilities;
        }


        int tresureLvl = ProbabilityManager.GetRandomIndex(possibleTreasures);
        cofre = nivelCofre[tresureLvl];
        Debug.Log(cofre);
        GetTreasureRewards(cofre);


        //InputManager.Instance.OnInteractPerformed -= OnInteract;
    }


    public void GetTreasureRewards(CofresSO cofreSeleccionado)
    {
        int comida = Random.Range(cofreSeleccionado.minComida, cofreSeleccionado.maxComida + 1);
        int oro = Random.Range(cofreSeleccionado.minOro, cofreSeleccionado.maxOro + 1);
        int madera = Random.Range(cofreSeleccionado.minMadera, cofreSeleccionado.maxMadera + 1);

        Debug.Log($"Comida {comida}, Oro {oro}, Madera {madera}");
        


        gameManager.comida += comida;
        gameManager.oro += oro;
        gameManager.madera += madera;

    }
}
