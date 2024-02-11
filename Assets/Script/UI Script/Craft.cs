using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 우클릭해서 create 할 수 있는 항목에 New Item > item을 추가 한 것! 생성했을 때 디폴트 이름이 fileName에 해당하는 이름
[CreateAssetMenu(fileName = "New Craft", menuName = "Custom/Craft")]
public class Craft : ScriptableObject        // 게임 오브젝트에 붙일 필요 없음
{
    public string craftName;
    [TextArea]
    public string craftDescription;
    public GameObject go_Prefab; // 실제 설치될 프리펩.
    public GameObject go_PreviewPrefab; // 미리보기 프리펩.

    public CraftType craftType; // 아이템 유형
    public Sprite craftImage;

    public enum CraftType
    {
        Fire,
        Trap,
    }
    // # enum으로 하는 것과 string/ dictionary 로 받아오는 것과의 차이?


}
