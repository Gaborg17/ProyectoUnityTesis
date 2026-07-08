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
            bountyChanged?.Invoke();
        }
        else
        {
            Debug.Log("No bounty aassigned");
        }


    }
    
}
