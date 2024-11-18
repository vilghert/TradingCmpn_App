using DTO;
using System.Security.Cryptography;

namespace TradingCompanyApp
{
    class Program
    {
        private static string connectionString = "Data Source=LAPTOP-E6KP9TMM\\MSSQLSERVER01;Initial Catalog=TradingCmpn_db;Integrated Security=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Trading Company!");

            var orderItemDal = new OrderItemDal(connectionString);
            var reviewDal = new ReviewDal(connectionString);
            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Show All Products");
                Console.WriteLine("3. Delete Product");
                Console.WriteLine("4. Add Order");
                Console.WriteLine("5. Show All Orders");
                Console.WriteLine("6. Delete Order");
                Console.WriteLine("7. Add Order Status");
                Console.WriteLine("8. Show All Order Statuses");
                Console.WriteLine("9. Delete Order Status");
                Console.WriteLine("10. Add User");
                Console.WriteLine("11. Show All Users");
                Console.WriteLine("12. Delete User");
                Console.WriteLine("13. Add Order Item");
                Console.WriteLine("14. Show All Order Items");
                Console.WriteLine("15. Delete Order Item");
                Console.WriteLine("16. Add Review");
                Console.WriteLine("17. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        DisplayAllProductsAsync();
                        break;
                    case "3":
                        DeleteProduct();
                        break;
                    case "4":
                        AddOrder(orderItemDal);
                        break;
                    case "5":
                        DisplayAllOrdersAsync(orderItemDal);
                        break;
                    case "6":
                        DeleteOrder(orderItemDal);
                        break;
                    case "7":
                        AddOrderStatus();
                        break;
                    case "8":
                        DisplayAllOrderStatuses();
                        break;
                    case "9":
                        DeleteOrderStatus();
                        break;
                    case "10":
                        AddUser();
                        break;
                    case "11":
                        DisplayAllUsers();
                        break;
                    case "12":
                        DeleteUser();
                        break;
                    case "13":
                        AddOrderItem(orderItemDal);
                        break;
                    case "14":
                        DisplayAllOrderItems(orderItemDal);
                        break;
                    case "15":
                        DeleteOrderItem(orderItemDal);
                        break;
                    case "16":
                        AddReview(reviewDal).Wait();
                        break;
                    case "17":
                        Console.WriteLine("Exiting the program...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void AddProduct()
        {
            Console.WriteLine("Enter product name:");
            string productName = Console.ReadLine();
            Console.WriteLine("Enter the category ID for the product:");
            if (int.TryParse(Console.ReadLine(), out int categoryId))
            {
                Console.WriteLine("Enter product price:");
                if (decimal.TryParse(Console.ReadLine(), out decimal price))
                {
                    var productDal = new ProductDal(connectionString);
                    var newProduct = new ProductDto { ProductName = productName, CategoryId = categoryId, Price = price };
                    productDal.InsertAsync(newProduct);
                    Console.WriteLine("Product added successfully!");
                }
                else
                {
                    Console.WriteLine("Invalid price format. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid category ID format. Please try again.");
            }
        }

        private static async Task DisplayAllProductsAsync()
        {
            var productDal = new ProductDal(connectionString);
            List<ProductDto> products = await productDal.GetAllAsync();

            Console.WriteLine("List of products:");
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.ProductId}, Name: {product.ProductName}, Category ID: {product.CategoryId}, Price: {product.Price}");
            }
        }

        private static void DeleteProduct()
        {
            Console.WriteLine("Enter the ID of the product to delete:");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                var productDal = new ProductDal(connectionString);
                productDal.DeleteAsync(productId);
                Console.WriteLine("Product deleted successfully!");
            }
            else
            {
                Console.WriteLine("Invalid ID format. Please try again.");
            }
        }

