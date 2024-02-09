using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    // ������ ������Ʈ�� �浹�� ������Ʈ�� �ݶ��̴�
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField] private int layerGround; // ���� ���̾�. (����)
    private const int IGNORE_RAYCAST_LAYER = 2; // IGNORERAYCAST ���̾� ��ȣ�� 2�̴�.

    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Remove(other);
        }
    }

    private void ChangeColor()
    {
        if (colliderList.Count > 0)
            SetColor(red);
        else
            SetColor(green);
    }

    private void SetColor(Material mat)
    {
        // �� ��ũ��Ʈ�� ���� ��ü (transform) ������ ��ü�� transform�� ���ʷ� �޾ƿ��� �ݺ���
        foreach(Transform tf_Child in transform)
        {
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];
            for (int i =0; i< tf_Child.GetComponent<Renderer>().materials.Length; i++)
            {
                newMaterials[i] = mat;
            }
            tf_Child.GetComponent<Renderer>().materials = newMaterials;

        }
    }


    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }
}
