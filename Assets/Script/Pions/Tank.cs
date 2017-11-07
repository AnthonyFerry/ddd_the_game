using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : BasePawn {

    public override void Init(PawnData datum)
    {
        base.Init(datum);
    }

    new void Update () {
        base.Update();
	}

    public override bool atkFunction(BasePawn target)
    {
        return base.atkFunction(target);
    }
}
