using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;


public class CraftManual : MonoBehaviour
{
    // ���º���
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField] private GameObject go_BaseUI; // �⺻ ���̽� UI. ���� �ѱ� ����
    [SerializeField] private Craft[] craft_fire; // ��ںҿ� ��

    private GameObject go_Preview; // �̸����� ������ ���� ����
    private GameObject go_Prefab; // ������ ������ ������ ���� ����


    [SerializeField] private Transform tf_Player;

    // Raycast �ʿ� ���� ����
    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;


    // ������Ʈ ���� ����
    [SerializeField]
    private GameObject[] tabList; // �� ����Ʈ ������ ����
    [SerializeField]
    private GameObject[] slotsList; // ���Ա׷츮��Ʈ ������ ����



    public void SlotClick(int _slotNumber)
    {
        CloseWindow();
        isPreviewActivated = true;

        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;

    }

    // Tab�� ������ �ش� Tab�� �ش��ϴ� ���๰��� ���� �׷��� �ٲ۴�.
    // �����Ǹ� ������ ������ �� ��ü/ ���� �׸��� ��ü �� ����Ʈ�� �޾ƿͼ� �����صΰ�, _tabNumber�� ���� �ε����� ���Ա׷��� �Ѵ� ������ �ϸ� �� ��.
    public void TabClick(int _tabNumber)
    {
        for (int i = 0; i < slotsList.Length; i++)
        {
            slotsList[i].SetActive(false);
        }
        slotsList[_tabNumber].SetActive(true);
    }


    // ���ο� ũ����Ʈ �����Ǹ� �����ϸ� ũ����Ʈ �Ŵ��� �߰��ϴ� ���.
    public void LearnBluePrint(string _craftName)
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            ToggleWindow();
        }

        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }

    private void Build()
    {
        if (!isPreviewActivated || !go_Preview.GetComponent<PreviewObject>().isBuildable())
            return;

        Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
        Destroy(go_Preview);

        isActivated = false;
        isPreviewActivated = false;
        
        go_Preview = null;
        go_Prefab = null;
    }
    
    private void PreviewPositionUpdate ()
    {
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point; // ���� ���̿� �浹�� ���� ��ǥ ��ȯ
                go_Preview.transform.position = _location;
            }
        }
    }

    private void Cancel()
    {
        if (isPreviewActivated )
            Destroy(go_Preview);

        CloseWindow();

        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;


    }


    private void ToggleWindow()
    {
        //isActivated = !isActivated;
        if (!isActivated)
        {
            OpenWindow();
        } else
        {
            CloseWindow();
        }
    }

    private void OpenWindow()
    {
        isActivated = true;
        GameManager.isOpenCraftManual = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        isActivated = false;
        GameManager.isOpenCraftManual = false;
        go_BaseUI.SetActive(false);
    }

}
