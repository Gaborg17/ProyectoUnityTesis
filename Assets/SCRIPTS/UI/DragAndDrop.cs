using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [HideInInspector] public AlliesSO personajeData; // Asignar al instanciar

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform parentContainer;
    private int originalSiblingIndex;
    private bool isLocked = false;
    private bool isDragging = false;
    private Vector2 originalAnchoredPosition;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLocked == true || personajeData == null)
        {
            eventData.Use();
            isDragging = false;
            return;
        }

        isDragging = true;
        originalAnchoredPosition = rectTransform.anchoredPosition;
        originalSiblingIndex = transform.GetSiblingIndex();
        parentContainer = transform.parent;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (isLocked == true|| personajeData == null)
        {
            rectTransform.anchoredPosition = originalAnchoredPosition;
            transform.SetSiblingIndex(originalSiblingIndex);
            return;
        }

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool placed = false;

        foreach (var hit in results)
        {
            DragAndDrop target = hit.gameObject.GetComponent<DragAndDrop>();
            if (target != null && target != this && !target.isLocked)
            {
                int targetIndex = target.transform.GetSiblingIndex();
                if (targetIndex < GameManager.Instance.maxAllies)
                {
                    int originalIndex = transform.GetSiblingIndex();
                    transform.SetSiblingIndex(targetIndex);
                    target.transform.SetSiblingIndex(originalIndex);

                    ActualizarLista();
                    placed = true;
                    break;
                }
            }
        }

        if (!placed)
        {
            rectTransform.anchoredPosition = originalAnchoredPosition;
            transform.SetSiblingIndex(originalSiblingIndex);
        }
        isDragging = false;
    }

    private void ActualizarLista()
    {
        GameManager gm = GameManager.Instance;
        int max = gm.maxAllies;

        List<AlliesSO> nuevaLista = new List<AlliesSO>(gm.allies);

        
        for (int i = 0; i < parentContainer.childCount && i < max; i++)
        {
            Transform child = parentContainer.GetChild(i);
            DragAndDrop item = child.GetComponent<DragAndDrop>();
            if (item != null && item.personajeData != null)
            {
                nuevaLista[i] = item.personajeData;
            }
            else
            {
                nuevaLista[i] = null;
            }
        }

        gm.allies = nuevaLista;
    }

    public void SetLocked(bool locked)
    {
        isLocked = locked;

        Image img = GetComponent<Image>();
        if(isLocked == true)
        {
            img.color = Color.darkSlateGray;
        }
        else if (isLocked == false && personajeData == null)
        {
            img.color = Color.black;
        }

        else
        {
            img.color= Color.cyan;
        }
        
    }
}
