/*
 * Created by Ranorex
 * User: administrator
 * Date: 01/02/19
 * Time: 1:49 AM
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;
using SWA_1_6_5_Release_Specific_Test_cases.CommonUtilities;
using SWA_1_6_5_Release_Specific_Test_cases.CommonFlows;

namespace SWA_1_6_5_Release_Specific_Test_cases.scripts
{
	/// <summary>
	/// Description of SQLWA864_Verifyifallthemachinesarebeingdisplayedunder_TopMachineswidget_whentimeintervalischanged.
	/// </summary>
	[TestModule("AFA34AA0-9988-4853-A8E4-0F4FE0BD879B", ModuleType.UserCode, 1)]
	public class SQLWA864_Verifyifallthemachinesarebeingdisplayedunder_TopMachineswidget_whentimeintervalischanged : ITestModule
	{
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		public SQLWA864_Verifyifallthemachinesarebeingdisplayedunder_TopMachineswidget_whentimeintervalischanged()
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
				string path1 = Ranorex.Core.Testing.TestSuite.WorkingDirectory;
				string parentpath = System.IO.Directory.GetParent(path1).ToString();
				string name = System.IO.Directory.GetParent(parentpath).ToString();
				string path=name +"\\DDT\\Script\\C187257.csv";
				string userName = SWAWebCommon.getVariableFromCSV(path,"UserName");
				string password = SWAWebCommon.getVariableFromCSV(path,"Password");
				string timeDuration = SWAWebCommon.getVariableFromCSV(path,"TimeDuration");
				string serverName = SWAWebCommon.getVariableFromCSV(path,"serverInstance");
				string topMachineToCheck = SWAWebCommon.getVariableFromCSV(path,"topMachineToCheck");
				string licenseKey =  SWAWebCommon.getVariableFromCSV(path,"licenseKey");
				
				Host.Local.OpenBrowser("https://localhost:9291/login", "Chrome", "",false, false, false,false,false);
				SWAWebCommon.certificatePage();
				var commonRepo = SWA_1_6_5_Release_Specific_Test_cases.Repositories.CommonRepo.Instance;
				Report.Log(ReportLevel.Info,"Wait for Loginscreen to be visible");
				commonRepo.IderaDashboard.LoginScreenInfo.WaitForExists(5000);
				commonRepo.IderaDashboard.UserName.PressKeys("{END}{SHIFT DOWN}{HOME}{SHIFT UP}{DELETE}");
				Report.Log(ReportLevel.Info,"Send UserName");
				commonRepo.IderaDashboard.UserName.PressKeys(userName);
				//commonRepo.IderaDashboard.UserName.PressKeys(@"simpsons\administrator");
				Report.Log(ReportLevel.Info,"Send Password");
				commonRepo.IderaDashboard.Password.PressKeys(password);
				//commonRepo.IderaDashboard.Password.PressKeys("control*88");
				Delay.Seconds(5);
				Report.Log(ReportLevel.Info,"Validate Submit button is enabled");
				Validate.AttributeEqual(commonRepo.IderaDashboard.submitButtonInfo,"enabled","True");
				Report.Log(ReportLevel.Info,"Click on Submit button");
				commonRepo.IderaDashboard.submitButton.MoveTo();
				commonRepo.IderaDashboard.submitButton.Click();
				
				
				Delay.Seconds(10);
				if(commonRepo.IderaDashboard.submitButtonInfo.Exists()){
					Report.Log(ReportLevel.Info,"Submit button still exists and sending the details again to click submit button");
					commonRepo.IderaDashboard.Password.PressKeys(password);
					//commonRepo.IderaDashboard.Password.PressKeys("control*88");
					commonRepo.IderaDashboard.submitButton.Click();
				}
				SWAWebCommon.NavigatetoDashboard();
				commonRepo.IderaDashboard.BC2Un2.ContentInfo.WaitForAttributeEqual(5000,"Visible","True");
				
				string machineName = serverName;
				//int delay=10;
				
				//adding unlimited license
				var administrationButton = commonRepo.IderaDashboard.BC2Un2.ADMINISTRATION;
//				commonRepo.IderaDashboard.BC2Un2.ManageLicense.PerformClick();
//				commonRepo.IderaDashboard.BC2Un2.LicenseKey.PressKeys(licenseKey);
//				commonRepo.IderaDashboard.BC2Un2.ApplyButton.PerformClick();
//				commonRepo.IderaDashboard.BC2Un2.Button.PerformClick();
//				Delay.Seconds(5);
//
//
//				Installation newInstance = new Installation();
//				newInstance.Addinstance("sa","control*88",machineName);
//				Keyboard.Press(Keys.F5);
//				Delay.Seconds(60);
				//SWAWebCommon.waitForDataToPopulate(timeDuration);
				
				var homeButton = commonRepo.IderaDashboard.BC2Un2.SpanTagHOME;
				homeButton.PerformClick();


				Report.Log(ReportLevel.Info,"Select the server instance");
				TdTag serverInstance = "/dom[@domain='localhost:9291']//iframe[1]//table[#'instancesLeftDataTable']/tbody/?/?/td[@innertext='"+machineName+"']";
				serverInstance.PerformClick();


				IList<InputTag> timeSelection = commonRepo.IderaDashboard.BC2Un2.timeDelaySelection.FindDescendants<InputTag>();
				foreach(InputTag duration in timeSelection){
					
					Report.Log(ReportLevel.Info,"Select time interval");
					Delay.Seconds(5);
					duration.PerformClick();
					verifyTopMachine(machineName,duration,topMachineToCheck);
				}
				
				//Remove the instance and verify again.
				
				administrationButton.PerformClick();
				Delay.Seconds(2);
				
				commonRepo.IderaDashboard.BC2Un2.ManageSQLServerInstances.PerformClick();
				//	commonRepo.IderaDashboard.BC2Un2.ManageInstanceInfo.WaitForAttributeEqual(5000,"Visible","True");
				
				IList<TrTag> manageInstance = commonRepo.IderaDashboard.BC2Un2.ManageInstance.FindChildren<TrTag>();
				bool serverFound = false;
				foreach(TrTag instanceName in manageInstance)
				{
					
					IList<TdTag> tdName = instanceName.FindChildren<TdTag>();
					string tdServerName = tdName[1].InnerText;
					if(String.Equals(tdServerName,machineName))
					{
						InputTag checkboxPath = "/dom[@domain='localhost:9291']//iframe[1]//div[#'content']/div[2]/div[4]/div[3]/div/table//td[@innertext='"+machineName+"']/preceding-sibling::td/input";
						checkboxPath.PerformClick();
						ATag gearIcon = "/dom[@domain='localhost:9291']//iframe[1]//div[#'content']/div[2]/div[4]/div[3]/div/table//td[@innertext='"+machineName+"']/following-sibling::td/a";
						gearIcon.PerformClick();
						Delay.Seconds(2);
						
						LiTag removeInstance = "/dom[@domain='localhost:9291']//iframe[1]//div[#'content']/div[2]/div[4]/div[3]/div/table//td[@innertext='"+machineName+"']/following-sibling::td/ul/li[@innertext='Remove Instance']";
						removeInstance.PerformClick();
						
						var saveAfterRemove = commonRepo.IderaDashboard.BC2Un2.RemoveInstancebtn;
						saveAfterRemove.PerformClick();
						Delay.Seconds(5);
						
						serverFound = true;
						break;
						
					}
					
				}
				if(serverFound == false)
				{
					Report.Error("server not found");
				}
				
				
				var closeButton = commonRepo.IderaDashboard.BC2Un2.Button;
				closeButton.PerformClick();
				Delay.Seconds(5);
				
				homeButton.PerformClick();
				
				Keyboard.Press(Keys.F5);
				Delay.Seconds(60);
				
				
				
				//Verify if the instance is present after removing it.
				Report.Log(ReportLevel.Info,"Verify the Server Instance is removed");
				if(!(commonRepo.IderaDashboard.BC2Un2.InstancesLeftDataTableInfo.Exists())){
					Report.Log(ReportLevel.Info,"Server Instance removed successfully");
					Report.Screenshot();
				}
				
				else{
					Report.Error("Server instance not removed");
				}
				
				
			}finally{
				SWAWebCommon.closeBrowser();
			}
			
		}
		public void verifyTopMachine(String machineName,InputTag timeduration,String checkTopMachine)
		{
			var commonRepo = SWA_1_6_5_Release_Specific_Test_cases.Repositories.CommonRepo.Instance;
			
			commonRepo.IderaDashboard.BC2Un2.Content.Focus();
			//CommonUtilities.SWAWebCommon.Scroll(commonRepo.IderaDashboard.BC2Un2.topMachinesInfo,"down");
			
			Delay.Seconds(2);
			//Validate.AttributeEqual(commonRepo.IderaDashboard.BC2Un2.topMachinesInfo,"Visible","True");
			Report.Log(ReportLevel.Info,"Verify Server Instance is available in Top Machine Widget");
			IList<TrTag> machines = commonRepo.IderaDashboard.BC2Un2.topMachines.FindChildren<TrTag>();
			List<String> topMachinesList= new List<String>();
			foreach(TrTag tr in machines){
				string topMachine = tr.FindChild<TdTag>().FindChild<ATag>().InnerText;
				topMachinesList.Add(topMachine);
			}
			
			if(topMachinesList.Contains(checkTopMachine))
			{
				Report.Log(ReportLevel.Info,"The server instance is added to top machine "+timeduration+" time interval" );
				
			}
			else
			{
				Report.Log(ReportLevel.Failure,"The server instance is not added to the top machine "+timeduration+" time interval");
			}
			topMachinesList.Clear();
			
			
		}
	}

}