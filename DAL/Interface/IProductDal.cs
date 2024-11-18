using System.Collections.Generic;
public interface IProductDal
{
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDto> GetByIdAsync(int id);
    Task InsertAsync(ProductDto product);
    Task UpdateAsync(ProductDto product);
    Task DeleteAsync(int productId);
}
