using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{

    [HideInInspector] public AlliesSO personajeData;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform parentContainer;
    private int originalSiblingIndex;
    private bool isLocked = false;
    private bool isDragging = false;
    private Vector2 originalAnchoredPosition;

    [Header("Context Menu")]
    [SerializeField] private GameObject contextMenuPrefab;
    private GameObject currentMenuInstance;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging || isLocked || personajeData == null)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            CloseContextMenu();

            
            if (contextMenuPrefab != null)
            {
                currentMenuInstance = Instantiate(contextMenuPrefab, GetComponentInParent<Canvas>().transform);
                RectTransform menuRect = currentMenuInstance.GetComponent<RectTransform>();
                Vector2 localPoint;
                RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, null, out localPoint))
                {
                    menuRect.anchoredPosition = ClampPositionToScreen(localPoint, menuRect.rect.size, canvasRect.rect.size);
                }

                ConfigurarMenu(currentMenuInstance);
            }
        }
    }

    private Vector2 ClampPositionToScreen(Vector2 pos, Vector2 menuSize, Vector2 screenSize)
    {
        float x = Mathf.Clamp(pos.x, -screenSize.x / 2 + menuSize.x / 2, screenSize.x / 2 - menuSize.x / 2);
        float y = Mathf.Clamp(pos.y, -screenSize.y / 2 + menuSize.y / 2, screenSize.y / 2 - menuSize.y / 2);
        return new Vector2(x, y);
    }

    private void ConfigurarMenu(GameObject menu)
    {
        Button btnExpulsar = menu.transform.Find("Abandon")?.GetComponent<Button>();
        Button btnHabilidad = menu.transform.Find("AllyAbilityUse")?.GetComponent<Button>();

        if (btnExpulsar != null)
        {
            btnExpulsar.onClick.RemoveAllListeners();
            btnExpulsar.onClick.AddListener(() =>
            {
                AbandonAlly();
                ActualizarLista();
            });
        }

        if (btnHabilidad != null)
        {
            btnHabilidad.onClick.RemoveAllListeners();
            btnHabilidad.onClick.AddListener(() =>
            {
                AllyAbilityUse();
                CloseContextMenu();
            });
        }
    }

    private void AbandonAlly()
    {
        CloseContextMenu();
        if (personajeData == null) return;

        GameManager.Instance.RemoveAlly(personajeData);
        GameManager.Instance.UpdateTripulationTab();
    }

    private void AllyAbilityUse()
    {
        CloseContextMenu();
        if (personajeData == null) return;

        personajeData.AllyAbility();
    }

    private void CloseContextMenu()
    {
        if (currentMenuInstance != null)
        {
            Destroy(currentMenuInstance);
            currentMenuInstance = null;
        }
    }

    private void OnDestroy()
    {
        CloseContextMenu();
    }
}
