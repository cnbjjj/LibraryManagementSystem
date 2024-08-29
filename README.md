# LibraryManagementSyste

A simple c# console practice focusing on data strucutre and algorithm implementation by mimicking a library management system.

## Data Structures

- **Structs `Book`, `BookQuery`, `Member`, and `BorrowingRecord`** are implemented as structs because they represent simple data entities that are immutable and can be compared easily.
  - **Book**: Represents a book entity with attributes such as `ISBN`, `Title`, `Author`, `Year`, `Genre`, and `Quantity`. Each book has a unique, self-incremented ID managed using a static counter.
  - **Member**: Represents a user entity with attributes like `Name` and a list of borrowed books. Each member is assigned a unique, self-incremented ID.
  - **BorrowingRecord**: Represents a borrowing record, capturing the relationship between a `Member` and a `Book`. It includes attributes like `Borrow Date`, `Due Date`, and `Returned` status.
  - **BookQuery**: Represents search criteria for books, making it easy to extend with new search parameters.

- **Enums** are used to define menu options clearly, enhancing code readability and maintainability.
  - **ELMSChoice**: Represents the main actions users can perform, such as adding books, viewing books, searching for books, borrowing books, returning books, and exiting the system.
  - **ELMSSearchChoice**: Defines search options for books, including by `ID`, `Title`, and `Author`, facilitating easy extension for additional search criteria.

- **`List<T>` collection** is used for dynamically storing books, members, and borrowing records, allowing flexible sizing and easy iteration over the collections.
  - **Books**: `List<Book>` stores the collection of books.
  - **Members**: `List<Member>` maintains the collection of library members.
  - **Borrowing Records**: `List<BorrowingRecord>` holds the records of all book borrowings.

## Algorithms Implemented

- **Binary Search**: Used for quickly finding books by their `ID`, which is efficient and reduces the search time compared to linear search.
- **Linear Search**: Used for searching books by `Title` and `Author` due to the flexibility of partial and case-insensitive matches.

## Future Development

- **Member Management**: Implement functionalities for adding, removing, and viewing members to provide a more complete system.
- **Book Reservation**: Add features for reserving books that are currently not available.
- **Extended Search**: Enhance the `BookQuery` struct to support more sophisticated search queries, including by publication year or genre.
