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
        // delayA와 B 사이에서 계속 공격 닿았는지 반복적으로 체크
        while (isSwing)
        {
            if (CheckObject())
            {
                if(hitInfo.transform.tag == "Rock")
                    hitInfo.transform.GetComponent<Rock>().Mining();
                else if (hitInfo.transform.tag == "WeakAnimal")
                {
                    SoundManager.instance.PlaySE("Animal_Hit"); // 내가 동물 때리는 소리
                    hitInfo.transform.GetComponent<WeakAnimal>().Damaged(1, transform.position);
                }
                else if (hitInfo.transform.tag == "StrongAnimal")
                {

                }

                    
                isSwing = false;
            }
            yield return null;

        }
    }


}
