using System.Collections.Generic;
using UnityEngine;

enum NpcStates
{
    idle,
    walk,
    escaping
}
public class NpcStateFactory
{
    NpcStateMachine _context;
    Dictionary<NpcStates, NpcBaseState> _states = new Dictionary<NpcStates, NpcBaseState>();
    public NpcStateFactory(NpcStateMachine currentContext)
    {
        _context = currentContext;
        _states[NpcStates.idle] = new NpcIdleState(_context, this);
        _states[NpcStates.walk] = new NpcWalkingState(_context, this);
        _states[NpcStates.escaping] = new NpcEscapingState(_context, this);

    }

    public NpcBaseState Idle()
    {
        return _states[NpcStates.idle];
    }
    public NpcBaseState Walk()
    {
        return _states[NpcStates.walk];
    }

    public NpcBaseState IsEscaping()
    {
        return _states[NpcStates.escaping];
    }

}
