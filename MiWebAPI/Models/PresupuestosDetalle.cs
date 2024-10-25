public class PresupuestosDetalle
{
    private Producto producto;
    private int cantidad;

    public Producto Producto { get => producto;}
    public int Cantidad { get => cantidad;}

    public PresupuestosDetalle(){

    }
    public PresupuestosDetalle(Producto producto,int cantidad){
        this.producto = producto;
        this.cantidad = cantidad;
    }




}