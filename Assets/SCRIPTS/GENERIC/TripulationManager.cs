using UnityEngine;

public class TripulationManager : MonoBehaviour
{
    
    public Transform gridParent;       // El objeto con GridLayoutGroup
    private GameManager gm;

    private void OnEnable()
    {
        CargarPersonajes();
    }
    private void Start()
    {
        gm = GameManager.Instance;
        CargarPersonajes();
    }


    [ContextMenu("CargarP")]
    public void CargarPersonajes()
    {
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

}
