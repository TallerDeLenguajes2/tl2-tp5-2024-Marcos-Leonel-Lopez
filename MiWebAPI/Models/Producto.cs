public class Producto{
    private int idProducto;
    private string descripcion;
    private int precio;
    public Producto(){

    }
    public Producto(string desc,int precio){
        this.descripcion = desc;
        this.precio = precio;
    }
    public Producto(int id,string desc,int precio){
        this.idProducto = id;
        this.descripcion = desc;
        this.precio = precio;
    }

    public int IdProducto { get => idProducto;}
    public string Descripcion { get => descripcion;}
    public int Precio { get => precio;}
}