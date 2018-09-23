using UnityEngine;
using UnityEngine.UI;
using System;

public class Functions :  MonoBehaviour{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void ChangeColor(string CargoName, StorageBinState state)
    {
        CargoMessage CM = GameObject.Find(CargoName).GetComponent<ShowCargoInfo>().Cargomessage;
        int HighBayNum = CM.PositionInfo.HighBayNum; int FloorNum = CM.PositionInfo.FloorNum;
        int ColumnNum = CM.PositionInfo.ColumnNum; Place PlaceNum = CM.PositionInfo.place;

        string BinName = "StorageStateInterface/MainBody/Scroll View/Viewport/Content/ShelfPanel" + HighBayNum.ToString();
        BinName = BinName + "/Scroll View/Viewport/Content/Panel/BinsPanel/FloorItem" + FloorNum.ToString();
        BinName = BinName + "/" + PlaceNum.ToString() + "Panel/Bin_" + CargoName;
        switch (PlaceNum)
        {
            case Place.A:
                GlobalVariable.BinState[HighBayNum - 1, FloorNum - 1, ColumnNum - 1, 0] = state;
                break;
            case Place.B:
                GlobalVariable.BinState[HighBayNum - 1, FloorNum - 1, ColumnNum - 1, 1] = state;
                break;
        }
        switch (state)
        {
            case StorageBinState.NotStored:
                GameObject.Find(BinName).GetComponent<Image>().color = GlobalVariable.BinColor[0];
                break;
            case StorageBinState.Reserved:
                GameObject.Find(BinName).GetComponent<Image>().color = GlobalVariable.BinColor[1];
                break;
            case StorageBinState.InStore:
                GameObject.Find(BinName).GetComponent<Image>().color = GlobalVariable.BinColor[2];
                break;
            case StorageBinState.Stored:
                GameObject.Find(BinName).GetComponent<Image>().color = GlobalVariable.BinColor[3];
                break;
            case StorageBinState.Stay2Exit:
                GameObject.Find(BinName).GetComponent<Image>().color = GlobalVariable.BinColor[4];
                break;
            case StorageBinState.OutStore:
                GameObject.Find(BinName).GetComponent<Image>().color = GlobalVariable.BinColor[5];
                break;
        }
    }

    //接口
    public static void Connector(GameObject Cargo)
    {
        string Name = Cargo.name.Remove(0, 4); 
        int i1 = Name.IndexOf("_");
        string HighBayNum = Name.Substring(0, i1); Name = Name.Remove(0, i1 + 1);
        int i2 = Name.IndexOf("_");
        string FloorNum = Name.Substring(0, i2); Name = Name.Remove(0, i2 + 1);
        int i3 = Name.IndexOf("_");
        string ColumnNum = Name.Substring(0, i3);
        string PlaceNum = Name.Remove(0, i3 + 1);
        string CargoName = "Cargo_" + HighBayNum + "_" + FloorNum + "_" + ColumnNum + "_" + PlaceNum;
        int HighBayNum2 = int.Parse(HighBayNum);
        int FloorNum2 = int.Parse(FloorNum);
        int ColumnNum2 = int.Parse(ColumnNum);
        Place place1 = Place.A; 
		Cargo.transform.parent = GlobalVariable.WareHouse.transform;
        Cargo.name = CargoName;

        CargoMessage CI = new CargoMessage();
        CI.Name = Cargo.name;
        CI.Size = GlobalVariable.KPD.CargoSize;
        CI.Number1 = "123456789";
        CI.Num = 5;
        CI.InputTime = DateTime.Now.ToLocalTime().ToString(); 
        CI.Description = "This is a Cargo!";
        CI.PositionInfo.HighBayNum = HighBayNum2;
        CI.PositionInfo.ColumnNum = ColumnNum2;
        CI.PositionInfo.FloorNum = FloorNum2;
        switch (PlaceNum)
        {
            case "A":
                GlobalVariable.BinState[HighBayNum2 - 1, FloorNum2 - 1, ColumnNum2 - 1, 0] = StorageBinState.Reserved;
                place1 = Place.A;
                break;
            case "B":
                GlobalVariable.BinState[HighBayNum2 - 1, FloorNum2 - 1, ColumnNum2 - 1, 1] = StorageBinState.Reserved;
                place1 = Place.B;
                break;
        }
        CI.PositionInfo.place = place1;
        Cargo.AddComponent<ShowCargoInfo>().Cargomessage = CI;
        Cargo.AddComponent<OperatingState>().state = CargoState.WaitIn;
        

        GlobalVariable.EnterCargosNameList.Add(CargoName);
        GlobalVariable.TempQueue.Enqueue(Cargo);

        string BinName = "StorageStateInterface/MainBody/Scroll View/Viewport/Content/ShelfPanel" + HighBayNum;
        BinName = BinName + "/Scroll View/Viewport/Content/Panel/BinsPanel/FloorItem" + FloorNum;
        BinName = BinName + "/" + PlaceNum + "Panel/Bin_" + CargoName;
        GameObject.Find(BinName).GetComponent<Image>().color = GlobalVariable.BinColor[1];

        GameObject Item = Instantiate((GameObject)Resources.Load(GlobalVariable.RootName+"/Simulation/Item"));
        Item.name = Cargo.name;
        Item.transform.Find("Name").GetComponent<Text>().text = Item.name;
        Item.transform.Find("State").GetComponent<Text>().text = "货物状态：" + "等待入库";
        Item.transform.parent = GameObject.Find("ProcessInterface/MainBody/Scroll View/Viewport/Content").transform;
    }
}
