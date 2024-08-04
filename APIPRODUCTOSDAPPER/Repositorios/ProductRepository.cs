using APIPRODUCTOSDAPPER.Data;
using APIPRODUCTOSDAPPER.Models;
using Dapper;

namespace APIPRODUCTOSDAPPER.Repositorios
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContext _context;

        public ProductRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>("SELECT * FROM Products");
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> CreateAsync(Product product)
        {
            using var connection = _context.CreateConnection();
            var sql = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price); SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.QuerySingleAsync<int>(sql, product);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, product);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM Products WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

            
    }
}
