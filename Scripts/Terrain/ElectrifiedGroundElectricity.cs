using UnityEngine;

public class ElectrifiedGroundElectricity : MonoBehaviour
{
    public ElectrifiedGroundBehavior hi;
    public Animator ani;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hi = GetComponentInParent<ElectrifiedGroundBehavior>();
        ani = GetComponent<Animator>();
    }
    public void toggle_electrified(int on)
    {
        if (on == 1)
        {
            hi.electrified = true;
        }
        else
        {
            hi.electrified = false;
        }
    }

    public void end_shocking()
    {
        ani.SetBool("start_shocking", false);
        hi.end_electrify();
    }
}
