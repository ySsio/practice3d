using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false; // 인벤토리 활성화 여부 .. 활성화 되면 카메라 안움직이고 공격 기능 같은 거 모두 막음, 활성화/비활성화


    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase; // 평소에 꺼져있다가 i 키 누르면 활성화
    [SerializeField]
    private GameObject go_SlotsParent; // grid setting

    // 슬롯들
    private Slot[] slots;


    private void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>(); // 자식 개체들에 있는 slot 컴포넌트를 모두 읽어옴
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
            {
                // 인벤토리 활성화
                OpenInventory();
            }
            else
            {
                // 인벤토리 비활성화
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }


    // ActionController에서 호출하기 위해 public으로 설정
    public void AcquireItem(Item _item, int _count = 1)
    {
        // 이미 인벤토리에 같은 아이템이 있으면 개수만 추가 (item.itemType != Item.ItemType.Equipment 일 때만 해당)
        if (_item.itemType != Item.ItemType.Equipment) // 장비 템이면 이 과정을 무시하고 새로운 슬롯에 넣어줌
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                    continue;
                if (slots[i].item.itemName == _item.itemName)
                {
                    slots[i].SetSlotCount(_count);
                    return;
                }
            }
        }

        // 인벤토리에 같은 아이템이 없으면 새로운 슬롯에 넣어줌
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) 
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
