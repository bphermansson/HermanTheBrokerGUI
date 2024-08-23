using HermanTheBrokerGUI.Services;

namespace HermanTheBrokerGUI.Classes
{
    public class ResponseRoot
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public ResponseErrors errors { get; set; }
    }
}
