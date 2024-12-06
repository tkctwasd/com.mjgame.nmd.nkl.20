using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace MJGame
{
    public class JsonData
    {
        public JObject ObjectData;
        public JsonData(string text)
        {
            ObjectData = JsonConvert.DeserializeObject<JObject>(text);
        }
    }
}
