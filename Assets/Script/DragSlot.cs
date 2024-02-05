using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;


// Slot을 드래그 할 때 걔가 직접 움직이지 않고 DragSlot이 그때 그때 걔로 변신해서 대신 움직임
// DragSlot image 컴포넌트의 raycast target 체크 해제해야 slot이 감지되어서 드래그 이벤트가 발생하므로 꼭 체크 해제할 것.
// dragslot이 slot보다 앞에 있게 하려다보니 발생하는 문제지만 raycast target 해제하면 마우스 이벤트에 감지 안됨.
public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;

    public Slot dragSlot;

    [SerializeField]
    private Image imageItem;


    private void Start()
    {
        instance = this;
    }


    public void DragSetImage(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha) 
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
