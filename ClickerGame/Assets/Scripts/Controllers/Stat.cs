
public class Stat
{
    public virtual int Coin { get; set; }
    public virtual float HP { get; set; }
    public virtual float MaxHP { get; set; } 
    public virtual float ATK { get; set; }
    public virtual float DEF { get; set; }
    public virtual float AttackSpeed { get; set; }
    public virtual float Range { get; set; }
    public virtual float AttackCountdown { get; set; } = 0;
}
