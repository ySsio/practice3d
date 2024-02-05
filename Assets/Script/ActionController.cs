using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; // ������ ���� ���� �ִ� �Ÿ�

    private bool pickupActivated = false; // ���� ������ �� true (������ �ٶ󺸰� ��Ÿ� �ȿ� ������)

    private RaycastHit hitInfo; // �浹ü ���� (item ����)

    [SerializeField]
    private LayerMask layerMask; // Item ���̾ ���ؼ��� �����ϵ��� ���̾��ũ ����


    // �ʿ��� ������Ʈ
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
            CheckItem(); // ������ �浹 Ȯ��
            PickUp();
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))  // ������ �ٶ󺸸� �ݱ� Ȱ��ȭ, ���������� �ؽ�Ʈ ���
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
            
        }
        else        // ������ ���ٰ� �ٸ� �� ���� �ݱ� ��Ȱ��ȭ�ǰ� ���������� �ؽ�Ʈ�� �������
        {
            ItemInfoDisappear();
        }
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ��" + "<color=yellow> (E) </color>";
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
            if (hitInfo.transform != null) // Ȥ�� �� ���� ����
            {
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject); // ���� �ֿ� ������ ���忡�� ����
                ItemInfoDisappear(); // ȹ�� ��Ȱ��ȭ, �ؽ�Ʈ ��Ȱ��ȭ
            }
        }
    }
}
