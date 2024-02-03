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
    private CloseWeapon[] hands; // �� ���
    [SerializeField]
    private CloseWeapon[] axes; // ���� ���
    [SerializeField]
    private CloseWeapon[] pickaxes; // ���� ���

    // guns �迭�� � ���Ⱑ ����ִ��� �����ϱ� ���� �ϱ� ���� ��ųʸ� ����
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    // �ʿ��� ������Ʈ
    // �� ���� Ű�� �ٸ� ���� ��. gun�� hand�� �����ϱ� ���� ���� ����
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickaxeController thePickaxeController;


    // ���� ������ Ÿ�� (GUN / HAND / AXE)
    [SerializeField]
    private string currentWeaponType = "PICKAXE";

    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }

        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }

        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
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
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // ���� ��ü ���� (����ӽŰ�)
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                // ���� ��ü ���� (����)
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                // ���� ��ü ���� (���)
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));
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
            case "AXE":
                AxeController.isActivate = false;
                theAxeController.enabled = false;
                break;
            case "PICKAXE":
                PickaxeController.isActivate = false;
                thePickaxeController.enabled = false;
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
            theHandController.CloseWeaponChange(handDictionary[_name]);
            theHandController.enabled = true;
        }
        else if (_type == "AXE")
        {
            theAxeController.CloseWeaponChange(axeDictionary[_name]);
            theAxeController.enabled = true;
        }
        else if (_type == "PICKAXE")
        {
            thePickaxeController.CloseWeaponChange(pickaxeDictionary[_name]);
            thePickaxeController.enabled = true;
        }

        currentWeaponType = _type;
    }
}

