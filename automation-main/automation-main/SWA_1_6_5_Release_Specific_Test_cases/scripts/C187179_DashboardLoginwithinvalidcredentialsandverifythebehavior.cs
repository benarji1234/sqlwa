/*
 * Created by Ranorex
 * User: administrator
 * Date: 1/28/2019
 * Time: 6:09 AM
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
using SWA_1_6_5_Release_Specific_Test_cases.CommonUtilities;

namespace SWA_1_6_5_Release_Specific_Test_cases.scripts
{
	/// <summary>
	/// Description of C187179_DashboardLoginwithinvalidcredentialsandverifythebehavior.
	/// </summary>
	[TestModule("E37DC2F5-DDD6-45CA-AA65-BC2182BDA1B9", ModuleType.UserCode, 1)]
	public class C187179_DashboardLoginwithinvalidcredentialsandverifythebehavior : ITestModule
	{
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		public C187179_DashboardLoginwithinvalidcredentialsandverifythebehavior()
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
			try{
				Host.Local.OpenBrowser("https://localhost:9291/login", "Chrome", "",false, false, false,false,false);
				SWAWebCommon.certificatePage();
				Delay.Seconds(10);
				invalidCredentials(@"simpsons\administrator","control*89");
				Delay.Seconds(5);
				invalidCredentials("simpsons","control*88");
				Delay.Seconds(5);
				invalidCredentials("simpsons","Control*89");
				                            
				}finally{
				SWAWebCommon.closeBrowser();
			}
			
		}
		
		public static void invalidCredentials(String userName,String password)
		{
			var commonRepo = SWA_1_6_5_Release_Specific_Test_cases.Repositories.CommonRepo.Instance;
			Report.Log(ReportLevel.Info,"Wait for Loginscreen to be visible");
			commonRepo.IderaDashboard.LoginScreenInfo.WaitForExists(5000);
			commonRepo.IderaDashboard.UserName.PressKeys("{END}{SHIFT DOWN}{HOME}{SHIFT UP}{DELETE}");
			Report.Log(ReportLevel.Info,"Send UserName");
			commonRepo.IderaDashboard.UserName.PressKeys(userName);
			Delay.Seconds(5);
			Report.Log(ReportLevel.Info,"Send Password");
			commonRepo.IderaDashboard.Password.PressKeys(password);
			Delay.Seconds(5);
			Report.Log(ReportLevel.Info,"Validate Submit button is enabled");
			Validate.AttributeEqual(commonRepo.IderaDashboard.submitButtonInfo,"enabled","True");
			Report.Log(ReportLevel.Info,"Click on Submit button");
			commonRepo.IderaDashboard.submitButton.MoveTo();
			commonRepo.IderaDashboard.submitButton.Click();
			
			
			Delay.Seconds(5);
			
			
			Validate.Exists(commonRepo.IderaDashboard.M82U80);
			Report.Screenshot();
		}
	}
}
