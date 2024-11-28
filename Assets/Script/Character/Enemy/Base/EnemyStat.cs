
public class EnemyStat : BaseStatManager
{
    public static EnemyStat enemyStat;

    public override void OnEnable()
    {
        base.OnEnable();
        enemyStat = this;
    }
}
