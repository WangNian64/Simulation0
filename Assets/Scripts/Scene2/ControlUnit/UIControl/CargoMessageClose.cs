using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace BlackBox.WareHouse.ControlUnit.UIControl
{
    public class CargoMessageClose : MonoBehaviour
    {
        public void Click()
        {
            GameObject Cargo = (GameObject)Resources.Load("Scene/Simulation/Cargo");
            Material[] Material = Cargo.GetComponent<Renderer>().sharedMaterials;
            string CargoName = GameObject.Find("CargoMessageInterface").transform.Find("Panel").transform.Find("Item1").transform.Find("Value").GetComponent<Text>().text;
            GameObject.Find(CargoName).GetComponent<Renderer>().sharedMaterials = Material;
            DestroyImmediate(GameObject.Find("CargoMessageInterface"));
            Varibles.GlobalVariable.FollowState = false;
        }
    }

}
