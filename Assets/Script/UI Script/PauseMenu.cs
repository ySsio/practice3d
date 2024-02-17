using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI;

   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.isPause)
            {
                CallMenu();
            }
            else
            {
                CloseMenu();
            }
        }
    }

    private void CallMenu()
    {
        GameManager.isPause = true;
        go_BaseUI.SetActive(true);
        // �ð� �帧 ����
        Time.timeScale = 0f; // 0������� ����
    }

    private void CloseMenu()
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);
        Time.timeScale = 1f; // 1������� ����
    }

    public void ClickSave()
    {
        Debug.Log("���̺�");
    }

    public void ClickLoad()
    {
        Debug.Log("�ε�");
    }

    public void ClickExit()
    {
        Debug.Log("���� ����");
        Application.Quit();
    }


}
