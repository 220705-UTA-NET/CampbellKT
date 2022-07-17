using System;
using Npgsql;
using Budget.UserInteraction;

namespace Budget.RouteMethods
{
    public class PostAndPutRouteMethods : ApiMethods
    {
        private Expense expense;
        private int? id;
        private NpgsqlParameter? description;
        private NpgsqlParameter? amount;
        private NpgsqlParameter? category;
        private NpgsqlParameter? date;
        private NpgsqlParameter? updatedExpenseId;

        public PostAndPutRouteMethods(NpgsqlConnection dbConn, string commandText, Expense expense, int id = -1) : base(dbConn, commandText)
        {
            this.expense = expense;
            this.id = id;
        }

        private NpgsqlCommand SetSqlParameters()
        {
            // creating params for prepared statement
            description = new NpgsqlParameter("Description", expense.Description);
            amount = new NpgsqlParameter("Amount", expense.Amount);
            category = new NpgsqlParameter("Category", expense.Category);
            date = new NpgsqlParameter("Date", expense.Date);

            NpgsqlCommand command = new NpgsqlCommand(commandText, dbConn);

            command.Parameters.Add(description);
            command.Parameters.Add(amount);
            command.Parameters.Add(category);
            command.Parameters.Add(date);

            return command;
        }

        public int CreateNewExpense()
        {
            try
            {
                NpgsqlCommand command = SetSqlParameters();

                int reader = command.ExecuteNonQuery();
                command.Dispose();

                Console.WriteLine("\n --------------------------------------- \n");
                Console.WriteLine("\n Entry successfully added \n");
                Console.WriteLine("\n --------------------------------------- \n");

                commandMenu.displayInteractionMenu();

                return reader;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create new expense: {ex}");
            }
        }

        public int UpdateOldExpense()
        {
            try
            {
                NpgsqlCommand command = SetSqlParameters();

                updatedExpenseId = new NpgsqlParameter("Id", id);
                command.Parameters.Add(updatedExpenseId);

                int reader = command.ExecuteNonQuery();
                command.Dispose();

                Console.WriteLine("\n --------------------------------------- \n");
                Console.WriteLine("\n Entry successfully updated \n");
                Console.WriteLine("\n --------------------------------------- \n");

                commandMenu.displayInteractionMenu();

                return reader;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating previous expense: {ex}");
            }
        }
    }
}