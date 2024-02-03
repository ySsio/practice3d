using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // Ȱ��ȭ ����
    public static bool isActivate = false;

    // ���� ������ Hand�� Ÿ�� ����.
    [SerializeField]
    private Hand currentHand;

    // ������?
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo; // ray�� ���� ���� ������ �޾ƿ�

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
            TryAttack();

    }

    private void TryAttack()
    {
        if(Input.GetButton("Fire1")) // ��Ŭ�� �޾ƿ� .. Project Setting >> input manager ���� ���� ���� ����
        {
            if (!isAttack) 
            {
                // coroutine���� ����
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine() // ���� �׼��ϴ� �ڷ�ƾ
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack"); // Attack trigger �ߵ��Ǹ鼭 �ִϸ��̼� �����

        // ���� Ȱ��ȭ ����
        yield return new WaitForSeconds(currentHand.attackDelayA); // ���� ������ attackDelayA (=���� Ȱ��ȭ����) ��ŭ ������Ŵ
        isSwing = true;

        // ���� Ȱ��ȭ �Ǿ�����
        StartCoroutine(HitCoroutine());

        // ���� ��Ȱ��ȭ
        yield return new WaitForSeconds(currentHand.attackDelayB); // ���� ������ attackDelayB (=�� ���½���) ��ŭ ������Ŵ
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB); // ���� ���ݱ��� �����ð�
        isAttack = false;
    }

    IEnumerator HitCoroutine() // ���� �����ߴ��� �˾ƺ��� �ڷ�ƾ
    {
        // delayA�� B ���̿��� ��� ���� ��Ҵ��� �ݺ������� üũ
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

    private bool CheckObject() // ������ ��Ҵ��� raycast�� �����ϴ� �Լ� (�� ������ ���ͼ� ȭ�� ���ڰ��� ���� ray�ϵ�? ���� �ָ� collider�� ���� ���Ѵٰ� ��)
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range)) // ��Ÿ��� ���⿡�� �޾ƿ� # out hitInfo�� ���� raycast ���� �����ϱ�
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
