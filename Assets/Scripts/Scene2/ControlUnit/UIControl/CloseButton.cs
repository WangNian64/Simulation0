using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace BlackBox.WareHouse.ControlUnit.UIControl
{
    public class CloseButton : MonoBehaviour
    {

        // Use this for initialization
        public void Click()
        {
            GameObject Parent = this.transform.parent.transform.parent.gameObject;
            if (Parent.name == "MessageInterface")
            {
                Undo();
            }
            DestroyImmediate(Parent);
        }
        public void Undo()
        {
            string CargoName = this.transform.parent.transform.Find("Item1").transform.Find("Value").GetComponent<Text>().text;
            GameObject Cargo = (GameObject)Resources.Load(Varibles.GlobalVariable.RootName + "/Simulation/Cargo");
            Material[] Material = Cargo.GetComponent<Renderer>().sharedMaterials;
            GameObject.Find(CargoName).GetComponent<Renderer>().sharedMaterials = Material;
            Varibles.GlobalVariable.FollowState = false;
        }
    }

}
