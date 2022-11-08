/*
 * Created by Ranorex
 * User: administrator
 * Date: 11/27/2018
 * Time: 1:14 AM
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
    [TestModule("BB4D94C0-5957-4A68-8980-CA2458C98361", ModuleType.UserCode, 1)]
    public class UserCodeModule1 : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public UserCodeModule1()
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
           
            //SWA_1_6_5_Release_Specific_Test_cases.CommonFlows.Installation.Instance.Addinstance();
            
        }
    }
}
