using Microsoft.Data.Sqlite;

public class ProductRepository : IRepository<Product>
{
    private string cadenaConexion = "Data Source=db/Tienda.db;Cache=Shared";
    public List<Product> GetAll()
    {
        List<Product> productos = new List<Product>();
        string query = "SELECT * FROM Productos";
        using (var connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(query, connection);
            connection.Open();
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    productos.Add(new Product(Convert.ToInt32(reader["idProducto"]), reader["Descripcion"].ToString(), Convert.ToInt32(reader["Precio"])));
                }
            };
            connection.Close();
        }
        return productos;
    }

    public Product GetById(int id)
    {
        Product producto = null;
        string query = "SELECT * FROM Productos WHERE idProducto = @id";
        using (var connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", id));
            connection.Open();
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    producto = new Product(Convert.ToInt32(reader["idProducto"]), reader["Descripcion"].ToString(), Convert.ToInt32(reader["Precio"]));
                }
            };
            connection.Close();
        }
        return producto;
    }

    public Product Create(Product newProduct)
    {
        Product producto = newProduct;
        string query = "INSERT INTO Productos (Descripcion, Precio) VALUES (@desc, @precio);";
        using (var connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                command.Parameters.Add(new SqliteParameter("@desc", newProduct.Descripcion));
                command.Parameters.Add(new SqliteParameter("@precio", newProduct.Precio));
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
        return producto;
    }
}