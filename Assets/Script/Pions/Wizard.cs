using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : BasePawn {

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
        //attacks also deals damages to enemy close to the impact point
        BasePawn p = null;
        Vector2 mod;

        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0)
                    continue;
                
                if (target.PawnLocation.y % 2 == 0)
                {
                    if (i > 0 && (j == -1 || j == 1))
                        continue;
                }
                else {
                    if (i < 0 && (j == -1 || j == 1))
                        continue;
                }

                mod = new Vector2(i, j);
                p = GameManager.Instance.getPawnByLocation(target.PawnLocation + mod);
                if (p != null) {
                    int dmg = this.dealDamages(p.PawnType) / 2;
                    Debug.Log(dmg);
                    if (!p.takeDamages(dmg))
                        GameManager.Instance.destroyPawn(p);
                }
            }
        }
        
        return base.atkFunction(target);
    }
}
