using UnityEngine;

public class ElectrifiedGroundCollective : MonoBehaviour
{

    public ElectrifiedGroundFullBehavior piece1;
    public ElectrifiedGroundFullBehavior piece2;
    public ElectrifiedGroundFullBehavior piece3;
    public ElectrifiedGroundFullBehavior piece4;
    public ElectrifiedGroundFullBehavior piece5;
    public ElectrifiedGroundFullBehavior piece6;
    public ElectrifiedGroundFullBehavior piece7;

    public void start()
    {
        piece1.start_electrify();
        piece2.start_electrify();
        piece3.start_electrify();
        piece4.start_electrify();
        piece5.start_electrify();
        piece6.start_electrify();
        piece7.start_electrify();
    }
    public void end()
    {
        piece1.end_electrify();
        piece2.end_electrify();
        piece3.end_electrify();
        piece4.end_electrify();
        piece5.end_electrify();
        piece6.end_electrify();
        piece7.end_electrify();
    }
}
