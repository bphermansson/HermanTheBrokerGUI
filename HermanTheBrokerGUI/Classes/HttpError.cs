namespace HermanTheBrokerGUI.Classes
{
    public class Errors
    {
        public List<string> PasswordRequiresUpper { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public Errors errors { get; set; }
    }
}
