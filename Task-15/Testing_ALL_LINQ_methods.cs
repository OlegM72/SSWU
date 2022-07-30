using System;
using System.Collections.Generic;
using System.Linq;

namespace Testing
{
    #region Auxiliary Classes
    public class Book
    {
        public string Title; public string Author; public int Year;
        public Book(string title, string author, int num) { Title = title; Author = author; Year = num; }
        public override string ToString() { return $"({Title} by {Author}, {Year})"; }
    };
    public class NameYear
    {
        public string Name; public int Year;
        public override string ToString() { return $"({Name}, {Year})"; }
    }

    // or (without methods): record NameYear(string Title, int Year);

    public class Phone
    {
        public string Title; public string Company;
        public Phone(string title, string company) { Title = title; Company = company; }
        public override string ToString() { return $"{Title} by {Company}"; }
    }

    public class TitleOwner
    {
        public string Title; public string Owner;
        public TitleOwner(string title, string owner) { Title = title; Owner = owner; }
        public override string ToString() { return $"({Title} owned by {Owner})"; }
    }
    public class TitleOwnerYear
    {
        public string Title; public string Owner; public int Year;
        public TitleOwnerYear(string title, string owner, int year) { Title = title; Owner = owner; Year = year; }
        public override string ToString() { return $"({Title} ({Year}) owned by {Owner})"; }
    }
    #endregion

    internal class Linq_Testing
    {
        #region Auxiliary Methods
        static void PrintLine<T>(T obj)
        {
            Console.WriteLine(obj);
        }
        static void Print<T>(T obj)
        {
            Console.Write(obj);
        }
        static void PrintLine()
        {
            PrintLine("");
        }
        static void Print()
        {
            Print("");
        }
        static void PrintList<T>(IEnumerable<T> list)
        {
            foreach (T item in list) Print(item + " ");
            PrintLine();
        }
        static void PrintGroup<G, T>(IEnumerable<IGrouping<G, T>> list)
        {
            foreach (IGrouping<G, T> group in list)
            {
                Print(">> Group with Key = " + group.Key + ", elements: ");
                foreach (T item in group)
                    Print(item + " ");
                PrintLine();
            }
        }
        #endregion

