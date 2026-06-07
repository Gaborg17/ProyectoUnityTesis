using UnityEngine;

public abstract class EnemyBaseState
{
    private bool _isRootState = false;
    private EnemyStateMachine _ctx;
    private EnemyStateFactory _factory;
    private EnemyBaseState _currentSubState;
    private EnemyBaseState _currentSuperState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected EnemyStateMachine Ctx { get { return _ctx; } }
    protected EnemyStateFactory Factory { get { return _factory; } }
    public EnemyBaseState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }


    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    public abstract void InitializeSubState();


    public void EnterStates()
    {
        EnterState();
        if (_currentSubState != null)
        {
            _currentSubState.EnterStates();
        }
    }
    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }
    public void ExitStates()
    {
        ExitState();
        if (_currentSubState != null)
        {
            _currentSubState.ExitState();
            _currentSubState = null;
        }
    }
    protected void SwitchState(EnemyBaseState newState)
    {
        
        ExitStates();
        

        if (_isRootState)
        {
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
        newState.EnterStates();
    }
    protected void SetSuperState(EnemyBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(EnemyBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);

    }
}
