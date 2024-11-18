public class ProductService : IProductService
{
    private readonly IProductDal _productDal;

    public ProductService(IProductDal productDal)
    {
        _productDal = productDal;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        return await _productDal.GetAllAsync();
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        return await _productDal.GetByIdAsync(id);
    }

    public async Task CreateProductAsync(ProductDto product)
    {
        await _productDal.InsertAsync(product);
    }

    public async Task UpdateProductAsync(ProductDto product)
    {
        await _productDal.UpdateAsync(product);
    }

    public async Task DeleteProductAsync(int id)
    {
        await _productDal.DeleteAsync(id);
    }

    public async Task<List<ProductDto>> SearchProductsAsync(string searchQuery)
    {
        var allProducts = await _productDal.GetAllAsync();
        return allProducts
            .Where(p => p.ProductName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public async Task<List<ProductDto>> SortProductsAsync(string sortBy)
    {
        var allProducts = await _productDal.GetAllAsync();
        return sortBy switch
        {
            "Name" => allProducts.OrderBy(p => p.ProductName).ToList(),
            "Price" => allProducts.OrderBy(p => p.Price).ToList(),
            _ => allProducts
        };
    }
}