        static void Main_Test(string[] args)
        {
            #region All Methods

            int[] int_array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            (int, char)[] two_keys_array = { (1, 'a'), (1, 'b'), (2, 'c'), (2, 'd'), (3, 'e') };
            char[] char_array = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j' };
            string[] string_array = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" };

            IEnumerable<int> int_list = int_array
                .Where(n => n <= 5); // choose ints that <= 5, return to new list
            Print("WHERE n <= 5: ");
            PrintList<int>(int_list);                // 1 2 3 4 5
                                                     // PrintList(int_list) also works, the type <T> is defined from the parameter
            int_list = from n in int_array
                       where n <= 5
                       select n; // choose ints that <= 5, return to new list
            Print("where n <= 5: ");
            PrintList<int>(int_list);                // 1 2 3 4 5
            // PrintList(int_list) also works, the type <T> is defined from the parameter

            IEnumerable<bool> bool_list = int_list
                .Select(n => n > 2); // check for each element if it's > 2
            Print("SELECT n > 2: ");
            PrintList(bool_list);             // False False True True True

            bool_list = from n in int_list
                        select (n > 2); // check for each element if it's > 2
            Print("select n > 2: ");
            PrintList(bool_list);             // False False True True True

            Func<int, double> reverse = (n) => 1 / (double)n;

            IEnumerable<int> int_ordered = int_array
                .OrderBy(reverse); // sort by 1/n, return to new list
            Print("ORDER BY 1/n: ");
            PrintList(int_ordered);            // 10 9 8 7 6 5 4 3 2 1

            int_ordered = from n in int_array
                          orderby 1 / (double)n
                          select n; // sort by 1/n, return to new list
            Print("orderby 1/n: ");
            PrintList(int_ordered);            // 10 9 8 7 6 5 4 3 2 1

            IEnumerable<int> int_ordered_desc = int_list
                .OrderByDescending(n => n); // sort down by n, return to new list
            Print("ORDER BY DESCENDING n: ");
            PrintList(int_ordered_desc);       // 5 4 3 2 1

            int_ordered_desc = from n in int_list
                               orderby n descending
                               select n; // sort down by n, return to new list
            Print("orderby n descending: ");
            PrintList(int_ordered_desc);       // 5 4 3 2 1

            // Does not work with only a single key (n)
            int_ordered = int_array
                .OrderBy(reverse) // sort by 1/n    // 10 9 8 7 6 5 4 3 2 1
                .ThenBy(reverse); // sort the result by 1/n, return to new list
            Print("ORDER BY 1/n THEN BY (DESCENDING) 1/n - does not work with only a single key (n)): ");
            PrintList(int_ordered);            // 10 9 8 7 6 5 4 3 2 1

            int_ordered = from n in int_array
                          orderby 1 / (double)n // sort by 1/n    // 10 9 8 7 6 5 4 3 2 1
                          orderby 1 / (double)n
                          select n; // sort the result by 1/n, return to new list
            Print("orderby 1/n then again orderby 1/n - does not work with only a single key (n): ");
            PrintList(int_ordered);            // 10 9 8 7 6 5 4 3 2 1

            IEnumerable<int> int_reversed = int_ordered
                .Reverse();
            Print("REVERSE: ");
            PrintList(int_reversed);           // 1 2 3 4 5 6 7 8 9 10

            // there is no reverse keyword

            // with two or more keys only
            IEnumerable<(int, char)> two_keys_ordered = two_keys_array
                .OrderBy(item => -item.Item1) // sort down by ints
                .ThenBy(item => item.Item2); // sort up the groups of ints by chars, return to new list
            Print("ORDER BY -Item1 THEN BY Item2: ");
            PrintList(two_keys_ordered);       // (3, e) (2, c) (2, d) (1, a) (1, b)

            // there is no thenby keyword

            two_keys_ordered = from tuplet in two_keys_array
                               orderby -tuplet.Item1 // sort down by ints
                               orderby tuplet.Item2 // sort up by chars
                               select tuplet;
            Print("orderby -Item1 and orderby Item2 = simply by Item2: ");
            PrintList(two_keys_ordered);       // (1, a) (1, b) (2, c) (2, d) (3, e) 

            // with two or more keys only
            two_keys_ordered = two_keys_array
                .OrderBy(item => -item.Item1) // sort down by ints
                .ThenByDescending(item => item.Item2); // sort down the groups of ints by chars, return to new list
            Print("ORDER BY -Item1 THEN BY DESCENDING Item2: ");
            PrintList(two_keys_ordered);       // (3, e) (2, d) (2, c) (1, b) (1, a)

            two_keys_ordered = from tuplet in two_keys_array
                               orderby -tuplet.Item1 // sort down by ints
                               orderby tuplet.Item2 descending // sort down by chars
                               select tuplet;
            Print("orderby -Item1 and descending Item2 = simply by Item2 down: ");
            PrintList(two_keys_ordered);       // (3, e) (2, d) (2, c) (1, b) (1, a)

            IEnumerable<int> int_joined = int_array.Join(int_ordered, 
                first => first/3,   // outer key = int_array[i]
                second => second/3, // inner key = int_ordered[i] / 2
                (a, b) => a * b);   // search key1[i] among key2[*], add the product of corresponding elements if equal
            Print("JOIN: ");
            PrintList(int_joined);
            // the first list is 1 2 3 4 5 6 7 8 9 10, the second is 10 9 8 7 6 5 4 3 2 1
            // keys first, second: 1 4 9 16 25 36 49 64 81 100 --> 1*1, 2*2, ...
            // keys first, second/2: 3 2 10 8 21 18 36 32 50 --> 1 * (3 2), 2 * (5 4), 3 * (7 6), 4 * (9 8), 5 * 10
            // keys first/2, second/2: 1 6 4 9 6 20 16 25 20 42 36 49 42 72 64 81 72 100 --> 1*1, (2 3) * (3 2), (4 5) * (5 4), (6 7) * (7 6), (8 9) * (9 8), 10*10
            // keys first/3, second/3: 2 1 4 2 15 12 9 20 16 12 25 20 15 48 42 36 56 49 42 64 56 48 90 81 100 90 --> (1 2) * (2 1), (3 4 5) * (5 4 3), (6 7 8) * (8 7 6), (9 10) * (10 9)

            int_joined = from n in int_array
                         join m in int_ordered on n/3 equals m/3
                         select n * m;
            Print("join: ");
            PrintList(int_joined); // the same as above

            // List<int> int_group = int_ordered.ToList();
            var int_group_joined = int_array.GroupJoin(int_ordered,
                first => first / 3,     // outer key = int_array[i]
                second => second / 3,   // inner key = int_ordered[i] / 2
                (first, key_group) =>   // key_group is IEnumerable<int>, contains all seconds for this first
                    new { Key = first, Products = key_group.Select(second => second * first) });
            // int_group_joined is IEnumerable<'a>
            PrintLine("GROUPJOIN: ");
            foreach (var item in int_group_joined)
            {   Print(">> Key: "); Print(item.Key); Print(", elements: ");
                PrintList(item.Products); }
            // >> Key: 1, elements: 2 1
            // >> Key: 2, elements: 4 2
            // >> Key: 3, elements: 15 12 9
            // >> Key: 4, elements: 20 16 12
            // >> Key: 5, elements: 25 20 15
            // >> Key: 6, elements: 48 42 36
            // >> Key: 7, elements: 56 49 42
            // >> Key: 8, elements: 64 56 48
            // >> Key: 9, elements: 90 81
            // >> Key: 10, elements: 100 90

            int_group_joined = from n in int_array
                               join m in int_ordered on n / 3 equals m / 3 into key_group
                                   // select n * item;   --> 2 1 4 2 15 12 9 20 16 12 25 20 15 48 42 36 56 49 42 64 56 48 90 81 100 90 (the same as for join)
                               select new { Key = n, Products = key_group.Select(second => second * n) }; // maybe item is needed somewhere here :)
            PrintLine("join into: ");
            foreach (var item in int_group_joined)
            {
                Print(">> Key: "); Print(item.Key); Print(", elements: ");
                PrintList(item.Products);
            }   // the same as above

            IEnumerable<int> int_concat = int_array.Concat(int_ordered);
            Print("CONCAT: ");
            PrintList(int_concat);              // 1 2 3 4 5 6 7 8 9 10 10 9 8 7 6 5 4 3 2 1

            // concat keyword is absent
                        
            Print("AGGREGATE (SUM - 10): ");
            PrintLine(int_concat.Aggregate(-10, (total, n) => total + n));  // 100 = SUM - 10

            Print("AGGREGATE (SUM n*2 from the second element): ");
            PrintLine(int_concat.Aggregate((total, n) => total + n*2));     // 219 = SUM * 2 - 1 (the first element is not included to n)
            
            // aggregate keyword is absent

            IEnumerable<(int, int)> int_zip = int_array.Zip(int_ordered);
            Print("ZIP: ");
            PrintList(int_zip);              // (1, 10) (2, 9) (3, 8) (4, 7) (5, 6) (6, 5) (7, 4) (8, 3) (9, 2) (10, 1)

            int_zip = int_array.Zip(int_ordered, (a, b) => (-b, -a));
            Print("ZIP (-b, -a): ");
            PrintList(int_zip);              // (-10, -1) (-9, -2) (-8, -3) (-7, -4) (-6, -5) (-5, -6) (-4, -7) (-3, -8) (-2, -9) (-1, -10)

            IEnumerable<int> int_zip_func = int_array.Zip(int_ordered, (a, b) => a * b);
            Print("ZIP (a * b): ");
            PrintList(int_zip_func);              // 10 18 24 28 30 30 28 24 18 10

            // zip keyword is absent

            IEnumerable<int> int_union = int_array.Union(int_ordered);
            Print("UNION: ");
            PrintList(int_union);              // 1 2 3 4 5 6 7 8 9 10

            // union keyword is absent

            IEnumerable<int> int_except = int_array.Except(int_ordered_desc);
            Print("EXCEPT: ");
            PrintList(int_except);              // 6 7 8 9 10

            // except keyword is absent

            IEnumerable<int> int_intersect = int_joined.Intersect(int_concat);
            Print("INTERSECT: ");
            PrintList(int_intersect);              // 2 1 4 9

            // intersect keyword is absent

            IEnumerable<int> int_distinct = int_joined.Distinct();
            Print("DISTINCT: ");
            PrintList(int_distinct);              // 2 1 4 15 12 9 20 16 25 48 42 36 56 49 64 90 81 100

            // distinct keyword is absent
            
            IEnumerable<int> int_take = int_distinct.Take(10);
            Print("TAKE(10): ");
            PrintList(int_take);                  // 2 1 4 15 12 9 20 16 25 48

            // take keyword is absent
            
            IEnumerable<int> int_skip = int_distinct.Skip(10);
            Print("SKIP(10): ");
            PrintList(int_skip);                  // 42 36 56 49 64 90 81 100

            // skip keyword is absent

            IEnumerable<int> int_take_while = int_distinct.TakeWhile(n => n < 45);
            Print("TAKEWHILE < 45: ");
            PrintList(int_take_while);            // 2 1 4 15 12 9 20 16 25

            // takewhile keyword is absent

            IEnumerable<int> int_skip_while = int_distinct.SkipWhile(n => n < 45);
            Print("SKIPWHILE < 45: ");
            PrintList(int_skip_while);            // 48 42 36 56 49 64 90 81 100   --- though not all elements >= 45 but the first is > 45

            // skipwhile keyword is absent

            IEnumerable<IGrouping<int, int>> int_grouped = int_array
                .GroupBy(item => item / 3); // sort down by ints
            PrintLine("GROUPBY n/3: ");
            PrintGroup(int_grouped);
            // >> Group with Key = 0: elements 1 2
            // >> Group with Key = 1: elements 3 4 5
            // >> Group with Key = 2: elements 6 7 8
            // >> Group with Key = 3: elements 9 10

            int_grouped = from n in int_array
                          group n by n / 3; // sort down by ints
            PrintLine("group n by n/3: ");
            PrintGroup(int_grouped);        // same as above

            ILookup<int, int> int_grouped_lookup = int_array
                .ToLookup(item => item / 3); // sort down by ints
            PrintLine("TOLOOKUP n/3: ");
            PrintGroup(int_grouped_lookup);       // the same as with the GROUPBY command

            // lookup or toolokup keywords are absent

            // CompareTo: not Linq
            Print("ALL > \"0\": ");
            PrintLine(string_array
                .All(str => (str.CompareTo("0") > 0))); // all strings > "0"?    True

            // all keyword is absent but it is done by select

            Print("ANY > 10: ");
            PrintLine(int_array
                .Any(i => (i > 10))); // is there any int > 10?    False

            // Contains: not Linq
            Print("Contains 10 (not Linq): ");
            PrintLine(int_array
                .Contains(10)); // is there 10?    True

            // all other keywords are absent
            
            Print("FIRST n > 10: ");
            try { PrintLine(int_array
                .First(n => (n > 10)));
            } // find the first int > 10 -> System.InvalidOperationException: Sequence contains no matching element
            catch (Exception ex) { PrintLine(ex.GetType() + ": " + ex.Message); }

            Print("FIRST OR DEFAULT n > 10: ");
            PrintLine(int_array
                .FirstOrDefault(n => (n > 10))); // is there any int > 10?   Default = 0 (if "i > 9" returns 10)

            Print("LAST n > 10: ");
            try { PrintLine(int_array
                .Last(n => (n > 10)));
            } // find the last int > 10 -> System.InvalidOperationException: Sequence contains no matching element
            catch (Exception ex) { PrintLine(ex.GetType() + ": " + ex.Message); }

            Print("LAST OR DEFAULT n > 10: ");
            PrintLine(int_array
                .LastOrDefault(n => (n > 10))); // is there any int > 10?   Default = 0 (if "i > 9" returns 10)

            Print("ELEMENT AT [10]: ");
            try { PrintLine(int_array
                .ElementAt(10));
            } // System.ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index') 
            catch (Exception ex) { PrintLine(ex.GetType() + ": " + ex.Message); }

            Print("ELEMENT AT OR DEFAULT [10]: ");
            PrintLine(int_array
                .ElementAtOrDefault(10)); // 0
            
            Print("SINGLE: ");
            try { PrintLine(int_array
               .Single());
            } // System.InvalidOperationException: Sequence contains more than one element
            catch (Exception ex) { PrintLine(ex.GetType() + ": " + ex.Message); }

            Print("SINGLE OR DEFAULT: ");
            try { PrintLine(int_array
                .SingleOrDefault());
            } // System.InvalidOperationException: Sequence contains more than one element
            catch (Exception ex) { PrintLine(ex.GetType() + ": " + ex.Message); }

            Print("COUNT, AVERAGE, MAX, MIN, SUM of ints 1..10: ");
            Print(int_array.
                Count() + " "); // 10
            Print(int_array.
                Average() + " "); // 5.5
            Print(int_array.
                Max() + " "); // 10
            Print(int_array.
                Min() + " "); // 1
            PrintLine(int_array.
                Sum()); // 55

            #endregion

            #region FromLecture

            Print("EXAMPLES FROM THE LECTURE");

            List <Book> lib = new() {
                new("C# 10", "Author First", 2021),
                new("C# 11", "Author Second", 2022),
                new("Linq", "Author Third", 2000)
            };

            IEnumerable<string> query_12 = from item in lib
                                           let names = item.Author.Split(' ')
                                           from name in names
                                           orderby name.Length
                                           select name;
            Print("let (query_12): "); PrintList(query_12); // First Third Author Author Second Author

            IEnumerable<string> query_1 = from item in lib select item.Title;
            Print("select Title (query_1): "); PrintList(query_1); // C# 10 C# 11 Linq

            IEnumerable<NameYear> query_2 = from item in lib select new NameYear { Name = item.Title, Year = item.Year };
            Print("select NameYear (query_2): "); PrintList(query_2); // (C# 10, 2021) (C# 11, 2022) (Linq, 2000)

            // the same with anonymous type
            var query_2a = from item in lib select new{ Name = item.Title, Year = item.Year };
            Print("select NameYear (query_2 anonymous): "); PrintList(query_2a); //  { Title = C# 10, Year = 2021 } { Title = C# 11, Year = 2022 } { Title = Linq, Year = 2000 }

            IEnumerable<Book> query_2t = lib.Where(x => x.Title.Contains("C#"));
            Print("Where Contains C# (query_2t): "); PrintList(query_2t); // (C# 10 by Author First, 2021) (C# 11 by Author Second, 2022)

            Func<Book, bool> selector_by_title = book => book.Title.Contains("C#");
            IEnumerable<Book> query_3 = lib.Where(selector_by_title);
            Print("Where selector by title C# (query_3): "); PrintList(query_3); // (C# 10 by Author First, 2021) (C# 11 by Author Second, 2022)

            IEnumerable<Book> query_6 = from item in lib 
                                        orderby item.Title descending, 
                                                item.Year 
                                        select item;
            Print("orderby title descending then year (query_6): "); PrintList(query_6);
            // (Linq by Author Third, 2000) (C# 11 by Author Second, 2022) (C# 10 by Author First, 2021)

            IEnumerable<IGrouping<char, Book>> query_8 = from item in lib 
                                                         group item by item.Title[0];
            PrintLine("group by title[0] (query_8): "); PrintGroup(query_8);
            // >> Group with Key = C: elements (C# 10 by Author First, 2021) (C# 11 by Author Second, 2022)
            // >> Group with Key = L: elements (Linq by Author Third, 2000)

            IEnumerable<IGrouping<int, string>> query_9 = from item in lib 
                                                          group item.Title by item.Title.Length;
            PrintLine("group by title length (query_9): "); PrintGroup(query_9);
            // >> Group with Key = 5: elements C# 10 C# 11
            // >> Group with Key = 4: elements Linq

            IEnumerable<IGrouping<NameYear, Book>> query_10 = from item in lib 
                                                              group item by new NameYear{ Name = item.Title, Year = item.Year };
            PrintLine("group by NameYear type (query_10): "); PrintGroup(query_10);
            // >> Group with Key = (C# 10, 2021): elements (C# 10 by Author First, 2021)
            // >> Group with Key = (C# 11, 2022): elements (C# 11 by Author Second, 2022)
            // >> Group with Key = (Linq, 2000): elements(Linq by Author Third, 2000)

            var query_10a = from item in lib
                            group item by new { key1 = item.Title, key2 = item.Year };
            PrintLine("group by anonimous NameYear type (query_10a): "); PrintGroup(query_10a);
            // query_10a = IEnumerable<IGrouping<'a, Book>> 
            // >> Group with Key = { key1 = C# 10, key2 = 2021 }: elements (C# 10 by Author First, 2021)
            // >> Group with Key = { key1 = C# 11, key2 = 2022 }: elements (C# 11 by Author Second, 2022)
            // >> Group with Key = { key1 = Linq, key2 = 2000 }: elements(Linq by Author Third, 2000)

            List<TitleOwner> owners = new(){
                new( "C# 10", "Owner 1" ),
                new( "C# 10", "Owner 2" ),
                new( "C# 11", "Owner 3" ),
                new( "C# 11", "Owner 2" ),
                new( "Linq", "Owner 4" )
            };

            // Join is equivalent to join … in … on … equals …
            // з'єднує ДВІ послідовності на основі функції вибору ключа та витягує пари значень. 
            IEnumerable<string> join_query = from book in lib
                                             join owner in owners
                                             on book.Title equals owner.Title
                                             select $"({book.Title} ({book.Year}) owned by {owner.Owner})";
                                             // = select new TitleOwnerYear(book.Title, owner.Owner, book.Year);
            PrintLine("join by owner names, select books (join_query): "); PrintList(join_query);
            // (C# 10 (2021) owned by Owner 1) (C# 10 (2021) owned by Owner 2) (C# 11 (2022) owned by Owner 3) (C# 11 (2022) owned by Owner 2) (Linq (2000) owned by Owner 4)

            // GroupJoin is equivalent to join … in … on … equals … into …
            // з'єднує дві послідовності на основі функції вибору ключа та групує отримані збіги для кожного елемента. 
            IEnumerable<TitleOwnerYear> join_into = from book in lib
                                                    join owner in owners 
                                                    on book.Title equals owner.Title into ownerGroups
                                                    from ownerGroup in ownerGroups   // ownerGroups is IEnumerable<TitleOwner>
                                                    select new TitleOwnerYear(book.Title, ownerGroup.Owner, book.Year);
            PrintLine("groupjoin (join into): "); PrintList(join_into);  // the result is the same as above

            List<Phone> phones = new List<Phone> {
                            new Phone("Lumia 430", "Microsoft"),
                            new Phone("Mi 5", "Xiaomi"),
                            new Phone("G3", "LG"),
                            new Phone("iPhone 5", "Apple"),
                            new Phone("Lumia 930", "Microsoft"),
                            new Phone("iPhone 6", "Apple"),
                            new Phone("Lumia 630", "Microsoft"),
                            new Phone("G4", "LG")
                            };

            IEnumerable<IGrouping<string, Phone>> phoneGroups = from phone in phones
                                                                group phone by phone.Company;
            PrintLine("group by phone.Company: "); PrintGroup(phoneGroups);
            // >> Group with Key = Microsoft: elements Lumia 430 by Microsoft Lumia 930 by Microsoft Lumia 630 by Microsoft
            // >> Group with Key = Xiaomi: elements Mi 5 by Xiaomi
            // >> Group with Key = LG: elements G3 by LG G4 by LG
            // >> Group with Key = Apple: elements iPhone 5 by Apple iPhone 6 by Apple
            
            var phoneGroups2 = from phone in phones
                               group phone by phone.Company into _group  // _group is IGrouping<string, Phone>
                               select new { Title = _group.Key, Count = _group.Count() };
            // phoneGroups2 is IEnumerable<'a> 
            Print("group into by phone.Company: "); PrintList(phoneGroups2);
            // { Title = Microsoft, Count = 3 }
            // { Title = Xiaomi, Count = 1 }
            // { Title = LG, Count = 2 }
            // { Title = Apple, Count = 2 }

            int[] numbers = { 2, 3, 4 };
            Print("Aggregate, (total, n) => total + n*n: "); 
            PrintLine(numbers.Aggregate((total, n) => total + n * n)); // 27 = sum of squares - 2
            
            Print("Select n*n, Aggregate, (total, n) => total + n: ");
            PrintLine(numbers.Select(n => n * n).Aggregate((total, n) => total + n)); // 29 = sum of squares

            #endregion
        }
    }
}
