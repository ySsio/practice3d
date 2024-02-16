using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true; // 플레이어의 움직임 제어

    public static bool isOpenInventory = false; // 인벤토리 활성화
    public static bool isOpenCraftManual = false; // 건축메뉴 활성화

    public static bool isNight = false;
    public static bool isWater = false;

    public static bool isPause = false; // 메뉴 호출되면 true

    private WeaponManager theWM;
    private bool flag = false;

    void Start()
    {
        // 마우스를 가운데 고정하고 안보이게 만듦
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // 사실 이건 위 코드에 포함된 내용
        theWM = FindObjectOfType<WeaponManager>();
    }

    void Update()
    {
        if (isOpenInventory || isOpenCraftManual || isPause)
        {
            canPlayerMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else
        {
            canPlayerMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if(isWater)
        {
            if (!flag)
            {
                StopAllCoroutines();
                StartCoroutine(theWM.WeaponInCoroutine());
                flag = true;
            }
            
        }
        else
        {
            if (flag)
            {
                theWM.WeaponOut();
                flag = false;
            }
        }
            
    }
}
