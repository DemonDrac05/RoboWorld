using System;

public class PlayerStateFactory
{
    private Player player;
    private PlayerStateMachine stateMachine;

    public PlayerStateFactory(Player player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public PlayerState CreateState<T>() where T : PlayerState
    {
        return (T)Activator.CreateInstance(typeof(T), player, stateMachine);
    }
}
