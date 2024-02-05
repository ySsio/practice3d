using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private Text txt_ItemName;
    [SerializeField]
    private Text txt_Description;
    [SerializeField]
    private Text txt_How2Use;

    // Inventory에서 가져다 쓸 예정 (아이템에 마우스 올리면 툴팁을 띄운다)
    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);
        _pos += new Vector3 (go_Base.GetComponent<RectTransform>().rect.width * 0.5f, -go_Base.GetComponent<RectTransform>().rect.height * 0.5f, 0);
        go_Base.transform.position = _pos;

        txt_ItemName.text = _item.itemName;
        txt_Description.text = _item.itemDescription;

        switch (_item.itemType) {
            case Item.ItemType.Equipment:
                txt_How2Use.text = "우클릭 - 장착";
                break;
            case Item.ItemType.Used:
                txt_How2Use.text = "우클릭 - 사용";
                break;
            default:
                txt_How2Use.text = "";
                break;
        }
    }
    
    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