        private static void UpdateProduct()
        {
            Console.WriteLine("Enter the ID of the product to update:");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Enter new product name:");
                string newProductName = Console.ReadLine();
                Console.WriteLine("Enter new product price:");
                if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
                {
                    var productDal = new ProductDal(connectionString);
                    var product = new ProductDto { ProductId = productId, ProductName = newProductName, Price = newPrice };
                    productDal.UpdateAsync(product);
                    Console.WriteLine("Product updated successfully!");
                }
                else
                {
                    Console.WriteLine("Invalid price format. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format. Please try again.");
            }
        }

        private static void AddOrder(IOrderItemDal orderItemDal)
        {
            Console.WriteLine("Enter user ID:");
            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("Enter total amount:");
                if (decimal.TryParse(Console.ReadLine(), out decimal totalAmount))
                {
                    var orderDal = new OrderDal(connectionString, orderItemDal);
                    var newOrder = new OrderDto { UserId = userId, TotalAmount = totalAmount };
                    orderDal.InsertAsync(newOrder);
                    Console.WriteLine("Order added successfully!");
                }
                else
                {
                    Console.WriteLine("Invalid total amount format. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid user ID format. Please try again.");
            }
        }

        private static async Task DisplayAllOrdersAsync(IOrderItemDal orderItemDal)
        {
            var orderDal = new OrderDal(connectionString, orderItemDal);
            List<OrderDto> orders = await orderDal.GetAllAsync();

            Console.WriteLine("List of orders:");
            foreach (var order in orders)
            {
                Console.WriteLine($"ID: {order.OrderId}, User ID: {order.UserId}, Total Amount: {order.TotalAmount}");
            }
        }

