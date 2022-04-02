using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CNC_Drill_Controller1
{
    class TaskContainer
    {
        private BackgroundWorker asyncWorker = new BackgroundWorker();

        //for BackgroundWorker
        private DoWorkEventHandler lastTask;
        private CleanupDelegate CleanupCallback;

        private MainForm HostControl;
        private TaskDialog taskDialog;

        public UpdateNodeDelegate UpdateNodes;

        public class taskReport
        {
            public bool inhibit;
            public string message;
            public taskReport(bool inhibitProgress, string msg)
            {
                inhibit = inhibitProgress;
                message = msg;
            }
            public taskReport(bool inhibitProgress)
            {
                inhibit = inhibitProgress;
                message = "";
            }
            public taskReport()
            {
                inhibit = false;
                message = "";
            }
        };

        public TaskContainer(MainForm MainControl, TaskDialog RunningTaskDialog)
        {
            asyncWorker.RunWorkerCompleted += asyncWorkerComplete;
            asyncWorker.ProgressChanged += asyncWorkerProgressChange;

            asyncWorker.WorkerReportsProgress = true;
            asyncWorker.WorkerSupportsCancellation = true;

            HostControl = MainControl;
            taskDialog = RunningTaskDialog;
        }

        private void cancelTask()
        {
            if (asyncWorker.IsBusy)
            {
                ExtLog.AddLine("Cancelling Task...");
                asyncWorker.CancelAsync();
            }
        }

        #region Async delegates for UI thread
        private bool USB_Check_Limit_Switches()
        {
            if (HostControl.InvokeRequired)
            {
                return (bool)HostControl.Invoke((BoolVoidDelgetate)HostControl.USB.Check_Limit_Switches);
            }
            return HostControl.USB.Check_Limit_Switches();
        }

        private bool USB_IsOpen()
        {
            if (HostControl.InvokeRequired)
            {
                return (bool)HostControl.Invoke((BoolVoidDelgetate)USB_IsOpen);
            }
            return HostControl.USB.IsOpen;
        }

        private void USB_Inhibit_LimitSwitches_Warning(bool val)
        {
            if (HostControl.InvokeRequired)
            {
                HostControl.Invoke((CleanupDelegate)USB_Inhibit_LimitSwitches_Warning, new object[] { val });
                return;
            }
            HostControl.USB.Inhibit_LimitSwitches_Warning = val;
        }
        private void USB_CancelMove()
        {
            if (HostControl.InvokeRequired)
            {
                HostControl.Invoke((VoidDelgetate)USB_CancelMove);
                return;
            }
            HostControl.USB.CancelMove();
        }

        private bool USB_TopSwitch()
        {
            if (HostControl.InvokeRequired)
            {
                return (bool)HostControl.Invoke((BoolVoidDelgetate)USB_TopSwitch);
            }
            return HostControl.USB.TopSwitch;
        }
        private bool USB_BottomSwitch()
        {
            if (HostControl.InvokeRequired)
            {
                return (bool)HostControl.Invoke((BoolVoidDelgetate)USB_BottomSwitch);
            }
            return HostControl.USB.BottomSwitch;
        }

        private bool USB_MoveToPosition(float X, float Y)
        {
            if (HostControl.InvokeRequired)
            {
                return (bool)HostControl.Invoke((MoveDelegate)HostControl.USB.MoveToPosition, new object[] { X, Y });
            }
            return HostControl.USB.MoveToPosition(X, Y);
        }
        public bool USB_MinXswitch()
        {
            if (HostControl.InvokeRequired)
            {
                return (bool)HostControl.Invoke((BoolVoidDelgetate)USB_MinXswitch);
            }
            return HostControl.USB.MinXswitch;
        }
        public bool USB_MinYswitch()
        {
            if (HostControl.InvokeRequired)
            {
                return (bool)HostControl.Invoke((BoolVoidDelgetate)USB_MinYswitch);
            }
            return HostControl.USB.MinYswitch;
        }
        public bool USB_MaxXswitch()
        {
            if (HostControl.InvokeRequired)
            {
                return (bool)HostControl.Invoke((BoolVoidDelgetate)USB_MaxXswitch);
            }
            return HostControl.USB.MaxXswitch;
        }
        public bool USB_MaxYswitch()
        {
            if (HostControl.InvokeRequired)
            {
                return (bool)HostControl.Invoke((BoolVoidDelgetate)USB_MaxYswitch);
            }
            return HostControl.USB.MaxYswitch;
        }
        public void USB_Transfer()
        {
            if (HostControl.InvokeRequired)
            {
                HostControl.Invoke((VoidDelgetate)USB_Transfer);
                return;
            }
            HostControl.USB.Transfer();
        }
        public bool USB_MoveByStep(int byX, int byY)
        {
            if (HostControl.InvokeRequired)
            {
                return (bool)HostControl.Invoke((MoveByStepDelegate)HostControl.USB.MoveByStep, new object[] { byX, byY });
            }
            return HostControl.USB.MoveByStep(byX, byY);
        }
        private void USB_Cycle_Drill(bool val)
        {
            if (HostControl.InvokeRequired)
            {
                HostControl.Invoke((CleanupDelegate)USB_Cycle_Drill, new object[] { val });
                return;
            }
            HostControl.USB.Cycle_Drill = val;
        }
        #endregion

        public void startAsyncWorkerWithTask(string desc, DoWorkEventHandler asyncWork, CleanupDelegate asyncCleanup, object argument)
        {
            if (!asyncWorker.IsBusy)
            {
                if (USB_Check_Limit_Switches() && USB_IsOpen())
                {
                    ExtLog.AddLine("Starting Async Task");

                    USB_Inhibit_LimitSwitches_Warning(true);
                    HostControl.Enabled = false;

                    try
                    {
                        ExtLog.AddLine(desc);
                        asyncWorker.DoWork += asyncWork;
                        lastTask = asyncWork;
                        CleanupCallback = asyncCleanup;

                        asyncWorker.RunWorkerAsync(argument);
                    }
                    catch (Exception ex)
                    {
                        ExtLog.AddLine("Async Task failed: " + ex.Message);
                    }

                    ExtLog.AddLine("Async Started");

                    if (taskDialog.ShowDialog(HostControl) == DialogResult.Abort)
                    {
                        cancelTask();
                        USB_CancelMove();
                    }
                    HostControl.Enabled = true;
                    USB_Inhibit_LimitSwitches_Warning(false);
                }
                else ExtLog.AddLine("Can't init scripted sequence, limit switches are not properly set or USB interface is Closed");
            }
            else ExtLog.AddLine("Async Task Already Running");
        }

        private void asyncWorkerProgressChange(object sender, ProgressChangedEventArgs e)
        {
 
            var rep = (e.UserState != null) ? (taskReport)e.UserState : new taskReport();
            
            //var inhibit = e.UserState as taskReport? ?? false;
            if (!rep.inhibit) ExtLog.AddLine("Progress: " + e.ProgressPercentage.ToString("D") + "%");
            taskDialog.update(e.ProgressPercentage, rep.message);
        }

        private void asyncWorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            asyncWorker.DoWork -= lastTask;
            taskDialog.done();
            if (e.Error != null)
            {
                ExtLog.AddLine("Error running Task: " + e.Error.Message);
            }
            else if (e.Cancelled)
            {
                ExtLog.AddLine("Task Cancelled");
            }
            else
            {
                var success = e.Result as bool? ?? false;
                CleanupCallback?.Invoke(success);
            }
        }


        private bool Initiate_Drill_From_Top(int numTries, int TriesDelay)
        {
            var success = true;
            USB_Cycle_Drill(true);

            while (USB_TopSwitch() && success)
            {
                USB_Transfer();
                success = USB_IsOpen() && (numTries >= 0);
                numTries--;
                Thread.Sleep(TriesDelay);
            }
            return success;
        }
        private bool Wait_For_Drill_To_Top(int numTries, int TriesDelay)
        {
            var success = true;
            USB_Cycle_Drill(false);

            while (!USB_TopSwitch() && success)
            {
                USB_Transfer();
                success = !USB_IsOpen() || (numTries >= 0);
                numTries--;
                Thread.Sleep(TriesDelay);
            }
            return success;
        }

        private bool SeekXminSwitch(bool ExpectedInitialSwitchState, int byX, int byY, int TriesDelay)
        {
            var success = true;
            var numTryLeft = GlobalProperties.numSeekMin;
            while ((USB_MinXswitch() == ExpectedInitialSwitchState) && USB_IsOpen() && (numTryLeft > 0))
            {
                USB_MoveByStep(byX, byY);
                numTryLeft--;
                Thread.Sleep(TriesDelay);
                USB_Transfer();
                success = numTryLeft >= 0;
            }
            return success && (USB_MinXswitch() != ExpectedInitialSwitchState);
        }
        private bool SeekYminSwitch(bool ExpectedInitialSwitchState, int byX, int byY, int TriesDelay)
        {
            var success = true;
            var numTryLeft = GlobalProperties.numSeekMin;
            while ((USB_MinYswitch() == ExpectedInitialSwitchState) && USB_IsOpen() && (numTryLeft > 0))
            {
                USB_MoveByStep(byX, byY);
                numTryLeft--;
                Thread.Sleep(TriesDelay);
                USB_Transfer();
                success = numTryLeft >= 0;
            }
            return success && (USB_MinYswitch() != ExpectedInitialSwitchState);
        }
        private bool SeekXmaxSwitch(bool ExpectedInitialSwitchState, int byX, int byY, int TriesDelay)
        {
            var success = true;
            var numTryLeft = GlobalProperties.numSeekMax;
            while ((USB_MaxXswitch() == ExpectedInitialSwitchState) && USB_IsOpen() && (numTryLeft > 0))
            {
                USB_MoveByStep(byX, byY);
                numTryLeft--;
                Thread.Sleep(TriesDelay);
                USB_Transfer();
                success = numTryLeft >= 0;
            }
            return success && (USB_MaxXswitch() != ExpectedInitialSwitchState);
        }
        private bool SeekYmaxSwitch(bool ExpectedInitialSwitchState, int byX, int byY, int TriesDelay)
        {
            var success = true;
            var numTryLeft = GlobalProperties.numSeekMax;
            while ((USB_MaxYswitch() == ExpectedInitialSwitchState) && USB_IsOpen() && (numTryLeft > 0))
            {
                USB_MoveByStep(byX, byY);
                numTryLeft--;
                Thread.Sleep(TriesDelay);
                USB_Transfer();
                success = numTryLeft >= 0;
            }
            return success && (USB_MaxYswitch() != ExpectedInitialSwitchState);
        }

        public void asyncWorkerDoWork_FindAxisOrigin(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var success = USB_Check_Limit_Switches();
            asyncWorker.ReportProgress(30, new taskReport(false, "FindAxisOrigin.Check_Limit_Switches"));

            var axises = doWorkEventArgs.Argument as Tuple<bool, bool> ?? new Tuple<bool, bool>(false, false);

            if (axises.Item1 && success)
            {
                if (!asyncWorker.CancellationPending)
                {
                    success = SeekXminSwitch(false, GlobalProperties.fastSeekSteps, 0, GlobalProperties.fastSeekDelay);
                    asyncWorker.ReportProgress(45, new taskReport(false, "FindAxisOrigin.SeekXminSwitch fast"));
                }
                else doWorkEventArgs.Cancel = true;

                if (!asyncWorker.CancellationPending && success)
                {
                    if (success) success = SeekXminSwitch(true, 1, 0, GlobalProperties.slowSeekDelay);
                    asyncWorker.ReportProgress(60, new taskReport(false, "FindAxisOrigin.SeekXminSwitch"));
                }
                else doWorkEventArgs.Cancel = true;
            }

            if (axises.Item2 && success)
            {
                if (!asyncWorker.CancellationPending)
                {
                    if (success) success = SeekYminSwitch(false, 0, GlobalProperties.fastSeekSteps, GlobalProperties.fastSeekDelay);
                    asyncWorker.ReportProgress(75, new taskReport(false, "FindAxisOrigin.SeekYminSwitch fast"));
                }
                else doWorkEventArgs.Cancel = true;

                if (!asyncWorker.CancellationPending && success)
                {
                    if (success) success = SeekYminSwitch(true, 0, 1, GlobalProperties.slowSeekDelay);
                    asyncWorker.ReportProgress(90, new taskReport(false, "FindAxisOrigin.SeekYminSwitch"));
                }
                else doWorkEventArgs.Cancel = true;
            }

            if (!asyncWorker.CancellationPending)
            {
                asyncWorker.ReportProgress(100);
            }
            else doWorkEventArgs.Cancel = true;

            doWorkEventArgs.Result = success;
        }

        public void asyncWorkerDoWork_FindAxisLengths(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var success = USB_Check_Limit_Switches();
            asyncWorker.ReportProgress(30, new taskReport(false, "FindAxisLengths.Check_Limit_Switches"));

            var axises = doWorkEventArgs.Argument as Tuple<bool, bool> ?? new Tuple<bool, bool>(false, false);

            if (axises.Item1)
            {
                if (!asyncWorker.CancellationPending)
                {
                    success = SeekXmaxSwitch(false, -GlobalProperties.fastSeekSteps, 0, GlobalProperties.fastSeekDelay);
                    asyncWorker.ReportProgress(45, new taskReport(false, "FindAxisLengths.SeekXmaxSwitch fast"));
                }
                else doWorkEventArgs.Cancel = true;

                if (!asyncWorker.CancellationPending)
                {
                    if (success) success = SeekXmaxSwitch(true, -1, 0, GlobalProperties.slowSeekDelay);
                    asyncWorker.ReportProgress(60, new taskReport(false, "FindAxisLengths.SeekXmaxSwitch"));
                }
                else doWorkEventArgs.Cancel = true;
            }

            if (axises.Item2)
            {
                if (!asyncWorker.CancellationPending)
                {
                    if (success) success = SeekYmaxSwitch(false, 0, -GlobalProperties.fastSeekSteps, GlobalProperties.fastSeekDelay);
                    asyncWorker.ReportProgress(75, new taskReport(false, "FindAxisLengths.SeekYmaxSwitch fast"));
                }
                else doWorkEventArgs.Cancel = true;

                if (!asyncWorker.CancellationPending)
                {
                    if (success) success = SeekYmaxSwitch(true, 0, -1, GlobalProperties.slowSeekDelay);
                    asyncWorker.ReportProgress(90, new taskReport(false, "FindAxisLengths.SeekYmaxSwitch"));
                }
                else doWorkEventArgs.Cancel = true;
            }

            if (!asyncWorker.CancellationPending)
            {
                asyncWorker.ReportProgress(100);
            }
            else doWorkEventArgs.Cancel = true;

            doWorkEventArgs.Result = success;
        }


        public void asyncWorkerDoWork_DrillList(object sender, DoWorkEventArgs e)
        {
            var nodes = e.Argument as List<DrillNode> ?? new List<DrillNode>();
            var success = nodes.Count > 0;

            if (success) for (var i = 0; i < nodes.Count; i++)
                {
                    if (!asyncWorker.CancellationPending)
                    {
                        if (success && USB_Check_Limit_Switches() && USB_TopSwitch() && !USB_BottomSwitch())
                        {
                            if (nodes[i].status != DrillNode.DrillNodeStatus.Drilled)
                            {

                                UpdateNodes(nodes[i]._originalIndex, DrillNode.DrillNodeStatus.Next);

                                if ((nodes[i].location.X <= GlobalProperties.X_Length) && (nodes[i].location.Y <= GlobalProperties.Y_Length))
                                {
                                    ExtLog.AddLine($"Moving to [{(i + 1)}/{nodes.Count}]: {nodes[i]}");
                                    asyncWorker.ReportProgress(100 * (i + 1) / nodes.Count, new taskReport(true, $"DrillList.Moving to [{(i + 1)}/{nodes.Count}]: {nodes[i]}"));
                                    USB_MoveToPosition(nodes[i].location.X, nodes[i].location.Y);

                                    ExtLog.AddLine("Drilling...");

                                    //start drill from top
                                    if (USB_IsOpen() && USB_Check_Limit_Switches())
                                    {
                                        asyncWorker.ReportProgress(100 * (i + 1) / nodes.Count, new taskReport(true, "DrillList.Initiate_Drill_From_Top"));
                                        success = Initiate_Drill_From_Top(GlobalProperties.drillReleaseNumWait, GlobalProperties.drillReleaseWaitTime);
                                    }

                                    //wait for drill to reach back top
                                    if (success && USB_IsOpen() && USB_Check_Limit_Switches())
                                    {
                                        asyncWorker.ReportProgress(100 * (i + 1) / nodes.Count, new taskReport(true, "DrillList.Wait_For_Drill_To_Top"));
                                        success = Wait_For_Drill_To_Top(GlobalProperties.drillCycleNumWait, GlobalProperties.drillCycleWaitTime);
                                    }

                                    UpdateNodes(nodes[i]._originalIndex, DrillNode.DrillNodeStatus.Drilled);
                                } else
                                {
                                    ExtLog.AddLine($"Skipping Node [{(i + 1)}/{nodes.Count}]: {nodes[i]} -> Out Of Range");
                                }
                                asyncWorker.ReportProgress(100 * (i + 1) / nodes.Count, new taskReport(true, "DrillList.SeekYmaxSwitch"));
                            }
                            else
                            {
                                ExtLog.AddLine($"Skipping Node [{(i + 1)}/{nodes.Count}] -> Already Drilled");
                            }
                        }
                        else
                        {
                            success = false;
                        }
                    }
                    else e.Cancel = true;
                }

            asyncWorker.ReportProgress(100);
            e.Result = success;
        }


    }
}
