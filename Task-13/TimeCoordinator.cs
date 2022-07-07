using System;
using System.Collections.Generic;
using System.Threading;

namespace Task13
{
    internal class TimeCoordinator
    {
        int timeInterval = 3;   // time interval between the clients entered
        bool isProcess = true;  // the flag shows that the shop is in service now

        void StopTheProcess()
        {
            isProcess = false;
            Program.Message("The service process has stopped.", ConsoleColor.Yellow);
        }

        public List<string> Coordinate(Shop shop)
        {
            Random random = new Random();
            int enteredCounter = 0; // number of persons entered
            int servedCounter = 0;  // number of persons served
            // flag that shows the process pause for each cashier, so we check the queue size to resume
            bool[] wasQueueOverflow = new bool[shop.CashiersCount];

            int time = 0;
            PersonGenerator personGenerator = new PersonGenerator();
            List<Person> persons = personGenerator.Generate(); // read the persons database
            List<string> result = new();
            Program.Message(
                $"Press 1..{shop.CashiersCount} to close the corresponding cashier and move its queue to another cashier\r\n" +
                $"Press 1..{shop.CashiersCount} again to reopen the corresponding cashier\r\n" +
                $"Alt + 1..{shop.CashiersCount} pauses / resumes the cashier for clients instead of closing / reopening it\r\n" +
                "Press X to close all cashiers and exit, U to release cashier #1 for serving only disabled people\r\n" +
                "\r\n[Time, sec] Cashier #: Person description (Current number of people IN or OUT)");

            while (isProcess)
            {
                // enter new person each timeInterval seconds
                if (time % timeInterval == 0 && enteredCounter < persons.Count)
                {
                    Person currentPerson = persons[enteredCounter];

                    if (!shop.ThereAreAvailableCachiers(currentPerson))
                    {
                        Program.Message("No cashiers available!", ConsoleColor.Yellow);
                        // StopTheProcess(); // stop the service and return
                    }
                    else
                    {
                        // select the best cashier for the current person
                        // closed cashiers and unavailable for these person status are skipped
                        int bestCashier = shop.SelectBestCashier(currentPerson, enteredCounter + 1);

                        // put the current person to the queue of the best cashier
                        Cashier cashier = shop.Cashiers[bestCashier];
                        cashier.Enqueue(currentPerson);
                        Program.Message(
                            $"[{time,3}] IN --->>> Cashier #{cashier.Number}: " +
                            $"person {persons[enteredCounter]} (#{enteredCounter + 1}) " +
                            $"(#{cashier.PersonsInQueue()} in queue)");
                        enteredCounter++;
                        if (cashier.PersonsInQueue() > shop.MaximumQueueSizeAllowed) // check the queue overflow
                        {
                            shop.OnQueueOverflow(cashier);  // call the event and pause the cashier
                            wasQueueOverflow[cashier.Number - 1] = true;
                        }
                    }
                }

                foreach (Cashier cashier in shop.Cashiers)
                {
                    Person maxPriorityPerson;
                    // find the client with the largest priority and serve him first. If all priorities are equal, the first one is served
                    if (!cashier.IsEmpty() && (maxPriorityPerson = cashier.Peek()).ServiceTime-- <= 0)  // service time exhausted
                    {
                        servedCounter++;
                        // dequeue the last client served out
                        maxPriorityPerson = cashier.Dequeue();
                        // show the person information
                        Program.Message($"[{time,3}] OUT <<<-- Cashier #{cashier.Number}: " +
                                        $"person {maxPriorityPerson} (#{servedCounter}) " +
                                        $"({cashier.PersonsInQueue()} remained in queue)");
                        result.Add($"{servedCounter,-3} [time {time,3}]: cashier #{cashier.Number} has served the " +
                                   $"person: {maxPriorityPerson.ShowForReport()}");

                        // check the queue size to resume the paused cashiers
                        if (wasQueueOverflow[cashier.Number-1] &&    // only the cashiers paused by event will be resumed
                            cashier.PersonsInQueue() <= shop.MaximumQueueSizeAllowed / 2)
                            {
                                shop.PauseOrResumeCashier(cashier.Number); // resume the cashier
                                wasQueueOverflow[cashier.Number-1] = false;
                            }

                        // check for the process stop
                        if (enteredCounter == persons.Count     // all clients have entered
                            && servedCounter == enteredCounter) // all clients have been served
                        {
                            Program.Message("All people has been served.", ConsoleColor.Yellow);
                            StopTheProcess();                   // stop the service and return
                        }
                    }
                }
                Thread.Sleep(1000); // wait 1 second till the next person entering
                time++;

                // check for user's input and react with an action
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    // define an action
                    Action MenuAction(ConsoleKeyInfo keyInfo) => keyInfo.KeyChar switch {
                        '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9' => () => {
                            // close the given cashier and move the queue to another cashiers
                            // or re-open the closed cashier
                            int cashierNumber = Int32.Parse(keyInfo.KeyChar.ToString());
                            if (cashierNumber <= shop.CashiersCount)
                            {
                                if ((keyInfo.Modifiers & ConsoleModifiers.Alt) != 0)
                                {   // Alt pressed -> pause/resume instead of close/reopen
                                    Console.Write("Alt pressed. ");
                                    shop.PauseOrResumeCashier(cashierNumber);
                                }
                                else
                                    if (!shop.CloseOrReopenCashier(cashierNumber, enteredCounter + 1))
                                        { // StopTheProcess();  // if after closing no cashiers available
                                        } // we can stop the process but better show the message and let the user react
                            }
                        },
                            // open cashier #1 for serving only disabled people, distribute its queue to other cashiers
                        'u' or 'U' => () => { shop.SetServeStatus(1, PersonStatus.Disabled); 
                                              shop.MovePersonsWithStatusTo(1, PersonStatus.Disabled, enteredCounter + 1); },
                        'x' or 'X' => () => StopTheProcess(),    // do not accept and serve new persons, just exit
                        _ => () => { },                          // any other key: do nothing, continue the process
                    };
                    Program.Message("\r\n");
                    MenuAction(key).Invoke(); // perform the action
                    foreach (Cashier cashier in shop.Cashiers)
                        if (cashier.PersonsInQueue() > shop.MaximumQueueSizeAllowed) // check the queue overflow
                        {
                            shop.OnQueueOverflow(cashier);  // call the event and pause the cashier
                            wasQueueOverflow[cashier.Number - 1] = true;
                        }
                }
            }
            return result;
        }
    }
}
