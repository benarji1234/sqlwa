/*
 * Created by Ranorex
 * User: administrator
 * Date: 11/21/2018
 * Time: 4:33 AM
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace SWA_1_6_5_Release_Specific_Test_cases.CommonFlows
{
    /// <summary>
    /// Description of UserCodeModule1.
    /// </summary>
    [TestModule("2FAFBAF4-6F7B-4553-B543-D86C0CD9C0A9", ModuleType.UserCode, 1)]
    public class InstallationCode : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public InstallationCode()
        {
            // Do not delete - a parameterless constructor is required!
        }

        /// <summary>
        /// Performs the playback of actions in this module.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            string AppPath = "C:\\last_build\\IderaSQLwaInstallationKit.1.6.5.x64.exe";
            
            Report.Log(ReportLevel.Info, "Application", "Run application with file name from variable $AppPath with arguments '' in normal mode.", new RecordItemIndex(0));
            Host.Local.RunApplication(AppPath, "", "C:\\last_build", false);
            Delay.Milliseconds(0);
        }
    }
}
