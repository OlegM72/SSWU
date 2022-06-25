using System;
using System.Collections.Generic;

namespace Task_11
{
    internal class Comment
    {
        /*
        Завдання 11.1: Створити обгрунтування в текстовому форматі класу Storage та допоміжних сервісів, абстрактних класів 
        та інтерфейсів. Здійснити обдумане проєктування.

        Я бачу реалізацію цього проекту таким чином.
        1. Потрібно мати класи користувачів двох типів: відвідувач та адміністратор. Для взаємодії з користувачем 
        створюється база даних, що містить зареєстровані імена та паролі адміністраторів (у першій версії можна їх 
        задати простим текстом у файлі або навіть у самій програмі константами), і клас типу MenuService, що містить 
        діалоги вводу-виводу та звертається до методів класу Storage, доступних поточному користувачеві, а також 
        метод для входу з правами адміністратора, де вводиться пароль, користувач шукається в базі, пароль перевіряється 
        та надається доступ до методів адміністратора, або користувач залишається звичайним відвідувачем. Звичайний 
        відвідувач - клас User - має доступ до методів, що працюють тільки з виводом списку продуктів, покупок і т.п.
        Клас User можна зробити абстрактним, але не обов'язково, оскільки Admin теж має всі привілеї користувача, і його 
        можна успадкувати від User. Клас Admin повинен містити методи, що працюють з введенням, перевіркою помилок при 
        введенні, виведенням продуктів із терміном придатності, що минув, і зміною їх ціни (уцінкою). Реалізацію взаємодії з
        користувачем я бачу у вигляді меню, я таке вже реалізував в завданні про ресторанні меню, тут можна зробити таке саме.

        2. private class Product: поля name, price, weight. Містить визначення методу public virtual void PriceChange зміни 
        ціни на задану кількість відсотків та public override string ToString, а також методів доступу до приватних полів класу.
        Реалізація його у вигляді interface недоречна, я вважаю, так як interface неможливо використовувати у типизованих списках.
        3. public class UsualProduct : Product - продукт, що "не зношується", для якого не важливий термін придатності 
        (наприклад, кухонні ножі та мікрохвильові печі). Реалізує методи інтерфейсу Product. Перевизначає методи GetHashCode, 
        ToString, PriceChange та методи доступу до приватних полів класу. 
        4. public class SortedProduct : Product - продукт, що "не зношується", для якого відомий сорт (вищий, перший, другий). 
        Наприклад, борошно. Реалізує методи інтерфейсу Product і метод визначення сорту продукта.
        5. public class Dairy_product : SortedProduct - продукт з терміном придатності, який має метод уцінки, порівняння 
        поточної дати з терміном придатності, свій ToString(). Сорт продукту можна не використовувати, якщо його немає 
        (вказати сорт = 0). Перевизначає методи GetHashCode, ToString, PriceChange та методи доступу до приватних полів класу.
        6. public class Meat : Dairy_product - м'ясопродукт із зазначенням сорту та типу м'яса. Потрібен для встановлення 
        вищої ціни в залежності від типу м'яса, а також для пошуку за типом м'яса. Перевизначає методи GetHashCode, ToString, 
        PriceChange та методи доступу до приватних полів класу.
        7. internal class ProductComparer : IComparer<Product> - для визначення методу порівняння продуктів (за хешкодом або 
        іншим необхідним способом, в хешкод потрібно включати тільки поля, за якими потрібно порівнювати продукти для включення 
        в відсортований список або в колекцію типу Dictionary).

        8. public class Storage містить список продуктів, методи роботи з ним (наприклад, сортування, поєднання з іншим, введення 
        з файлу та виведення у файл), метод ToString, а також методи доступу до приватних полів класу GetTotalPrice, GetTotalWeight.

        9. У функції Main повинен здійснюватися виклик методу класу MenuService, у якому вестиметься діалог з користувачем.
        Також у Main перехоплюються винятки та виводяться на екран (або в лог-файл) критичні помилки, за яких програма не може 
        продовжувати роботу. Інші винятки повинні по можливості перехоплюватися на рівні тих методів, де вони з'являються, 
        та пізніше оброблюватись у діалозі з користувачем.
        */
    }
    /*
    ===============================================
    Завдання 11.2: Створити узагальнення класу з можливістю передачі типів, як параметрів.

    Приклад виконання цього завдання див. нижче.
    */
    internal class ProductList<T>
    {
        // ми можемо використовувати цей клас, наприклад, для зберігання та сортування списку з продуктів
        // будь-якого одного типу або одного типу Product, до якого приводяться будь-які продукти
        List<T> products;

        public ProductList()
        {
            products = new List<T>();
        }

        public T this[int index]
        {
            get
            {
                if (index >= 0 && index < products.Count)
                    return products[index];
                else throw new IndexOutOfRangeException("Wrong product number");
            }
            set
            {
                if (index >= 0 && index < products.Count)
                    products[index] = value;
            }
        }

        public int GetCount() => products.Count;

        public void Add(T item) => products.Add(item);

        public void Clear() => products.Clear();

        public bool Contains(T item) => products.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => products.CopyTo(array, arrayIndex);

        IEnumerator<T> GetEnumerator() => products.GetEnumerator();

        public void Sort() => products.Sort();  // needs to implement CompareTo or define GetHashCode for each Product type

        public int IndexOf(T item) => products.IndexOf(item);

        public void Insert(int index, T item) => products.Insert(index, item);

        public bool Remove(T item) => products.Remove(item);

        public void RemoveAt(int index) => products.RemoveAt(index);

        // similarly, we can add here other methods like AddRange, Exists, etc. Everything that we will use.

        public override string ToString() // products.ToString() not implemented, so we need to implement it
        {
            string result = "";
            foreach (T item in products)
                result += item.ToString() + " ";
            return result;
        }
    }

    internal class Program
    {
        static void Main()
        {
            // An example of using the ProductList<T>:
            ProductList<int> ints = new();
            ints.Add(3);
            ints.Add(1);
            ints.Add(4);
            ints.Add(2);
            ints.Add(5);
            ints.Sort(); // sorting standard integers is implemented already
            Console.WriteLine(ints);  // 1 2 3 4 5

            ProductList<string> strs = new();
            strs.Add("C");
            strs.Add("A");
            strs.Add("D");
            strs.Add("B");
            strs.Add("E");
            strs.Sort(); // sorting standard strings is implemented already
            Console.WriteLine(strs); // A B C D E
        }
    }
}