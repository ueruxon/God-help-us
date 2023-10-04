namespace Game.Scripts.GameplayLogic.Animations
{
    internal interface IAnimationStateReader
    {
        void EnteredState(int stateHash);
        void ExitedState(int stateHash);
    }
}