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

        private IUSB_Controller USB;
        private Control HostControl;
        private TaskDialog taskDialog;

        public UpdateNodeDelegate UpdateNodes;

        public TaskContainer(Control MainControl, IUSB_Controller usb, TaskDialog RunningTaskDialog)
        {
            asyncWorker.RunWorkerCompleted += asyncWorkerComplete;
            asyncWorker.ProgressChanged += asyncWorkerProgressChange;

            asyncWorker.WorkerReportsProgress = true;
            asyncWorker.WorkerSupportsCancellation = true;

            USB = usb;
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

        public void startAsyncWorkerWithTask(string desc, DoWorkEventHandler asyncWork, CleanupDelegate asyncCleanup, object argument)
        {
            if (!asyncWorker.IsBusy)
            {
                if (USB.Check_Limit_Switches() && USB.IsOpen)
                {
                    ExtLog.AddLine("Starting Async Task");

                    USB.Inhibit_LimitSwitches_Warning = true;
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
                        USB.CancelMove();
                    }
                    HostControl.Enabled = true;
                    USB.Inhibit_LimitSwitches_Warning = false;
                }
                else ExtLog.AddLine("Can't init scripted sequence, limit switches are not properly set or USB interface is Closed");
            }
            else ExtLog.AddLine("Async Task Already Running");
        }

        private void asyncWorkerProgressChange(object sender, ProgressChangedEventArgs e)
        {
            var inhibit = e.UserState as bool? ?? false;
            if (!inhibit) ExtLog.AddLine("Progress: " + e.ProgressPercentage.ToString("D") + "%");
            taskDialog.update(e.ProgressPercentage);
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
                if (CleanupCallback != null) CleanupCallback(success);
            }
        }


        private bool Initiate_Drill_From_Top(int numTries, int TriesDelay)
        {
            var success = true;
            USB.Cycle_Drill = true;

            while (USB.TopSwitch && success)
            {
                USB.Transfer();
                success = USB.IsOpen && (numTries >= 0);
                numTries--;
                Thread.Sleep(TriesDelay);
            }
            return success;
        }
        private bool Wait_For_Drill_To_Top(int numTries, int TriesDelay)
        {
            var success = true;
            USB.Cycle_Drill = false;

            while (!USB.TopSwitch && success)
            {
                USB.Transfer();
                success = !USB.IsOpen || (numTries >= 0);
                numTries--;
                Thread.Sleep(TriesDelay);
            }
            return success;
        }

        private bool SeekXminSwitch(bool AxisDirection, int byX, int byY, int TriesDelay)
        {
            var success = true;
            var maxTries = GlobalProperties.numSeek;
            while ((USB.MinXswitch == AxisDirection) && USB.IsOpen && (maxTries > 0))
            {
                USB.MoveByStep(byX, byY);
                maxTries--;
                Thread.Sleep(TriesDelay);
                USB.Transfer();
                success = maxTries >= 0;
            }
            return success && (USB.MinXswitch != AxisDirection);
        }
        private bool SeekYminSwitch(bool AxisDirection, int byX, int byY, int TriesDelay)
        {
            var success = true;
            var maxTries = GlobalProperties.numSeek;
            while ((USB.MinYswitch == AxisDirection) && USB.IsOpen && (maxTries > 0))
            {
                USB.MoveByStep(byX, byY);
                maxTries--;
                Thread.Sleep(TriesDelay);
                USB.Transfer();
                success = maxTries >= 0;
            }
            return success && (USB.MinYswitch != AxisDirection);
        }

        public void asyncWorkerDoWork_DrillSelected(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var loc = doWorkEventArgs.Argument as PointF? ?? PointF.Empty;

            USB.MoveToPosition(loc.X, loc.Y);

            var success = USB.Check_Limit_Switches();
            asyncWorker.ReportProgress(50);

            //start drill from top
            if (!asyncWorker.CancellationPending)
            {
                if (success && USB.IsOpen && USB.Check_Limit_Switches())
                {
                    success = Initiate_Drill_From_Top(GlobalProperties.drillReleaseNumWait, GlobalProperties.drillReleaseWaitTime);
                }
                asyncWorker.ReportProgress(75);
            }
            else doWorkEventArgs.Cancel = true;

            //wait for drill to reach back top
            if (!asyncWorker.CancellationPending)
            {
                if (success && USB.IsOpen && USB.Check_Limit_Switches())
                {
                    success = Wait_For_Drill_To_Top(GlobalProperties.drillCycleNumWait, GlobalProperties.drillCycleWaitTime);
                }
                asyncWorker.ReportProgress(100);
            }
            else doWorkEventArgs.Cancel = true;

            doWorkEventArgs.Result = success;
        }

        public void asyncWorkerDoWork_FindAxisOrigin(object sender, DoWorkEventArgs doWorkEventArgs)
        {


            var success = USB.Check_Limit_Switches();
            asyncWorker.ReportProgress(30);

            if (!asyncWorker.CancellationPending)
            {
                success = SeekXminSwitch(false, GlobalProperties.fastSeekSteps, 0, GlobalProperties.fastSeekDelay); 
                asyncWorker.ReportProgress(45);
            }
            else doWorkEventArgs.Cancel = true;

            if (!asyncWorker.CancellationPending)
            {
                if (success) success = SeekXminSwitch(true, 1, 0, GlobalProperties.slowSeekDelay);
                asyncWorker.ReportProgress(60);
            }
            else doWorkEventArgs.Cancel = true;

            if (!asyncWorker.CancellationPending)
            {
                if (success) success = SeekYminSwitch(false, 0, GlobalProperties.fastSeekSteps, GlobalProperties.fastSeekDelay);
                asyncWorker.ReportProgress(75);
            }
            else doWorkEventArgs.Cancel = true;

            if (!asyncWorker.CancellationPending)
            {
                if (success) success = SeekYminSwitch(true, 0, 1, GlobalProperties.slowSeekDelay);
                asyncWorker.ReportProgress(90);
            }
            else doWorkEventArgs.Cancel = true;

            if (!asyncWorker.CancellationPending)
            {
                asyncWorker.ReportProgress(100);
            }
            else doWorkEventArgs.Cancel = true;

            doWorkEventArgs.Result = success;
        }




        public void asyncWorkerDoWork_DrillAll(object sender, DoWorkEventArgs e)
        {
            var nodes = e.Argument as List<DrillNode> ?? new List<DrillNode>();
            var success = nodes.Count > 0;

            if (success) for (var i = 0; i < nodes.Count; i++)
                {
                    if (!asyncWorker.CancellationPending)
                    {
                        if (success && USB.Check_Limit_Switches() && USB.TopSwitch && !USB.BottomSwitch)
                        {
                            if (nodes[i].status != DrillNode.DrillNodeStatus.Drilled)
                            {

                                UpdateNodes(i, DrillNode.DrillNodeStatus.Next);

                                ExtLog.AddLine("Moving to [" + (i + 1) + "/" + nodes.Count + "]: " + nodes[i].Location);
                                USB.MoveToPosition(nodes[i].location.X, nodes[i].location.Y);

                                ExtLog.AddLine("Drilling...");

                                //start drill from top
                                if (USB.IsOpen && USB.Check_Limit_Switches())
                                {
                                    success = Initiate_Drill_From_Top(GlobalProperties.drillReleaseNumWait, GlobalProperties.drillReleaseWaitTime);
                                }

                                //wait for drill to reach back top
                                if (success && USB.IsOpen && USB.Check_Limit_Switches())
                                {
                                    success = Wait_For_Drill_To_Top(GlobalProperties.drillCycleNumWait, GlobalProperties.drillCycleWaitTime);
                                }

                                UpdateNodes(i, DrillNode.DrillNodeStatus.Drilled);
                                asyncWorker.ReportProgress(100 * (i + 1) / nodes.Count, true);
                            }
                            else
                            {
                                ExtLog.AddLine("Node [" + (i + 1) + "/" + nodes.Count + "] already drilled");
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
