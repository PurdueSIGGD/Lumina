using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDecision : ScriptableObject {

    public abstract void Decide(BaseEnemy enemy);
}
