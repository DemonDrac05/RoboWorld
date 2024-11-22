
public class EnemyStat : BaseStatManager
{
    public static EnemyStat enemyStat;

    public override void Awake()
    {
        base.Awake();
        enemyStat = this;
    }
}
