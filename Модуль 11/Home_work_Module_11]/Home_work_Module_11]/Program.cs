using System;
using System.Collections.Generic;
using System.Linq;

namespace Home_work_Module_11_
{
    public class Book
    {
        private string v1;
        private string v2;
        private int v3;

        public string Title { get; set; }
        public string Author { get; set; }
        public int ISBN { get; set; }
        public bool Status { get; set; }

        public Book (string title, string author, int iSBN, bool status)
        {
            Title = title;
            Author = author;
            ISBN = iSBN;
            Status = true;
        }

        public Book(string title, string author, int isbn)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
        }
    }
    public class Library
    {
        public List<Book> Books { get; set; } = new List<Book>();
        public void AddBook(Book book) => Books.Add(book);
        public void RemoveBook (Book book) => Books.Remove(book);
        public List<Book> SearchBook(string query) => Books.Where(b => b.Title.Contains(query) || b.Author.Contains(query)).ToList();
        public List<Book> GetBooks() => Books;
    }

    public class Reader
    {
        public string Name { get; set; }
        private List<Book> rentedBooks = new List<Book>();
        private const int MaxRentedBooks = 5;

        public Reader(string name ) => Name = name;

        public bool BorrowBook(Book book)
        {
            if(book.Status && rentedBooks.Count < MaxRentedBooks)
            {
                book.Status = false;
                rentedBooks.Add(book);
                return true;
            }
            return false;
        }

        public void ReturnBook (Book book)
        {
            if(rentedBooks.Contains(book))
            {
                book.Status = true;
                rentedBooks.Remove(book);
            }
        }
    }
    public class Librarian
    {
        public string Name { get; set; }
        public Librarian (string name)
            { name = name.ToLower(); }
        public void ManageBook(Library library, Book book, bool isAdding)
        {
            if (isAdding)
            {
                library.AddBook(book);

            }
            else 
            { 
                library.RemoveBook(book);
            }
        }
    }

    public class Program
    {
        public static void Main()
        {
            Library library = new Library();

            Console.WriteLine("Введите имя библиотекаря:");
            string librarianName = Console.ReadLine();
            Librarian librarian = new Librarian(librarianName);

            // Ввод книг
            Console.WriteLine("Сколько книг вы хотите добавить?");
            int bookCount = int.Parse(Console.ReadLine());
            for (int i = 0; i < bookCount; i++)
            {
                Console.WriteLine($"\nВведите данные для книги #{i + 1}");

                Console.Write("Название: ");
                string title = Console.ReadLine();

                Console.Write("Автор: ");
                string author = Console.ReadLine();

                Console.Write("ISBN: ");
                int isbn = int.Parse(Console.ReadLine());

                Book book = new Book(title, author, isbn);
                librarian.ManageBook(library, book, true);
            }

            // Ввод данных читателя
            Console.WriteLine("\nВведите имя читателя:");
            string readerName = Console.ReadLine();
            Reader reader = new Reader(readerName);

            // Попытка взять книгу в аренду
            Console.WriteLine("\nВыберите номер книги для аренды:");
            List<Book> books = library.GetBooks();
            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {books[i].Title} - {books[i].Author}");
            }
            int bookNumber = int.Parse(Console.ReadLine()) - 1;
            Book selectedBook = books[bookNumber];

            if (reader.BorrowBook(selectedBook))
                Console.WriteLine($"{reader.Name} арендовал книгу '{selectedBook.Title}'.");

            // Показать все книги
            Console.WriteLine("\nВсе книги в библиотеке:");
            foreach (var book in library.GetBooks())
                Console.WriteLine($"{book.Title} - {book.Author} (Доступна: {book.Status})");

            // Вернуть книгу
            reader.ReturnBook(selectedBook);
            Console.WriteLine($"\n{reader.Name} вернул книгу '{selectedBook.Title}'.");

            // Показать все книги после возврата
            Console.WriteLine("\nВсе книги в библиотеке после возврата:");
            foreach (var book in library.GetBooks())
                Console.WriteLine($"{book.Title} - {book.Author} (Доступна: {book.Status})");

            Console.ReadKey();
        }
    }
}
