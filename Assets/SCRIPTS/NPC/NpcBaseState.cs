using UnityEngine;

public abstract class NpcBaseState
{
    private bool _isRootState = false;
    private NpcStateMachine _ctx;
    private NpcStateFactory _factory;
    private NpcBaseState _currentSubState;
    private NpcBaseState _currentSuperState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected NpcStateMachine Ctx { get { return _ctx; } }
    protected NpcStateFactory Factory { get { return _factory; } }
    public NpcBaseState(NpcStateMachine currentContext, NpcStateFactory npcStateFactory)
    {
        _ctx = currentContext;
        _factory = npcStateFactory;
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
    protected void SwitchState(NpcBaseState newState)
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
    protected void SetSuperState(NpcBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(NpcBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);

    }
}
