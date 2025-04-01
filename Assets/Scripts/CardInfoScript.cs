using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CardInfoScript : MonoBehaviour
{
    public CardController CC;
    public Image Logo;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Attack;
    public TextMeshProUGUI Defense;
    public TextMeshProUGUI Manacost;
    public GameObject HideObject;
    public GameObject HighlightetObject;
    public Color NormalColor;
    public Color TargetColor;
    public Color SpellTargetColor;
    public void HideCardInfo()
    {
        //ShowCardInfo(card);
        HideObject.SetActive(true);
        Manacost.text = "";
    }
    public void ShowCardInfo()
    {
        HideObject.SetActive(false);
        Logo.sprite = CC.Card.Logo;
        Logo.preserveAspect = true;
        Name.text = CC.Card.Name;

        if (CC.Card.IsSpell)
        {
            Attack.gameObject.SetActive(false);
            Defense.gameObject.SetActive(false);
        }

        RefreshData();
    }
    public void RefreshData()
    {
        Attack.text = CC.Card.Attack.ToString();
        Defense.text = CC.Card.Defense.ToString();
        Manacost.text = CC.Card.Manacost.ToString();
    }
    public void HighlightCard(bool highlight)
    {
        HighlightetObject.SetActive(highlight);
    }
    public void HighlightManaAvaliability(int currentMana)
    {
        GetComponent<CanvasGroup>().alpha = currentMana >= CC.Card.Manacost ? 1 : 0.8f;
    }
    public void HighlihghtsAsTarget(bool highilihgt)
    {
        GetComponent<Image>().color = highilihgt ? TargetColor : NormalColor;
    }

    public void HighlihghtsAsSpellTarget(bool highilihgt)
    {
        GetComponent<Image>().color = highilihgt ? SpellTargetColor : NormalColor;
    }
}