        private static void DeleteOrder(IOrderItemDal orderItemDal)
        {
            Console.WriteLine("Enter the ID of the order to delete:");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                var orderDal = new OrderDal(connectionString, orderItemDal);
                orderDal.Delete(orderId);
                Console.WriteLine("Order deleted successfully!");
            }
            else
            {
                Console.WriteLine("Invalid ID format. Please try again.");
            }
        }

        private static void UpdateOrder(IOrderItemDal orderItemDal)
        {
            Console.WriteLine("Enter the ID of the order to update:");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Enter new user ID:");
                if (int.TryParse(Console.ReadLine(), out int newUserId))
                {
                    Console.WriteLine("Enter new total amount:");
                    if (decimal.TryParse(Console.ReadLine(), out decimal newTotalAmount))
                    {
                        var orderDal = new OrderDal(connectionString, orderItemDal);
                        var order = new OrderDto { OrderId = orderId, UserId = newUserId, TotalAmount = newTotalAmount };
                        orderDal.Update(order);
                        Console.WriteLine("Order updated successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid total amount format. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid user ID format. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format. Please try again.");
            }
        }

        private static void AddOrderStatus()
        {
            Console.WriteLine("Enter order status name:");
            string statusName = Console.ReadLine();

            var orderStatusDal = new OrderStatusDal(connectionString);
            var newOrderStatus = new OrderStatusDto { OrderStatusName = statusName };
            orderStatusDal.Insert(newOrderStatus);
            Console.WriteLine("Order status added successfully!");
        }

        private static void DisplayAllOrderStatuses()
        {
            var orderStatusDal = new OrderStatusDal(connectionString);
            List<OrderStatusDto> orderStatuses = orderStatusDal.GetAll();

            Console.WriteLine("List of order statuses:");
            foreach (var orderStatus in orderStatuses)
            {
                Console.WriteLine($"ID: {orderStatus.OrderStatusId}, Status: {orderStatus.OrderStatusName}");
            }
        }

        private static void DeleteOrderStatus()
        {
            Console.WriteLine("Enter the ID of the order status to delete:");
            if (int.TryParse(Console.ReadLine(), out int orderStatusId))
            {
                var orderStatusDal = new OrderStatusDal(connectionString);
                orderStatusDal.Delete(orderStatusId);
                Console.WriteLine("Order status deleted successfully!");
            }
            else
            {
                Console.WriteLine("Invalid ID format. Please try again.");
            }
        }

        private static void AddUser()
        {
            Console.WriteLine("Enter username:");
            string userName = Console.ReadLine();

            Console.WriteLine("Enter email:");
            string email = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            var authService = new AuthService(connectionString);
            var (passwordHash, passwordSalt) = authService.CreatePasswordHash(password);

            Console.WriteLine("Enter RoleID (1 for Admin, 2 for Common User):");
            int roleId;
            while (!int.TryParse(Console.ReadLine(), out roleId) || (roleId != 1 && roleId != 2))
            {
                Console.WriteLine("Invalid input. Please enter 1 for Admin or 2 for Common User:");
            }

            var userDal = new UserDal(connectionString);
            var newUser = new UserDto
            {
                UserName = userName,
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.Now,
                RoleID = roleId
            };

            userDal.Insert(newUser);
            Console.WriteLine("User added successfully!");
        }

        private static void DisplayAllUsers()
        {
            var userDal = new UserDal(connectionString);
            List<UserDto> users = userDal.GetAll();

            Console.WriteLine("List of users:");
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.UserId}, Name: {user.UserName}");
            }
        }

        private static void DeleteUser()
        {
            Console.WriteLine("Enter the ID of the user to delete:");
            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                var userDal = new UserDal(connectionString);
                userDal.Delete(userId);
                Console.WriteLine("User deleted successfully!");
            }
            else
            {
                Console.WriteLine("Invalid ID format. Please try again.");
            }
        }
        private static void AddOrderItem(IOrderItemDal orderItemDal)
        {
            Console.WriteLine("Enter order ID:");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Enter product ID:");
                if (int.TryParse(Console.ReadLine(), out int productId))
                {
                    Console.WriteLine("Enter quantity:");
                    if (int.TryParse(Console.ReadLine(), out int quantity))
                    {
                        var newOrderItem = new OrderItemDto { OrderId = orderId, ProductId = productId, Quantity = quantity };
                        orderItemDal.InsertAsync(newOrderItem);
                        Console.WriteLine("Order item added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity format. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid product ID format. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid order ID format. Please try again.");
            }
        }

        private static void DisplayAllOrderItems(IOrderItemDal orderItemDal)
        {
            List<OrderItemDto> orderItems = orderItemDal.GetAll();

            Console.WriteLine("List of order items:");
            if (orderItems.Count == 0)
            {
                Console.WriteLine("No order items found.");
                return;
            }

            foreach (var orderItem in orderItems)
            {
                Console.WriteLine($"ID: {orderItem.OrderItemId}, Order ID: {orderItem.OrderId}, Product ID: {orderItem.ProductId}, Quantity: {orderItem.Quantity}");
            }
        }

        private static void DeleteOrderItem(IOrderItemDal orderItemDal)
        {
            Console.WriteLine("Enter the ID of the order item to delete:");
            if (int.TryParse(Console.ReadLine(), out int orderItemId))
            {
                orderItemDal.DeleteByOrderId(orderItemId);
                Console.WriteLine("Order item deleted successfully!");
            }
            else
            {
                Console.WriteLine("Invalid ID format. Please try again.");
            }
        }
        private static async Task AddReview(ReviewDal reviewDal)
        {
            Console.WriteLine("Enter Product ID:");
            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Invalid Product ID. Please try again.");
                return;
            }

            Console.WriteLine("Enter User ID:");
            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("Invalid User ID. Please try again.");
                return;
            }

            Console.WriteLine("Enter Review Text:");
            string reviewText = Console.ReadLine();

            var newReview = new ReviewDto
            {
                ProductId = productId,
                UserId = userId,
                ReviewText = reviewText
            };

            try
            {
                int reviewId = await reviewDal.InsertAsync(newReview);
                Console.WriteLine($"Review added successfully! Review ID: {reviewId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the review: {ex.Message}");
            }
        }
    }
}