using UnityEngine;

public abstract class AllyBaseState
{
    private bool _isRootState = false;
    private AllyStateMachine _ctx;
    private AllyStateFactory _factory;
    private AllyBaseState _currentSubState;
    private AllyBaseState _currentSuperState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected AllyStateMachine Ctx { get { return _ctx; } }
    protected AllyStateFactory Factory { get { return _factory; } }
    public AllyBaseState(AllyStateMachine currentContext, AllyStateFactory allyStateFactory)
    {
        _ctx = currentContext;
        _factory = allyStateFactory;
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
    protected void SwitchState(AllyBaseState newState)
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
    protected void SetSuperState(AllyBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(AllyBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);

    }
}
