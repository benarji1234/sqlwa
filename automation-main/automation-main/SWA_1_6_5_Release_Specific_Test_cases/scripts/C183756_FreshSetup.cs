/*
 * Created by Ranorex
 * User: Administrator
 * Date: 12/25/2018
 * Time: 10:12 PM
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
	/// Description of C183759_FreshSetup.
	/// </summary>
	[TestModule("564EF1CE-301F-412C-9D65-0CE735ADF0EC", ModuleType.UserCode, 1)]
	public class C183756_FreshSetup : ITestModule
	{
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		public C183756_FreshSetup()
		{
			// Do not delete - a parameterless constructor is required!
		}

		string _Url = "https://localhost:9291/login";
		[TestVariable("6c30583e-189f-41cf-95dd-2f431af7f62b")]
		public string Url
		{
			get { return _Url; }
			set { _Url = value; }
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
			string sqlstatementselected = "";
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
				commonRepo.IderaDashboard.BC2Un2.Content.Click();
				Report.Log(ReportLevel.Info,"scroll down to Top Machines widget");
				//SWAWebCommon.Scroll(commonRepo.IderaDashboard.BC2Un2.MachinesInfo,"down");
				Delay.Seconds(3);
				Report.Log(ReportLevel.Info,"Click on machine name from Top Machine Widget");
				String topMachineName = topMachineToCheck;
				ATag instanceName = "/dom[@domain='localhost:9291']//iframe[1]//table[#'machinesDataTable']//td[@title='"+topMachineName+"']/a[@innertext='"+topMachineName+"']";
				
				instanceName.PerformClick();
				Delay.Seconds(10);
				IList<TrTag> topPrgramsList = commonRepo.IderaDashboard.BC2Un2.TopProgramsDataTable.FindChildren<TrTag>();
				if(topPrgramsList.Count != 0){
					Report.Log(ReportLevel.Info,"Click 1st item on Top Programs Widget");
					Delay.Seconds(3);
					topPrgramsList[0].FindChild<TdTag>().FindChild<ATag>().PerformClick();
				}else{
					Report.Log(ReportLevel.Error,"No top programs list is available");
				}
				Delay.Seconds(10);
				commonRepo.IderaDashboard.BC2Un2.TopStatementsInfo.WaitForAttributeEqual(5000,"Visible","True");
				IList<TrTag> topStatements = commonRepo.IderaDashboard.BC2Un2.TopStatements.FindChildren<TrTag>();
				if(topStatements.Count != 0){
					Report.Log(ReportLevel.Info,"Click 1st Item on Top SQL Statements Widget");
					IList<ATag> sqlQueries = commonRepo.IderaDashboard.BC2Un2.TopStatementsDataTable.FindDescendants<ATag>();
					Delay.Seconds(5);
					bool topSQLStatements = false;
					foreach(ATag query in sqlQueries){
						if(query.InnerText.Contains("DELETE") || query.InnerText.Contains("delete")){
							sqlstatementselected = query.InnerText;
							query.PerformClick();
							topSQLStatements = true;
							break;
						}
					}
					if(topSQLStatements==false){
						Ranorex.Validate.Fail("Delete Statement is not available");
						//Report.Log(ReportLevel.Error,"Update Query is not available");
					}
					
				}else{
					Report.Log(ReportLevel.Error,"Top SQL Statemets are not available");
				}
				Delay.Seconds(10);
				commonRepo.IderaDashboard.BC2Un2.Content.Focus();
				//commonRepo.IderaDashboard.BC2Un2.SQLStatementFull.PerformClick();
				//SWAWebCommon.Scroll(commonRepo.IderaDashboard.BC2Un2.SQLStatementFullInfo,"down");
				string queryBuilder = "";
				IList<SpanTag> queries = commonRepo.IderaDashboard.BC2Un2.SQLStatementFull.FindChildren<SpanTag>();
				foreach(SpanTag query in queries){
					queryBuilder = queryBuilder + query.InnerText;
				}
				string sqlStatement = Regex.Replace(queryBuilder,@"\n","");
				string OriginalString = Regex.Replace(sqlstatementselected,@"\s+",string.Empty);
				string stringValueDisplayed = Regex.Replace(sqlStatement,@"\s+",string.Empty);
				//SpanTag sqlStatement = "/dom[@domain='localhost:9291']//iframe[1]//li[#'statementFullText']/div[3]/div//span";
				Report.Log(ReportLevel.Info,"Validate SQL Statement Full Text");
				Validate.Equals(OriginalString,stringValueDisplayed);
				//SWAWebCommon.Scroll(commonRepo.IderaDashboard.BC2Un2.UsersDataTableInfo,"down");
				IList<TrTag> topLogins = commonRepo.IderaDashboard.BC2Un2.TopLogins.FindChildren<TrTag>();
				if(topLogins.Count!=0){
					Report.Log(ReportLevel.Info,"Click 1st Item on Logins Widget");
					topLogins[0].FindChild<TdTag>().FindChild<ATag>().PerformClick();
				}else{
					Report.Log(ReportLevel.Error,"No Logins are available");
				}
				//SpanTag sqlStatementAfterLogin = "/dom[@domain='localhost:9291']//iframe[1]//li[#'statementFullText']/div[3]//span";
				string queryBuilder1 = "";
				IList<SpanTag> queries1 = commonRepo.IderaDashboard.BC2Un2.SQLStatementFull.FindChildren<SpanTag>();
				foreach(SpanTag query1 in queries1){
					queryBuilder1 = queryBuilder1 + query1.InnerText;
				}
				string sqlStatement1 = Regex.Replace(queryBuilder,@"\n","");
				string sqlStatementAfterLogin = Regex.Replace(sqlStatement,@"\s+",string.Empty);
				Report.Log(ReportLevel.Info,"Validate the SQL Statement full text is displayed correctly");
				Validate.Equals(OriginalString,sqlStatementAfterLogin);
			}finally{
				SWAWebCommon.closeBrowser();
			}
		}
	}
}
