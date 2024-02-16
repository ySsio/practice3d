using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true; // �÷��̾��� ������ ����

    public static bool isOpenInventory = false; // �κ��丮 Ȱ��ȭ
    public static bool isOpenCraftManual = false; // ����޴� Ȱ��ȭ

    public static bool isNight = false;
    public static bool isWater = false;

    public static bool isPause = false; // �޴� ȣ��Ǹ� true

    private WeaponManager theWM;
    private bool flag = false;

    void Start()
    {
        // ���콺�� ��� �����ϰ� �Ⱥ��̰� ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // ��� �̰� �� �ڵ忡 ���Ե� ����
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
