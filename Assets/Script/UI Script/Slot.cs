using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// class는 1개만 상속 받을 수 있고, interface는 여러 개 상속 받을 수 있음
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage; // 아이템의 이미지

    // 필요한 컴포넌트
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage; // 아이템 개수 이미지. 아이템 없으면 띄우지 않아야 하므로 껏다 켰다 해야 함

    private ItemEffectDatabase theItemEffectDatabase;

    void Start()
    {
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
    }

    private void SetColor(float _alpha) // 0~1?
    {
        Color color = itemImage.color;  // 현재 이미지의 색을 가져옴
        color.a = _alpha;               // 알파값만 변경해서
        itemImage.color = color;        // 다시 넣어줌
    }

    // 최초 1회 획득시 = 슬롯에 아무것도 없다가 새로운 아이템 들어올 때
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage; // item.itemImage가 sprite 속성

        // 장비 아이템은 슬롯 한 칸에 1개만 넣을 수 있게. 나머지 아이템은 여러 개 넣을 수 있고 개수를 표시해줌.
        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else                                                // # 이거 굳이 왜?
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }
        
        SetColor(1);
    }

    // 아이템 추가 획득/ 사용 등 개수 변동만 설정하는 함수
    public void SetSlotCount (int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }


    // 슬롯 초기화 (개수 다 사용한거)
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    // 이 스크립트 attach된 객체에 마우스를 가져다 대고 우클릭하면 이게 실행됨
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                theItemEffectDatabase.UseItem(item);
                if (item.itemType == Item.ItemType.Used)
                    SetSlotCount(-1);
            }
        }
    }

    // drag가 좌클릭으로 하는 드래그 전제인가 보네
    // Slot이 직접 움직이지 않고 Dragslot이 이미지를 복사해서 대신 움직임
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;                                      // 아이템 위치 교환할 때 이전 슬롯에 대한 정보를 저장하기 위함
            DragSlot.instance.DragSetImage(itemImage);

            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    // OnEndDrag는 드래그가 끝난 객체에서 발생하는 이벤트 (A를 끌다가 어딘가에 떨구면 OnEndDrag가 실행됨)
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }


    // OnDrop은 드래그 드롭이 떨어진 객체에서 발생하는 이벤트 (A를 끌다가 B에다 떨구면 A에서는 OnEndDrag가 실행되고 B에서는 OnDrop이 실행됨)
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot == null)
            return;
        ChangeSlot();
    }

    // A를 B에 드래그 해서 넣으면 B자리에 A가 들어가고 A자리에 B가 들어가는 상황 가정
    // OnDrop에서 호출하는거니까 현재 객체가 B이고, Dragslot.instance.dragSlot이 A인 상황임
    private void ChangeSlot()
    {
        // B (OnDrop이 발생한 객체 slot)의 정보를 임시저장함
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        // B slot 객체에 A의 아이템 정보를 넣음. AddItem()이 애초에 슬롯 정보를 덮어씌우는 함수임. 비울 필요 x
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        // B slot이 원래 빈 슬롯이었느지 확인
        if (_tempItem != null)
            // A slot에 B slot의 아이템을 넣음
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        else
            // B slot이 비어있었으면 A slot을 비움
            DragSlot.instance.dragSlot.ClearSlot();
    }

}
