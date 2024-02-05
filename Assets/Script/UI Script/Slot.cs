using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage; // 아이템의 이미지


    // 필요한 컴포넌트
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage; // 아이템 개수 이미지. 아이템 없으면 띄우지 않아야 하므로 껏다 켰다 해야 함

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
}
