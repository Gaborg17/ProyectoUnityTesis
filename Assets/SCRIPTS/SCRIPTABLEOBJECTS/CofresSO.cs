using UnityEngine;

[CreateAssetMenu(fileName = "Cofre", menuName = "Niveles/Cofres")]
public class CofresSO : ScriptableObject
{
    [SerializeField] private Rareza rareza;

    public int minComida;
    public int maxComida;

    public int minOro;
    public int maxOro;

    public int minMadera;
    public int maxMadera;
}


public enum Rareza
{
    Comun, PocoComun, Raro, Epico, Legendario
}
