namespace CNC_Drill_Controller1
{

    // delegate ThingDelegate (interface declaration)
    // ThingDelegate ThingCallback (method container)
    // void thingCallback(interface) (method to call async/Invoke)
    // void OnThing(interface) (method that request Invoke(ThingCallback))


    public delegate void UpdateNodeDelegate(int nodeIndex, DrillNode.DrillNodeStatus newStatus);
    public delegate void Void_IntBoolDelegate(int i, bool b);
    public delegate void Void_BoolDelegate(bool b);
    public delegate void Void_StringDelegate(string s);
    public delegate bool Bool_FloatFloatDelegate(float X, float Y);
    public delegate bool Bool_IntIntDelegate(int X, int Y);
    public delegate void Void_VoidDelgetate();
    public delegate bool Bool_VoidDelgetate();

}
