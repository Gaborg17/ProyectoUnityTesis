using UnityEngine;


[CreateAssetMenu(fileName = "InfoIsla", menuName = "Islas/ InformacionIslas")]
public class IslasSO : ScriptableObject
{
    [SerializeField] private IsleType tipoDeIsla;
    [SerializeField] private int isla;
    public Vector3 posicionDeSpawn;
}


public enum IsleType
{
    IslaDesierta, IslaTesoro, IslaPueblo
}
