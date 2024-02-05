using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ŭ���ؼ� create �� �� �ִ� �׸� New Item > item�� �߰� �� ��! �������� �� ����Ʈ �̸��� fileName�� �ش��ϴ� �̸�
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject        // ���� ������Ʈ�� ���� �ʿ� ����
{
    public string itemName; // ������ �̸�
    public ItemType itemType;
    public Sprite itemImage; // �������� �̹��� (�κ��丮 �̹���)
                             // image�� sprite�� ���� : image�� canvas���� ��� �� �ְ� sprite�� world���� ���� ��½�ų �� ����.
    public GameObject itemPrefab; // �������� prefab
    public string weaponType; // "GUN", "PICKAXE" �� ���� ����. ���� �����ۿ� �ش��� ���

    public enum ItemType
    {
        Equipment,          // ��� ��
        Used,               // �Ҹ� ��
        Ingredient,         // ��� ��
        ETC                 // ��Ÿ ��
    }
    // # enum���� �ϴ� �Ͱ� string/ dictionary �� �޾ƿ��� �Ͱ��� ����?


}
