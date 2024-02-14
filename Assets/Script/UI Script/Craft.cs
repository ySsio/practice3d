using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ŭ���ؼ� create �� �� �ִ� �׸� New Item > item�� �߰� �� ��! �������� �� ����Ʈ �̸��� fileName�� �ش��ϴ� �̸�
[CreateAssetMenu(fileName = "New Craft", menuName = "Custom/Craft")]
public class Craft : ScriptableObject        // ���� ������Ʈ�� ���� �ʿ� ����
{
    public string craftName;
    [TextArea]
    public string craftDescription;
    public GameObject go_Prefab; // ���� ��ġ�� ������.
    public GameObject go_PreviewPrefab; // �̸����� ������.

    public CraftType craftType; // ������ ����
    public Sprite craftImage;

    public enum CraftType
    {
        Fire,
        Trap,
    }
    // # enum���� �ϴ� �Ͱ� string/ dictionary �� �޾ƿ��� �Ͱ��� ����?


}
