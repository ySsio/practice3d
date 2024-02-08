using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{
    // 추상클래스


    

    // 현재 장착된 Hand형 타입 무기.
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // 공격중?
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo; // ray에 닿은 놈의 정보를 받아옴
    protected int targetMask = (-1) - (1 << 11); // 플레이어 레이어 제외한 모든 레이어만 감지함
    

    // Update is called once per frame
    
    protected void TryAttack()
    {
        if (Inventory.inventoryActivated)
            return;
        if (Input.GetButton("Fire1")) // 좌클릭 받아옴 .. Project Setting >> input manager 에서 변수 설정 가능
        {
            if (!isAttack)
            {
                // coroutine으로 구현
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected IEnumerator AttackCoroutine() // 공격 액션하는 코루틴
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack"); // Attack trigger 발동되면서 애니메이션 실행됨

        // 공격 활성화 시작
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA); // 현재 무기의 attackDelayA (=공격 활성화시점) 만큼 지연시킴
        isSwing = true;

        // 공격 활성화 되어있음
        StartCoroutine(HitCoroutine());

        // 공격 비활성화
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB); // 현재 무기의 attackDelayB (=팔 접는시점) 만큼 지연시킴
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB); // 다음 공격까지 지연시간
        isAttack = false;
    }

    // abstract는 자식클래스에서 완성시킨다는 뜻.
    protected abstract IEnumerator HitCoroutine(); // 공격 적중했는지 알아보는 코루틴
    

    protected bool CheckObject() // 공격이 닿았는지 raycast로 판정하는 함수 (내 몸에서 나와서 화면 십자가로 뻗는 ray일듯? 실제 주먹 collider로 구현 안한다고 함)
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, (-1) - (1 << 11))) // 사거리는 무기에서 받아옴 # out hitInfo는 뭐야 raycast 문법 공부하기
        {
            return true;
        }
        return false;
    }


    // 가상함수?? virtual 뭐임 
    // 완성 함수이지만, 추가 편집 가능한 함수
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentCloseWeapon = _closeWeapon;

        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;

        currentCloseWeapon.gameObject.SetActive(true);
    }
}
