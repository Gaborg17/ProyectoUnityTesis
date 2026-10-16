using UnityEngine;
using UnityEngine.UI;
using Rive;
using Rive.Components;

public class EmojiHealthController : MonoBehaviour
{
    [Header("Rive Settings")]
    [SerializeField] private RiveWidget m_riveWidget;

    [Header("Health Settings")]
    [SerializeField] private Slider healthSlider;
    private int currentHealth = 10; // Máximo por defecto ahora es 10

    private ViewModelInstanceBooleanProperty m_isDeadProp;
    private ViewModelInstanceBooleanProperty m_healthLowProp;
    private ViewModelInstanceBooleanProperty m_healthHalfProp;
    private ViewModelInstanceBooleanProperty m_healthFullProp;

    void OnEnable()
    {
        if (m_riveWidget != null)
        {
            m_riveWidget.OnWidgetStatusChanged += HandleWidgetStatusChanged;
        }

        if (healthSlider != null)
        {
            healthSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    void OnDisable()
    {
        if (m_riveWidget != null)
        {
            m_riveWidget.OnWidgetStatusChanged -= HandleWidgetStatusChanged;
        }

        if (healthSlider != null)
        {
            healthSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }

    void Start()
    {
        CheckWidgetLoaded();
    }

    private void HandleWidgetStatusChanged()
    {
        CheckWidgetLoaded();
    }

    private void CheckWidgetLoaded()
    {
        if (m_riveWidget != null && m_riveWidget.Status == WidgetStatus.Loaded)
        {
            var vmInstance = m_riveWidget.StateMachine?.ViewModelInstance;
            if (vmInstance != null)
            {
                m_isDeadProp = vmInstance.GetBooleanProperty("is_dead");
                m_healthLowProp = vmInstance.GetBooleanProperty("health_low");
                m_healthHalfProp = vmInstance.GetBooleanProperty("health_half");
                m_healthFullProp = vmInstance.GetBooleanProperty("health_full");

                if (healthSlider != null)
                {
                    UpdateEmojiHealth((int)healthSlider.value);
                }
                else
                {
                    UpdateEmojiHealth(currentHealth);
                }
            }
        }
    }

    private void OnSliderValueChanged(float value)
    {
        UpdateEmojiHealth((int)value);
    }

    public void UpdateEmojiHealth(int newHealth)
    {
        currentHealth = newHealth;

        // Nuevos rangos adaptados a escala de 0 a 10:
        if (m_isDeadProp != null)
            m_isDeadProp.Value = (currentHealth <= 0); // Muerto en 0

        if (m_healthLowProp != null)
            m_healthLowProp.Value = (currentHealth > 0 && currentHealth <= 3); // Vida baja (1, 2 o 3)

        if (m_healthHalfProp != null)
            m_healthHalfProp.Value = (currentHealth >= 4 && currentHealth <= 6); // Vida media (4, 5 o 6)

        if (m_healthFullProp != null)
            m_healthFullProp.Value = (currentHealth >= 7); // Vida alta (7 a 10)
    }
}