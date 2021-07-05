using System.Collections.Generic;
using System.IO;

namespace Todo.Utils {
    public class Stuff {
        public static string TipsFileName { get; set; }

        public static List<string> GetTips() {
            List<string> tips = null;
            if (File.Exists(TipsFileName)) {
                var text = File.ReadAllText(TipsFileName);
                tips = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(text);
            }
            if (tips == null) tips = new List<string>();
            return tips;
        }
    }
}
