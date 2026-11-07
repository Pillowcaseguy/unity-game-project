using UnityEngine;

public class Soldier_Torso_Behavior : MonoBehaviour
{
    public Animator ani;
    private void Awake()
    {
        ani = GetComponent<Animator>();
    }
    public void end_deflect()
    {
        ani.SetBool("knife_deflecting", false);
    }
}
