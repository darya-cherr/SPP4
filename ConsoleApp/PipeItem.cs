namespace ConsoleApp
{
    public class PipeItem
    {
        public string path;
        public string code;

        public PipeItem(string path)
        {
            this.path = path;
        }
        
        public PipeItem(string path, string code)
        {
            this.path = path;
            this.code = code;
        }
        
    }
}