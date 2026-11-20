using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTest : MonoBehaviour
{
    [Header("Referencia al UI Controller")]
    [SerializeField] private DialogueUIController dialogueUI;

    [System.Serializable]
    public struct DialogueTestEntry
    {
        [Tooltip("Nombre de referencia para identificar este diálogo en el inspector")]
        public string testName;

        [Header("Datos del Diálogo")]
        public CharacterData character;
        [TextArea(2, 5)] public string text;

        [Header("Tecla para activar")]
        public Key keyToPress;
    }

    [Header("Lista de Diálogos de Prueba")]
    [SerializeField] private List<DialogueTestEntry> testDialogues = new List<DialogueTestEntry>();

    [Header("Tecla para Ocultar Panel")]
    [SerializeField] private Key hidePanelKey = Key.Digit4;

    private void Update()
    {
        if (Keyboard.current == null || dialogueUI == null) return;

        // Recorrer la lista configurable desde el Inspector
        foreach (var entry in testDialogues)
        {
            if (entry.character != null && Keyboard.current[entry.keyToPress].wasPressedThisFrame)
            {
                dialogueUI.ShowDialogue(entry.character, entry.text);
            }
        }

        // Tecla asignada para ocultar/cerrar el panel de diálogo
        if (Keyboard.current[hidePanelKey].wasPressedThisFrame)
        {
            dialogueUI.HideDialogue();
        }
    }
}