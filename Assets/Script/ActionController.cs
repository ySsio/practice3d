using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; // 아이템 습득 가능 최대 거리

    private bool pickupActivated = false; // 습득 가능할 시 true (아이템 바라보고 사거리 안에 닿으면)

    private RaycastHit hitInfo; // 충돌체 정보 (item 정보)

    [SerializeField]
    private LayerMask layerMask; // Item 레이어에 대해서만 반응하도록 레이어마스크 설정


    // 필요한 컴포넌트
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;


    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
        Debug.DrawRay(transform.position, transform.forward * range, Color.red);
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem(); // 아이템 충돌 확인
            PickUp();
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))  // 아이템 바라보면 줍기 활성화, 아이템정보 텍스트 띄움
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
            
        }
        else        // 아이템 보다가 다른 거 보면 줍기 비활성화되고 아이템정보 텍스트도 사라지게
        {
            ItemInfoDisappear();
        }
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득" + "<color=yellow> (E) </color>";
    }

    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void PickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null) // 혹시 모를 오류 방지
            {
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject); // 내가 주운 아이템 월드에서 삭제
                ItemInfoDisappear(); // 획득 비활성화, 텍스트 비활성화
            }
        }
    }
}
