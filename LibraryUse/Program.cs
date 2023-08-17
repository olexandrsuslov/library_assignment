
using System.Diagnostics;
using FileLib;

class Program
{
    static void Main(string[] args)
    {
        int row = 0;
        string? input;
        do
        {
            if (row == 0 || row >= 25)
                ResetConsole();
            Console.WriteLine("Choose xml or text or bin file");
            string command = Console.ReadLine();
            FileLibrary lib = new FileLibrary();
            Console.WriteLine($"enter {command} file name");
            input = Console.ReadLine();
            
            Console.WriteLine("edit record - edit");
            Console.WriteLine("add record - add");
            Console.WriteLine("delete records - delete");
            Console.WriteLine("convert file to other format - convert");
            Console.WriteLine("<-----------file output------------>");
            switch (command)
            {
                case "xml":
                    
                    if (string.IsNullOrEmpty(input)) break;
                    lib.ReadFromXmlFile(input);
                    XmlFileOperations(ref lib);
                    break;

                case "text":
                    if (string.IsNullOrEmpty(input)) break;
                    lib.ReadFromTextFile(input);
                    TextFileOperations(ref lib);
                    break;
                case "bin":
                    if (string.IsNullOrEmpty(input)) break;
                    lib.ReadFromBinFile(input);
                    BinFileOperations(ref lib);
                    break;
            }
            
            
            
             Console.WriteLine();
            row += 4;
        } while (true);
        return;

        // ResetConsole  method
        void ResetConsole()
        {
            if (row > 0)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            Console.Clear();
            Console.WriteLine($"{Environment.NewLine}Press <Enter> only to exit; otherwise, enter a file name and press <Enter>:{Environment.NewLine}");
            row = 3;
        }

        void TextFileOperations(ref FileLibrary lib)
        {
            
            string command = Console.ReadLine();
            switch (command)
            {
                case "edit":
                    Console.WriteLine("enter row number to edit");
                    int id = Int32.Parse(Console.ReadLine());
                    lib.UpdateRecordFile(id);
                    lib.WriteToTextFile();
                    break;

                case "add":
                    lib.AddRecordTextFile();
                    lib.WriteToTextFile();
                    break;
                case "delete":
                    Console.WriteLine("enter row number to delete");
                    id = Int32.Parse(Console.ReadLine());
                    lib.DeleteRecordFile(id);
                    lib.WriteToTextFile();
                    break;
                case "convert":
                    lib.WriteToXmlFile();
                    break;
            }
        }
        
        void XmlFileOperations(ref FileLibrary lib)
        {
            string command = Console.ReadLine();
            switch (command)
            {
                case "edit":
                    Console.WriteLine("enter row number to edit");
                    int id = Int32.Parse(Console.ReadLine());
                    lib.UpdateRecordXmlFile(id);
                    break;

                case "add":
                    lib.AddRecordXmlFile();
                    break;
                case "delete":
                    Console.WriteLine("enter row number to delete");
                    id = Int32.Parse(Console.ReadLine());
                    lib.DeleteRecordXmlFile(id);
                    break;
                case "convert":
                    lib.WriteToBinaryFile();
                    break;
            }
        }
        
        
        void BinFileOperations(ref FileLibrary lib)
        {
            
            string command = Console.ReadLine();
            switch (command)
            {
                case "edit":
                    Console.WriteLine("enter row number to edit");
                    int id = Int32.Parse(Console.ReadLine());
                    lib.UpdateRecordFile(id);
                    lib.WriteToBinaryFile();
                    break;

                case "add":
                    lib.AddRecordTextFile();
                    lib.WriteToBinaryFile();
                    break;
                case "delete":
                    Console.WriteLine("enter row number to delete");
                    id = Int32.Parse(Console.ReadLine());
                    lib.DeleteRecordFile(id);
                    lib.WriteToBinaryFile();
                    break;
                case "convert":
                    lib.WriteToXmlFile();
                    break;
            }
        }
        
        
    }
}