using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTransferMove : MonoBehaviour {
    public enum Pattern
    {
        up, down,off
    }

    private GameObject LiftPart;
    public GameObject Cargo;
    public float Speed;
    private float High2;
    public Pattern pattern;
    private Vector3 TargetPosition1;
    private Vector3 TargetPosition2;
    public bool Finish1;
    public bool Finish2;
	void Start () {
        string name = this.name + "/LiftPart";
        LiftPart = GameObject.Find(name);
        //KeyPositionsData KPD = GameObject.Find("WarehouseScene").GetComponent<ShowKeyPositionData>().KeyPositionsData;
        float temphigh = GlobalVariable.KPD.HighValues[0] - GlobalVariable.KPD.HighValues[1];
        
        TargetPosition1 = LiftPart.transform.localPosition;
        TargetPosition2 = TargetPosition1; TargetPosition2.y = TargetPosition1.y + temphigh;
        //pattern = Pattern.off;
        Speed = 0.8f;
        //Cargo.transform.parent = LiftPart.transform;
        Finish1 = false;
        Finish2 = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (pattern == Pattern.up)
        {   
            LiftPart.transform.localPosition = Vector3.MoveTowards(LiftPart.transform.localPosition, TargetPosition2, Speed * Time.deltaTime);
            if (LiftPart.transform.localPosition == TargetPosition2)
            {
                Finish1 = true;
                pattern = Pattern.off;
            }
        }
        else if (pattern == Pattern.down)
        {
            LiftPart.transform.localPosition = Vector3.MoveTowards(LiftPart.transform.localPosition, TargetPosition1, Speed * Time.deltaTime);
            if (LiftPart.transform.localPosition == TargetPosition1)
            {
                Finish2 = true;
                pattern = Pattern.off;
                //DestroyImmediate(this.GetComponent<LiftTransferMove>());
            }
        }
        
	}
}
