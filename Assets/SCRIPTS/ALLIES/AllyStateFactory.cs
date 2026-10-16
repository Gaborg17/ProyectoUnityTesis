using System.Collections.Generic;
using UnityEngine;

enum AllyStates
{
    walk,
    idle,
    combat,
    attack,
    chase,
    retreat
}
public class AllyStateFactory
{
    AllyStateMachine _context;
    Dictionary<AllyStates, AllyBaseState> _states = new Dictionary<AllyStates, AllyBaseState>();
    public AllyStateFactory(AllyStateMachine currentContext)
    {
        _context = currentContext;
        _states[AllyStates.idle] = new AllyIdleState(_context, this);
        _states[AllyStates.walk] = new AllyWalkState(_context, this);
        _states[AllyStates.combat] = new AllyCombatState(_context, this);
        _states[AllyStates.attack] = new AllyAttackState(_context, this);
        _states[AllyStates.chase] = new AllyChaseState(_context, this);
        _states[AllyStates.retreat] = new AllyRetreatState(_context, this);

    }

    public AllyBaseState Idle()
    {
        return _states[AllyStates.idle];
    }
    public AllyBaseState Walk()
    {
        return _states[AllyStates.walk];
    }
    public AllyBaseState Combat()
    {
        return _states[AllyStates.combat];
    }
    public AllyBaseState Attack()
    {
        return _states[AllyStates.attack];
    }
    public AllyBaseState Chase()
    {
        return _states[AllyStates.chase];
    }
    public AllyBaseState Retreat()
    {
        return _states[AllyStates.retreat];
    }
}
