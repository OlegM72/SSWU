using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task_15
{
    record Abiturient(int SchoolNumber, int YearEntered, string LastName);

    record Supplier(int Code, int BirthYear, string Street); 
    record SupplierDiscount(int Code, string ShopName, int DiscountPercent);

    internal class Linq_Study
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
                Print(">> Group with Key = " + group.Key + ": elements ");
                foreach (T item in group)
                    Print(item + " ");
                PrintLine();
            }
        }
        #endregion

        static void Main(string[] args)
        {
            #region Task 15.1
            // Дано послідовність цілих натуральних чисел і послідовність рядків stringList.
            // Отримайте нову послідовність рядків за таким правилом: для кожного значення n з порядкових номерів
            // виберіть рядок зі списку послідовності stringList, який починається з цифри та має довжина n.
            // Якщо в послідовності stringList є кілька необхідних рядків, поверніть перший;
            // якщо їх немає, тоді поверніть рядок «Не знайдено» (щоб вирішити ситуацію, пов’язану
            // з відсутністю необхідних рядків, використовуйте ?? операцію)

            int[] int_array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            string[] stringList = { "", "1", "02", "003", "0004", "0005", "a0006", "b007", "c08", "d09", "e10" };

            IEnumerable<string> result1 = int_array.Select(num => // for each number from int_array
                    stringList.FirstOrDefault(str =>              // search the first from all strings from stringList
                        str != "" &&
                        char.IsDigit(str[0]) &&
                        str.Length == num)      // FirstOrDefault
                    ?? "Не знайдено");          // Select

            PrintLine<string>("TASK 15.1:");
            Print("Int array: "); PrintList(int_array);
            Print("String array: \"\""); PrintList(stringList);
            Print("Result of extensions: "); PrintList<string>(result1); // PrintList(result1) also works, the type <T> is defined from the parameter
            // result: 1 02 003 0004 Не знайдено Не знайдено Не знайдено Не знайдено Не знайдено Не знайдено

            // the same as an expression
            result1 = from num in int_array
                      select stringList.FirstOrDefault(str =>
                          str != "" &&
                          char.IsDigit(str[0]) &&
                          str.Length == num)
                      ?? "Не знайдено";
            Print("Result of expression: "); PrintList(result1);         // same as above
            #endregion

            #region Task 15.2
            // 2.Задана послідовність непустих рядків stringList, що містять лише великі літери латинського алфавіту.
            // Для всіх рядків, що починаються з однієї і тієї ж літери, визначити їх сумарну довжину та отримати послідовність
            // рядків виду «літера - S», де S — сумарна довжина всіх рядків з stringList, які починаються з символу C.
            // Послідовність упорядкувати за зменшенням числових значень сум, а за рівних значення сум — за зростанням коду символу C.

            string[] stringList2 =
                {"ZERO", "UNO", "DOS", "TRES", "CUATRO", "CINCO", "SEIS", "SIETE", "OCHO", "NUEVE", "DIEZ",
                 "ONCE", "DOCE", "TRECE", "CATORCE", "QUINCE", "DIECISEIS", "DIECISIETE", "DIECIOCHO", "DIECINUEVE", "VEINTE" };

            var result2 = stringList2
                         .Where(str => str != "")
                         .GroupBy(str => str[0])
                         .Select(group => new { Letter = group.Key, TotalLength = group.Sum(str => str.Length) })
                         .OrderByDescending(item => item.TotalLength)
                         .ThenBy(item => item.Letter);
            // result2 is IOrderedEnumerable<'a>
            PrintLine("\r\nTASK 15.2:");
            Print("String array: "); PrintList(stringList2);
            Print("Result of extensions: "); PrintList(result2);
            // { Letter = D, TotalLength = 49 } { Letter = C, TotalLength = 18 } { Letter = S, TotalLength = 9 }
            // { Letter = T, TotalLength = 9 } { Letter = O, TotalLength = 8 } { Letter = Q, TotalLength = 6 }
            // { Letter = V, TotalLength = 6 } { Letter = N, TotalLength = 5 } { Letter = Z, TotalLength = 4 } { Letter = U, TotalLength = 3 }

            var result2_1 = from str in stringList2
                      where str != ""
                      group str by str[0] into _group
                        let totalLength = _group.Sum(str => str.Length)
                        orderby totalLength descending,
                                _group.Key
                        select new { Letter = _group.Key, TotalLength = totalLength };
            // result2 is IEnumerable<'a>
            Print("Result of expression: "); PrintList(result2_1); // same as above
            #endregion

            #region Task 15.3
            // 3.Задано послідовність даних про абітурієнтів nameList та послідовність цілих чисел року, що означають роки.
            // Кожен елемент послідовності nameList включає поля <Номер школи> < Рік вступу > < Прізвище >.
            // Потрібно отримати дані про кількість різних шкіл, які закінчили абітурієнти, для кожного року зі списку років.
            // Якщо у заданий рік вступу абітурієнтів із перерахованих шкіл немає, вказати в полі<Кількість шкіл> нуль.
            // Список результату має бути впорядкованим за зростанням кількості шкіл, а для значень, що збігаються, — за зростанням номера року.

            Abiturient[] nameList = {
                new(37, 1990, "Мельников"),
                new(121, 2020, "Буєвич"),
                new(100, 2020, "Толмачов"),
                new(17, 2019, "Босий"),
                new(11, 2019, "Павленко"),
                new(27, 2020, "Гагарин"),
                new(55, 2018, "Катайцев"),
                new(55, 2018, "Яловега")
            };

            int[] years = { 1990, 1995, 2000, 2005, 2010, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022 };

            var result3 = years.Select(
                           year => new {
                                    Year = year,
                                    SchoolsNumber = nameList
                                        .Where(abiturient => abiturient.YearEntered == year)
                                        .Distinct()
                                        .Count()
                                })
                                .OrderBy(item => item.SchoolsNumber)
                                .ThenBy(item => item.Year);
            // result3 is IOrderedEnumerable<'a>
            PrintLine("\r\nTASK 15.3:");
            Print("Abiturients array: "); PrintList(nameList);
            Print("Years array: "); PrintList(years);
            Print("Result of extensions: "); PrintList(result3);
            //  { Year = 1995, SchoolsNumber = 0 } { Year = 2000, SchoolsNumber = 0 } { Year = 2005, SchoolsNumber = 0 }
            //  { Year = 2010, SchoolsNumber = 0 } { Year = 2015, SchoolsNumber = 0 } { Year = 2016, SchoolsNumber = 0 }
            //  { Year = 2017, SchoolsNumber = 0 } { Year = 2021, SchoolsNumber = 0 } { Year = 2022, SchoolsNumber = 0 }
            //  { Year = 1990, SchoolsNumber = 1 } { Year = 2018, SchoolsNumber = 2 } { Year = 2019, SchoolsNumber = 2 } { Year = 2020, SchoolsNumber = 3 }

            var result3_1 = from year in years
                            let schoolsCountPerYear = nameList
                                .Where(abiturient => abiturient.YearEntered == year)
                                .Distinct()
                                .Count()
                            orderby schoolsCountPerYear, year
                            select new { Year = year, SchoolsNumber = schoolsCountPerYear };
            // result3_1 is IEnumerable<'a>
            Print("Result of expression: "); PrintList(result3_1); // same as above
            #endregion

            #region Task 15.4
            // 4. Задано послідовність відомостей про споживачів у вигляді списку типу Supplier
            // та послідовність знижок для споживачів у різних магазинах supplierDiscountList типу SupplierDiscount.
            // Кожен елемент послідовності supplierList включає поля <Код споживача>, <Рік народження>, <Вулиця проживання>. 
            // Кожен елемент послідовності supplierDiscountList включає поля <Код споживача>, <Назва магазину>, <Знижка(у відсотках)>
            // Отримати список (значення MaxDiscountOwner) всіх магазинів та для кожного з них споживача,
            // який має максимальну знижку у цьому магазині. Якщо для деякого магазину є кілька споживачів з максимальною знижкою,
            // то взяти дані про споживачеві з мінімальним кодом. Список впорядковувати за назвами магазинів алфавітному порядку за зростанням.

            Supplier[] supplierList = {
                new(37, 1970, "Хрещатик"),
                new(121, 2000, "Хрещатик"),
                new(100, 2000, "Перова"),
                new(17, 1999, "Микояна"),
                new(11, 1998, "Сiкорьского"),
                new(27, 1998, "Лесi Українки"),
                new(55, 1995, "Алматинська"),
                new(57, 2002, "Празька")
            };

            SupplierDiscount[] supplierDiscountList = {
                new(37, "Фора", 25),
                new(121, "Фора", 20),
                new(100, "Бiлла", 15),
                new(17, "Фора", 25),
                new(11, "Нова лiнiя", 10),
                new(27, "Нова лiнiя", 10),
                new(55, "Бiлла", 10),
                new(57, "Рошен", 5)
            };

            Func<string, int> MaxDiscountOwnerCode = shop => 
                                supplierDiscountList
                                .Where(dc => dc.ShopName == shop)
                                .Where(dc => dc.DiscountPercent == 
                                        supplierDiscountList.Where(dc => dc.ShopName == shop).Max(dc => dc.DiscountPercent))
                                .OrderBy(dc => dc.Code)
                                .Min(dc => dc.Code);
            
            var result4 = from discountRecord in supplierDiscountList
                          group (discountRecord.DiscountPercent, discountRecord.Code) by discountRecord.ShopName into _group
                          orderby _group.Key
                          select new {
                              Shop = _group.Key,
                              MaxDiscountOwner = $"Supplier " +
                                supplierList.First(s => s.Code == MaxDiscountOwnerCode(_group.Key)).Street ?? "is absent in database"
                          };
            PrintLine("\r\nTASK 15.4:");
            Print("Suppliers array: "); PrintList(supplierList);
            Print("SupplierDiscounts array: "); PrintList(supplierDiscountList);
            Print("Result of extensions: "); PrintList(result4);
            // { Shop = Бiлла, MaxDiscountOwner = Supplier Перова }
            // { Shop = Нова лiнiя, MaxDiscountOwner = Supplier Сiкорьского }
            // { Shop = Рошен, MaxDiscountOwner = Supplier Празька }
            // { Shop = Фора, MaxDiscountOwner = Supplier Микояна }
            #endregion

            #region Task 15.5
            // 5. Задано послідовність даних про абітурієнтів, що включає поля <Номер школи> <Рік вступу> <Прізвище>.
            // Створити словник, ключами якого будуть роки, що зустрічаються в першій послідовності, а значеннями -
            // список прізвищ абітурієнтів.

            Abiturient[] abiturients = {
                new(37, 1990, "Мельников"),
                new(121, 2020, "Буєвич"),
                new(100, 2020, "Толмачов"),
                new(17, 2019, "Босий"),
                new(11, 2019, "Павленко"),
                new(27, 2020, "Гагарин"),
                new(55, 2018, "Катайцев"),
                new(55, 2018, "Яловега")
            };

            Dictionary<int, IEnumerable<Abiturient>> dictionary = new();
            var result5 = abiturients.GroupBy(abiturient => abiturient.YearEntered)
                                     .Select(group => new { Year = group.Key, Abiturients = group })
                                     .OrderBy(item => item.Year);
            // result5 is IOrderedEnumerable<'a>
            // converting the result to dictionary
            foreach (var group in result5)
                dictionary[group.Year] = group.Abiturients;
            PrintLine("\r\nTASK 15.5:");
            Print("Abiturients array: "); PrintList(nameList);
            PrintLine("Result of extensions: ");
            // Printing dictionary
            foreach (KeyValuePair<int, IEnumerable<Abiturient>> item in dictionary)
            {
                Print(">> Key = " + item.Key + ", elements: ");
                foreach (Abiturient i in item.Value)
                    Print(i.ToString() + " ");
                PrintLine();
            }
            // >> Key = 1990, elements: Abiturient { SchoolNumber = 37, YearEntered = 1990, LastName = Мельников }
            // >> Key = 2018, elements: Abiturient { SchoolNumber = 55, YearEntered = 2018, LastName = Катайцев } Abiturient { SchoolNumber = 55, YearEntered = 2018, LastName = Яловега }
            // >> Key = 2019, elements: Abiturient { SchoolNumber = 17, YearEntered = 2019, LastName = Босий } Abiturient { SchoolNumber = 11, YearEntered = 2019, LastName = Павленко }
            // >> Key = 2020, elements: Abiturient { SchoolNumber = 121, YearEntered = 2020, LastName = Буєвич } Abiturient { SchoolNumber = 100, YearEntered = 2020, LastName = Толмачов } Abiturient { SchoolNumber = 27, YearEntered = 2020, LastName = Гагарин }

            // Method to print the group (result5) is:
            // foreach (var group in result5)
            // {
            //    Print(">> Group with Key = " + group.Year + ": elements ");
            //    foreach (var item in group.Abiturients)
            //        Print(item + " ");
            //    PrintLine();
            //  }

            var result5_1 = from abiturient in abiturients
                            group abiturient by abiturient.YearEntered into _group
                            orderby _group.Key
                            select new { Year = _group.Key, Abiturients = _group };
            // result5_1 is IEnumerable<'a>
            dictionary = new();
            foreach (var group in result5)
                dictionary[group.Year] = group.Abiturients;
            PrintLine("Result of expression: "); 
            foreach (KeyValuePair<int, IEnumerable<Abiturient>> item in dictionary)
            {
                Print(">> Key = " + item.Key + ", elements: ");
                foreach (Abiturient i in item.Value)
                    Print(i.ToString() + " ");
                PrintLine();
            }
            // the resutl is same as above
            #endregion
        }
    }
}
