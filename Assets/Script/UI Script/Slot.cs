using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// class�� 1���� ��� ���� �� �ְ�, interface�� ���� �� ��� ���� �� ����
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item; // ȹ���� ������
    public int itemCount; // ȹ���� �������� ����
    public Image itemImage; // �������� �̹���

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage; // ������ ���� �̹���. ������ ������ ����� �ʾƾ� �ϹǷ� ���� �״� �ؾ� ��

    private ItemEffectDatabase theItemEffectDatabase;

    void Start()
    {
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
    }

    private void SetColor(float _alpha) // 0~1?
    {
        Color color = itemImage.color;  // ���� �̹����� ���� ������
        color.a = _alpha;               // ���İ��� �����ؼ�
        itemImage.color = color;        // �ٽ� �־���
    }

    // ���� 1ȸ ȹ��� = ���Կ� �ƹ��͵� ���ٰ� ���ο� ������ ���� ��
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage; // item.itemImage�� sprite �Ӽ�

        // ��� �������� ���� �� ĭ�� 1���� ���� �� �ְ�. ������ �������� ���� �� ���� �� �ְ� ������ ǥ������.
        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else                                                // # �̰� ���� ��?
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }
        
        SetColor(1);
    }

    // ������ �߰� ȹ��/ ��� �� ���� ������ �����ϴ� �Լ�
    public void SetSlotCount (int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }


    // ���� �ʱ�ȭ (���� �� ����Ѱ�)
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    // �� ��ũ��Ʈ attach�� ��ü�� ���콺�� ������ ��� ��Ŭ���ϸ� �̰� �����
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

    // drag�� ��Ŭ������ �ϴ� �巡�� �����ΰ� ����
    // Slot�� ���� �������� �ʰ� Dragslot�� �̹����� �����ؼ� ��� ������
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;                                      // ������ ��ġ ��ȯ�� �� ���� ���Կ� ���� ������ �����ϱ� ����
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

    // OnEndDrag�� �巡�װ� ���� ��ü���� �߻��ϴ� �̺�Ʈ (A�� ���ٰ� ��򰡿� ������ OnEndDrag�� �����)
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }


    // OnDrop�� �巡�� ����� ������ ��ü���� �߻��ϴ� �̺�Ʈ (A�� ���ٰ� B���� ������ A������ OnEndDrag�� ����ǰ� B������ OnDrop�� �����)
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot == null)
            return;
        ChangeSlot();
    }

    // A�� B�� �巡�� �ؼ� ������ B�ڸ��� A�� ���� A�ڸ��� B�� ���� ��Ȳ ����
    // OnDrop���� ȣ���ϴ°Ŵϱ� ���� ��ü�� B�̰�, Dragslot.instance.dragSlot�� A�� ��Ȳ��
    private void ChangeSlot()
    {
        // B (OnDrop�� �߻��� ��ü slot)�� ������ �ӽ�������
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        // B slot ��ü�� A�� ������ ������ ����. AddItem()�� ���ʿ� ���� ������ ������ �Լ���. ��� �ʿ� x
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        // B slot�� ���� �� �����̾����� Ȯ��
        if (_tempItem != null)
            // A slot�� B slot�� �������� ����
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        else
            // B slot�� ����־����� A slot�� ���
            DragSlot.instance.dragSlot.ClearSlot();
    }

}
