
using System;

class Program
{
class User
{
    private string Imie;
    private string Nazwisko;
    private string Numertel;
    private string Mail;
    private double Pieniadze;
    private List<BikeRent> Rent_Active;
    private List<BikeRent> Rent_History;
    public User(string Imie, string Nazwisko, string Numertel, string Mail, double Pieniadze = 0)
    {
        this.Imie = Imie;
        this.Nazwisko = Nazwisko;
        this.Numertel = Numertel;
        this.Mail = Mail;
        this.Pieniadze = Pieniadze;
        //tworzenie listy Rent_Active
        this.Rent_Active = new List<BikeRent>();
        //tworzenie listy Rent_History
        this.Rent_History = new List<BikeRent>();
    }
    public string getImie()
    {
        return Imie;
    }
    public string getNazwisko()
    {
        return Nazwisko;
    }
    public string getNumertel()
    {
        return Numertel;
    }
    public string getMail()
    {
        return Mail;
    }

        //poniższe tylko wtedy gdy zalogowany:
        public void Rent(int Bike_id, string Station_name, string Start_time)
        {
            BikeStation a = agregatorStacji.getStation(Station_name);
            if (a != null && this.Pieniadze > 0)
            {
                DateTime dataa;
                if (DateTime.TryParse(Start_time, out dataa))
                {

                }
                else
                {
                    Console.WriteLine("Niepoprawny format daty.");
                    return;
                }
                Bike rentowany = a.RemoveBike(Bike_id);
                if (rentowany != null)
                {
                    Rent_Active.Add(new BikeRent(rentowany, dataa, a));
                }
                else
                {
                    Console.WriteLine("Nie znaleziono tego roweru");
                }
            }
            else
            {
                Console.WriteLine("Nazwa stacji niepoprawna lub brakuje ci środków");
                return;
            }


        }
    public void Return_Bike(int Bike_id, string Station_name, string End_time)
    {
        BikeRent aa = null;
        for (int i = 0; i < Rent_Active.Count; i++)
        {
                //if (Rent_Active[i].rentBike(Bike_id) != null)
            if (Rent_Active[i].Bicycle.ID == Bike_id)
            {
                aa = Rent_Active[i];
                break;
            }
        }
        if (aa == null) { Console.WriteLine("Nie znaleziono wypożyczenia"); return; }
        BikeStation ostatnia = agregatorStacji.getStation(Station_name);
        if (ostatnia == null) { Console.WriteLine("Nie znaleziono stacji końcowej"); return; }
        if (ostatnia.FreeSlots > 0)
        {
            
                DateTime data;
                if (DateTime.TryParse(End_time, out data))
                {
                    aa.SetEndTime(data);
                }
                else
                {
                    Console.WriteLine("Niepoprawny format daty.");
                    return;
                }
                ostatnia.AddBike(aa.Bicycle);
            aa.SetEndStation(Station_name);
            aa.SetCost();
            Rent_Active.Remove(aa);

            Rent_History.Add(aa);
        }
        else
        {
            Console.WriteLine("Zabrakło wolnych miejsc na stacji rowerowej. Wybierz inną stację.");
            return;
        }

    }

    public void Deposit(double Value)
    {
        if (Value > 0)
        {
            this.Pieniadze += Value;
        }
        else
        {
            Console.WriteLine("Nieodpowiednia kwota");
        }
    }
    public void Display_Rent_Active()
    {
        Console.WriteLine("Aktywne wypożyczenia:");
        for (int i = 0; i < Rent_Active.Count; i++)
        {
            Console.WriteLine(Rent_Active[i].ToString());
        }
        if (Rent_Active.Count == 0) Console.WriteLine("brak");
    }
    public void Display_Rent_History()
    {
        Console.WriteLine("Niekatywne wypożyczenia:");
        for (int i = 0; i < Rent_History.Count; i++)
        {
            Console.WriteLine(Rent_History[i].ToString());
        }
        if (Rent_History.Count == 0) Console.WriteLine("brak");
    }
}

class Users_Manager
{
    private List<User> Users;
    private User CurrentUser;
    public Users_Manager()
    {
        this.Users = new List<User>();
        this.CurrentUser = null;
    }
    public void Register(string Imie, string Nazwisko, string Numertel, string Mail, double Pieniadze = 0)
    {
        for (int i = 0; i < Users.Count; i++)
        {
            if (Users[i].getImie() == Imie && Users[i].getNazwisko() == Nazwisko && (Users[i].getNumertel() == Numertel || Users[i].getMail() == Mail))
            {
                Console.WriteLine("Już posiadasz konto");
                return;
            }
        }
        if (Numertel.Length != 9) { Console.WriteLine("Niepoprawny numer telefonu"); return; }
        if (Mail.Contains("@") == true && Pieniadze >= 0)
        {
            Users.Add(new User(Imie, Nazwisko, Numertel, Mail, Pieniadze));
        }
        else
        {
            Console.WriteLine("Niepoprawna kwota lub adres e-mail");
        }
    }
    public void Login(string Imie, string Nazwisko, string Numertel, string Mail)
    {
        if (CurrentUser != null)
        {
            Console.WriteLine("Jesteś już zalogowany");
            return;
        }
        for (int i = 0; i < Users.Count; i++)
        {
            if (Users[i].getImie() == Imie && Users[i].getNazwisko() == Nazwisko && Users[i].getNumertel() == Numertel && Users[i].getMail() == Mail)
            {
                CurrentUser = Users[i];
                return;
            }
        }
        Console.WriteLine("Nie ma takiego użytkownika");
    }
    public void Logout()
    {
        CurrentUser = null;
    }

