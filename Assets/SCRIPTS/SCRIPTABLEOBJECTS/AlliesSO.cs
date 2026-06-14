using UnityEngine;
using System.Collections.Generic;


public abstract class AlliesSO : ScriptableObject
{

    public string allyName;
    public int allyLevel;

    public int health;
    public int maxHealth;

    public float moveSpeed;

    public int recruitmentPrice;

    public int damage;

    public abstract string Description { get; }



    public virtual void AllyAbility()
    {

    }

}

