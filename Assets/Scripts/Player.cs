
using UnityEngine;

public class Player
{
    public int HP, Mana, Manapool;
    const int MAX_MANAPOOL = 5;
    public Player()
    {
        HP = 30;
        Mana = Manapool = 1;
    }
    public void RestoreRoundMana()
    {
        Mana = Manapool;
    }
    public void IncreaseManapool()
    {
        Manapool = Mathf.Clamp(Manapool + 1, 0, MAX_MANAPOOL);
    }

    public void GetDamage(int damgae)
    {
        HP = Mathf.Clamp(HP - damgae, 0, int.MaxValue);
    }
}
