using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false; // �κ��丮 Ȱ��ȭ ���� .. Ȱ��ȭ �Ǹ� ī�޶� �ȿ����̰� ���� ��� ���� �� ��� ����, Ȱ��ȭ/��Ȱ��ȭ


    // �ʿ��� ������Ʈ
    [SerializeField]
    private GameObject go_InventoryBase; // ��ҿ� �����ִٰ� i Ű ������ Ȱ��ȭ
    [SerializeField]
    private GameObject go_SlotsParent; // grid setting

    private ItemEffectDatabase theItemEffectDatabase;

    // ���Ե�
    private Slot[] slots;

    public Slot[] GetSlots() { return slots; }

    [SerializeField] private Item[] items;

    public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
            if (items[i].itemName == _itemName)
                slots[_arrayNum].AddItem(items[i], _itemNum);
    }


    private void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>(); // �ڽ� ��ü�鿡 �ִ� slot ������Ʈ�� ��� �о��
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
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
                // �κ��丮 Ȱ��ȭ
                OpenInventory();
            }
            else
            {
                // �κ��丮 ��Ȱ��ȭ
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        GameManager.isOpenInventory = true;
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        GameManager.isOpenInventory = false;
        go_InventoryBase.SetActive(false);
        theItemEffectDatabase.HideToolTip();
    }


    // ActionController���� ȣ���ϱ� ���� public���� ����
    public void AcquireItem(Item _item, int _count = 1)
    {
        // �̹� �κ��丮�� ���� �������� ������ ������ �߰� (item.itemType != Item.ItemType.Equipment �� ���� �ش�)
        if (_item.itemType != Item.ItemType.Equipment) // ��� ���̸� �� ������ �����ϰ� ���ο� ���Կ� �־���
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

        // �κ��丮�� ���� �������� ������ ���ο� ���Կ� �־���
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
