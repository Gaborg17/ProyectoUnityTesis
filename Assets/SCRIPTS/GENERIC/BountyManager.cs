using System;
using System.Collections.Generic;
using UnityEngine;

public enum Crime
{
    Steal, CivilianAttacked, MarineAttacked
}


public class BountyManager : MonoBehaviour
{


    private static readonly Dictionary<Crime, int> BountyValues = new()
    {
        {Crime.Steal, 200 },
        {Crime.CivilianAttacked, 500},
        {Crime.MarineAttacked, 1000 },
    };

    protected int _totalbounty;

    public int TotalBounty { get { return _totalbounty; } }

    public float eventProbability;
    public event Action bountyChanged;

    private void Start()
    {
        bountyChanged?.Invoke();
    }
    public void ChangeBounty(Crime crime)
    {
        if(BountyValues.TryGetValue(crime, out int bounty))
        {
            _totalbounty += bounty;

            if(_totalbounty < 1000)
            {
                eventProbability = 10f;
            }
            else if(_totalbounty < 50000)
            {
                eventProbability = 20f;
                GameManager.Instance.allowLvl2Allies = true;

            }
            else
            {
                eventProbability = 30f;
                GameManager.Instance.allowLvl3Allies = true;
            }
        }
        else
        {
            Debug.Log("No bounty aassigned");
        }


    }

    public void UpdateBounty()
    {
        bountyChanged?.Invoke();
    }

}
