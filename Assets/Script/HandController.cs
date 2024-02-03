using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // 활성화 여부
    public static bool isActivate = false;

    // 현재 장착된 Hand형 타입 무기.
    [SerializeField]
    private Hand currentHand;

    // 공격중?
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo; // ray에 닿은 놈의 정보를 받아옴

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
            TryAttack();

    }

    private void TryAttack()
    {
        if(Input.GetButton("Fire1")) // 좌클릭 받아옴 .. Project Setting >> input manager 에서 변수 설정 가능
        {
            if (!isAttack) 
            {
                // coroutine으로 구현
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine() // 공격 액션하는 코루틴
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack"); // Attack trigger 발동되면서 애니메이션 실행됨

        // 공격 활성화 시작
        yield return new WaitForSeconds(currentHand.attackDelayA); // 현재 무기의 attackDelayA (=공격 활성화시점) 만큼 지연시킴
        isSwing = true;

        // 공격 활성화 되어있음
        StartCoroutine(HitCoroutine());

        // 공격 비활성화
        yield return new WaitForSeconds(currentHand.attackDelayB); // 현재 무기의 attackDelayB (=팔 접는시점) 만큼 지연시킴
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB); // 다음 공격까지 지연시간
        isAttack = false;
    }

    IEnumerator HitCoroutine() // 공격 적중했는지 알아보는 코루틴
    {
        // delayA와 B 사이에서 계속 공격 닿았는지 반복적으로 체크
        while (isSwing)
        {
            if(CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;

        }
    }

    private bool CheckObject() // 공격이 닿았는지 raycast로 판정하는 함수 (내 몸에서 나와서 화면 십자가로 뻗는 ray일듯? 실제 주먹 collider로 구현 안한다고 함)
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range)) // 사거리는 무기에서 받아옴 # out hitInfo는 뭐야 raycast 문법 공부하기
        {
            return true;
        }
        return false;
    }

    public void HandChange(Hand _hand)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentHand = _hand;

        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero;

        currentHand.gameObject.SetActive(true);
        isActivate = true;
    }
}
