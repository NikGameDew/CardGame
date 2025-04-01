using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public void MakeTurn()
    {
        StartCoroutine(EnemyTurn(GameManagerScript.Instance.EnemyHandCards));
    }
    private IEnumerator EnemyTurn(List<CardController> cards)
    {
        yield return new WaitForSeconds(1);
        int count = cards.Count == 1 ? 1 : Random.Range(0, cards.Count);
        for (int i = 0; i < count; i++)
        {
            if (GameManagerScript.Instance.EnemyFieldCards.Count > 5 ||
                GameManagerScript.Instance.CurrentGame.Enemy.Mana == 0 ||
                GameManagerScript.Instance.EnemyHandCards.Count == 0)
                break;

            List<CardController> cardList = cards.FindAll(x => GameManagerScript.Instance.CurrentGame.Enemy.Mana >= x.Card.Manacost);
            if (cardList.Count == 0)
                break;

            if (cardList[0].Card.IsSpell)
            {
                CastSpell(cardList[0]);
                yield return new WaitForSeconds(.51f);
            }
            else
            {
                cardList[0].GetComponent<CardMoveMentScript>().MoveToField(GameManagerScript.Instance.EnemyField);
                yield return new WaitForSeconds(.51f);
                cardList[0].transform.SetParent(GameManagerScript.Instance.EnemyField);
                cardList[0].OnCast();
            }
        }
        yield return new WaitForSeconds(1);
        while (GameManagerScript.Instance.EnemyFieldCards.Exists(x => x.Card.CanAttack))
        {
            var activeCard = GameManagerScript.Instance.EnemyFieldCards.FindAll(x => x.Card.CanAttack)[0];
            bool hasProvocation = GameManagerScript.Instance.PlayerFieldCards.Exists(x => x.Card.IsProvocation);
            if (hasProvocation ||
                Random.Range(0, 2) == 0 &&
                GameManagerScript.Instance.PlayerFieldCards.Count > 0)
            {
                CardController enemy;
                if (hasProvocation)
                    enemy = GameManagerScript.Instance.PlayerFieldCards.Find(x => x.Card.IsProvocation);

                else
                    enemy = GameManagerScript.Instance.PlayerFieldCards[Random.Range(0, GameManagerScript.Instance.PlayerFieldCards.Count)];



                Debug.Log(activeCard.Card.Name + " (" + activeCard.Card.Attack + ";" + activeCard.Card.Defense +
                           ")" + "--->" + enemy.Card.Name + " (" + enemy.Card.Attack + ";" + enemy.Card.Defense + ")");

                activeCard.Card.CanAttack = false;

                activeCard.Movement.MoveToTarget(enemy.transform);
                yield return new WaitForSeconds(.75f);

                GameManagerScript.Instance.CardsFight(enemy, activeCard);
            }
            else
            {
                Debug.Log(activeCard.Card.Name + " (" + activeCard.Card.Attack + ") Attacked hero");

                activeCard.Card.CanAttack = false;

                activeCard.GetComponent<CardMoveMentScript>().MoveToTarget(GameManagerScript.Instance.PlayerHero.transform);
                yield return new WaitForSeconds(.75f);

                GameManagerScript.Instance.DamageHero(activeCard, false);
            }
            yield return new WaitForSeconds(.2f);
        }
        yield return new WaitForSeconds(1);
        GameManagerScript.Instance.ChangeTurn();
    }
    void CastSpell(CardController card)
    {
        switch (((SpellCard)card.Card).SpellTarget)
        {
            case SpellCard.TargetType.NO_TARGET:
                switch (((SpellCard)card.Card).Spell)
                {
                    case SpellCard.SpellType.HEAL_ALLY_FIELD_CARDS:

                        if (GameManagerScript.Instance.EnemyFieldCards.Count > 0)
                            StartCoroutine(CastCard(card));
                        break;

                    case SpellCard.SpellType.DAMAGE_ENEMY_FIELD_CARDS:
                        if (GameManagerScript.Instance.PlayerFieldCards.Count > 0)
                            StartCoroutine(CastCard(card));
                        break;

                    case SpellCard.SpellType.HEAL_ALLY_HERO:
                        StartCoroutine(CastCard(card));
                        break;

                    case SpellCard.SpellType.DAMAGE_ENEMY_HERO:
                        StartCoroutine(CastCard(card));
                        break;
                }
                break;
            case SpellCard.TargetType.ALLY_CARD_TARGET:
                if (GameManagerScript.Instance.EnemyFieldCards.Count > 0)
                    StartCoroutine(CastCard(card,
                GameManagerScript.Instance.EnemyFieldCards[Random.Range(0, GameManagerScript.Instance.EnemyFieldCards.Count)]));
                break;
            case SpellCard.TargetType.ENEMY_CARD_TARGET:
                if (GameManagerScript.Instance.PlayerFieldCards.Count > 0)
                    StartCoroutine(CastCard(card,
                GameManagerScript.Instance.PlayerFieldCards[Random.Range(0, GameManagerScript.Instance.PlayerFieldCards.Count)]));
                break;
        }
    }
    IEnumerator CastCard(CardController spell, CardController target = null)
    {
        if (((SpellCard)spell.Card).SpellTarget == SpellCard.TargetType.NO_TARGET)
        {
            spell.GetComponent<CardMoveMentScript>().MoveToField(GameManagerScript.Instance.EnemyField);
            yield return new WaitForSeconds(.51f);

            spell.OnCast();
        }
        else
        {
            spell.Info.ShowCardInfo();
            spell.GetComponent<CardMoveMentScript>().MoveToTarget(target.transform);
            yield return new WaitForSeconds(.51f);

            GameManagerScript.Instance.EnemyHandCards.Remove(spell);
            GameManagerScript.Instance.EnemyFieldCards.Add(spell);
            GameManagerScript.Instance.ReduceMana(false, spell.Card.Manacost);

            spell.Card.IsPlaced = true;

            spell.UseSpell(target);
        }
        string targetStr = target == null ? "no_target" : target.Card.Name;
        Debug.Log("AI spell cast: " + (spell.Card).Name + " target: " + targetStr);
    }
}
