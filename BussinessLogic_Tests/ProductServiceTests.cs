using Moq;

[TestFixture]
public class ProductServiceTests
{
    private Mock<IProductDal> _mockProductDal;
    private ProductService _productService;

    [SetUp]
    public void SetUp()
    {
        _mockProductDal = new Mock<IProductDal>();
        _productService = new ProductService(_mockProductDal.Object);
    }

    [Test]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        var products = new List<ProductDto>
        {
            new ProductDto { ProductId = 1, ProductName = "Product 1", CategoryId = 1, Price = 10.00m },
            new ProductDto { ProductId = 2, ProductName = "Product 2", CategoryId = 2, Price = 20.00m }
        };

        _mockProductDal.Setup(dal => dal.GetAllAsync()).ReturnsAsync(products);

        var result = await _productService.GetAllProductsAsync();

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Product 1", result[0].ProductName);
    }

    [Test]
    public async Task GetProductByIdAsync_ReturnsProduct_WhenProductExists()
    {
        var product = new ProductDto { ProductId = 1, ProductName = "Product 1", CategoryId = 1, Price = 10.00m };
        _mockProductDal.Setup(dal => dal.GetByIdAsync(1)).ReturnsAsync(product);

        var result = await _productService.GetProductByIdAsync(1);

        Assert.IsNotNull(result);
        Assert.AreEqual("Product 1", result.ProductName);
    }

    [Test]
    public async Task CreateProductAsync_CallsInsertAsyncOnDal()
    {
        var product = new ProductDto { ProductName = "New Product", CategoryId = 1, Price = 15.00m };

        await _productService.CreateProductAsync(product);

        _mockProductDal.Verify(dal => dal.InsertAsync(product), Times.Once);
    }

    [Test]
    public async Task UpdateProductAsync_CallsUpdateAsyncOnDal()
    {
        var product = new ProductDto { ProductId = 1, ProductName = "Updated Product", CategoryId = 1, Price = 15.00m };

        await _productService.UpdateProductAsync(product);

        _mockProductDal.Verify(dal => dal.UpdateAsync(product), Times.Once);
    }

    [Test]
    public async Task DeleteProductAsync_CallsDeleteAsyncOnDal()
    {
        int productId = 1;

        await _productService.DeleteProductAsync(productId);

        _mockProductDal.Verify(dal => dal.DeleteAsync(productId), Times.Once);
    }

    [Test]
    public async Task SearchProductsAsync_ReturnsFilteredProducts()
    {
        var products = new List<ProductDto>
        {
            new ProductDto { ProductId = 1, ProductName = "Product A", CategoryId = 1, Price = 10.00m },
            new ProductDto { ProductId = 2, ProductName = "Product B", CategoryId = 2, Price = 20.00m }
        };

        _mockProductDal.Setup(dal => dal.GetAllAsync()).ReturnsAsync(products);

        var result = await _productService.SearchProductsAsync("A");

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Product A", result[0].ProductName);
    }

    [Test]
    public async Task SortProductsAsync_SortsByName()
    {
        var products = new List<ProductDto>
        {
            new ProductDto { ProductId = 2, ProductName = "Product B", CategoryId = 2, Price = 20.00m },
            new ProductDto { ProductId = 1, ProductName = "Product A", CategoryId = 1, Price = 10.00m }
        };

        _mockProductDal.Setup(dal => dal.GetAllAsync()).ReturnsAsync(products);

        var result = await _productService.SortProductsAsync("Name");

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Product A", result[0].ProductName);
    }

    [Test]
    public async Task SortProductsAsync_SortsByPrice()
    {
        var products = new List<ProductDto>
        {
            new ProductDto { ProductId = 2, ProductName = "Product B", CategoryId = 2, Price = 20.00m },
            new ProductDto { ProductId = 1, ProductName = "Product A", CategoryId = 1, Price = 10.00m }
        };

        _mockProductDal.Setup(dal => dal.GetAllAsync()).ReturnsAsync(products);

        var result = await _productService.SortProductsAsync("Price");

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(10.00m, result[0].Price);
    }
}
