/*
* Book Struct - Represents a book with an ID, ISBN, Title, Author, Year, Genre, and Quantity.
*/
public struct Book
{
    static int IDCounter = 1;
    public int ID;
    public string ISBN;
    public string Title;
    public string Author;
    public int Year;
    public string Genre;
    public int Quantity;

    public Book()
    {
        ID = IDCounter++;
        ISBN = string.Empty;
        Title = string.Empty;
        Author = string.Empty;
        Genre = string.Empty;
        Quantity = 0;
        Year = DateTime.Now.Year;
    }

    public override bool Equals(object obj)
    {
        return obj is Book book &&
               Title == book.Title &&
               Author == book.Author &&
               Year == book.Year &&
               Genre == book.Genre;
    }

    public override string ToString()
    {
        return $"ID: {ID}, ISBN: {ISBN}, Title: {Title}, Author: {Author}, Year: {Year}, Genre: {Genre}, Quantity: {Quantity}";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Title, Author, Year, Genre, Quantity);
    }

    public void Display()
    {
        Console.WriteLine(ToString());
    }

}

/*
* Book Query Struct - Represents a query to search for a book by ID, Title, or Author.
*/
public struct BookQuery
{
    public int ID;
    public string Title;
    public string Author;

    public BookQuery(int id = -1, string title = "", string author = "")
    {
        ID = id;
        Title = title;
        Author = author;
    }
}

/*
* Member Struct - Represents a member with an ID, Name, and a list of Books Borrowed.
*/
public struct Member
{
    static int IDCounter = 1;
    public int ID;
    public string Name;
    public List<Book> BooksBorrowed = new List<Book>();
    public Member()
    {
        ID = IDCounter++;
    }
    public override bool Equals(object obj)
    {
        return obj is Member member &&
               Name == member.Name;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }
    public override string ToString()
    {
        return $"ID: {ID}, Name: {Name}, Books Borrowed: {string.Join(", ", BooksBorrowed.Select(book => book.ID))}";
    }
    public void Display()
    {
        Console.WriteLine(ToString());
    }
}

/*
* BorrowingRecord Struct - Represents a record of a book borrowed by a member with a Book ID, Member ID, Date, Due Date, and Returned status.
*/
public struct BorrowingRecord
{
    public int BookID;
    public int MemberID;
    public string Date;
    public string DueDate;
    public bool Returned;

    public override bool Equals(object obj)
    {
        return obj is BorrowingRecord record &&
               BookID == record.BookID &&
               MemberID == record.MemberID &&
               Date == record.Date &&
               DueDate == record.DueDate &&
               Returned == record.Returned;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(BookID, MemberID, Date, DueDate, Returned);
    }
    public override string ToString()
    {
        return $"Book ID: {BookID}, Member ID: {MemberID}, Date: {Date}, Due Date: {DueDate}, Returned: {Returned}";
    }
    public void Display()
    {
        Console.WriteLine(ToString());
    }
}

public class LibraryManagementSystem
{
    /*
    * ELMSChoice Enum - Represents the choices available in the Library Management System.
    */
    public enum ELMSChoice
    {
        Add_Book = 1,
        View_Books,
        Search_Books,
        Borrow_Book,
        Return_Book,
        Exit
    };
    /*
    * ELMSSearchChoice Enum - Represents the choices available for searching books in the Library Management System.
    */
    public enum ELMSSearchChoice
    {
        ID = 1,
        Title,
        Author
    };
    const string WARNING = "Invalid input. Please try again!";
    static List<Book> books = new List<Book>();
    static List<Member> members = new List<Member>();
    static List<BorrowingRecord> records = new List<BorrowingRecord>();
    Member defaultMember;

