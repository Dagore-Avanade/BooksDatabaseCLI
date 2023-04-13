using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksDatabaseCLI
{
    internal record Book(string Author, string Title)
    {
        public static Book FromCSVRow (string row)
        {
            string[] fields = row.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length == 2 )
            {
                return new Book(fields[0], fields[1]);
            }
            else
            {
                throw new FormatException();
            }
        }
        public override string ToString()
        {
            return $"{Author} - {Title}";
        }
        public string ToCSVRow()
        {
            return $"{Author},{Title}";
        }
    }
}