    public User getCurrentUser()
    {
        return CurrentUser;
    }
}

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
        public StationsAggregation()
        {}

        // Metody
        //
        public BikeStation getStation(string Name)
        {
            for (int i = 0; i < stations.Count;i++)
            {
                if (stations[i].getName() == Name)
                {
                return stations[i];
                }
            }
            Console.WriteLine("Nie ma takiej stacji");
            return null;   
        }
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
        private string name;
        private List<Bike> allBikes = new List<Bike>();
        private int totalSlots;
        private int freeSlots;
        public int FreeSlots  // getter
        {
            get { return freeSlots; }
        }

        // Konstruktor
        public BikeStation(string name, int totalSlots)
        {
            this.name = name;
            this.totalSlots = totalSlots;
            freeSlots = totalSlots - allBikes.Count;
        }

        // Metody
        //
        // Dodaje podany rower do stacji, jeśli jest miejsce
        public void AddBike(Bike newBike)
        {
            if (freeSlots > 0)
            {
                allBikes.Add(newBike);
                freeSlots = totalSlots - allBikes.Count;
            }
        }

        // Usuwa podany rower ze stacji
        public Bike RemoveBike(int removedBike_id)
        {
            int czy_znaleziono = 0;
            for (int i = 0; i < allBikes.Count; i++)
            {
                if (removedBike_id == allBikes[i].ID)
                {
                    Bike nasz = allBikes[i];
                    allBikes.RemoveAt(i);
                    freeSlots = totalSlots - allBikes.Count;
                    czy_znaleziono = 1;
                    return nasz;
                    break;
                }
            }
            if (czy_znaleziono == 0) Console.WriteLine("nie znaleziono takiego roweru");
            return null;
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

        public string getName()
        {
            return name;
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
        private Bike bicycle;
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
        string endStation;
        public string EndStation  // getter, setter
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
        public BikeRent(Bike bicycl, DateTime startTime, BikeStation startStation)
        {
            this.bicycle = bicycl;
            this.startTime = startTime;
            this.startStation = startStation;
        }


        // Metody
        //
        // Ustawienie ceny wypożyczenia roweru
        public void SetCost()
        {
            TimeSpan span = endTime - startTime;  // Czas od początku do końca wypożyczenia roweru.
            double totalTime = span.TotalMinutes;
            // Cena = cena roweru + czas wypożyczenia, gdzie każda minuta kosztuje 2 grosze
            cost = (totalTime * 0.02); /*bikePrice +*/ 
            
        }

        public void SetEndTime(DateTime EndTime2)
        {
            this.endTime = EndTime2;
        }

        public void SetEndStation(string EndStation2)
        {
            this.endStation = EndStation2;
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


    public static StationsAggregation agregatorStacji;
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

        agregatorStacji = new StationsAggregation();
        agregatorStacji.AddStation(stacja);
        agregatorStacji.GetStationsInfo();
        agregatorStacji.RemoveStation(stacja);
        agregatorStacji.GetStationsInfo();

        Users_Manager manago = new Users_Manager();

        string im, na, nr, ma,input,nazwa_stacji,czass;
        double cash;
        int idd;
        User currentt = null;

        // Menu główne
        while (true)
        {
            Console.WriteLine("Wybierz operację:");
            Console.WriteLine("1: Zaloguj się\n2: Wyloguj się\n3: Zarejestruj się\n4: Wypożycz rower\n5: Historia wypożyczeń użytkownika\n" +
                "6: Lista wypożyczalni rowerów\n7: Lista rowerów w danej wypożyczalni\n8: Zwróć rower \n9: Dodaj pieniądze na konto\n10: Aktywne wypożyczenia\n11: Zakończ program");

            string command = Console.ReadLine();


            switch (command)
            {
                case "1":  // Logowanie
                    Console.WriteLine("Podaj imię");
                    im = Console.ReadLine();
                    Console.WriteLine("Podaj nazwisko");
                    na = Console.ReadLine();
                    Console.WriteLine("Podaj numer telefonu");
                    nr = Console.ReadLine();
                    Console.WriteLine("Podaj mail");
                    ma = Console.ReadLine();
                    manago.Login(im, na, nr, ma);
                    break;

                case "2":  // Wylogowanie
                    manago.Logout();
                    break;

                case "3":  // Rejestracja
                    Console.WriteLine("Podaj imię");
                    im = Console.ReadLine();
                    Console.WriteLine("Podaj nazwisko");
                    na = Console.ReadLine();
                    Console.WriteLine("Podaj numer telefonu");
                    nr = Console.ReadLine();
                    Console.WriteLine("Podaj mail");
                    ma = Console.ReadLine();
                    Console.WriteLine("Jeśli chcesz, podaj sumę, którą chcesz wpłacić");
                    input = Console.ReadLine();
                    if (double.TryParse(input, out cash))
                    {

                    }
                    else
                    {
                        Console.WriteLine("Błędny format liczby!");
                        break;
                    }
                    manago.Register(im, na, nr, ma, cash);
                    break;

                case "4":  // Wypożycz rower
                           //public void Rent(int Bike_id, string Station_name, string Start_time)
                    Console.WriteLine("Podaj nr ID roweru");
                    input = Console.ReadLine();
                    if (int.TryParse(input, out idd))
                    {

                    }
                    else
                    {
                        Console.WriteLine("Błędnie wpisany numer ID");
                        break;
                    }
                    Console.WriteLine("Podaj nazwę stacji");
                    nazwa_stacji = Console.ReadLine();
                    Console.WriteLine("Podaj czas w formacie DateTime");
                    //ewentualnie tu można zrobić DateTime.Now
                    czass = Console.ReadLine();
                    currentt = manago.getCurrentUser();
                    currentt.Rent(idd, nazwa_stacji, czass);
                    

                    break;

                case "5":  // Historia wypożyczeń użytkownika
                    currentt = manago.getCurrentUser();
                    currentt.Display_Rent_History();
                    break;

                case "6":  // Lista wypożyczalni rowerów

                    break;

                case "7":  // Lista rowerów dostępnych w wypożyczalni

                    break;
                case "8": //Zwróc rower
                    Console.WriteLine("Podaj nr ID roweru");
                    input = Console.ReadLine();
                    if (int.TryParse(input, out idd))
                    {

                    }
                    else
                    {
                        Console.WriteLine("Błędnie wpisany numer ID");
                        break;
                    }
                    Console.WriteLine("Podaj nazwę stacji");
                    nazwa_stacji = Console.ReadLine();
                    Console.WriteLine("Podaj czas w formacie DateTime");
                    //ewentualnie tu można zrobić DateTime.Now
                    czass = Console.ReadLine();
                    currentt = manago.getCurrentUser();
                    currentt.Return_Bike(idd, nazwa_stacji, czass);
                    break;
                case "9": //Dodaj pieniadze na konto
                    currentt = manago.getCurrentUser();
                    Console.WriteLine("Podaj kwotę");
                    input = Console.ReadLine();
                    if (double.TryParse(input, out cash))
                    {

                    }
                    else
                    {
                        Console.WriteLine("Błędnie wpisana kwota");
                        break;
                    }
                    currentt.Deposit(cash);
                    break;
                case "10": //Aktywne wypożyczenia użytkownika
                currentt = manago.getCurrentUser();
                    currentt.Display_Rent_Active();
                    break;

                case "11":  // Koniec programu
                    return;

                default:
                    Console.WriteLine("BŁĄD! Wybierz poprawną operację!");
                    break;
            }

        }





    }
}
