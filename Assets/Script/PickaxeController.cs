using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    void Update()
    {
        TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        // delayA�� B ���̿��� ��� ���� ��Ҵ��� �ݺ������� üũ
        while (isSwing)
        {
            if (CheckObject())
            {
                if(hitInfo.transform.tag == "Rock")
                    hitInfo.transform.GetComponent<Rock>().Mining();
                else if (hitInfo.transform.tag == "NPC")
                    hitInfo.transform.GetComponent<Pig>().Damaged(1,transform.position);
                SoundManager.instance.PlaySE("Animal_Hit");
                isSwing = false;
            }
            yield return null;

        }
    }


}
