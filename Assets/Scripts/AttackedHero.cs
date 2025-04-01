using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AttackedHero : MonoBehaviour, IDropHandler
{
    public enum HeroType
    {
        ENEMY,
        HERO
    }
    public HeroType Type;
    public Color NormalColor;
    public Color TargetColor;

    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManagerScript.Instance.isPlayerTurn)
            return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if (card && 
            card.Card.CanAttack &&
            Type == HeroType.ENEMY &&
            !GameManagerScript.Instance.EnemyFieldCards.Exists(x => x.Card.IsProvocation))
        {
            GameManagerScript.Instance.DamageHero(card, true);
        }
    }
    public void HighlihghtsAsTarget(bool highilihgt)
    {
        GetComponent<Image>().color = highilihgt ? TargetColor : NormalColor;
    }
}
