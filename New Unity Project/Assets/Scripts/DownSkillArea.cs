using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownSkillArea : MonoBehaviour
{
    public int attackPower = 60;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Orc")
        {
            OrcLogic logic = other.transform.GetComponent<OrcLogic>();
            logic.TakeDamage(attackPower);
        }
        else if (other.transform.tag == "HunterGoblin")
        {
            GoblinLogic logic = other.transform.GetComponent<GoblinLogic>();
            logic.TakeDamage(attackPower);
        }
        else if (other.transform.tag == "GroundGoblin")
        {
            GoblinFSM logic = other.transform.GetComponent<GoblinFSM>();
            logic.TakeDamage(attackPower);
        }
        else if (other.transform.tag == "Golem")
        {
            Golem logic = other.transform.GetComponent<Golem>();
            logic.TakeDamage(attackPower);
        }
        else if (other.transform.tag == "Boss")
        {

            Dragon logic = other.transform.GetComponent<Dragon>();
            logic.TakeDamage(attackPower);
        }
    }
}
