using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public enum AbilityType
    {
        NO_ABILITY,// йюпрю аег яонянамняреи.
        INSTANT_ACTIVE,// пшбнй.
        DOUBLE_ATTACK,// мехярнбярбн берпю.
        SHIELD,//анфеярбеммши ыхр.
        PROVOCATION,//опнбнйюжхъ.
        REGENERATION_EACH_TURN,//бняярюмюбкхбюер У гднпнбэъ б йнмже ундю.
        COUNTER_ATTACK//юрюйсер б нрбер.
    }
    


    public string Name;
    public Sprite Logo;
    public int Attack;
    public int Defense;
    public int Manacost;
    public bool CanAttack;
    public bool IsPlaced;
    public List<AbilityType> Abilities;
    public bool IsSpell;
    public bool IsAlive
    {
        get
        {
            return Defense > 0;
        }
    }
    public bool HasAbility
    {
        get
        {
            return Abilities.Count > 0;
        }
    }
    public bool IsProvocation
    {
        get
        {
            return Abilities.Exists(x => x == AbilityType.PROVOCATION);
        }
    }
    public int TimesDealDamage;
    public Card(string name, string logoPath, int attack, int defence, int manacost, AbilityType abilityType = 0)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defence;
        CanAttack = false;
        Manacost = manacost;
        IsPlaced = false;



        Abilities = new List<AbilityType>();

        if (abilityType != 0)
            Abilities.Add(abilityType);



        TimesDealDamage = 0;
    }
   public Card(Card card)
    {
        Name = card.Name;
        Logo = card.Logo;
        Attack = card.Attack;
        Defense = card.Defense;
        Manacost = card.Manacost;
        CanAttack = false;
        IsPlaced = false;

        Abilities = new List<AbilityType>(card.Abilities);
        TimesDealDamage = 0;
    }
    public void GetDamage(int dmg)
    {
        if (dmg > 0)
        {
            if (Abilities.Exists(x => x == AbilityType.SHIELD))
                Abilities.Remove(AbilityType.SHIELD);
            else
                Defense -= dmg;
        }
    }
    public Card GetCopy()
    {
        return new Card(this);
    }
}

public class SpellCard : Card
{
    public enum SpellType
    {
        NO_SPELL,//мер гюйкхмюмхъ.
        HEAL_ALLY_FIELD_CARDS,//кевемхе бяеу ябнху йюпр мю У едхмхж.
        DAMAGE_ENEMY_FIELD_CARDS,//мюмняхр У спнмю бяел йюпрюл бпюцю
        HEAL_ALLY_HERO,//кевхр ябнецн цепнъ мю У ед.
        DAMAGE_ENEMY_HERO,//мюмняхр У спнмю бпюфеяйнлс цепнч.
        HEAL_ALLY_CARD,//кевхр ябнч йюпрс мю У ед.
        DAMAGE_ENEMY_CARD,//мюмняхр У спнмю бпюфеяйни йюпре.
        SHIELD_ON_ALLY_CARD,//мюйкюдшбюер анфеярбеммши ыхр мю ябнч йюпрс.
        PROVOCATION_ON_ALLY_CARD,//мюйкюдшбюер опнбнйюжхч мю ябнч йюпрс.
        BUFF_CARD_DAMAGE,//сбекхвхбюер спнм мю У ед ябнеи йюпре.
        DEBUFF_CARD_DAMAGE,//слемэьюер спнм мю У ед бпюфеяйни йюпре.
    }
    public enum TargetType
    {
        NO_TARGET,
        ALLY_CARD_TARGET,
        ENEMY_CARD_TARGET,
    }
    public SpellType Spell;
    public TargetType SpellTarget;
    public int SpellValue;

    public SpellCard(string name, string logoPath, int manacot, SpellType spellType = 0,
                     int spellValue = 0, TargetType targetType = 0): base(name, logoPath, 0, 0, manacot)
    {
        IsSpell = true;

        Spell = spellType;
        SpellTarget = targetType;
        SpellValue = spellValue;
    }
    public SpellCard(SpellCard card): base(card)
    {
        IsSpell = true;

        Spell = card.Spell;
        SpellTarget = card.SpellTarget;
        SpellValue = card.SpellValue;
    }

    public new SpellCard GetCopy()
    {
        return new SpellCard(this);
    }
}
public static class CardManagerStatic
{
    public static List<Card> AllCards = new List<Card>();
}
public class CardManager : MonoBehaviour
{
  
