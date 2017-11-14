using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : BasePawn {

    public override void Init(PawnData datum)
    {
        base.Init(datum);
    }

    new void Update()
    {
        base.Update();
    }

    public override bool atkFunction(BasePawn target)
    {
        //if not next to the target, can move to the closer position
        //moveFunction(_movements.findNearestDestination(target.PawnLocation));
        return base.atkFunction(target);
    }
}
