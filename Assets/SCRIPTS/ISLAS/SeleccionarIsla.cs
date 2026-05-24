using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SeleccionarIsla : MonoBehaviour, IClickable
{
    public UnityEvent<Transform> OnIslaSeleccionada;

    public static Dictionary<string, IslasSO> islaACargar = new Dictionary<string, IslasSO>();

    [SerializeField] private string nombreDeIsla;

    public IslasSO IslaSeleccionada;

    private Collider colliderisla;
    public Collider[] islasAccesibles;

    public Material islaDisponibleMaterial;
    public Material islaBloqueadaMaterial;

    private void Awake()
    {
        if (islaACargar.ContainsKey(nombreDeIsla))
        {
            IslaSeleccionada = islaACargar[nombreDeIsla];
        }

    }


    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RemoveFromArray(this.gameObject);

        if (!string.IsNullOrEmpty(nombreDeIsla))
        {
            islaACargar[nombreDeIsla] = IslaSeleccionada;
        }
    }


    private void Start()
    {
        
        colliderisla = GetComponent<Collider>();
        GameManager.Instance.AddToArray(this.gameObject);
        if (GameManager.Instance.islaActual == this.gameObject.name)
        {
            StartCoroutine(ActivarIslasConRetraso());
        }
    }
    public void CambiarValor(IslasSO isla)
    {
        IslaSeleccionada = isla;
        if(!string.IsNullOrEmpty(nombreDeIsla))
            islaACargar[nombreDeIsla] = isla;
    }

    public void OnClick()
    {
        if(IslaSeleccionada == null)
        {
            //Get Random Isle
        }

        GameManager.Instance.islaSeleccionada = IslaSeleccionada;

        ActivarIslas();

        OnIslaSeleccionada?.Invoke(transform.GetChild(0));
    }

    public void ActivarIslas()
    {
        GameManager.Instance.DeactivateIsles(islaBloqueadaMaterial);
        colliderisla.enabled = true;
        foreach(Collider isla in islasAccesibles)
        {
            isla.enabled = true;
            isla.gameObject.GetComponent<MeshRenderer>().material = islaDisponibleMaterial;
            
        }
    }
    private IEnumerator ActivarIslasConRetraso()
    {
        yield return null;
        ActivarIslas();
    }
}
