using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;


[System.Serializable]
public class Craft
{
    public string craftName;
    public GameObject go_Prefab; // 실제 설치될 프리펩.
    public GameObject go_PreviewPrefab; // 미리보기 프리펩.
}
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

    

    public void SlotClick(int _slotNumber)
    {
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;

        isPreviewActivated = true;
        go_BaseUI.SetActive(false);

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

        isActivated = false;
        go_BaseUI.SetActive(false);

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
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }

}
