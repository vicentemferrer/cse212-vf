﻿/// <summary>
/// Maintain a Customer Service Queue.  Allows new customers to be 
/// added and allows customers to be serviced.
/// </summary>
public class CustomerService {
    public static void Run() {
        // Example code to see what's in the customer service queue:
        // var cs = new CustomerService(10);
        // Console.WriteLine(cs);

        // Test Cases

        // Test 1
        // Scenario: The user shall specify the maximum size of the 
        // Customer Service Queue when it is created. If the size is invalid 
        // (less than or equal to 0) then the size shall default to 10. 
        // Expected Result: 10
        Console.WriteLine("Test 1");

        CustomerService cServ = new CustomerService(-2);

        Console.WriteLine(cServ._maxSize);

        // Defect(s) Found: No defects found.

        Console.WriteLine("=================");

        // Test 2
        // Scenario: The AddNewCustomer method shall enqueue a new customer into the queue.
        // Expected Result: 1
        Console.WriteLine("Test 2");

        cServ.AddNewCustomer();

        Console.WriteLine(cServ._queue.Count);

        // Defect(s) Found: No defects found.

        Console.WriteLine("=================");

        // Test 3
        // Scenario: If the queue is full when trying to add a customer, then an error message will be displayed.
        // Expected Result: Maximum Number of Customers in Queue.
        Console.WriteLine("Test 3");

        // CustomerService cServ2 = new CustomerService(2);

        // for (int i = 0; i <= cServ2._maxSize; i++) {
        //     cServ2.AddNewCustomer();
        // }

        // Defect(s) Found: AddNewCustomer exceed the _maxSize and after acts as expected.

        Console.WriteLine("=================");

        // Test 4
        // Scenario: The ServeCustomer function shall dequeue the next customer from the 
        // queue and display the details.
        // Expected Result: Frank (1234) : His car
        Console.WriteLine("Test 4");

        cServ.ServeCustomer();

        // Defect(s) Found: No customer details are displayed (OutOfRangeException), 
        // because the customer is retrieved after being deleted.

        Console.WriteLine("=================");

        // Test 5
        // Scenario: If the queue is empty when trying to serve a customer, 
        // then an error message will be displayed.
        // Expected Result: Cannot serve an empty customers list.
        Console.WriteLine("Test 5");

        cServ.ServeCustomer();

        // Defect(s) Found: There was not an handling case for this scenario in ServeCustomer method.
    }

    private readonly List<Customer> _queue = new();
    private readonly int _maxSize;

    public CustomerService(int maxSize) {
        if (maxSize <= 0)
            _maxSize = 10;
        else
            _maxSize = maxSize;
    }

    /// <summary>
    /// Defines a Customer record for the service queue.
    /// This is an inner class.  Its real name is CustomerService.Customer
    /// </summary>
    private class Customer {
        public Customer(string name, string accountId, string problem) {
            Name = name;
            AccountId = accountId;
            Problem = problem;
        }

        private string Name { get; }
        private string AccountId { get; }
        private string Problem { get; }

        public override string ToString() {
            return $"{Name} ({AccountId})  : {Problem}";
        }
    }

    /// <summary>
    /// Prompt the user for the customer and problem information.  Put the 
    /// new record into the queue.
    /// </summary>
    private void AddNewCustomer() {
        // Verify there is room in the service queue
        if (_queue.Count >= _maxSize) {
            Console.WriteLine("Maximum Number of Customers in Queue.");
            return;
        }

        Console.Write("Customer Name: ");
        var name = Console.ReadLine()!.Trim();
        Console.Write("Account Id: ");
        var accountId = Console.ReadLine()!.Trim();
        Console.Write("Problem: ");
        var problem = Console.ReadLine()!.Trim();

        // Create the customer object and add it to the queue
        var customer = new Customer(name, accountId, problem);
        _queue.Add(customer);
    }

    /// <summary>
    /// Dequeue the next customer and display the information.
    /// </summary>
    private void ServeCustomer() {
        if (_queue.Count <= 0) {
            Console.WriteLine("Cannot serve an empty customers list.");
            return;
        }

        var customer = _queue[0];
        _queue.RemoveAt(0);
        Console.WriteLine(customer);
    }

    /// <summary>
    /// Support the WriteLine function to provide a string representation of the
    /// customer service queue object. This is useful for debugging. If you have a 
    /// CustomerService object called cs, then you run Console.WriteLine(cs) to
    /// see the contents.
    /// </summary>
    /// <returns>A string representation of the queue</returns>
    public override string ToString() {
        return $"[size={_queue.Count} max_size={_maxSize} => " + string.Join(", ", _queue) + "]";
    }
}