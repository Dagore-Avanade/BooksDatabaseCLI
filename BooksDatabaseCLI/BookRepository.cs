using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BooksDatabaseCLI
{
    internal class BookRepository
    {
        readonly List<Book> books = new();
        private BookRepository(IEnumerable<Book> books) 
        {
            this.books = (List<Book>)books;
        }
        public static BookRepository FromCSV (string path)
        {
            if (File.Exists(path))
            {
                StreamReader streamReader = new(path);
                List<Book> books = new();
                string row;

                while (!streamReader.EndOfStream)
                {
                    row = streamReader.ReadLine();
                    Book book = Book.FromCSVRow(row);
                    books.Add(book);
                }

                streamReader.Dispose();
                return new BookRepository(books);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public void Add(string author, string title)
        {
            Book book = new Book(author, title);
            books.Add(book);
        }
        public string AllBooksToString()
        {
            StringBuilder stringBuilder = new();
            for (int i = 0; i < books.Count; i++)
            {
                Book? book = books[i];
                if (book is not null)
                    stringBuilder
                        .Append($"{i,4}: ")
                        .AppendLine(book.ToString());
            }
            return stringBuilder.ToString();
        }
        public Book[] FindBooksByTitle(string search)
        {
            return books.FindAll(book => book.Title.Contains(search)).ToArray();
        }
        public Book? DeleteByIndex(int index)
        {
            if (index >= books.Count)
                return null;
            
            Book book = books[index];
            books.RemoveAt(index);
            return book;
        }
        public void Save(string path)
        {
            if (File.Exists(path))
            {
                StreamWriter streamWriter = new(path);

                foreach (Book book in books)
                {
                    streamWriter.WriteLine(book.ToCSVRow());
                }

                streamWriter.Dispose();
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }
}
