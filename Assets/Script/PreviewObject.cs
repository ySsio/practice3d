using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    // 프리뷰 오브젝트가 충돌한 오브젝트의 콜라이더
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField] private int layerGround; // 지상 레이어. (무시)
    private const int IGNORE_RAYCAST_LAYER = 2; // IGNORERAYCAST 레이어 번호는 2이다.

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
        // 이 스크립트가 붙은 객체 (transform) 하위의 객체의 transform을 차례로 받아오는 반복문
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
