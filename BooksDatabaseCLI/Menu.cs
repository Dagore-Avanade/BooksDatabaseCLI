using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksDatabaseCLI
{
    internal class Menu
    {
        static bool watch = true;
        static string[] arguments = Array.Empty<string>();
        static string filePath = string.Empty;
        static BookRepository repository = null!;
        static bool changes = false;
        public static void MainLoop()
        {
            Console.WriteLine("Book Database");
            Console.WriteLine();
            LoadDatabase();

            while (watch)
            {
                Console.WriteLine();
                PrintOptions();
                string input = Console.ReadLine();
                if (input.Length != 0)
                {
                    arguments = input.Split(separator: new char[] { ' ' }, options: StringSplitOptions.RemoveEmptyEntries);
                    string command = arguments[0];
                    switch (command)
                    {
                        case "ADD":
                        case "add":
                            Add();
                            break;
                        case "SHOW":
                        case "show":
                            Show();
                            break;
                        case "SEARCH":
                        case "search":
                            Search();
                            break;
                        case "DELETE":
                        case "delete":
                            DeleteByIndex();
                            break;
                        default:
                            watch = !ConfirmExit();
                            break;
                    }
                }
                else
                {
                    watch = !ConfirmExit();
                }
                if (!watch)
                {
                    ConfirmSave();
                }
            }
        }
        static void LoadDatabase()
        {
            Console.WriteLine("Aporta una fichero con los registros (CSV), si no se aporta ninguna se usará una por defecto: ");
            filePath = Console.ReadLine();

            try
            {
                repository = BookRepository.FromCSV(filePath);
            }
            catch
            {
                Console.WriteLine("La carga del fichero propocionado ha fallado y se procede con el fichero por defecto.");
                try
                {
                    filePath = "../../../../books.csv";
                    repository = BookRepository.FromCSV(filePath);
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error inesperado: {ex.Message}");
                    return;
                }
            }
        }
        static void PrintOptions()
        {
            Console.WriteLine();
            TabWriteLine("\"ADD\" para agregar un nuevo libro.");
            TabWriteLine("\"SHOW\" para mostrar los libros.");
            TabWriteLine("\"SEARCH Title\" para buscar en la base de datos por título.");
            TabWriteLine("\"DELETE Id\" para eliminar un libro.");
            TabWriteLine("Puede abandonar el programa con cualquier tecla + ENTER.");
            Console.WriteLine();
            Console.Write("Escribe aquí el comando a ejecutar: ");
        }
        static void Show()
        {
            Console.WriteLine();
            Console.WriteLine(repository.AllBooksToString());
        }
        static void Add()
        {
            Console.Write("Introduzca el autor del libro: ");
            string author = Console.ReadLine();
            Console.Write("Introduzca el título del libro: ");
            string title = Console.ReadLine();

            repository.Add(author, title);
            changes = true;
        }
        static void Search()
        {
            if (arguments.Length > 1)
            {
                string search = JoinOtherArguments();
                Book[] books = repository.FindBooksByTitle(search);
                Console.WriteLine();
                if (books.Length > 0)
                {
                    foreach (Book book in books)
                    {
                        Console.WriteLine(book.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("No se han encontrado resultados.");
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("No entiendo el comando que ha ingresado.");
            }
        }
        static void DeleteByIndex()
        {
            if (arguments.Length > 1 && int.TryParse(arguments[1], out int index))
            {
                if (repository.DeleteByIndex(index) == null)
                {
                    Console.WriteLine("No hay ningún libro en el índice especificado.");
                }
                else
                {
                    changes = true;
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("No entiendo el comando que ha ingresado.");
            }
        }
        static bool ConfirmExit()
        {
            Console.WriteLine();
            Console.WriteLine("¿Está seguro de querer salir?");
            Console.Write("Pulse cualquier tecla para dejar la aplicación o \"N\" para permanecer en ella: ");
            string input = Console.ReadLine();

            if (input == "N")
                return false;
            else
                return true;

        }
        static void ConfirmSave()
        {
            if (changes)
            {
                Console.WriteLine("¿Quiere guardar los cambios? (Y/y para sí, cualquier otra tecla no)");
                string input = Console.ReadLine();
                if (input == "Y" || input == "y")
                {
                    repository.Save(filePath);
                }
            }
        }
        static string JoinOtherArguments()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 1; i < arguments.Length; i++)
            {
                stringBuilder.Append(arguments[i]).Append(' ');
            }
            return stringBuilder.ToString();
        }
        static void TabWriteLine(string s)
        {
            Console.WriteLine($"    {s}");
        }
    }
}
