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
        // delayA�� B ���̿��� ��� ���� ��Ҵ��� �ݺ������� üũ
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
