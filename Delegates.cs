namespace CNC_Drill_Controller1
{

    // delegate ThingDelegate (interface declaration)
    // ThingDelegate ThingCallback (method container)
    // void thingCallback(interface) (method to call async/Invoke)
    // void OnThing(interface) (method that request Invoke(ThingCallback))


    delegate void ProgressDelegate(int Progress, bool Done);
    delegate void CleanupDelegate(bool Success);
    delegate void UpdateNodeDelegate(int nodeIndex, DrillNode.DrillNodeStatus newStatus);
    delegate void AddLineDelegate(string text);
    delegate void MoveDelegate(float X, float Y);
    delegate void VoidDelgetate();

}