    public void ViewBooks()
    {
        Console.WriteLine("1. All Books");
        Console.WriteLine("2. By Genre");
        int choice = ReadInputAsInt("Enter your choice: ", "", WARNING, 1, 2);

        switch (choice)
        {
            case 1:
                Console.WriteLine("All Books: ");
                ListBooks();
                break;
            case 2:
                string genre = ReadInputAsString("Enter the genre: ");
                Console.WriteLine($"All Books in [{genre}] genre: ");
                ListBooks(genre);
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }
    public void ListBooks(string genre = "")
    {
        bool found = false;
        foreach (Book book in books)
        {
            if (genre == "" || book.Genre.ToLower().Contains(genre.ToLower()))
            {
                book.Display();
                found = true;
            }
        }
        if (!found)
            Console.WriteLine("No books found.");
    }
    public Book AddBook(Book book)
    {
        if (books.Contains(book))
        {
            Console.WriteLine("Book already exists. Updating quantity...");
            Book existingBook = books[books.IndexOf(book)];
            existingBook.Quantity += book.Quantity;
            books[books.IndexOf(book)] = existingBook;
            book = existingBook;
        }
        else
            books.Add(book);
        return book;
    }

    public void AddBook()
    {
        string title = ReadInputAsString("Enter the title: ");
        string author = ReadInputAsString("Enter the author: ");
        int year = ReadInputAsInt("Enter the publication year: ");
        string genre = ReadInputAsString("Enter the genre: ");
        int quantity = ReadInputAsInt("Enter the quantity: ");
        Book book = AddBook(new Book() { Title = title, Author = author, Year = year, Genre = genre, Quantity = quantity });
        Console.WriteLine("Book added successfully:");
        book.Display();
    }

    // Binary search by book ID
    public Book SearchBookByID(int id)
    {
        int left = 0, right = books.Count - 1;
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (books[mid].ID == id)
                return books[mid];
            if (books[mid].ID < id)
                left = mid + 1;
            else
                right = mid - 1;
        }
        return new Book() { ID = -1 };
    }

    public List<Book> SearchBooks(BookQuery query)
    {
        List<Book> foundBooks = new List<Book>();
        if (query.Title == "" && query.Author == "" && query.ID < 0)
        {
            Console.WriteLine("No search query specified."); ;
        }
        else
        {
            if(query.ID > -1)
            {
                Book book = SearchBookByID(query.ID);
                if (book.ID > -1)
                {
                    foundBooks.Add(book);
                    return foundBooks;
                }
            }
            foreach (Book book in books)
            {
                if (!string.IsNullOrEmpty(query.Title) && book.Title.ToLower().Contains(query.Title.ToLower()))
                {
                    foundBooks.Add(book);
                }
                if (!string.IsNullOrEmpty(query.Author) && book.Author.ToLower().Contains(query.Author.ToLower()))
                {
                    foundBooks.Add(book);
                }
            }
        }
        return foundBooks;
    }

