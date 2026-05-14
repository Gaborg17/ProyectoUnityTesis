using UnityEngine;
using UnityEngine.Events;

public class SeleccionarIsla : MonoBehaviour, IClickable
{
    public UnityEvent<Transform> OnIslaSeleccionada;
    public void OnClick()
    {
        OnIslaSeleccionada?.Invoke(transform.GetChild(0));
    }



}
