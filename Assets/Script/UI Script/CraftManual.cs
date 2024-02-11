using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;


public class CraftManual : MonoBehaviour
{
    // 상태변수
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField] private GameObject go_BaseUI; // 기본 베이스 UI. 껐다 켜기 위해
    [SerializeField] private Craft[] craft_fire; // 모닥불용 탭

    private GameObject go_Preview; // 미리보기 프리펩 저장 변수
    private GameObject go_Prefab; // 실제로 생성될 프리펩 저장 변수


    [SerializeField] private Transform tf_Player;

    // Raycast 필요 변수 선언
    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;


    // 컴포넌트 저장 변수
    [SerializeField]
    private GameObject[] tabList; // 탭 리스트 저장할 변수
    [SerializeField]
    private GameObject[] slotsList; // 슬롯그룹리스트 저장할 변수



    public void SlotClick(int _slotNumber)
    {
        CloseWindow();
        isPreviewActivated = true;

        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;

    }

    // Tab을 누르면 해당 Tab에 해당하는 건축물들로 슬롯 그룹을 바꾼다.
    // 레시피를 습득할 때마다 탭 객체/ 슬롯 그리드 객체 를 리스트로 받아와서 저장해두고, _tabNumber와 같은 인덱스의 슬롯그룹을 켜는 식으로 하면 될 듯.
    public void TabClick(int _tabNumber)
    {
        for (int i = 0; i < slotsList.Length; i++)
        {
            slotsList[i].SetActive(false);
        }
        slotsList[_tabNumber].SetActive(true);
    }


    // 새로운 크래프트 레시피를 습득하면 크래프트 매뉴얼에 추가하는 기능.
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
                Vector3 _location = hitInfo.point; // 실제 레이와 충돌한 지점 좌표 반환
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
