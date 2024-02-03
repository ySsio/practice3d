using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp; // 바위의 체력

    [SerializeField]
    private float destroyTime; // 파편 제거 시간

    [SerializeField]
    private SphereCollider col; // 구체 콜라이더 : 곡괭이 히트 범위, 파괴되면 없애야 함

    // 필요한 게임오브젝트 (바위)
    [SerializeField]
    private GameObject go_rock; // 일반 바위
    [SerializeField]
    private GameObject go_debris; // 깨진 바위
    [SerializeField]
    private GameObject go_effect_prefabs; // 채굴 이펙트


    // 효과음 이름
    [SerializeField]
    private string strike_Sound;
    [SerializeField]
    private string destroy_Sound;


    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound);
        hp--;
        if(hp<=0)
        {
            Destruction(); // 파괴시키는 함수
            return;
        }

        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, 2f);
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound);
        Destroy(go_rock); // 기존 돌을 아예 메모리에서 제거해버림
        col.enabled = false;

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime); // destroyTime 만큼의 시간 뒤에 메모리에서 제거됨
    }
}
