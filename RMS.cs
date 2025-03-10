using System;
using System.Data;

namespace RMSApp
{
    internal class Program
    {
        public static void UpdateData(DataTable dt)
        {
            Console.WriteLine("Enter CustomerId for which you want to update:");
            int cid = Convert.ToInt32(Console.ReadLine());
            DataRow[] rows = dt.Select($"CustomerID = {cid}");
            Console.WriteLine("Enter the updated customer Name : "); string cname = Console.ReadLine();
            Console.WriteLine("Enter the Updated Email : ");  string email = Console.ReadLine();
            Console.WriteLine("Enter the Updated Phone number : "); string num = Console.ReadLine();
            if (rows.Length > 0)
            {
                DataRow row = rows[0];
                row["CustomerName"] = cname;
                row["Email"] = email;
                row["Phone"] = num;
                //dt.AcceptChanges();
                Console.WriteLine("Record updated successfully!");
            }
            else
            {
                Console.WriteLine("Record not found.");
            }
        }

        public static void DeleteData(DataTable dt)
        {
            Console.WriteLine("Enter the CustomerID which you want to delete: "); int cid= Convert.ToInt32(Console.ReadLine());
            DataRow[] rows = dt.Select($"CustomerID = {cid}");
            if (rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    row.Delete();
                }
                Console.WriteLine("Record(s) deleted successfully!");
            }
            else
            {
                Console.WriteLine("No matching records found!");
            }
        }

        public static void InsertData(DataTable dt)
        {
            Console.WriteLine("Enter the customer Name: "); string name = Console.ReadLine();
            Console.WriteLine("Enter the customer email: "); string email = Console.ReadLine() ;
            Console.WriteLine("Enter the customer phone number: ");string num = Console.ReadLine();
            DataRow row = dt.NewRow();
            row["CustomerName"] = name;
            row["Email"]= email;
            row["Phone"]= num;
            dt.Rows.Add(row);
            Console.WriteLine("Customer Added Successfully");
        }
        static void Main(string[] args)
        {
            DataSet retailDataSet = new DataSet("RetailStore");

            DataTable customersTable = new DataTable("Customers");

            DataColumn customerID = new DataColumn("CustomerID",typeof(int));//CustomerID
            customerID.AutoIncrement = true;
            customerID.AutoIncrementSeed = 1;
            customerID.AutoIncrementStep= 1;
            customerID.Unique = true;
            customersTable.Columns.Add(customerID);
            customersTable.Columns.Add("CustomerName",typeof(string));//CustomerName
            customersTable.Columns.Add("Email",typeof (string)).Unique= true;//Email
            customersTable.Columns.Add("Phone",typeof(string));//Phone

            customersTable.PrimaryKey = new DataColumn[] { customerID };
            retailDataSet.Tables.Add(customersTable);
            //============================================================================

            DataTable productsTable = new DataTable("Products");
            DataColumn productsID = new DataColumn("ProductID", typeof(int));//ProductID
            productsID.AutoIncrement = true;
            productsID.Unique = true;
            productsTable.Columns.Add(productsID);
            productsTable.Columns.Add("ProductName", typeof(string));//ProductName
            productsTable.Columns.Add("Price",typeof(decimal));//Price

            productsTable.PrimaryKey = new DataColumn[] { productsID };
            retailDataSet.Tables.Add(productsTable);
            //============================================================================

            DataTable ordersTable = new DataTable("Orders");

            DataColumn ordersID = new DataColumn("OrderID",typeof(int));
            ordersID.AutoIncrement = true;
            ordersID.Unique = true;
            ordersTable.Columns.Add(ordersID);
            ordersTable.Columns.Add("CustomerID", typeof(int));
            ordersTable.Columns.Add("ProductID", typeof(int));
            ordersTable.Columns.Add("Quantity", typeof(int));
            ordersTable.Columns.Add("OrderDate",typeof(DateTime));

            ordersTable.PrimaryKey = new DataColumn[] { ordersID };
            retailDataSet.Tables.Add(ordersTable);
            //============================================================================
            
            //DataRelation customerOrderRelation = new DataRelation("CustomerOrderRelation", customersTable.Columns["CustomerID"], ordersTable.Columns["CustomerID"]);
            //retailDataSet.Relations.Add(customerOrderRelation);

            //DataRelation productOrderRelation = new DataRelation("ProductOrderRelation", productsTable.Columns["ProductID"], ordersTable.Columns["ProductID"]);
            //retailDataSet.Relations.Add(productOrderRelation);
            //============================================================================
            ordersTable.Constraints.Add(new ForeignKeyConstraint("FK_Customer_Orders", customersTable.Columns["CustomerID"], ordersTable.Columns["CustomerID"]));
            ordersTable.Constraints.Add(new ForeignKeyConstraint("FK_Product_Orders", productsTable.Columns["ProductID"], ordersTable.Columns["ProductID"]));
            //============================================================================

            ordersTable.Columns["Quantity"].AllowDBNull = false;
            ordersTable.Columns["OrderDate"].AllowDBNull=false;
            //============================================================================
            DataRow customer1 = customersTable.NewRow();
            customer1["CustomerName"] = "Alice";
            customer1["Email"] = "alice@example.com";
            customer1["Phone"] = "1234567890";
            customersTable.Rows.Add(customer1);

            DataRow product1 = productsTable.NewRow();
            product1["ProductName"] = "Laptop";
            product1["Price"] = 1200.50m;
            productsTable.Rows.Add(product1);

            DataRow order1 = ordersTable.NewRow();
            order1["CustomerID"] = customer1["CustomerID"];
            order1["ProductID"] = product1["ProductID"];
            order1["Quantity"] = 1;
            order1["OrderDate"]= DateTime.Now;
            ordersTable.Rows.Add(order1);
            //============================================================================

            DataView ordersView = new DataView(ordersTable);
            ordersView.RowFilter = "CustomerID = " + customer1["CustomerID"];
            ordersView.Sort = "OrderDate DESC";
            //============================================================================
            InsertData(customersTable);
            ReadData(customersTable);
            UpdateData(customersTable);
            ReadData(customersTable);
            DeleteData(customersTable);
            ReadData(customersTable);

        }

        public static void ReadData(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    Console.Write($"{column.ColumnName}: {row[column]} ");
                }
                Console.WriteLine();
            }
        }
    }
}
