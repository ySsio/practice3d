using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // static은 클래스 변수를 선언하는 것. 모든 객체가 공유하는 변수
    // 인스턴스를 선언하지 않아도 클래스에서 바로 가져다 쓸 수 있음. WeaponManager.isChaneWeapon 이렇게

    // 무기 중복 교체 실행 방지.
    public static bool isChangeWeapon = false;

    // 현재 무기와 애니메이션
    public static Transform currentWeapon; // Transform class로 정의한 이유는 이 변수가 껐다 키는 역할만 할거고,
                                           // 현재 무기가 Gun type 일 수도 있고 Hand type일 수 도 있는데
                                           // 이거를 한 개의 변수에 받기 위해 Transform type으로 지정한 것.
    public static Animator currentWeaponAnim;

    // 무기 교체 딜레이
    [SerializeField]
    private float changeWeaponDelayTime; // 손 넣는 시간
    [SerializeField]
    private float changeWeaponEndDelayTime; // 무기 꺼내는 시간

    // 무기 종류 전부 관리
    [SerializeField]
    private Gun[] guns; // 무기 목록
    [SerializeField]
    private CloseWeapon[] hands; // 손 목록
    [SerializeField]
    private CloseWeapon[] axes; // 도끼 목록
    [SerializeField]
    private CloseWeapon[] pickaxes; // 도끼 목록

    // guns 배열에 어떤 무기가 들어있는지 참조하기 쉽게 하기 위해 딕셔너리 선언
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    // 필요한 컴포넌트
    // 한 쪽을 키면 다른 쪽을 끔. gun과 hand를 구분하기 위해 변수 선언
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickaxeController thePickaxeController;


    // 현재 무기의 타입 (GUN / HAND / AXE)
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
                // 무기 교체 실행 (맨손)
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // 무기 교체 실행 (서브머신건)
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                // 무기 교체 실행 (도끼)
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                // 무기 교체 실행 (곡괭이)
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

