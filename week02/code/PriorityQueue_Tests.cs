using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Can I add priorities items and dequeue the higher priority one?
    // Expected Result: "potato" and 3.
    // Defect(s) Found: Dequeue method for loop ended before analyze last array value,
    // and does not delete the item from queue.
    public void TestPriorityQueue_1()
    {
        var priorityQueue = new PriorityQueue();

        string[] testValues = ["banana", "apple", "tomato", "potato"];

        for (int i = 0; i < testValues.Length; i++) {
            priorityQueue.Enqueue(testValues[i], i + 1);
        }

        int currentLength = priorityQueue.Length;

        string higherValue = priorityQueue.Dequeue();

        Assert.AreEqual(testValues[testValues.Length - 1], higherValue);
        Assert.AreEqual(currentLength - 1, priorityQueue.Length);
    }

    [TestMethod]
    // Scenario: Can I dequeue closest item to queue beginning? What happens if I dequeue with an empty queue?
    // Expected Result: "The queue is empty." and "tomato".
    // Defect(s) Found: Before two items of same priority level, last one is dequeued.
    public void TestPriorityQueue_2()
    {
        var priorityQueue = new PriorityQueue();

        string[] testValues = ["banana", "apple", "tomato", "potato"];

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
        catch (AssertFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            Assert.Fail(
                 string.Format("Unexpected exception of type {0} caught: {1}",
                                e.GetType(), e.Message)
            );
        }

        for (int i = 0; i < testValues.Length; i++) {
            int toAdd = testValues[i] == "tomato" ? 2 : 1;
            priorityQueue.Enqueue(testValues[i], i + toAdd);
        }

        string higherValue = priorityQueue.Dequeue();

        Assert.AreEqual(testValues[testValues.Length - 2], higherValue);
    }

    // Add more test cases as needed below.
}