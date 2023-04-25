public interface IStateModule
{
    void Enter();
    void Exit();
    void UpdateLogic();
    void UpdatePhysics();
}