    public void SearchBooks()
    {
        Console.WriteLine("1. Search by ID");
        Console.WriteLine("2. Search by Title");
        Console.WriteLine("3. Search by Author");
        ELMSSearchChoice choice = (ELMSSearchChoice)ReadInputAsInt("Enter your choice: ", "", WARNING, 1, 3);

        List<Book> foundBooks = new List<Book>();
        BookQuery query = new BookQuery();
        switch (choice)
        {
            case ELMSSearchChoice.ID:
                query.ID = ReadInputAsInt("Enter the ID: ");
                break;
            case ELMSSearchChoice.Title:
                query.Title = ReadInputAsString("Enter the title: ");
                break;
            case ELMSSearchChoice.Author:
                query.Author = ReadInputAsString("Enter the author: ");
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
        foundBooks = SearchBooks(query);
        Console.WriteLine($"Search result, {foundBooks.Count} books found: ");
        foreach (Book book in foundBooks)
            book.Display();

    }

    public void DisplayBorrowedBooks(Member member)
    {
        if(member.BooksBorrowed.Count == 0)
        {
            Console.WriteLine("No books borrowed by you.");
            return;
        }
        Console.WriteLine("Books borrowed by you:");
        Console.WriteLine(string.Join("\n", member.BooksBorrowed.Select(book => $"ID: {book.ID}, Title: {book.Title}, Due Date: {records.Find(r => r.BookID == book.ID && r.MemberID == member.ID && !r.Returned).DueDate}")));
    }

    public void BorrowBook()
    {
        Console.WriteLine("Available Books: ");
        ListBooks();
        DisplayBorrowedBooks(defaultMember);
        int bookID = ReadInputAsInt("Enter the book ID: ");
        BookQuery query = new BookQuery(bookID);

        List<Book> foundBooks = SearchBooks(query);

        // Book not found
        if (foundBooks.Count == 0)
        {
            Console.WriteLine("Book not found.");
            return;
        }

        // Book found
        Book book = foundBooks[0];
        // Book not available
        if (book.Quantity == 0)
        {
            Console.WriteLine("Book not available.");
            return;
        }
        Member member = GetMember();

        (bool success, BorrowingRecord record) = AddBorrowRecord(
            new BorrowingRecord
            { 
                BookID = book.ID, 
                MemberID = member.ID, 
                Date = DateTime.Now.ToString("dd/MM/yyyy"), 
                DueDate = DateTime.Now.AddDays(14).ToString("dd/MM/yyyy"), 
                Returned = false 
            }
        );
        if (!success)
        {
            Console.WriteLine("You already borrowed the book, borrowing record shows as below:");
        }
        else
        {
            book.Quantity--;
            books[books.FindIndex(b => b.ID == book.ID)] = book;
            member.BooksBorrowed.Add(book);
            members[members.FindIndex(m => m.ID == member.ID)] = member;
            Console.WriteLine($"You have successfully borrowed the book, borrowing record shows as below:");
        }
        record.Display();

    }

    public void ReturnBook()
    {
        DisplayBorrowedBooks(defaultMember);
        int bookID = ReadInputAsInt("\nEnter the book ID to return: ");
        List<Book> foundBooks = SearchBooks(new BookQuery(bookID));
        if (foundBooks.Count == 0)
        {
            Console.WriteLine("Book not found.");
            return;
        }
        Book book = foundBooks[0];
        Member member = GetMember();

        BorrowingRecord record = records.Find(r => r.BookID == book.ID && r.MemberID == member.ID && !r.Returned);
        if (record.BookID == 0)
        {
            Console.WriteLine("Borrowing record not found.");
            return;
        }
        book.Quantity++;
        books[books.FindIndex(b => b.ID == book.ID)] = book;
        member.BooksBorrowed.Remove(book);
        members[members.FindIndex(m => m.ID == member.ID)] = member;
        record.Returned = true;
        records[records.FindIndex(r => r.BookID == book.ID && r.MemberID == member.ID)] = record;
        Console.WriteLine($"Returned the book ID: {book.ID} - {book.Title}");
    }

    public (bool, BorrowingRecord) AddBorrowRecord(BorrowingRecord record)
    {
        if (records.Contains(record))
            return (false, record);
        records.Add(record);
        return (true, record);
    }

    public Member GetMember()
    {
        /*
            int memberID = ReadInputAsInt("Enter the member ID (Press enter to use default member): ");
            if (memberID == int.MinValue)
                memberID = defaultMember.ID;
            Member member = SearchMemberByID(memberID);
            if (member.ID < 0)
            {
                Console.WriteLine("Member not found.");
            }
            return member;
        */

        // Use default member for demonstration purpose
        return defaultMember;
    }

    public void AddMember(Member member)
    {
        if (!members.Contains(member))
            members.Add(member);
    }

    public Member SearchMemberByID(int id)
    {
        foreach (Member member in members)
        {
            if (member.ID == id)
            {
                member.Display();
                return member;
            }

        }
        return new Member() { ID = -1 };
    }

    void InitBooks()
    {
        Book book;
        book = new Book
        {
            Title = "The Lost Treasure",
            ISBN = "978-16-0",
            Author = "Jane Smith",
            Year = 2003,
            Genre = "Adventure",
            Quantity = 10
        };
        AddBook(book);

        book = new Book
        {
            Title = "Mysteries of the Mind",
            ISBN = "978-16-1",
            Author = "John Doe",
            Year = 2010,
            Genre = "Psychology",
            Quantity = 7
        };
        AddBook(book);

        book = new Book
        {
            Title = "Galactic Horizons",
            ISBN = "978-16-2",
            Author = "Emily Clark",
            Year = 2017,
            Genre = "Science Fiction",
            Quantity = 12
        };
        AddBook(book);

        book = new Book
        {
            Title = "Whispers in the Dark",
            ISBN = "978-16-3",
            Author = "Michael Johnson",
            Year = 1999,
            Genre = "Horror",
            Quantity = 4
        };
        AddBook(book);

        book = new Book
        {
            Title = "The Art of War",
            ISBN = "978-16-4",
            Author = "Sun Tzu",
            Year = 500,
            Genre = "Military",
            Quantity = 1
        };
        AddBook(book);
    }

    void InitMembers()
    {
        defaultMember = new Member
        {
            Name = "JJ"
        };
        AddMember(defaultMember);
    }
    public void Run()
    {
        Console.Clear();
        //
        // Initialize books sample data
        InitBooks();
        // Initialize a default member for demonstration purpose
        InitMembers();
        //Main menu
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Welcome to the Library Management System!");
            foreach (ELMSChoice name in Enum.GetValues(typeof(ELMSChoice)))
            {
                Console.WriteLine($"{(int)name}. {name.ToString().Replace("_", " ")}");
            }
            ELMSChoice choice = (ELMSChoice)ReadInputAsInt("Enter your choice: ", "", WARNING, 1, 6);
            switch (choice)
            {
                case ELMSChoice.Add_Book:
                    Console.WriteLine("Adding a book:");
                    AddBook();
                    break;
                case ELMSChoice.View_Books:
                    Console.WriteLine("Viewing books: ");
                    ViewBooks();
                    break;
                case ELMSChoice.Search_Books:
                    Console.WriteLine("Searching for a book:");
                    SearchBooks();
                    break;
                case ELMSChoice.Borrow_Book:
                    Console.WriteLine("Borrowing a book:");
                    BorrowBook();
                    break;
                case ELMSChoice.Return_Book:
                    Console.WriteLine("Returning a book:");
                    ReturnBook();
                    break;
                case ELMSChoice.Exit:
                    Console.WriteLine("Exiting the Library Management System...");
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            ReadRawInput("\nPress enter to continue...");
            Console.WriteLine("\n------------------------------------------------------------\n");
        }
    }

    // Utility Functions
    int ReadInputAsInt(string question, string quit = null, string waring = WARNING, int min = int.MinValue, int max = int.MaxValue)
    {
        string res = ReadInputAsString(question, "", waring, str => int.TryParse(str, out int num) && num >= min && num <= max);
        if (res == quit) return int.MinValue;
        return int.Parse(res);
    }

    string ReadInputAsString(string question, string quit = null, string warning = WARNING, Func<string, bool> validator = null, Func<string, string> Prompt = null)
    {
        if (validator == null) validator = (str) => !string.IsNullOrEmpty(str);
        if (Prompt == null) Prompt = ReadRawInput;
        string str = Prompt(question).ToLower();
        while (str != quit && !validator(str))
        {
            Console.WriteLine(warning);
            str = Prompt(question).ToLower();
        }
        return str;
    }

    string ReadRawInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }

    // Main Program
    public static void Main(string[] args)
    {
        Console.Clear();
        LibraryManagementSystem lms = new LibraryManagementSystem();
        lms.Run();
    }
}