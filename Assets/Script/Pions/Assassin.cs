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
        //return base.atkFunction(target);
        int totalDamages = this.dealDamages(target.PawnType) * criticalHit();
        bool result = target.takeDamages(totalDamages);
        if (GameManager.Instance.getRemainingOpponentsAmount(this.isPlayer) > 0)
        {
            if (result || GameManager.Instance.getRemainingOpponentsAmount(this.isPlayer) > 1)
            {
                GameManager.Instance.TurnTransition();
            }
        }
        return result;
    }

    int criticalHit() {
        /*if(Mathf.FloorToInt(Random.Range(0.0f, 100.0f)) <= 5) {
            return 2;
        } else {
            return 1;
        }*/
        return Mathf.FloorToInt(Random.Range(0.0f, 100.0f)) <= 5 ? 2 : 1;
    }
}
