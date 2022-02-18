using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
namespace ConsoleChap17FileIOApp
{
    /// <summary>
    /// This class creates and reads from file for patient data
    /// </summary>
    /// <Student>Paul Lazaros</Student>
    /// <Class>CIS297</Class>
    /// <Semester>Winter 2022</Semester>
    class Program
    {
        static void Main(string[] args)
        {
            FileOperations();
            DirectoryOperations();
            FileStreamOperations();
            SequentialAccessWriteOperation();
            ReadSequentialAccessOperation();
            FindPatients();
        }
        //File operations
        /// <summary>
        /// This method opens a file
        /// </summary>
        /// <Student>Paul Lazaros</Student>
        /// <Class>CIS297</Class>
        /// <Semester>Winter 2022</Semester>
        static void FileOperations()
        {
            string fileName;
            Write("Enter a filename >> ");
            fileName = ReadLine();
            if (File.Exists(fileName))
            {
                WriteLine("File exists");
                WriteLine("File was created " +
                   File.GetCreationTime(fileName));
                WriteLine("File was last written to " +
                   File.GetLastWriteTime(fileName));
            }
            else
            {
                WriteLine("File does not exist");
            }
        }
        //Directory Operations
        /// <summary>
        /// This method opens a folder
        /// </summary>
        /// <Student>Paul Lazaros</Student>
        /// <Class>CIS297</Class>
        /// <Semester>Winter 2022</Semester>
        static void DirectoryOperations()
        {
            //Directory operations
            string directoryName;
            string[] listOfFiles;
            Write("Enter a folder >> ");
            directoryName = ReadLine();
            if (Directory.Exists(directoryName))
            {
                WriteLine("Directory exists, and it contains the following:");
                listOfFiles = Directory.GetFiles(directoryName);
                for (int x = 0; x < listOfFiles.Length; ++x)
                    WriteLine("   {0}", listOfFiles[x]);
            }
            else
            {
                WriteLine("Directory does not exist");
            }
        }
        //Using FileStream to create and write some text into it
        /// <summary>
        /// This method creates a file
        /// </summary>
        /// <Student>Paul Lazaros</Student>
        /// <Class>CIS297</Class>
        /// <Semester>Winter 2022</Semester>
        static void FileStreamOperations()
        {
            FileStream outFile = new
            FileStream("SomeText.txt", FileMode.Create,
            FileAccess.Write);
            StreamWriter writer = new StreamWriter(outFile);
            Write("Enter some text >> ");
            string text = ReadLine();
            writer.WriteLine(text);
            // Error occurs if the next two statements are reversed
            writer.Close();
            outFile.Close();
        }
        //Writing data to a Sequential Access text file
        /// <summary>
        /// This method writes data to a file
        /// </summary>
        /// <Student>Paul Lazaros</Student>
        /// <Class>CIS297</Class>
        /// <Semester>Winter 2022</Semester>
        static void SequentialAccessWriteOperation()
        {
            const int END = 999;
            const string DELIM = ",";
            const string FILENAME = "PatientData.txt";
            Patient pat = new Patient();
            FileStream outFile = new FileStream(FILENAME,
               FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(outFile);
            Write("Enter ID number or " + END +
               " to quit >> ");
            try
            {
                pat.IDNum = Convert.ToInt32(ReadLine());
            }
            catch
            {
                Console.WriteLine("An error occured");
            }
            finally
            {
                Console.WriteLine("Invalid ID, NOTE: Must be numerical");
            }
            while (pat.IDNum != END)
            {
                Write("Enter last name >> ");
                pat.Name = ReadLine();
                Write("Enter balance owed >> ");
                try
                {
                    pat.BalanceOwed = Convert.ToDouble(ReadLine());
                }
                catch
                {
                    Console.WriteLine("An error occured");
                }
                finally
                {
                    Console.WriteLine("Invalid balance, NOTE: Must be numerical");
                }
                writer.WriteLine(pat.IDNum + DELIM + pat.Name +
                   DELIM + pat.BalanceOwed);
                Write("Enter next ID number or " +
                   END + " to quit >> ");
                pat.IDNum = Convert.ToInt32(ReadLine());
            }
            writer.Close();
            outFile.Close();
        }
        //Read data from a Sequential Access File
        /// <summary>
        /// This method reads data from a file
        /// </summary>
        /// <Student>Paul Lazaros</Student>
        /// <Class>CIS297</Class>
        /// <Semester>Winter 2022</Semester>
        static void ReadSequentialAccessOperation()
        {
            const char DELIM = ',';
            const string FILENAME = "PatientData.txt";
            Patient pat = new Patient();
            FileStream inFile = new FileStream(FILENAME,
               FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string recordIn;
            string[] fields;
            WriteLine("\n{0,-5}{1,-12}{2,8}\n",
               "ID", "Name", "Balance");
            recordIn = reader.ReadLine();
            while (recordIn != null)
            {
                fields = recordIn.Split(DELIM);
                pat.IDNum = Convert.ToInt32(fields[0]);
                pat.Name = fields[1];
                pat.BalanceOwed = Convert.ToDouble(fields[2]);
                WriteLine("{0,-5}{1,-12}{2,8}",
                   pat.IDNum, pat.Name, pat.BalanceOwed.ToString("C"));
                recordIn = reader.ReadLine();
            }
            reader.Close();
            inFile.Close();
        }
        //repeatedly searches a file to produce 
        //lists of employees who meet a minimum salary requirement
        /// <summary>
        /// This method finds and displays data for patients with >= minimum balance
        /// </summary>
        /// <Student>Paul Lazaros</Student>
        /// <Class>CIS297</Class>
        /// <Semester>Winter 2022</Semester>
        static void FindPatients()
        {
            const char DELIM = ',';
            const int END = 999;
            const string FILENAME = "PatientData.txt";
            Patient pat = new Patient();
            FileStream inFile = new FileStream(FILENAME,
               FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string recordIn;
            string[] fields;
            double minBalance;
            Write("Enter minimum balance owed to find or " +
               END + " to quit >> ");
            minBalance = Convert.ToDouble(Console.ReadLine());
            while (minBalance != END)
            {
                WriteLine("\n{0,-5}{1,-12}{2,8}\n",
                   "ID", "Name", "Balance");
                inFile.Seek(0, SeekOrigin.Begin);
                recordIn = reader.ReadLine();
                while (recordIn != null)
                {
                    fields = recordIn.Split(DELIM);
                    pat.IDNum = Convert.ToInt32(fields[0]);
                    pat.Name = fields[1];
                    pat.BalanceOwed = Convert.ToDouble(fields[2]);
                    if (pat.BalanceOwed >= minBalance)
                        WriteLine("{0,-5}{1,-12}{2,8}", pat.IDNum,
                           pat.Name, pat.BalanceOwed.ToString("C"));
                    recordIn = reader.ReadLine();
                }
                Write("\nEnter minimum salary to find or " +
                   END + " to quit >> ");
                try
                {
                    minBalance = Convert.ToDouble(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("An error occured");
                }
                finally
                {
                    Console.WriteLine("Invalid balance, NOTE: Must be numerical");
                }
            }
            reader.Close();  // Error occurs if
            inFile.Close(); //these two statements are reversed
        }
        /// <summary>
        /// This class holds data for the patient
        /// </summary>
        class Patient
        {
            public int IDNum { get; set; }
            public string Name { get; set; }
            public double BalanceOwed { get; set; }
        }
    }
}