using Moq;
using DTO;

[TestFixture]
public class OrderStatusServiceTests
{
    private Mock<IOrderStatusDal> _mockOrderStatusDal;
    private OrderStatusService _orderStatusService;

    [SetUp]
    public void SetUp()
    {
        _mockOrderStatusDal = new Mock<IOrderStatusDal>();
        _orderStatusService = new OrderStatusService(_mockOrderStatusDal.Object);
    }

    [Test]
    public void GetAllOrderStatuses_ReturnsAllOrderStatuses()
    {
     
        var orderStatuses = new List<OrderStatusDto>
        {
            new OrderStatusDto { OrderStatusId = 1, OrderStatusName = "Pending" },
            new OrderStatusDto { OrderStatusId = 2, OrderStatusName = "Completed" }
        };
        _mockOrderStatusDal.Setup(dal => dal.GetAll()).Returns(orderStatuses);


        var result = _orderStatusService.GetAllOrderStatuses();


        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Pending", result[0].OrderStatusName);
    }

    [Test]
    public void GetOrderStatusById_ReturnsOrderStatus_WhenExists()
    {

        var orderStatus = new OrderStatusDto { OrderStatusId = 1, OrderStatusName = "Pending" };
        _mockOrderStatusDal.Setup(dal => dal.GetById(1)).Returns(orderStatus);


        var result = _orderStatusService.GetOrderStatusById(1);


        Assert.IsNotNull(result);
        Assert.AreEqual("Pending", result.OrderStatusName);
    }

    [Test]
    public void GetOrderStatusById_ReturnsNull_WhenDoesNotExist()
    {

        _mockOrderStatusDal.Setup(dal => dal.GetById(1)).Returns((OrderStatusDto)null);


        var result = _orderStatusService.GetOrderStatusById(1);


        Assert.IsNull(result);
    }

    [Test]
    public async Task CreateOrderStatus_CallsInsertAsyncOnDal()
    {

        var orderStatus = new OrderStatusDto { OrderStatusName = "New Status" };
        _mockOrderStatusDal.Setup(dal => dal.Insert(orderStatus)).Verifiable();

  
        await Task.Run(() => _orderStatusService.CreateOrderStatus(orderStatus));

   
        _mockOrderStatusDal.Verify(dal => dal.Insert(orderStatus), Times.Once);
    }

    [Test]
    public async Task UpdateOrderStatus_CallsUpdateOnDal()
    {

        var orderStatus = new OrderStatusDto { OrderStatusId = 1, OrderStatusName = "Updated Status" };
        _mockOrderStatusDal.Setup(dal => dal.Update(orderStatus)).Verifiable();


        await Task.Run(() => _orderStatusService.UpdateOrderStatus(orderStatus));

  
        _mockOrderStatusDal.Verify(dal => dal.Update(orderStatus), Times.Once);
    }

    [Test]
    public async Task DeleteOrderStatus_CallsDeleteOnDal()
    {

        int orderStatusId = 1;
        _mockOrderStatusDal.Setup(dal => dal.Delete(orderStatusId)).Verifiable();


        await Task.Run(() => _orderStatusService.DeleteOrderStatus(orderStatusId));


        _mockOrderStatusDal.Verify(dal => dal.Delete(orderStatusId), Times.Once);
    }
}