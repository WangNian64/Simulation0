using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Click()
    {
        GameObject CargoMessageInterface = Instantiate((GameObject)Resources.Load(GlobalVariable.RootName+"/Simulation/MessageInterface1"));
        CargoMessageInterface.name = "MessageInterface";
        GameObject Panel = CargoMessageInterface.transform.Find("Panel").gameObject;

        GameObject parent = this.transform.parent.gameObject;
        string CargoName = parent.transform.Find("Name").GetComponent<Text>().text;
        CargoMessage CM = GameObject.Find(CargoName).GetComponent<ShowCargoInfo>().Cargomessage;
        //调整面板显示信息
        Panel.transform.Find("Item1").transform.Find("Value").GetComponent<Text>().text = CM.Name;
        Panel.transform.Find("Item2").transform.Find("Value").GetComponent<Text>().text = CM.Size.x.ToString() + "*" + CM.Size.y.ToString() + "*" + CM.Size.z.ToString();
        Panel.transform.Find("Item3").transform.Find("Value").GetComponent<Text>().text = CM.Number1;
        Panel.transform.Find("Item4").transform.Find("Value").GetComponent<Text>().text = CM.InputTime;
        string position = "第" + CM.PositionInfo.HighBayNum + "货架" + CM.PositionInfo.FloorNum + "层" + CM.PositionInfo.ColumnNum + "列" + CM.PositionInfo.place.ToString() + "位";
        Panel.transform.Find("Item5").transform.Find("Value").GetComponent<Text>().text = position;
        Panel.transform.Find("Item6").transform.Find("Value").GetComponent<Text>().text = CM.Description;
        //更改货物颜色
        Material[] Material1 = GameObject.Find(CargoName).GetComponent<Renderer>().sharedMaterials;
        Material[] Material2 = Material1;
        Material Material3 = (Material)Resources.Load(GlobalVariable.RootName+"/Cargo/Material3");
        Material2[10] = Material3;
        GameObject.Find(CargoName).GetComponent<Renderer>().sharedMaterials = Material2;
        //进行相机跟随
        GlobalVariable.FollowPlayer = GameObject.Find(CargoName);
        GlobalVariable.FollowState = true;

    }

    public void Click2()
    {
        string ButtonName = this.name;
        string CargoName = ButtonName.Substring(4);
        CargoMessage CM = GameObject.Find(CargoName).GetComponent<ShowCargoInfo>().Cargomessage;
        int HighBayNum = CM.PositionInfo.HighBayNum; int FloorNum = CM.PositionInfo.FloorNum;
        int ColumnNum = CM.PositionInfo.ColumnNum; Place PlaceNum = CM.PositionInfo.place;
        StorageBinState state = StorageBinState.NotStored;
        switch (PlaceNum)
        {
            case Place.A:
                state = GlobalVariable.BinState[HighBayNum - 1, FloorNum - 1, ColumnNum - 1, 0];
                break;
            case Place.B:
                state = GlobalVariable.BinState[HighBayNum - 1, FloorNum - 1, ColumnNum - 1, 1];
                break;
        }


        if (state == StorageBinState.Stored)
        {
            GameObject CargoMessageInterface = Instantiate((GameObject)Resources.Load(GlobalVariable.RootName+"/Simulation/MessageInterface2"));
            CargoMessageInterface.name = "MessageInterface";
            GameObject Panel = CargoMessageInterface.transform.Find("Panel").gameObject;
            //调整面板显示信息
            Panel.transform.Find("Item1").transform.Find("Value").GetComponent<Text>().text = CM.Name;
            Panel.transform.Find("Item2").transform.Find("Value").GetComponent<Text>().text = CM.Size.x.ToString() + "*" + CM.Size.y.ToString() + "*" + CM.Size.z.ToString();
            Panel.transform.Find("Item3").transform.Find("Value").GetComponent<Text>().text = CM.Number1;
            Panel.transform.Find("Item4").transform.Find("Value").GetComponent<Text>().text = CM.InputTime;
            string position = "第" + CM.PositionInfo.HighBayNum + "货架" + CM.PositionInfo.FloorNum + "层" + CM.PositionInfo.ColumnNum + "列" + CM.PositionInfo.place.ToString() + "位";
            Panel.transform.Find("Item5").transform.Find("Value").GetComponent<Text>().text = position;
            Panel.transform.Find("Item6").transform.Find("Value").GetComponent<Text>().text = CM.Description;
            //更改货物颜色
            Material[] Material1 = GameObject.Find(CargoName).GetComponent<Renderer>().sharedMaterials;
            Material[] Material2 = Material1;
            Material Material3 = (Material)Resources.Load(GlobalVariable.RootName+"/Cargo/Material3");
            Material2[10] = Material3;
            GameObject.Find(CargoName).GetComponent<Renderer>().sharedMaterials = Material2;
            //进行相机跟随
            GlobalVariable.FollowPlayer = GameObject.Find(CargoName);
            GlobalVariable.FollowState = true;
        }
        else
        {
            GameObject CargoMessageInterface = Instantiate((GameObject)Resources.Load(GlobalVariable.RootName+"/Simulation/MessageInterface1"));
            CargoMessageInterface.name = "MessageInterface";
            GameObject Panel = CargoMessageInterface.transform.Find("Panel").gameObject;
            //调整面板显示信息
            Panel.transform.Find("Item1").transform.Find("Value").GetComponent<Text>().text = CM.Name;
            Panel.transform.Find("Item2").transform.Find("Value").GetComponent<Text>().text = CM.Size.x.ToString() + "*" + CM.Size.y.ToString() + "*" + CM.Size.z.ToString();
            Panel.transform.Find("Item3").transform.Find("Value").GetComponent<Text>().text = CM.Number1;
            Panel.transform.Find("Item4").transform.Find("Value").GetComponent<Text>().text = CM.InputTime;
            string position = "第" + CM.PositionInfo.HighBayNum + "货架" + CM.PositionInfo.FloorNum + "层" + CM.PositionInfo.ColumnNum + "列" + CM.PositionInfo.place.ToString() + "位";
            Panel.transform.Find("Item5").transform.Find("Value").GetComponent<Text>().text = position;
            Panel.transform.Find("Item6").transform.Find("Value").GetComponent<Text>().text = CM.Description;
            //更改货物颜色
            Material[] Material1 = GameObject.Find(CargoName).GetComponent<Renderer>().sharedMaterials;
            Material[] Material2 = Material1;
            Material Material3 = (Material)Resources.Load(GlobalVariable.RootName+"/Cargo/Material3");
            Material2[10] = Material3;
            GameObject.Find(CargoName).GetComponent<Renderer>().sharedMaterials = Material2;
            //进行相机跟随
            GlobalVariable.FollowPlayer = GameObject.Find(CargoName);
            GlobalVariable.FollowState = true;
        }
        
    }
}
