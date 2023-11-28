using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace BankClass
{
    class Bank
    {
        private int _money;
        private string _name;
        private int _percent;
        private readonly object _lock = new object();

        public int Money
        {
            get => _money;
            set
            {
                lock (_lock)
                {
                    _money = value;
                    WriteDataToFile();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                lock (_lock)
                {
                    _name = value;
                    WriteDataToFile();
                }
            }
        }

        public int Percent
        {
            get => _percent;
            set
            {
                lock (_lock)
                {
                    _percent = value;
                    WriteDataToFile();
                }
            }
        }

        public Bank(int money, string name, int percent)
        {
            _money = money;
            _name = name;
            _percent = percent;
        }

        private void WriteDataToFile()
        {
            Thread thread = new Thread(() =>
            {
                lock (_lock)
                {
                    string data = $"Money: {_money}, Name: {_name}, Percent: {_percent}%";
                    string filePath = "bank_data.txt";
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(filePath, false))
                        {
                            writer.WriteLine(data);
                        }
                        Console.WriteLine("Data has been written to file.");
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine($"An error occurred while writing to file: {e.Message}");
                    }
                }
            });
            thread.Start();
        }
    }

    class Program
    {
        static void Main()
        {
            Bank bank = new Bank(1000, "MyBank", 5);
            bank.Money = 1500;
            bank.Name = "UpdatedBank";
            bank.Percent = 7;
            Console.ReadLine();
        }
    }
}
