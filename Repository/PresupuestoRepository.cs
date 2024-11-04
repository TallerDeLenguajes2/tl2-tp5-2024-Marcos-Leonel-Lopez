using Microsoft.Data.Sqlite;

// private int idPresupuesto;  => auto incremental
// private string nombreDestinatario; => viene de afuera
// private List<PresupuestosDetalle> detalle; =>    private Product producto;  =>   private int idProducto;
//                                                  private int cantidad;           private string descripcion;
//                                                                                  private int precio;


namespace PresupuestoRepo
{
    public class PresupuestoRepository : IRepository<Presupuesto>
    {
        private string cadenaConexion = "Data Source=db/Tienda.db;Cache=Shared";
        private int obtenerId(Presupuesto presupuesto)
        {
            var idBuscado = -999;
            string query = "SELECT idPresupuesto FROM Presupuestos WHERE NombreDestinatario = @nomb";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.Add(new SqliteParameter("@nomb", presupuesto.NombreDestinatario));
                connection.Open();
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idBuscado = Convert.ToInt32(reader["idPresupuesto"]);
                    }
                };
                connection.Close();
            }
            return idBuscado;
        }
        private void auxSetId(Presupuesto presupuesto)
        {
            int idCorrespondiente = this.obtenerId(presupuesto);
            if (idCorrespondiente != -999) presupuesto.setId(idCorrespondiente);
        }
        public List<Presupuesto> GetAll()
        {
            List<Presupuesto> presupuestos = new List<Presupuesto>();
            string queryPresupuestos = "SELECT idPresupuesto, NombreDestinatario FROM Presupuestos";
            string queryDetalles = @"
                                    SELECT pd.idProducto, pd.Cantidad, p.Descripcion, p.Precio
                                    FROM PresupuestoDetalle pd
                                    INNER JOIN Productos p USING(idProducto)
                                    WHERE pd.idPresupuesto = @idPresupuesto";


            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand(queryPresupuestos, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idPresupuesto = reader.GetInt32(0);
                            string nombreDestinatario = reader.GetString(1);

                            //----
                            List<PresupuestosDetalle> detalles = new List<PresupuestosDetalle>();
                            using (SqliteCommand detalleCommand = new SqliteCommand(queryDetalles, connection))
                            {
                                detalleCommand.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                                using (SqliteDataReader detalleReader = command.ExecuteReader())
                                {
                                    while (detalleReader.Read())
                                    {
                                        int idProducto = detalleReader.GetInt32(0);
                                        int cantidad = detalleReader.GetInt32(1);
                                        string descripcion = detalleReader.GetString(2);
                                        int precio = detalleReader.GetInt32(3);

                                        Product producto = new Product(idProducto, descripcion, precio);

                                        PresupuestosDetalle detalle = new PresupuestosDetalle(producto, cantidad);

                                        detalles.Add(detalle);
                                    }
                                }
                            }
                            //----
                            presupuestos.Add(new Presupuesto(idPresupuesto, nombreDestinatario, detalles));
                        }
                    }
                }
                connection.Close();
            }
            return presupuestos;
        }
        public Presupuesto GetById(int id)
        {
            Presupuesto presupuesto = null;
            string queryPresupuestos = "SELECT idPresupuesto, NombreDestinatario FROM Presupuestos WHERE idPresupuesto = @id";
            string queryDetalles = @"
                                    SELECT pd.idProducto, pd.Cantidad, p.Descripcion, p.Precio
                                    FROM PresupuestoDetalle pd
                                    INNER JOIN Productos p USING(idProducto)
                                    WHERE pd.idPresupuesto = @idPresupuesto";


            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand(queryPresupuestos, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@id", id));


                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idPresupuesto = reader.GetInt32(0);
                            string nombreDestinatario = reader.GetString(1);

                            //----
                            List<PresupuestosDetalle> detalles = new List<PresupuestosDetalle>();
                            using (SqliteCommand detalleCommand = new SqliteCommand(queryDetalles, connection))
                            {
                                detalleCommand.Parameters.AddWithValue("@idPresupuesto", id);
                                using (SqliteDataReader detalleReader = command.ExecuteReader())
                                {
                                    while (detalleReader.Read())
                                    {
                                        int idProducto = detalleReader.GetInt32(0);
                                        int cantidad = detalleReader.GetInt32(1);
                                        string descripcion = detalleReader.GetString(2);
                                        int precio = detalleReader.GetInt32(3);

                                        Product producto = new Product(idProducto, descripcion, precio);

                                        PresupuestosDetalle detalle = new PresupuestosDetalle(producto, cantidad);

                                        detalles.Add(detalle);
                                    }
                                }
                            }
                            //----
                            presupuesto = new Presupuesto(idPresupuesto, nombreDestinatario, detalles);
                        }
                    }
                }
                connection.Close();
            }
            return presupuesto;
        }
        public Presupuesto Create(Presupuesto obj)
        {
            string queryInsertPresupuesto = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@nombreDestinatario, @fechaCreacion); SELECT last_insert_rowid();";
            string queryInsertDetalle = "INSERT INTO PresupuestoDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad);";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                // Insertar el presupuesto
                using (var command = new SqliteCommand(queryInsertPresupuesto, connection))
                {
                    command.Parameters.AddWithValue("@nombreDestinatario", obj.NombreDestinatario);
                    command.Parameters.AddWithValue("@fechaCreacion", DateTime.Now);
                    command.ExecuteNonQuery();
                }
                // Insertar los detalles del presupuesto
                foreach (var detalle in obj.Detalle)
                {
                    using (var detalleCommand = new SqliteCommand(queryInsertDetalle, connection))
                    {
                        detalleCommand.Parameters.AddWithValue("@idPresupuesto", obj.IdPresupuesto);
                        detalleCommand.Parameters.AddWithValue("@idProducto", detalle.Product.IdProducto);
                        detalleCommand.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                        detalleCommand.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
            this.auxSetId(obj);
            return obj;
        }
        public bool Remove(int id)
        {
            string queryDeletePresupuesto = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
            string queryDeleteDetalles = "DELETE FROM PresupuestoDetalle WHERE idPresupuesto = @id";

            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();


                // Eliminar detalles primero
                using (var commandDetalles = new SqliteCommand(queryDeleteDetalles, connection))
                {
                    commandDetalles.Parameters.AddWithValue("@id", id);
                    commandDetalles.ExecuteNonQuery();
                }

                // Luego eliminar el presupuesto
                using (var commandPresupuesto = new SqliteCommand(queryDeletePresupuesto, connection))
                {
                    commandPresupuesto.Parameters.AddWithValue("@id", id);
                    return true;
                }

                connection.Close();
            }
            return false;
        }
        public Presupuesto Update(Presupuesto obj, int id)
        {
            string queryUpdatePresupuesto = "UPDATE Presupuestos SET NombreDestinatario = @nombreDestinatario, FechaCreacion = @fechaCreacion WHERE idPresupuesto = @id";
            string queryDeleteDetalles = "DELETE FROM PresupuestoDetalle WHERE idPresupuesto = @id";
            string queryInsertDetalle = "INSERT INTO PresupuestoDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad)";

            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                        // Actualizar el presupuesto
                        using (var command = new SqliteCommand(queryUpdatePresupuesto, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@nombreDestinatario", obj.NombreDestinatario);
                            command.Parameters.AddWithValue("@fechaCreacion", DateTime.Now);
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }

                        // Eliminar detalles actuales
                        using (var deleteCommand = new SqliteCommand(queryDeleteDetalles, connection, transaction))
                        {
                            deleteCommand.Parameters.AddWithValue("@id", id);
                            deleteCommand.ExecuteNonQuery();
                        }

                        // Insertar los nuevos detalles
                        foreach (var detalle in obj.Detalle)
                        {
                            using (var detalleCommand = new SqliteCommand(queryInsertDetalle, connection, transaction))
                            {
                                detalleCommand.Parameters.AddWithValue("@idPresupuesto", id);
                                detalleCommand.Parameters.AddWithValue("@idProducto", detalle.Product.IdProducto);
                                detalleCommand.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                                detalleCommand.ExecuteNonQuery();
                            }
                        }

                }
            }
            this.auxSetId(obj);
            return obj;
        }
    }

}



