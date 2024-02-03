using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // static�� Ŭ���� ������ �����ϴ� ��. ��� ��ü�� �����ϴ� ����
    // �ν��Ͻ��� �������� �ʾƵ� Ŭ�������� �ٷ� ������ �� �� ����. WeaponManager.isChaneWeapon �̷���

    // ���� �ߺ� ��ü ���� ����.
    public static bool isChangeWeapon = false;

    // ���� ����� �ִϸ��̼�
    public static Transform currentWeapon; // Transform class�� ������ ������ �� ������ ���� Ű�� ���Ҹ� �ҰŰ�,
                                           // ���� ���Ⱑ Gun type �� ���� �ְ� Hand type�� �� �� �ִµ�
                                           // �̰Ÿ� �� ���� ������ �ޱ� ���� Transform type���� ������ ��.
    public static Animator currentWeaponAnim;

    // ���� ��ü ������
    [SerializeField]
    private float changeWeaponDelayTime; // �� �ִ� �ð�
    [SerializeField]
    private float changeWeaponEndDelayTime; // ���� ������ �ð�

    // ���� ���� ���� ����
    [SerializeField]
    private Gun[] guns; // ���� ���
    [SerializeField]
    private Hand[] hands; // �� ���

    // guns �迭�� � ���Ⱑ ����ִ��� �����ϱ� ���� �ϱ� ���� ��ųʸ� ����
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

    // �ʿ��� ������Ʈ
    // �� ���� Ű�� �ٸ� ���� ��. gun�� hand�� �����ϱ� ���� ���� ����
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;


    // ���� ������ Ÿ�� (GUN / HAND)
    [SerializeField]
    private string currentWeaponType;

    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }

        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // ���� ��ü ���� (�Ǽ�)
                StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�"));
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // ���� ��ü ���� (����ӽŰ�)
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
        }
    }


    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type,_name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CancelReload();
                GunController.isActivate = false;
                theGunController.enabled = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                theHandController.enabled = false;
                break;
        }
    }


    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
        {
            theGunController.GunChange(gunDictionary[_name]);
            theGunController.enabled = true;
        }
        else if (_type == "HAND")
        {
            theHandController.HandChange(handDictionary[_name]);
            theHandController.enabled = true;
        }

        currentWeaponType = _type;
    }
}

