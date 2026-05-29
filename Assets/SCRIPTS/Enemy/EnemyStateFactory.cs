using System.Collections.Generic;

enum EnemyStates
{
    idle,
    walk,
    combat,
    grounded,
}
public class EnemyStateFactory
{
    EnemyStateMachine _context;
    Dictionary<EnemyStates, EnemyBaseState> _states = new Dictionary<EnemyStates, EnemyBaseState>();
    public EnemyStateFactory(EnemyStateMachine currentContext)
    {
        _context = currentContext;
        _states[EnemyStates.idle] = new EnemyIdleState(_context, this);
        _states[EnemyStates.walk] = new EnemyWalkState(_context, this);
        _states[EnemyStates.combat] = new EnemyCombatState(_context, this);
        _states[EnemyStates.grounded] = new EnemyGroundedState(_context, this);
    }

    public EnemyBaseState Idle()
    {
        return _states[EnemyStates.idle];
    }
    public EnemyBaseState Walk()
    {
        return _states[EnemyStates.walk];
    }

    public EnemyBaseState InCombat()
    {
        return _states[EnemyStates.combat];
    }

    public EnemyBaseState Grounded()
    {
        return _states[EnemyStates.grounded];
    }

}
