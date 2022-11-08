/*
 * Created by Ranorex
 * User: administrator
 * Date: 1/28/2019
 * Time: 3:57 AM
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
	/// Description of C187257_PopupwhenTopAccessedObjectsisselected.
	/// </summary>
	[TestModule("82A20D69-5F3B-46AF-92C8-F3D7F0A1D006", ModuleType.UserCode, 1)]
	public class C187257_PopupwhenTopAccessedObjectsisselected : ITestModule
	{
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		public C187257_PopupwhenTopAccessedObjectsisselected()
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
					Report.Log(ReportLevel.Info,"Submit button stil exists and sending the details again to click submit button");
					commonRepo.IderaDashboard.Password.PressKeys(password);
					//commonRepo.IderaDashboard.Password.PressKeys("control*88");
					commonRepo.IderaDashboard.submitButton.Click();
				}
				SWAWebCommon.NavigatetoDashboard();
				commonRepo.IderaDashboard.BC2Un2.ContentInfo.WaitForAttributeEqual(5000,"Visible","True");
				IList<InputTag> timeSelection = commonRepo.IderaDashboard.BC2Un2.timeDelaySelection.FindDescendants<InputTag>();
				foreach(InputTag duration in timeSelection){
					if(duration.Value==timeDuration){
						Report.Log(ReportLevel.Info,"Click on "+duration.Value+" time interval");
						duration.PerformClick();
					}
				}
				SWAWebCommon.waitForDataToPopulate(timeDuration);
				Report.Log(ReportLevel.Info,"Select the server instance");
				TdTag serverInstance = "/dom[@domain='localhost:9291']//iframe[1]//table[#'instancesLeftDataTable']/tbody/?/?/td[@innertext='"+serverName+"']";
				
				serverInstance.PerformClick();
				Report.Log(ReportLevel.Info,"Click on IO Waits in DB Highlights");
				var repo = SWA_1_6_5_Release_Specific_Test_casesRepository.Instance;
				ATag OSWait  = "/dom[@domain='localhost:9291']//iframe[1]//table[#'entityOverviewDataTable']//a[@innertext='OS Wait']";
				
				OSWait.PerformClick();
				Delay.Seconds(3);
				commonRepo.IderaDashboard.BC2Un2.Content.Click();
				CommonUtilities.SWAWebCommon.Scroll(commonRepo.IderaDashboard.BC2Un2.TopStatementsInfo,"down");
				Delay.Seconds(10);
				Report.Log(ReportLevel.Info,"Click on 1st item Top SQL Statements");
				IList<ATag> sqlQueries = commonRepo.IderaDashboard.BC2Un2.TopStatementsDataTable.FindDescendants<ATag>();
				Delay.Seconds(5);
				foreach(ATag query in sqlQueries){
					if(query.InnerText.Contains("UPDATE")|| query.InnerText.Contains("update")){
						query.PerformClick();
						break;
					}
				}
				
				commonRepo.IderaDashboard.BC2Un2.TopAccessObjectsInfo.WaitForAttributeEqual(5000,"Visible","True");
				Delay.Seconds(2);
				TdTag topAccessObject = "/dom[@domain='localhost:9291']//iframe[1]//table[#'topAccessObjectsDataTable']//td[1]";
				if(topAccessObject.InnerText == "No Top Access Objects available"){
					Report.Log(ReportLevel.Failure,"No Top Access objects available for the selected query");
				}else{
					Report.Log(ReportLevel.Info,"Click on 1st item in the Top Access Objects");
					IList<TrTag> topAccessDataTable = commonRepo.IderaDashboard.BC2Un2.TopAccessObjectsDataTable.FindChildren<TrTag>();
					string tableName = topAccessDataTable[0].FindChild<TdTag>().FindChild<ATag>().InnerText;
					Report.Log(ReportLevel.Info,"Table Name is:"+tableName);
					IList<TdTag> databaseNameTag = topAccessDataTable[0].FindChildren<TdTag>();
					string databaseName = databaseNameTag[2].InnerText;
					Report.Log(ReportLevel.Info,"Database Name is:"+databaseName);
					Report.Log(ReportLevel.Info,"Click on 1st Top Access Object");
					topAccessDataTable[0].FindChild<TdTag>().FindChild<ATag>().PerformClick();
					Validate.AttributeEqual(commonRepo.IderaDashboard.BC2Un2.OverlappingPopupInfo,"Visible","false");
					Report.Log(ReportLevel.Info,"Pop-up appears for time duration 1-Day");
					
				}
					
				}finally{
					Report.Log(ReportLevel.Info,"Closing the browser");
					SWAWebCommon.closeBrowser();
				}
			}
		}
	}
