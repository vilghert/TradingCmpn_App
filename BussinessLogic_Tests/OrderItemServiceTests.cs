using Moq;

[TestFixture]
public class OrderItemServiceTests
{
    private Mock<IOrderItemDal> _mockOrderItemDal;
    private OrderItemService _orderItemService;

    [SetUp]
    public void SetUp()
    {
        _mockOrderItemDal = new Mock<IOrderItemDal>();
        _orderItemService = new OrderItemService(_mockOrderItemDal.Object);
    }

    [Test]
    public void GetOrderItemsByOrderId_ReturnsItemsForSpecificOrder()
    {
        var orderId = 1;
        var orderItems = new List<OrderItemDto>
        {
            new OrderItemDto { OrderItemId = 1, OrderId = orderId, ProductId = 1, Quantity = 2 },
            new OrderItemDto { OrderItemId = 2, OrderId = orderId, ProductId = 2, Quantity = 5 }
        };
        _mockOrderItemDal.Setup(d => d.GetByOrderId(orderId)).Returns(orderItems);

        var result = _orderItemService.GetOrderItemsByOrderId(orderId);

        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.All(item => item.OrderId == orderId));
    }

    [Test]
    public void UpdateOrderItem_CallsUpdateOnDal()
    {
        var orderItemToUpdate = new OrderItemDto { OrderItemId = 1, OrderId = 1, ProductId = 1, Quantity = 3 };

        _orderItemService.UpdateOrderItem(orderItemToUpdate);

        _mockOrderItemDal.Verify(d => d.Update(orderItemToUpdate), Times.Once);
    }

    [Test]
    public void DeleteOrderItemsByOrderId_CallsDeleteByOrderIdOnDal()
    {
        int orderId = 1;

        _orderItemService.DeleteOrderItemsByOrderId(orderId);

        _mockOrderItemDal.Verify(d => d.DeleteByOrderId(orderId), Times.Once);
    }
}
