using System.Text.Json.Serialization;
public class Product{
    private int idProducto;
    private string descripcion;
    private int precio;
    public Product(){

    }
    public Product(string descripcion,int precio){ // necesario utilizar el mismo nombre en los parametros y las variables
        this.descripcion = descripcion;
        this.precio = precio;
    }
    [JsonConstructor]// fundamental para poder crear el objeto a partir del body
    public Product(int idProducto,string descripcion,int precio){
        this.idProducto = idProducto;
        this.descripcion = descripcion;
        this.precio = precio;
    }

    public int IdProducto { get => idProducto;}
    public string Descripcion { get => descripcion; }
    public int Precio { get => precio; }
    public void setId(int id){
        this.idProducto = id;
    }
}