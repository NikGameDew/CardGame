using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
public class CardMoveMentScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardController CC;
    private Camera MainCamera;
    private Vector3 offset;
    public Transform DefaultParent;
    public Transform DefaultTempParent;
    private GameObject TempCardGO;
    private bool isDraggble;
    private int startID;
    private void Awake()
    {
        //MainCamera = Camera.main;
        MainCamera = Camera.allCameras[0];
        TempCardGO = GameObject.Find("TempCardGO");
    }
    //»нтерфейс будет выполнен единожды, как только мы начнем перет€гивать объект.
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);

        DefaultParent = DefaultTempParent = transform.parent;

        isDraggble = GameManagerScript.Instance.isPlayerTurn &&
            (
            (DefaultParent.GetComponent<DropPlayScript>().Type == FieldType.SELF_HAND &&
            GameManagerScript.Instance.CurrentGame.Player.Mana >= CC.Card.Manacost) ||
            (DefaultParent.GetComponent<DropPlayScript>().Type == FieldType.SELF_FIELD &&
            CC.Card.CanAttack)
            );

        if (!isDraggble)
            return;

        startID = transform.GetSiblingIndex();

        if(CC.Card.IsSpell || CC.Card.CanAttack)
        GameManagerScript.Instance.HighlightTargets(CC, true);

        TempCardGO.transform.SetParent(DefaultParent);
        TempCardGO.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(DefaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    //Ѕудет выполн€тс€ каждый кадр, пока мы будем т€нуть объект.
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggble)
            return;
        Vector3 newPos = MainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + offset;

        if (!CC.Card.IsSpell)
        {
            if (TempCardGO.transform.parent != DefaultTempParent)
                TempCardGO.transform.SetParent(DefaultTempParent);

            if (DefaultParent.GetComponent<DropPlayScript>().Type != FieldType.SELF_FIELD)
                CheckPosition();
        }
    }
    //Ѕудет выполнен единожды когда мы отпустим объект.
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggble)
            return;

        GameManagerScript.Instance.HighlightTargets(CC, false);

        transform.SetParent(DefaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.SetSiblingIndex(TempCardGO.transform.GetSiblingIndex());
        TempCardGO.transform.SetParent(GameObject.Find("Canvas").transform);
        TempCardGO.transform.localPosition = new Vector3(2340, 0);
    }
    void CheckPosition()
    {
        int newIndex = DefaultTempParent.childCount;
        for (int i = 0; i < DefaultTempParent.childCount; i++)
        {
            if (transform.position.x < DefaultTempParent.GetChild(i).position.x)
            {
                newIndex = i;
                if (TempCardGO.transform.GetSiblingIndex() < newIndex)
                    newIndex--;
                break;
            }
        }
        if (TempCardGO.transform.parent == DefaultParent)
            newIndex = startID;

        TempCardGO.transform.SetSiblingIndex(newIndex);
    }
   public void MoveToField(Transform field)
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        transform.DOMove(field.position, .5f);
    }
    public void MoveToTarget(Transform target)
    {
        StartCoroutine(MoveToTargetCor(target));
    }
    IEnumerator MoveToTargetCor(Transform target)
    {
        Vector3 pos = transform.position;
        Transform parent = transform.parent;
        int index = transform.GetSiblingIndex();


        if(transform.parent.GetComponent<HorizontalLayoutGroup>())
        transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;

        transform.SetParent(GameObject.Find("Canvas").transform);

        transform.DOMove(target.position, .25f);

        yield return new WaitForSeconds(.25f);

        transform.DOMove(pos, .25f);

        yield return new WaitForSeconds(.25f);

        transform.SetParent(parent);
        transform.SetSiblingIndex(index);

        if(transform.parent.GetComponent<HorizontalLayoutGroup>())
        transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;
    }
}
