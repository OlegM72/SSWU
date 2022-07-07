using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task13
{
    internal class Shop // Collection of cashiers
    {
        public int MaximumQueueSizeAllowed { get; set; }

        private List<Cashier> cashiers;

        public List<Cashier> Cashiers { get => cashiers; }

        public int CashiersCount => cashiers.Count;

        public Shop() : this(10, null) { } // open an empty shop

        public Shop(int maxQueue, List<Cashier>? cashiers)
        {
            this.cashiers = new List<Cashier>();
            if (cashiers is not null)
                this.cashiers.AddRange(cashiers); // deep copy
            MaximumQueueSizeAllowed = maxQueue;
        }

        internal static void Shop_QueueOverflow(object sender)
        // event handler for the case if a cashier is overqueued and should be paused
        {
            if (sender is not Cashier)
                return;
            if (!(sender as Cashier).IsPaused()) 
            {
                Program.Message($"*** Cashier #{(sender as Cashier).Number} is overflowed! ***", ConsoleColor.Yellow);
                (sender as Cashier).PauseOrResume();
            }
        }

        public void ExecuteShopProcess()       // processing the shop service
        {
            PersonGenerator generator = new();
            generator.WriteRandomGenerate(15); // generate 15 new clients and append them into persons database

            TimeCoordinator timeCoordinator = new(); // read the persons database (together with the the old persons)

            // start the process of service for those persons
            List<string> result = timeCoordinator.Coordinate(this);

            // output the service log
            Task13.ResultWriter resultWriter = new();
            resultWriter.WritePerson(result);
        }

        public void AddCashier(Cashier cashier)
        {
            if (cashiers is not null && cashier is not null)
                cashiers.Add(cashier);
        }

        bool CashierAvailable(Cashier cashier, Person person)
        // returns true if this cashier is not closed or paused and serves for this person status
        {
            return !cashier.IsClosed() && !cashier.IsPaused() &&
              (cashier.ServeStatus == PersonStatus.Ordinary || cashier.ServeStatus == person.Status);
        }

        public bool ThereAreAvailableCachiers(Person person)
        // find if there are cashiers which are not closed and available for this person status
        {
            foreach (Cashier cashier in cashiers)
                if (CashierAvailable(cashier, person))
                    return true;
            return false;
        }

        public int SelectBestCashier(Person currentPerson, int totalPeopleinShop)
        // selecting the best cashier for the current person
        // closed cashiers and unavailable for these person status are skipped
        {
            if (!ThereAreAvailableCachiers(currentPerson))
                return -1;

            // Хочу Вам показати, що я вже вмію використовувати лямбда-вирази, функції та предікати. :)
            // Звичайні методи пошуку могли би бути бистріше, я думаю, але в даному випадку це неважливо.
            // selector for Min method to find the shortest queue
            Func<Cashier, int> QueueSize =
                cashier => CashierAvailable(cashier, currentPerson) ? cashier.PersonsInQueue() : 100000;
            // condition to find the shortest queue among not closed cashiers, only cashiers available for this person are selected
            Predicate<Cashier> IsShortest = cashier => (QueueSize(cashier) == cashiers.Min(QueueSize));
            // selector for Min to find closest cashier
            Func<Cashier, double> Distance =
                cashier => CashierAvailable(cashier, currentPerson) ? cashier.Coordinate - currentPerson.Coordinate : 100000;
            // condition to find closest available cashier, we can also search it from the set of cashiers with the shortest queue
            Predicate<Cashier> IsClosest = cashier => (Distance(cashier) == cashiers.Min(Distance));

            int shortest = cashiers.FindIndex(IsShortest); // the cashier with the shortest queue
            if (shortest == -1) // all cashiers are not available
                return -1;
            return
                (CashiersCount * QueueSize(cashiers[shortest]) != totalPeopleinShop) // if not all queues are equally long
                ? shortest                                              // then take the one with shortest queue
                : cashiers.FindIndex(cashier => IsClosest(cashier));    // else take the closest one
        }

        public int FindCashierIndex(int cashierNumber)
        {
            int cashierIndex = cashiers.FindIndex(cashier => cashier.Number == cashierNumber);
            if (cashierIndex < 0)
                throw new IndexOutOfRangeException($"Could not find the cashier with the number {cashierNumber}");
            return cashierIndex;
        }

        public void PauseOrResumeCashier(int numToPauseOrResume)   // pause the given cashier (no new clients enqueued)
        {
            int cashierToPauseOrResume = FindCashierIndex(numToPauseOrResume);
            cashiers[cashierToPauseOrResume].PauseOrResume();
        }

        public bool EnqueueToBestCashier(Person person, int totalPeopleinShop)
        {
            if (!ThereAreAvailableCachiers(person)) {
                    Program.Message("No cashiers available!", ConsoleColor.Yellow);
                    return false;
                }
            // select the best cashier for the current person
            // closed cashiers and unavailable for these person status are skipped
            int bestCashier = SelectBestCashier(person, totalPeopleinShop);

            // put the current person to the queue of the best cashier
            cashiers[bestCashier].Enqueue(person);
            Program.Message(
                    $"*** Person {person} is moved to Cashier #{cashiers[bestCashier].Number} " +
                    $"(# {cashiers[bestCashier].PersonsInQueue()} in queue)", ConsoleColor.Yellow);
            return true;
        }

        public bool CloseOrReopenCashier(int numToCloseOrOpen, int totalPeopleinShop)
        // close the given cashier and move the queue to another cashiers
        // returns false if all cashiers are closed or not available
        {
            int cashierToCloseOrOpen = FindCashierIndex(numToCloseOrOpen);
            if (cashiers[cashierToCloseOrOpen].IsClosed())
            {
                cashiers[cashierToCloseOrOpen].Open();
                return true;
            }
            // close the cashier and take the persons from its queue
            List<Person> personsList = cashiers[cashierToCloseOrOpen].Close();

            foreach (Person person in personsList)
                if (!EnqueueToBestCashier(person, totalPeopleinShop))
                    return false;
            return true;
        }

        public void SetServeStatus(int cashierNumber, PersonStatus statusToServe) // open a cashier for serving only persons with this status
        {
            int cashierToChange = FindCashierIndex(cashierNumber);
            if (cashiers[cashierToChange].ServeStatus != statusToServe) {
                cashiers[cashierToChange].ServeStatus = statusToServe;
                Program.Message($"*** Cashier #{cashierNumber} is set to serve only persons with the status {statusToServe}! ***",
                    ConsoleColor.Yellow);
                cashiers[cashierToChange].Open();
            }
        }

        public void MovePersonsWithStatusTo(int cashierNumber, PersonStatus statusToServe, int totalPeopleinShop)
        // collect all existing persons with this status from other cashiers to this cashier
        // and move other persons from this cashier to other cashiers
        {
            int cashierToMoveTo = FindCashierIndex(cashierNumber);

            foreach (Cashier cashier in cashiers)
            {
                if (cashier.Number == cashierNumber) {
                    // move all persons except with statusToServe to other cashiers
                    // this cashier already accepts only this status
                    List<Person> personsToDistribute = cashier.DequeueOtherPersons(statusToServe);
                    foreach (Person person in personsToDistribute)
                        if (!EnqueueToBestCashier(person, totalPeopleinShop)) {
                        Program.Message($"Cannot redistibute persons from cashier #{cashierNumber}, other cashiers are not available",
                            ConsoleColor.Red);
                        return; // leave the situation as is, the process will be continued
                    }
                }
                else                                   // move al persons with statusToServe to this cashier
                    cashier.MoveStatusPersonsToCashier(cashiers[cashierToMoveTo], statusToServe);
            }
        }
    }
}
