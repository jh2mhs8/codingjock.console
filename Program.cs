using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace codingjock.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var columns = new [] 
            {
                "Column 1",
                "Column 2",
                "Column 3"
            };
            // var items = new List<string> 
            // {
            //     "Column 1",
            //     "Column 2",
            //     "Column 3",
            //     "Column 4",
            //     "Column 5",
            //     "Column 6",
            //     "Column 7",
            // };
            var items = new List<Item>();
            for (int i = 0; i < 2000001; i++)
            {
                items.Add(new Item
                {
                    FirstName = "Jay" + i,
                    LastName = "Hackett" + i,
                    SomeNumber = i
                });
            }
            // var collection = new List<List<string>>();
            // collection.Add(items);

            var bytes = GetExcel(columns, items);
            File.WriteAllBytes(@"C:\Users\jayha\Desktop\test.csv", bytes);

            
        }

        private static byte[] GetExcel<T>(string[] columns, List<T> items)
        {
            var properties = typeof(T).GetProperties();
            var itemCount = typeof(T).GetProperties().Count();
            if (columns.Length != itemCount)
            {
                return null;
            }
            var count = columns.Length;
            var format = string.Empty;
            for (int i = 0; i < count; i++)
            {
                var start = '{';
                var end = '}';
                var separator = ",";
                var lineEnd = "\n";

                format += start;
                format += i;
                format += end;
                

                if (i == count - 1)
                {
                    format += lineEnd;
                }else
                {
                    format += separator;
                }

            }
            var builder = new StringBuilder();
            builder.AppendFormat(format, columns);

            for (int i = 0; i < items.Count(); i++)
            {
                object[] objects = new object[itemCount];

                for (int j = 0; j < itemCount; j++)
                {
                    objects[j] = items[i].GetType().GetProperties()[j].GetValue(items[i], null).ToString();
                }

                
                builder.AppendFormat(format, objects);
            }

            var csvBytes = Encoding.ASCII.GetBytes(builder.ToString());

            return csvBytes;
        }
    }

    internal class Item
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SomeNumber { get; set; }
    }
}
