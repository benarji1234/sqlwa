/*
 * Created by Ranorex
 * User: administrator
 * Date: 12/20/2018
 * Time: 10:05 PM
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

namespace SWA_1_6_5_Release_Specific_Test_cases.Scripts
{
	/// <summary>
	/// Description of C184752_Internalwaitwidget_1DayTimeFrameDelay.
	/// </summary>
	[TestModule("6F0DE30B-E6F6-458C-9A35-A1133EC1C042", ModuleType.UserCode, 1)]
	public class C184752_Internalwaitwidget_1DayTimeFrameDelay : ITestModule
	{
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		public C184752_Internalwaitwidget_1DayTimeFrameDelay()
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
				//Validate.AttributeEqual(commonRepo.IderaDashboard.BC2Un2.OverlappingPopupInfo,"Visible","true");
				//Report.Screenshot();
				
				
				
				SWAWebCommon.waitForDataToPopulate(timeDuration);
				Report.Log(ReportLevel.Info,"Select the server instance");
				TdTag serverInstance = "/dom[@domain='localhost:9291']//iframe[1]//table[#'instancesLeftDataTable']/tbody/?/?/td[@innertext='"+serverName+"']";
				serverInstance.PerformClick();
				ATag internalWaitPath  = "/dom[@domain='localhost:9291']//iframe[1]//table[#'entityOverviewDataTable']//a[@innertext='Internal Wait']";
				Report.Log(ReportLevel.Info,"Click on Internal Waits in DB Highlights");
				
				internalWaitPath.PerformClick();
				Delay.Seconds(3);
				commonRepo.IderaDashboard.BC2Un2.TopStatementsLeftDataTable1Info.WaitForAttributeEqual(5000,"Visible","True");
				IList<TrTag> topStatements = commonRepo.IderaDashboard.BC2Un2.TopStatementsLeftDataTable1.FindChildren<TrTag>();
				foreach(TrTag statement in topStatements){
					IList<TdTag> statementColumns  = statement.FindChildren<TdTag>();
					if(statementColumns[0].InnerText.Contains("DELETE")|| statementColumns[0].InnerText.Contains("delete")){
						statementColumns[0].PerformClick();
						Delay.Seconds(10);
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
					Validate.AttributeEqual(commonRepo.IderaDashboard.BC2Un2.OverlappingPopupInfo,"Visible","True");
					Report.Screenshot();
					commonRepo.IderaDashboard.BC2Un2.OkButton.PerformClick();
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
				}
			}finally{
				SWAWebCommon.closeBrowser();
			}
		}
	}
}
