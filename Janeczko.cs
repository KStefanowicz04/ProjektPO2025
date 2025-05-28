using System;

public class Test2
{
    public static void PowiedzHej()
    {
        Console.WriteLine("Hej z Plik2!");
    }
}

class User
{
    private string Imie;
    private string Nazwisko;
    private string Numertel;
    private string Mail;
    private double Pieniadze;
    private List<Bike_Rent> Rent_Active;
    private List<Bike_Rent> Rent_History;
    public User(string Imie, string Nazwisko, string Numertel, string Mail, double Pieniadze = 0)
    {
        this.Imie = Imie;
        this.Nazwisko = Nazwisko;
        this.Numertel = Numertel;
        this.Mail = Mail;
        this.Pieniadze = Pieniadze;
        //tworzenie listy Rent_Active
        this.Rent_Active = new List<Bike_Rent>();
        //tworzenie listy Rent_History
        this.Rent_History = new List<Bike_Rent>();
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
        Bike_Station a = Agregat.getStation(Station_name);
        /*if (a != null && a.getBike(Bike_id) != null && this.Pieniadze>0)
        {
            Rent_Active.Add(new Bike_Rent(a.getBike(Bike_id), Start_time, a)); 
            //a.FreeSlots++;
        }*/

        //if(a== null || a.getBike(Bike_id)==null){Console.WriteLine("Nie znaleziono roweru lub stacji"); return ;}

    }
    public void Return_Bike(int Bike_id, string Station_name, string End_time)
    {
        Bike_Rent aa = null;
        for (int i = 0; i < Rent_Active.Count; i++)
        {
            if (Rent_Active[i].getBike(Bike_id) != null)
            {
                Bike_Rent aa = Rent_Active[i];
                break;
            }
        }
        if (aa == null) { Console.WriteLine("Nie znaleziono wypożyczenia"); return; }
        Bike_Station ostatnia = Agregat.getStation(Station_name);
        if (ostatnia == null) { Console.WriteLine("Nie znaleziono stacji końcowej"; return;)}
        if (ostatnia.getFreeSlots() == true)
        {
            Rent_Active.Remove(aa);
            aa.setEnd_Time(End_time);
            aa.setEnd_Station(Station_name);
            aa.setCost();

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