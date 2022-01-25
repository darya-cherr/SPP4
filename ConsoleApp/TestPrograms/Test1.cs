namespace ConsoleApp.TestPrograms
{
    public class Test1
    {
        public string Field1;
        public bool Field2;
                
        public int CharCount(string str, char c)
        { 
            int counter = 0; 
            for (int i = 0; i < str.Length; i++) 
            { 
                if (str[i] == c) 
                    counter++;
            } 
            return counter;
        }

    }
}