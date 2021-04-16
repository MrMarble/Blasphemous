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
				var cells = _RendererConfig.GetAllRevealedCells();
				var result = getJson(cells);
				
				GUIUtility.systemCopyBuffer = result; // Save to clipboard
			}
			
			// Enable debug console
			if (GUI.Button(new Rect(0, Screen.height - 50, 150, 50), "Toggle Console")) {
				_Console = FindObjectOfType<ConsoleWidget>();
				consoleStatus = !consoleStatus;
				_Console.SetEnabled(consoleStatus);
			}
		}
		
		private string getJson(List<CellData> cells) {
			Dictionary<string, Dictionary<string, string>> cellDict = new Dictionary<string, Dictionary<string, string>>();
			foreach (CellData cellData in cells)
			{
				string cellCoords = cellData.CellKey.X.ToString() + "_" + cellData.CellKey.Y.ToString();
				string zoneId = cellData.ZoneId.GetKey();
				Dictionary<string, string> cellDataDict = new Dictionary<string, string>();
				string spriteName = string.Empty;
				using (List<EditorMapCellData.CellSide>.Enumerator spriteEnumerator = MapRendererConfig.spriteKeyOrder.GetEnumerator())
				{
					while (spriteEnumerator.MoveNext())
					{
						int currentIndex = (int)spriteEnumerator.Current;
						string cellSideType = "_"; // _ => Empty, W => Wall, D => Door
						if (cellData.Doors[currentIndex])
						{
							cellSideType = "D";
						}
						else if (cellData.Walls[currentIndex])
						{
							cellSideType = "W";
						}
						spriteName += cellSideType;
					}
				}
				if (!cellDict.ContainsKey(cellCoords))
				{
					cellDataDict.Add("ZoneId", zoneId);
					cellDataDict.Add("Type", Enum.GetName(typeof(EditorMapCellData.CellType), cellData.Type));
					cellDataDict.Add("Sprite", spriteName);
					cellDict.Add(cellCoords, cellDataDict);
				}
			}
			return Json.Serialize(cellDict);
		}
		
		
		private NewMapManager _RendererConfig;
		private string status;
		private bool consoleStatus = false;
		private ConsoleWidget _Console;
	}
}