using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Dialogue/Character Data")]
public class CharacterData : ScriptableObject
{
    // Agrega aquí todas las facciones o roles que necesites
    public enum Faction
    {
        Ally,
        Enemy,
        Villager,   // Aldeanos
        Merchant,   // Comerciantes
        Neutral     // Personaje equis / Extra
    }

    public string characterName;
    public Sprite portrait;
    public Faction faction;
}