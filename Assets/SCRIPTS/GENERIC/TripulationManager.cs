using UnityEngine;
using UnityEngine.UI;

public class TripulationManager : MonoBehaviour
{
    public GameObject[] hearts;


    public Transform gridParent;
    private GameManager gm;

    private void OnEnable()
    {
        GameManager.Instance.Tripulation += UpdateShipHealth;
        GameManager.Instance.Tripulation += CargarPersonajes;
        CargarPersonajes();
        UpdateShipHealth();
    }
    private void Start()
    {
        GameManager.Instance.Tripulation += UpdateShipHealth;
        GameManager.Instance.Tripulation += CargarPersonajes;
        gm = GameManager.Instance;
        CargarPersonajes();
        UpdateShipHealth();
    }

    private void OnDisable()
    {
        GameManager.Instance.Tripulation -= UpdateShipHealth;
        GameManager.Instance.Tripulation -= CargarPersonajes;
    }

    private void OnDestroy()
    {
        GameManager.Instance.Tripulation -= UpdateShipHealth;
        GameManager.Instance.Tripulation -= CargarPersonajes;
    }


    [ContextMenu("CargarP")]
    public void CargarPersonajes()
    {
        if (gm == null) return;

        int cantidadAliados = gm.allies.Count;
        int cantidadHijos = gridParent.childCount;
        int limit = gm.maxAllies;


        for (int i = 0; i < cantidadHijos; i++)
        {
            Transform child = gridParent.GetChild(i);
            DragAndDrop item = child.GetComponent<DragAndDrop>();


            if (i < limit)
            {
                AlliesSO data = gm.allies[i];
                if (data != null)
                {
                    item.personajeData = data;
                    item.gameObject.name = data.allyName;
                    item.SetLocked(false);
                }
                else
                {
                    item.personajeData = null;
                    item.gameObject.name = "Empty";
                    
                    item.SetLocked(false);
                }
            }
            else
            {
                
                item.personajeData = null;
                item.gameObject.name = "Locked";
                item.SetLocked(true);
            }


        }
    }


    public void UpdateShipHealth()
    {
        if (gm == null) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(false);

            if(i < gm.shipHealth)
            {
                hearts[i].GetComponent<Image>().color = Color.red;
            }
            else
            {
                hearts[i].GetComponent <Image>().color = Color.black;
            }
        }

        for(int i = 0;i < gm.maxshipHealth; i++)
        {
            hearts[i].SetActive(true);
        }


    }
}
