/*
 * Created by Ranorex
 * User: administrator
 * Date: 12/21/2018
 * Time: 8:03 AM
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
	/// Description of C184763_LogWaitWidgetonPerformingDrillDown.
	/// </summary>
	[TestModule("E0906550-F401-42C3-96BB-653EE898EBDB", ModuleType.UserCode, 1)]
	public class C184763_LogWaitWidgetonPerformingDrillDown : ITestModule
	{
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		public C184763_LogWaitWidgetonPerformingDrillDown()
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
				string path=name +"\\DDT\\Script\\Common.csv";
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
					Report.Log(ReportLevel.Info,"Submit button still exists and sending the details again to click submit button");
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
				Report.Screenshot();
				Delay.Seconds(10);
				TdTag serverInstance = "/dom[@domain='localhost:9291']//iframe[1]//table[#'instancesLeftDataTable']/tbody/?/?/td[@innertext='"+serverName+"']";
				
				serverInstance.PerformClick();
				Report.Log(ReportLevel.Info,"Click on Log Waits in DB Highlights");
				var repo = SWA_1_6_5_Release_Specific_Test_casesRepository.Instance;
				ATag logWait = "/dom[@domain='localhost:9291']//iframe[1]//table[#'entityOverviewDataTable']//a[@innertext='Log Wait']";
				
				logWait.PerformClick();
				Report.Screenshot();
				Delay.Seconds(3);
				commonRepo.IderaDashboard.BC2Un2.Content.Focus();
				CommonUtilities.SWAWebCommon.Scroll(commonRepo.IderaDashboard.BC2Un2.TopStatementsInfo,"down");
				Delay.Seconds(10);
				Report.Log(ReportLevel.Info,"Click on 1st item Top SQL Statements");
				IList<ATag> sqlQueries = commonRepo.IderaDashboard.BC2Un2.TopStatementsDataTable.FindDescendants<ATag>();
				Delay.Seconds(5);
				foreach(ATag query in sqlQueries){
					if(query.InnerText.Contains("DELETE") || query.InnerText.Contains("delete")){
						query.PerformClick();
						break;
					}
				}
				
				Report.Log(ReportLevel.Info,"Wait for Top Access Objects to be visible");
				commonRepo.IderaDashboard.BC2Un2.TopAccessObjectsInfo.WaitForAttributeEqual(5000,"Visible","True");
				Delay.Seconds(40);
				Report.Log(ReportLevel.Info,"Click on 1st item in the Top Access Objects");
				Report.Screenshot();
				IList<TrTag> topAccessDataTable = commonRepo.IderaDashboard.BC2Un2.TopAccessObjectsDataTable.FindChildren<TrTag>();
				string tableName = topAccessDataTable[0].FindChild<TdTag>().FindChild<ATag>().InnerText;
				Report.Log(ReportLevel.Info,"Table Name is:"+tableName);
				IList<TdTag> databaseNameTag = topAccessDataTable[0].FindChildren<TdTag>();
				string databaseName = databaseNameTag[2].InnerText;
				Report.Log(ReportLevel.Info,"Database Name is:"+databaseName);
				Report.Log(ReportLevel.Info,"Click on 1st Top Access Object");
				topAccessDataTable[0].FindChild<TdTag>().FindChild<ATag>().PerformClick();
				Validate.AttributeNotEqual(commonRepo.IderaDashboard.BC2Un2.OverlappingPopupInfo,"Visible","True");
				Delay.Seconds(50);
				Report.Log(ReportLevel.Info,"Verify the left component is displayed");
				IList<TrTag> leftComponent = commonRepo.IderaDashboard.BC2Un2.LeftComponent.FindChildren<TrTag>();
				foreach(TrTag tr in leftComponent){
					IList<TdTag> tdTags = tr.FindChildren<TdTag>();
					Report.Log(ReportLevel.Info,"Validate table Name");
					Validate.Equals(tableName,tdTags[0].InnerText);
					Report.Log(ReportLevel.Info,"Validate Wait time is not null");
					if(tdTags[1].FindChild<DivTag>().InnerText.IsEmpty()){
						Report.Log(ReportLevel.Failure,"Wait time is not populated");
					}else{
						Report.Log(ReportLevel.Info,tdTags[1].FindChild<DivTag>().InnerText);
						Report.Log(ReportLevel.Info,"Wait time data is populated");
					}
					Report.Log(ReportLevel.Info,"Validate Operation field");
					if(tdTags[2].InnerText.IsEmpty()){
						Report.Log(ReportLevel.Failure,"Operations Field is not populated");
					}else{
						Report.Log(ReportLevel.Info,tdTags[2].InnerText);
						Report.Log(ReportLevel.Info,"Operation field is populated");
					}
					Report.Log(ReportLevel.Info,"Validate Space Field");
					if(tdTags[3].InnerText.IsEmpty()){
						Report.Log(ReportLevel.Failure,"Space field is not populated");
					}else{
						Report.Log(ReportLevel.Info,tdTags[3].InnerText);
						Report.Log(ReportLevel.Info,"Space field is populated");
					}
					Report.Log(ReportLevel.Info,"Validate database Name");
					Validate.Equals(tdTags[4].InnerText,databaseName);
					break;
				}
				
			}finally{
				SWAWebCommon.closeBrowser();
			}
		}
	}
}
