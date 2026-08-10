using UnityEngine;

public class GuideRay : MonoBehaviour
{
    public static GuideRay Instance { get; private set; }

    [Header("Configuración del Rayo")]
    public float rayHeight = 20f;
    public float rayWidth = 0.3f;
    public Material rayMaterial;

    private LineRenderer lineRenderer;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        SetLineRenderer();
        HideRay();
    }

    void SetLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;

        if (rayMaterial != null)
            lineRenderer.material = rayMaterial;
    }

    public void ShowRay(Vector3 objectivePosition)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, objectivePosition);
        lineRenderer.SetPosition(1, objectivePosition + Vector3.up * rayHeight);
    }

    public void HideRay()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }
}
