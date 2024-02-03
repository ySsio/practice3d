using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    


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
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;

        }
    }

    
}
