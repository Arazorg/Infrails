public abstract class BaseEnemyState
{
    protected readonly IEnemyStateSwitcher StateSwitcher;

    protected BaseEnemyState(IEnemyStateSwitcher stateSwitcher)
    {
        StateSwitcher = stateSwitcher;
    }

    public abstract void Idle();

    public abstract void Start();

    public abstract void Move(); 

    public abstract void Attack();

    public abstract void Stun();

    public abstract void Stop();
}
