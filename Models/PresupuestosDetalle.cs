using System.Text.Json.Serialization;

public class PresupuestosDetalle
{
    private Product producto;
    private int cantidad;


    public PresupuestosDetalle(){

    }
    [JsonConstructor]
    public PresupuestosDetalle(Product producto,int cantidad){
        this.producto = producto;
        this.cantidad = cantidad;
    }

    public Product Producto { get => producto;}
    public int Cantidad { get => cantidad;}



}