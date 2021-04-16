using System;
using UnityEngine;
using System.Collections.Generic;
using Framework.Map;
using Gameplay.UI.Others.MenuLogic;
using Gameplay.UI.Widgets;
using Framework.Managers;
using MiniJSON;

namespace BlasphemousExtractor
{
	class Main : MonoBehaviour
	{

		public void Start()
		{
			status = "LOAD DATA";
		}
		public void Update()
		{
			if (_RendererConfig.Equals(null)) {
				status = "LOAD DATA";
			} else {
				status = "DATA LOADED";
			}
		}
		public void OnGUI()
		{
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 150f, 50f), "DLL LOADED");
			
			// Extract data
			if (GUI.Button(new Rect(155, Screen.height - 50, 150, 50), status)) {
				_RendererConfig = Core.NewMapManager;
				var celdas = _RendererConfig.GetAllRevealedCells();
				var result = getJson(celdas);
				
				GUIUtility.systemCopyBuffer = result; // Save to clipboard
			}
			
			// Enbale debug console
			if (GUI.Button(new Rect(0, Screen.height - 50, 150, 50), "Toggle Console")) {
				_Console = FindObjectOfType<ConsoleWidget>();
				consoleStatus = !consoleStatus;
				_Console.SetEnabled(consoleStatus);
			}
		}
		
		private string getJson(List<CellData> celdas) {
			Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
			foreach (CellData cellData in celdas)
			{
				string key = cellData.CellKey.X.ToString() + "_" + cellData.CellKey.Y.ToString();
				string key2 = cellData.ZoneId.GetKey();
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
				/*foreach (KeyValuePair<string, Color> keyValuePair in BlasHack.ZonesColor)
				{
					if (key2.Contains(keyValuePair.Key))
					{
						byte b = (byte)(keyValuePair.Value.r * 256f);
						byte b2 = (byte)(keyValuePair.Value.g * 256f);
						byte b3 = (byte)(keyValuePair.Value.b * 256f);
						dictionary2.Add("Color", "#" + b.ToString("X2") + b2.ToString("X2") + b3.ToString("X2"));
					}
				}*/
				string text = string.Empty;
				using (List<EditorMapCellData.CellSide>.Enumerator enumerator3 = MapRendererConfig.spriteKeyOrder.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						int num = (int)enumerator3.Current;
						string str = "_";
						int num2 = num;
						if (cellData.Doors[num2])
						{
							str = "D";
						}
						else if (cellData.Walls[num2])
						{
							str = "W";
						}
						text += str;
					}
				}
				if (!dictionary.ContainsKey(key))
				{
					dictionary2.Add("ZoneId", key2);
					dictionary2.Add("Type", Enum.GetName(typeof(EditorMapCellData.CellType), cellData.Type));
					dictionary2.Add("Sprite", text);
					dictionary.Add(key, dictionary2);
				}
			}
			return Json.Serialize(dictionary);
		}
		
		
		private NewMapManager _RendererConfig;
		private string status;
		private bool consoleStatus = false;
		private ConsoleWidget _Console;
	}
}