    public void Awake()
    {
        CardManagerStatic.AllCards.Add(new Card("arni", "Sprite/Cards/arni", 5, 5, 4));
        CardManagerStatic.AllCards.Add(new Card("raw", "Sprite/Cards/raw", 4, 3, 3));
        CardManagerStatic.AllCards.Add(new Card("car", "Sprite/Cards/car", 1, 2, 1));
        CardManagerStatic.AllCards.Add(new Card("most", "Sprite/Cards/most", 2, 1, 1));
        CardManagerStatic.AllCards.Add(new Card("vol", "Sprite/Cards/vol", 3, 3, 2));
        CardManagerStatic.AllCards.Add(new Card("job", "Sprite/Cards/job", 4, 1, 2));

        CardManagerStatic.AllCards.Add(new Card("provocation", "Sprite/Cards/provocation", 2, 5, 5, Card.AbilityType.PROVOCATION));
        CardManagerStatic.AllCards.Add(new Card("regeneration", "Sprite/Cards/regeneration", 2, 3, 4, Card.AbilityType.REGENERATION_EACH_TURN));
        CardManagerStatic.AllCards.Add(new Card("doubleAttack", "Sprite/Cards/doubleAttack", 2, 2, 3, Card.AbilityType.DOUBLE_ATTACK));
        CardManagerStatic.AllCards.Add(new Card("instantActive", "Sprite/Cards/instantActive", 3, 1, 2, Card.AbilityType.INSTANT_ACTIVE));
        CardManagerStatic.AllCards.Add(new Card("shield", "Sprite/Cards/shield", 3, 1, 2, Card.AbilityType.SHIELD));
        CardManagerStatic.AllCards.Add(new Card("counterAttack", "Sprite/Cards/counterAttack", 2, 2, 4, Card.AbilityType.COUNTER_ATTACK));

        CardManagerStatic.AllCards.Add(new SpellCard("HEAL_ALLY_FIELD_CARDS", "Sprite/Cards/HealAllyField", 2,
            SpellCard.SpellType.HEAL_ALLY_FIELD_CARDS, 2, SpellCard.TargetType.NO_TARGET));
        CardManagerStatic.AllCards.Add(new SpellCard("DAMAGE_ENEMY_FIELD_CARDS", "Sprite/Cards/DamageEnemyField", 2,
            SpellCard.SpellType.DAMAGE_ENEMY_FIELD_CARDS, 2, SpellCard.TargetType.NO_TARGET));
        CardManagerStatic.AllCards.Add(new SpellCard("HEAL_ALLY_HERO", "Sprite/Cards/HealAllyHero", 2,
            SpellCard.SpellType.HEAL_ALLY_HERO, 2, SpellCard.TargetType.NO_TARGET));
        CardManagerStatic.AllCards.Add(new SpellCard("DAMAGE_ENEMY_HERO", "Sprite/Cards/DamageEnemyHero", 2,
            SpellCard.SpellType.DAMAGE_ENEMY_HERO, 2, SpellCard.TargetType.NO_TARGET));
        CardManagerStatic.AllCards.Add(new SpellCard("HEAL_ALLY_CARD", "Sprite/Cards/HealAllyCard", 0,
            SpellCard.SpellType.HEAL_ALLY_CARD, 2, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManagerStatic.AllCards.Add(new SpellCard("DAMAGE_ENEMY_CARD", "Sprite/Cards/DamaheEnemyCard", 2,
            SpellCard.SpellType.DAMAGE_ENEMY_CARD, 2, SpellCard.TargetType.ENEMY_CARD_TARGET));
        CardManagerStatic.AllCards.Add(new SpellCard("SHIELD_ON_ALLY_CARD", "Sprite/Cards/ShieldOnAllyCard", 2,
            SpellCard.SpellType.SHIELD_ON_ALLY_CARD, 0, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManagerStatic.AllCards.Add(new SpellCard("PROVOCATION_ON_ALLY_CARD", "Sprite/Cards/Provocation", 2,
            SpellCard.SpellType.PROVOCATION_ON_ALLY_CARD, 0, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManagerStatic.AllCards.Add(new SpellCard("BUFF_CARD_DAMAGE", "Sprite/Cards/BuffCardDamage",2,
            SpellCard.SpellType.BUFF_CARD_DAMAGE, 2, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManagerStatic.AllCards.Add(new SpellCard("DEBUFF_CARD_DAMAGE", "Sprite/Cards/DebuffCardDamage", 2,
            SpellCard.SpellType.DEBUFF_CARD_DAMAGE, 2, SpellCard.TargetType.ENEMY_CARD_TARGET));
    }
}
