using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; // 아이템 이름 (Item.itemName과 같은 키값)
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능합니다.")]
    public string[] part; // 어떤 요소를 회복/감소 시킬건지.. hp sp dp 등
    public int[] num; // 수치 (체력 10감소 굶주림 10증가 기력 0증가 이런 느낌) 배열로 받아와서

}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    // 필요한 컴포넌트
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
                        // # 의문점 : 지금 코드 상태 보아하니 part에 효과 있는 상태만 [HP,HUNGRY] 이런식으로 넣어주는건가본데
                        // # 그냥 part[6]로 사이즈 고정해서 인덱스 순서대로 HP~SATISFY라고 하고 num[0]~num[6]까지 차례대로 회복하거나 감소시키면 되는거 아닌가
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
                                Debug.Log("존재하지 않는 Status part입니다");
                                break;
                        }
                    }
                    Debug.Log(_item.itemName + " 을 사용했습니다");
                    return;
                }
            }
            Debug.Log("ItemEffectDatabase에 일치하는 itemName이 없습니다");
        }
        else if (_item.itemType == Item.ItemType.Equipment)
        {
            
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }
    }
}
