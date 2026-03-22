namespace MindGuard.Core.StateMachine
{
    /// <summary>
    /// Generic interface for a state in a state machine.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner object that owns this state</typeparam>
    public interface IState<TOwner>
    {
        /// <summary>
        /// Called when entering this state.
        /// </summary>
        /// <param name="owner">The owner object of this state</param>
        void Enter(TOwner owner);

        /// <summary>
        /// Called every frame while this state is active.
        /// </summary>
        /// <param name="owner">The owner object of this state</param>
        void Update(TOwner owner);

        /// <summary>
        /// Called when exiting this state.
        /// </summary>
        /// <param name="owner">The owner object of this state</param>
        void Exit(TOwner owner);
    }
}
