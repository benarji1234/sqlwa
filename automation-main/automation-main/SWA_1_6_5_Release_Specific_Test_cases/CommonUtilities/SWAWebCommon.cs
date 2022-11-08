/*
 * Created by Ranorex
 * User: administrator
 * Date: 11/21/2018
 * Time: 6:01 AM
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
using Ranorex.Core.Repository;

namespace SWA_1_6_5_Release_Specific_Test_cases.CommonUtilities
{
	/// <summary>
	/// Ranorex user code collection. A collection is used to publish user code methods to the user code library.
	/// </summary>
	[UserCodeCollection]
	public class SWAWebCommon
		
	{
		
		// You can use the "Insert New User Code Method" functionality from the context menu,
		// to add a new method with the attribute [UserCodeMethod].
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void Login(string Url,string username,string password)
		{
			var repo=SWA_1_6_5_Release_Specific_Test_casesRepository.Instance;
			Host.Current.OpenBrowser(Url, "chrome", "", false, false, false, false, false);
			Delay.Seconds(30);
			SWAWebCommon.certificatePage();
			Delay.Seconds(5);
			var ideraDashboard = repo.IderaDashboard;
			
			ideraDashboard.JHwPv.Focus();
			Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
			
			ideraDashboard.JHwPv.PressKeys(username);
			
			Delay.Seconds(5);
			ideraDashboard.JHwPy.Focus();
			ideraDashboard.JHwPy.PressKeys(password);
			//IReportLogger(password);
			Delay.Seconds(10);

			//var qK0Qg = repo.IderaDashboard.QK0Qg;
			//ideraDashboard.JR7Q00.MoveTo();
			
			ideraDashboard.JR7Q00.Focus();
			Delay.Seconds(2);
			ideraDashboard.JR7Q00.PerformClick();
			Delay.Seconds(10);
			
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void Logout()
		{
			//IderaDashboard.C8FQ30B;
			
			var overview=SWA_1_6_5_Release_Specific_Test_casesRepository.Instance.IderaDashboard;
			//overview repo=SWA_1_6_5_Release_Specific_Test_casesRepository.Instance;
			
			overview.C8FQ30B.MoveTo();
			overview.C8FQ30B.PerformClick();
			
			Delay.Seconds(2);
			overview.C8FQ70A.PerformClick();
			
			Delay.Seconds(10);
			
			
		}
		
		
		/// <summary>
		/// This is a placeholder text. Please describe the purpose of the
		/// user code method here. The method is published to the user code library
		/// within a user code collection.
		/// </summary>
		[UserCodeMethod]
		public static void NavigatetoDashboard()
		{
			var repo = SWA_1_6_5_Release_Specific_Test_casesRepository.Instance;
			//var pQGQi = repo.Https10220201289291Gui460Rende.PQGQi;
			
			//pQGQi.PerformClick();

			
			var tMxPe = repo.IderaDashboard.TMxPe;
			tMxPe.PerformClick();
			Delay.Seconds(10);

			var tMxPo = repo.IderaDashboard.TMxPo;
			tMxPo.PerformClick();
			Delay.Seconds(5);

			
			
		}
		
		public static void certificatePage(){
			Delay.Seconds(10);
			var repo = SWA_1_6_5_Release_Specific_Test_cases.Repositories.CommonRepo.Instance;
			//repo.certificatePage.SelfInfo.Exists()
			if(repo.PrivacyErrorGoogleChrome.TitleBarInfo.Exists()){
				Keyboard.Press("{Tab}");
				Keyboard.Press("{Tab}");
				Keyboard.Press("{Tab}");
				Keyboard.Press("{Tab}");
				Keyboard.Press("{Tab}");
				Keyboard.Press("{Tab}");
				Keyboard.Press("{Enter}");
				Keyboard.Press("{Tab}");
				Keyboard.Press("{Enter}");
				Delay.Seconds(20);
			}
		}
		
		[UserCodeMethod]
		public static void closeBrowser(){
			IList<Ranorex.WebDocument> webList = Host.Local.Find<Ranorex.WebDocument>("/dom[@domain='localhost:9291']");
				foreach (Ranorex.WebDocument webdoc in webList)
				{  
    				webdoc.Close();
				}
		}
		
		//Method to Wait for data to be populated
		public static void waitForDataToPopulate(string timeInterval){
			var commonRepo = SWA_1_6_5_Release_Specific_Test_cases.Repositories.CommonRepo.Instance;
			bool widgetLoaded = false;
			int start = 0;
			int endTrigger = 10;
			while(start < endTrigger){
				start++;
				Delay.Seconds(50);
				if(commonRepo.IderaDashboard.BC2Un2.EntityOverviewDataTable.FindChildren<TrTag>().Count > 1){
					Report.Log(ReportLevel.Info,"Total DB highlights widget is visible");
					widgetLoaded = true;
					break;
				}
				Keyboard.Press(Keys.F5);
				Delay.Seconds(10);
				IList<InputTag> timeSelection = commonRepo.IderaDashboard.BC2Un2.timeDelaySelection.FindDescendants<InputTag>();
				foreach(InputTag duration in timeSelection){
					if(duration.Value==timeInterval){
						duration.PerformClick();
					}
				}
			}
			if(widgetLoaded.Equals(false)){
				Report.Log(ReportLevel.Error,"Total DB highlights widget is not loaded even after 20 min of wait time");
			}
		}
		public static string getVariableFromCSV(string filepath, string key){
			
			Ranorex.Core.Data.CsvDataConnector csvConnector=new Ranorex.Core.Data.CsvDataConnector("CSVConnector",filepath,true);
			csvConnector.SeparatorChar=',';
			Ranorex.Core.Data.ColumnCollection outColCollection;
			Ranorex.Core.Data.RowCollection outRowCollection;
			
			csvConnector.LoadData(out outColCollection, out outRowCollection);
			string rvalue = "";
			foreach(Ranorex.Core.Data.Row dataRow in outRowCollection)
			{
				rvalue = dataRow[key].ToString();
				Report.Info(rvalue);
			}
			return rvalue;
		}
		//Method for page scroll
		public static void Scroll(RepoItemInfo repoInfoElement, string Directon){
			if (repoInfoElement != null){
				Ranorex.Unknown itemAdapter = repoInfoElement.CreateAdapter<Ranorex.Unknown>(false);
				do {
					switch(Directon)
					{
						case "up" :
							Keyboard.Press("{PageUp}");
							break;
						case "down" :
							Keyboard.Press("{Next}");
							break;
					}
					
				} while (itemAdapter.Visible == false);
				itemAdapter.Click();
			}
		}
	}
}
