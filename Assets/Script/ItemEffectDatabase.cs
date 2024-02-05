using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; // ������ �̸� (Item.itemName�� ���� Ű��)
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY�� �����մϴ�.")]
    public string[] part; // � ��Ҹ� ȸ��/���� ��ų����.. hp sp dp ��
    public int[] num; // ��ġ (ü�� 10���� ���ָ� 10���� ��� 0���� �̷� ����) �迭�� �޾ƿͼ�

}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private StatusController theStatusController;
    [SerializeField]
    private WeaponManager theWeaponManager;

    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    public void UseItem (Item _item)
    {
        if (_item.itemType == Item.ItemType.Used)
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if (itemEffects[x].itemName == _item.itemName)
                {
                    for (int y = 0; y < itemEffects[x].part.Length; y++)
                    {
                        // # �ǹ��� : ���� �ڵ� ���� �����ϴ� part�� ȿ�� �ִ� ���¸� [HP,HUNGRY] �̷������� �־��ִ°ǰ�����
                        // # �׳� part[6]�� ������ �����ؼ� �ε��� ������� HP~SATISFY��� �ϰ� num[0]~num[6]���� ���ʴ�� ȸ���ϰų� ���ҽ�Ű�� �Ǵ°� �ƴѰ�
                        switch (itemEffects[x].part[y])
                        {
                            case HP:
                                theStatusController.RecoverHP(itemEffects[x].num[y]);
                                break;
                            case SP:
                                theStatusController.RecoverSP(itemEffects[x].num[y]);
                                break;
                            case DP:
                                theStatusController.RecoverDP(itemEffects[x].num[y]);
                                break;
                            case HUNGRY:
                                theStatusController.RecoverHungry(itemEffects[x].num[y]);
                                break;
                            case THIRSTY:
                                theStatusController.RecoverThirsty(itemEffects[x].num[y]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("�������� �ʴ� Status part�Դϴ�");
                                break;
                        }
                    }
                    Debug.Log(_item.itemName + " �� ����߽��ϴ�");
                    return;
                }
            }
            Debug.Log("ItemEffectDatabase�� ��ġ�ϴ� itemName�� �����ϴ�");
        }
        else if (_item.itemType == Item.ItemType.Equipment)
        {
            
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }
    }
}
