using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true; // �÷��̾��� ������ ����

    public static bool isOpenInventory = false; // �κ��丮 Ȱ��ȭ
    public static bool isOpenCraftManual = false; // ����޴� Ȱ��ȭ

    public static bool isNight = false;
    public static bool isWater = false;

    void Start()
    {
        // ���콺�� ��� �����ϰ� �Ⱥ��̰� ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // ��� �̰� �� �ڵ忡 ���Ե� ����
    }

    void Update()
    {
        if (isOpenInventory || isOpenCraftManual)
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
            
    }
}
