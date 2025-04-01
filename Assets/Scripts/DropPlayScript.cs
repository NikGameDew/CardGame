using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum FieldType
{
    SELF_HAND,
    SELF_FIELD,
    ENEMY_HAND,
    ENEMY_FIELD
}
                                                           //Эти два интрефейса отслеживают наведение
                                                           // и отведение мыши от границ объекта.
public class DropPlayScript : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public FieldType Type;
    public void OnDrop(PointerEventData eventData)
    {
        if (Type != FieldType.SELF_FIELD)
        {
            return;
        }
        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if (card  &&
            GameManagerScript.Instance.isPlayerTurn &&
            GameManagerScript.Instance.CurrentGame.Player.Mana >= card.Card.Manacost &&
            !card.Card.IsPlaced)
        {  
            if(!card.Card.IsSpell)
            card.Movement.DefaultParent = transform;

            card.OnCast();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || Type == FieldType.ENEMY_FIELD ||
            Type == FieldType.ENEMY_HAND || Type == FieldType.SELF_HAND)
        {
            return;
        }
        CardMoveMentScript card = eventData.pointerDrag.GetComponent<CardMoveMentScript>();
        if (card)
        {
            card.DefaultTempParent = transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }
        CardMoveMentScript card = eventData.pointerDrag.GetComponent<CardMoveMentScript>();
        if (card && card.DefaultTempParent == transform)
        {
            card.DefaultTempParent = card.DefaultParent;
        }
    }
}
