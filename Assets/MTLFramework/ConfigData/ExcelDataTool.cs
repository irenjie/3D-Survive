using System.IO;
using UnityEngine;

namespace MTLFramework.Editor {

    public class ExcelDataTool {
        public const string _NameSpace = "MTLFrameWork.MTLExcelDataClass";
        public const string _DataPath = "Assets/Config";
        public const string _GeneratePath = "Assets/Script/ConfigData";

        public void GenerateScript() {
            string[] files = Directory.GetFiles(_DataPath, "*.xlsx");
            foreach (string file in files) {
                FileStream stream = File.Open(file,FileMode.Open,FileAccess.Read);
                
            }
        }
    }
}