public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();  
    Task<ProductDto> GetProductByIdAsync(int id);  
    Task CreateProductAsync(ProductDto product);  
    Task UpdateProductAsync(ProductDto product);   
    Task DeleteProductAsync(int id);              
    Task<List<ProductDto>> SearchProductsAsync(string searchQuery); 
    Task<List<ProductDto>> SortProductsAsync(string sortBy);     
}