using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 우클릭해서 create 할 수 있는 항목에 New Item > item을 추가 한 것! 생성했을 때 디폴트 이름이 fileName에 해당하는 이름
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject        // 게임 오브젝트에 붙일 필요 없음
{
    public string itemName; // 아이템 이름
    public ItemType itemType;
    public Sprite itemImage; // 아이템의 이미지 (인벤토리 이미지)
                             // image와 sprite의 차이 : image는 canvas에만 띄울 수 있고 sprite는 world에서 직접 출력시킬 수 있음.
    public GameObject itemPrefab; // 아이템의 prefab
    public string weaponType; // "GUN", "PICKAXE" 등 무기 유형. 무기 아이템에 해당할 경우

    public enum ItemType
    {
        Equipment,          // 장비 템
        Used,               // 소모 템
        Ingredient,         // 재료 템
        ETC                 // 기타 템
    }
    // # enum으로 하는 것과 string/ dictionary 로 받아오는 것과의 차이?


}
