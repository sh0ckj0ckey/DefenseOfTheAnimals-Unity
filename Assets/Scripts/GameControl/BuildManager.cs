using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    public GuardianData CactusGuardian;
    public GuardianData SoulStreamGuardian;
    public GuardianData ChainLightningGuardian;

    public GuardianData ChaosMeteor;
    public GuardianData Portal;
    public GuardianData DeleteGuardian;
    public GuardianData UpgradeGuardian;

    // 当前选中的守卫
    public GuardianData SelectedGuardian = null;

    public Text MoneyText;

    public Animator MoneyAnimator;

    public double Money = 50;

    public void AddMoney(double change = 0)
    {
        Money += change;
        MoneyText.text = Money.ToString();
    }

    private void Awake()
    {
        Instance = this;    //把当前的对象赋给Instance便于外部访问
    }

    private void Start()
    {
        SelectedGuardian = CactusGuardian;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                var map = LayerMask.GetMask("MapCube");
                bool isCollider = Physics.Raycast(ray, out hit, 1000, map);
                if (isCollider)
                {
                    MapCube mapCube = hit.collider.GetComponent<MapCube>();   // 得到点击的MapCube
                    if (mapCube != null)
                    {
                        if (SelectedGuardian.type == GuardianType.Cactus ||
                            SelectedGuardian.type == GuardianType.SoulStream ||
                            SelectedGuardian.type == GuardianType.ChainLightning)
                        {
                            if (mapCube.GuardianGameObject == null)
                            {
                                if (Money >= SelectedGuardian.Cost)
                                {
                                    AddMoney(-SelectedGuardian.Cost);
                                    mapCube.SummonGuardian(SelectedGuardian.GuardianPrefab);
                                }
                                else
                                {
                                    MoneyAnimator.SetTrigger("Flick");
                                }
                            }
                        }
                        else if (SelectedGuardian.type == GuardianType.Upgrade)
                        {
                            if (mapCube.GuardianGameObject != null && mapCube.CubeGuardian != null && mapCube.CubeGuardian.bGuardianUpgraded == false)
                            {
                                //升级
                                if (Money >= SelectedGuardian.UpgradeCost)
                                {
                                    AddMoney(-SelectedGuardian.UpgradeCost);
                                    mapCube.UpgradeGuardian();
                                }
                            }
                        }
                        else if (SelectedGuardian.type == GuardianType.Delete)
                        {
                            if (mapCube.GuardianGameObject != null)
                            {
                                //拆除
                                var guardian = mapCube.GuardianGameObject.GetComponent<Guardian>();
                                double money = 0;
                                switch (guardian.GuardianType)
                                {
                                    case GuardianType.Cactus:
                                        money = CactusGuardian.Cost * (0.5 + (mapCube.CubeGuardian.bGuardianUpgraded ? 0.5 : 0));
                                        break;
                                    case GuardianType.SoulStream:
                                        money = SoulStreamGuardian.Cost * (0.5 + (mapCube.CubeGuardian.bGuardianUpgraded ? 0.5 : 0));
                                        break;
                                    case GuardianType.ChainLightning:
                                        money = ChainLightningGuardian.Cost * (0.5 + (mapCube.CubeGuardian.bGuardianUpgraded ? 0.5 : 0));
                                        break;
                                    default:
                                        break;
                                }
                                AddMoney(money);
                                mapCube.DeleteGuardian();
                            }
                        }
                    }
                    else
                    {
                        RoadCube roadCube = hit.collider.GetComponent<RoadCube>();
                        if (roadCube != null)
                        {
                            if (roadCube.CubeItemGameObject == null)
                            {
                                if (SelectedGuardian.type == GuardianType.ChaosMeteor)
                                {
                                    //if (Money >= SelectedGuardian.Cost)
                                    //{
                                    //    AddMoney(-SelectedGuardian.Cost);
                                    //    roadCube.SummonChaosMeteor(SelectedGuardian.GuardianPrefab);
                                    //}
                                    //else
                                    //{
                                    //    MoneyAnimator.SetTrigger("Flick");
                                    //}
                                }
                                else if (SelectedGuardian.type == GuardianType.Portal)
                                {
                                    if (Money >= SelectedGuardian.Cost)
                                    {
                                        AddMoney(-SelectedGuardian.Cost);
                                        //roadCube.SummonGuardian(SelectedGuardian.GuardianPrefab);
                                    }
                                    else
                                    {
                                        MoneyAnimator.SetTrigger("Flick");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void OnCactusGuardianSelected(bool isOn)
    {
        if (isOn)
        {
            SelectedGuardian = CactusGuardian;
        }
    }
    public void OnSoulStreamGuardianSelected(bool isOn)
    {
        if (isOn)
        {
            SelectedGuardian = SoulStreamGuardian;
        }
    }
    public void OnChainLightningGuardianSelected(bool isOn)
    {
        if (isOn)
        {
            SelectedGuardian = ChainLightningGuardian;
        }
    }
    public void OnChaosMeteorSelected(bool isOn)
    {
        if (isOn)
        {
            SelectedGuardian = ChaosMeteor;
        }
    }
    public void OnPortalSelected(bool isOn)
    {
        if (isOn)
        {
            SelectedGuardian = Portal;
        }
    }
    public void OnDeleteGuardianSelected(bool isOn)
    {
        if (isOn)
        {
            SelectedGuardian = DeleteGuardian;
        }
    }
    public void OnUpgradeGuardianSelected(bool isOn)
    {
        if (isOn)
        {
            SelectedGuardian = UpgradeGuardian;
        }
    }

    public void GiveMeMoney(double money)
    {
        Money = money;
        MoneyText.text = Money.ToString();
    }
}
