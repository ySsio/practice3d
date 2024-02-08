using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{
    // �߻�Ŭ����


    

    // ���� ������ Hand�� Ÿ�� ����.
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // ������?
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo; // ray�� ���� ���� ������ �޾ƿ�
    protected int targetMask = (-1) - (1 << 11); // �÷��̾� ���̾� ������ ��� ���̾ ������
    

    // Update is called once per frame
    
    protected void TryAttack()
    {
        if (Inventory.inventoryActivated)
            return;
        if (Input.GetButton("Fire1")) // ��Ŭ�� �޾ƿ� .. Project Setting >> input manager ���� ���� ���� ����
        {
            if (!isAttack)
            {
                // coroutine���� ����
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected IEnumerator AttackCoroutine() // ���� �׼��ϴ� �ڷ�ƾ
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack"); // Attack trigger �ߵ��Ǹ鼭 �ִϸ��̼� �����

        // ���� Ȱ��ȭ ����
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA); // ���� ������ attackDelayA (=���� Ȱ��ȭ����) ��ŭ ������Ŵ
        isSwing = true;

        // ���� Ȱ��ȭ �Ǿ�����
        StartCoroutine(HitCoroutine());

        // ���� ��Ȱ��ȭ
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB); // ���� ������ attackDelayB (=�� ���½���) ��ŭ ������Ŵ
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB); // ���� ���ݱ��� �����ð�
        isAttack = false;
    }

    // abstract�� �ڽ�Ŭ�������� �ϼ���Ų�ٴ� ��.
    protected abstract IEnumerator HitCoroutine(); // ���� �����ߴ��� �˾ƺ��� �ڷ�ƾ
    

    protected bool CheckObject() // ������ ��Ҵ��� raycast�� �����ϴ� �Լ� (�� ������ ���ͼ� ȭ�� ���ڰ��� ���� ray�ϵ�? ���� �ָ� collider�� ���� ���Ѵٰ� ��)
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, (-1) - (1 << 11))) // ��Ÿ��� ���⿡�� �޾ƿ� # out hitInfo�� ���� raycast ���� �����ϱ�
        {
            return true;
        }
        return false;
    }


    // �����Լ�?? virtual ���� 
    // �ϼ� �Լ�������, �߰� ���� ������ �Լ�
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
