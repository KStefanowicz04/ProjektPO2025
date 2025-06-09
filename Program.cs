using System;

class Program
{
    // Klasa abstrakcyjna Bike
    public abstract class Bike
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
            // Losowanie 4-numerowego ID roweru
            Random rand = new Random();
            id = rand.Next() % 9999;

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
        public ChildBike(double price) : base(price) { }
    }

    // Rower tandem
    public class TandemBike : Bike
    {
        // Konstruktor
        public TandemBike(double price) : base(price) { }
    }


    // Agregator stacji rowerowych
    public class StationsAggregation
    {
        // Lista wszystkich stacji
        List<BikeStation> stations = new List<BikeStation>();

        // Konstruktor
        public StationsAggregation() { }

        // Metody
        //
        // Dodaje podaną stację do listy stacji
        public void AddStation(BikeStation newStation)
        {
            stations.Add(newStation);
        }

        // Usuwa podaną stację z listy stacji
        public void RemoveStation(BikeStation removedStation)
        {
            stations.Remove(removedStation);
        }

        // Wypisuje informacje o stacjach w liście stacji
        public void GetStationsInfo()
        {
            // Jeśli w agregatorze nie ma żadnych stacji...
            if (stations.Count <= 0)
            {
                Console.WriteLine("W agregatorze nie ma żadnych stacji!");
                return;
            }

            // W przeciwnym wypadku...
            Console.WriteLine("Informacje o stacjach w tym agregatorze stacji:");
            foreach (BikeStation station in stations)
            {
                Console.WriteLine(station.ToString());
            }
            
        }
    }


    // Stacja rowerowa
    public class BikeStation
    {
        // Pola
        string? name;
        List<Bike> allBikes = new List<Bike>();
        int totalSlots;
        int freeSlots;
        public int FreeSlots  // getter
        {
            get { return freeSlots; }
        }

        // Konstruktor
        public BikeStation(string name, int totalSlots)
        {
            this.name = name;
            this.totalSlots = totalSlots;
            freeSlots = totalSlots - allBikes.Count();
        }

        // Metody
        //
        // Dodaje podany rower do stacji, jeśli jest miejsce
        public void AddBike(Bike newBike)
        {
            if (freeSlots > 0)
            {
                allBikes.Add(newBike);
                freeSlots = totalSlots - allBikes.Count();
            }
        }

        // Usuwa podany rower ze stacji
        public void RemoveBike(Bike removedBike)
        {
            allBikes.Remove(removedBike);
            freeSlots = totalSlots - allBikes.Count();
        }


        // Zwraca listę zwykłych rowerów
        public List<Bike> GetRegular()
        {
            List<Bike> regularBikes = new List<Bike>();  // Lista tymczasowa

            foreach (Bike bikeElement in allBikes)
            {
                // Jeśli dany rower jest zwykłym rowerem, zostaje dodany do zwracanej listy
                if (bikeElement is RegularBike)
                {
                    regularBikes.Add(bikeElement);
                    Console.WriteLine(bikeElement);
                }
            }

            return regularBikes;
        }

        // Zwraca listę rowerów dziecięcych
        public List<Bike> GetChild()
        {
            List<Bike> childBikes = new List<Bike>();

            foreach (Bike bikeElement in allBikes)
            {
                if (bikeElement is ChildBike)
                {
                    childBikes.Add(bikeElement);
                    Console.WriteLine(bikeElement);
                }
            }

            return childBikes;
        }

        // Zwraca listę rowerów tandem
        public List<Bike> GetTandem()
        {
            List<Bike> tandemBikes = new List<Bike>();

            foreach (Bike bikeElement in allBikes)
            {
                if (bikeElement is TandemBike)
                {
                    tandemBikes.Add(bikeElement);
                    Console.WriteLine(bikeElement);
                }
            }

            return tandemBikes;
        }

        // Zwraca informacje o stacji rowerowej
        public override string ToString()
        {
            return "Nazwa stacji: " + name + "\nIlosc miejsc: " + totalSlots + "\nIlosc wolnych miejsc: " + freeSlots;
        }
    }

    // Wypożyczenie roweru
    public class BikeRent
    {
        // Pola
        //
        // Rower dowolnego rodzaju
        Bike bicycle = new Bike;
        public Bike Bicycle   // getter
        {
            get { return bicycle; }
        }

        // Czas w którym rower został wypożyczony
        DateTime startTime;
        public DateTime StartTime  // getter
        {
            get { return startTime; }
        }

        // Czas w którym rower został oddany do stacji
        DateTime endTime;
        public DateTime EndTime  // getter, setter
        {
            get { return endTime; }
            set { endTime = value; }
        }

        // Stacja początkowa wypożyczenia, ustawiana przy wypożyczeniu
        BikeStation startStation;
        public BikeStation StartStation  // getter
        {
            get { return startStation; }
        }

        // Stacja końcowa wypożyczenia, ustawiana przy oddaniu rowera
        BikeStation endStation;
        public BikeStation EndStation  // getter, setter
        {
            get { return endStation; }
            set { endStation = value; }
        }

        // Kosz wypożyczenia roweru
        // Pole ma setter jako metodę
        double cost;
        public double Cost  // getter
        {
            get { return cost; }
        }

        // Konstruktor
        public BikeRent() { }

        // Metody
        //
        // Ustawienie ceny wypożyczenia roweru
        public void SetCost(string startTime, string endTime, double bikePrice)
        {
            // Cena = cena roweru + czas wypożyczenia, gdzie każda minuta kosztuje 2 grosze
            cost = bikePrice + (endTime - startTime) * 0.02;
        }

        // Zwraca string z informacjami o wypożyczeniu - informacje o rowerze, długość czasu wypożyczenia, stacja początkowa, stacja końcowa (jeśli jest)
        public override string ToString()
        {
            string returnString = ("Rower: " + bicycle.ToString() + "\nCzas wypożyczenia: " + (endTime - startTime) + "\nStacja początkowa: " + startStation.ToString());

            // Stacja końcowa może nie być zapisana, np. gdy rower jest jeszcze używany.
            if (endStation != null)
            {
                returnString += ("\nStacja końcowa: " + endStation.ToString());
            }

            return returnString;
        }
    }



    // Funkcja main
    static void Main(string[] args)
    {
        // Przykładowe rowery
        RegularBike rower1 = new RegularBike(20);
        RegularBike rower2 = new RegularBike(20);
        ChildBike crower1 = new ChildBike(20);
        ChildBike crower2 = new ChildBike(20);
        ChildBike crower3 = new ChildBike(20);

        // Stacja rowerowa
        BikeStation stacja = new BikeStation("Stacja 1", 6);
        stacja.AddBike(rower1);
        stacja.AddBike(rower2);
        stacja.AddBike(crower1);
        stacja.AddBike(crower2);
        stacja.AddBike(crower3);

        stacja.GetRegular();
        stacja.GetChild();
        stacja.GetTandem();
        Console.WriteLine("Free: " + stacja.FreeSlots);

        //Console.WriteLine(stacja.ToString());

        StationsAggregation agregatorStacji = new StationsAggregation();
        agregatorStacji.AddStation(stacja);
        agregatorStacji.GetStationsInfo();
        agregatorStacji.RemoveStation(stacja);
        agregatorStacji.GetStationsInfo();




        // Menu główne
        while (true)
        {
            Console.WriteLine("Wybierz operację:");
            Console.WriteLine("1: Zaloguj się\n2: Wyloguj się\n3: Zarejestruj się\n4: Wypożycz rower\n5: Historia użytkownika\n" +
                "6: Lista wypożyczalni rowerów\n7: Lista rowerów w danej wypożyczalni\n8: Zakończ program");

            string command = Console.ReadLine();


            switch (command)
            {
                case "1":  // Logowanie

                    break;

                case "2":  // Wylogowanie

                    break;

                case "3":  // Rejestracja

                    break;

                case "4":  // Wypożycz rower

                    break;

                case "5":  // Historia użytkownika

                    break;

                case "6":  // Lista wypożyczalni rowerów

                    break;

                case "7":  // Lista rowerów dostępnych w wypożyczalni

                    break;

                case "8":  // Koniec programu
                    return;

                default:
                    Console.WriteLine("BŁĄD! Wybierz poprawną operację!");
                    break;
            }

        }





    }
}