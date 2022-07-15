using System;
using System.Text.Json;

namespace Budget.Tracking
{
    public class BudgetTracking
    {
        public int currentBudget;
        public double currentExpenseTotal;

        public void fetchUserBudgetInfo()
        {
            if (!File.Exists("./budget.json"))
            {
                Dictionary<string, string> defaultTracker = new Dictionary<string, string>()
                {
                    {"currentBudget", "0"},
                    {"currentExpenseTotal", "0"}
                };

                string serializedDefault = JsonSerializer.Serialize(defaultTracker);

                File.WriteAllText("./budget.json", serializedDefault);

                Console.WriteLine("\n Initializing expense and budget goal tracking... \n");
            }
            else
            {    
                getBudgetAndExpense();

                Console.WriteLine($"\n Current budget goal: \n {currentBudget}");
                Console.WriteLine($"\n Current expense total:\n {currentExpenseTotal}");
                Console.WriteLine($"\n You have ${currentBudget - currentExpenseTotal} remaining \n");
                Console.WriteLine("\n --------------------------------------- \n");
            }
        }

        public Dictionary<string, string> getBudgetAndExpense()
        {
            string budgetJson = File.ReadAllText("./budget.json");

            Dictionary<string, string>? previousBudgetInfo = JsonSerializer.Deserialize<Dictionary<string, string>>(budgetJson) ?? throw new ArgumentNullException(nameof(previousBudgetInfo));

            // write values to global variables for expense total and budget goal
            try 
            {
                currentBudget = Int32.Parse(previousBudgetInfo["currentBudget"]);
                currentExpenseTotal = Convert.ToDouble(previousBudgetInfo["currentExpenseTotal"]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing budget info: {ex}");
            }

            return previousBudgetInfo;
        }
    }
}