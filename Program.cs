using System;

class Program
{
    // Klasa abstrakcyjna Bike
    abstract class Bike
    {
        // Pola
        int id;
        public int ID  // getter
        {
            get { return id; }
        }
        double price;
        public double Price  // getter
        {
            get { return price; }
        }

        // Konstruktor
        public Bike(double price)
        {
            // Losowanie ID roweru

            this.price = price;
        }
    }

    // Rower zwykły
    public class RegularBike : Bike
    {
        // Konstruktor
        public RegularBike(double price) : base(price) { }
    }

    // Rower dziecięcy
    public class ChildBike : Bike
    {
        // Konstruktor
        public RegularBike(double price) : base(price) { }
    }

    // Rower tandem
    public class TandemBike : Bike
    {
        // Konstruktor
        public RegularBike(double price) : base(price) { }
    }


    // Funkcja main
    static void Main(string[] args)
    {
        Test1.PowiedzCzesc();
        Test2.PowiedzHej();
    }
}