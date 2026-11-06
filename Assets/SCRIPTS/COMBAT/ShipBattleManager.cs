using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipBattleManager : MonoBehaviour
{
    public int enemiesToDefeat;
    public int defeatedEnemies;

    private void Start()
    {
        defeatedEnemies = 0;
    }
    private void Update()
    {
        if (defeatedEnemies >= enemiesToDefeat)
        {
            EndBattle();
        }
    }
    public void EndBattle()
    {
        SceneManager.LoadScene("Islas");
    }

}
