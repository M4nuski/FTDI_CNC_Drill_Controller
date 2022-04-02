namespace CNC_Drill_Controller1
{

    // delegate ThingDelegate (interface declaration)
    // ThingDelegate ThingCallback (method container)
    // void thingCallback(interface) (method to call async/Invoke)
    // void OnThing(interface) (method that request Invoke(ThingCallback))


    public delegate void ProgressDelegate(int Progress, bool Done);
    public delegate void CleanupDelegate(bool Success);
    public delegate void UpdateNodeDelegate(int nodeIndex, DrillNode.DrillNodeStatus newStatus);
    public delegate void AddLineDelegate(string text);
    public delegate bool MoveDelegate(float X, float Y);
    public delegate bool MoveByStepDelegate(int X, int Y);
    public delegate void VoidDelgetate();
    public delegate bool BoolVoidDelgetate();

}
