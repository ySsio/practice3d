using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item; // ȹ���� ������
    public int itemCount; // ȹ���� �������� ����
    public Image itemImage; // �������� �̹���


    // �ʿ��� ������Ʈ
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage; // ������ ���� �̹���. ������ ������ ����� �ʾƾ� �ϹǷ� ���� �״� �ؾ� ��

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
}
