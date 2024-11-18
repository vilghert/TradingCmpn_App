using DTO;
public interface IOrderStatusDal
{
    List<OrderStatusDto> GetAll();
    OrderStatusDto GetById(int id);
    void Insert(OrderStatusDto orderStatus);
    void Update(OrderStatusDto orderStatus);
    void Delete(int id);
}