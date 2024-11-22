

public class PlayerStat : BaseStatManager
{
    public static PlayerStat playerStat;

    public override void Awake()
    {
        base.Awake();
        playerStat = this;
    }
}
