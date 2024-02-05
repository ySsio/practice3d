using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;


// Slot�� �巡�� �� �� �°� ���� �������� �ʰ� DragSlot�� �׶� �׶� �·� �����ؼ� ��� ������
// DragSlot image ������Ʈ�� raycast target üũ �����ؾ� slot�� �����Ǿ �巡�� �̺�Ʈ�� �߻��ϹǷ� �� üũ ������ ��.
// dragslot�� slot���� �տ� �ְ� �Ϸ��ٺ��� �߻��ϴ� �������� raycast target �����ϸ� ���콺 �̺�Ʈ�� ���� �ȵ�.
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
