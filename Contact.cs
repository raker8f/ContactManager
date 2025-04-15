public class Contact
{
    public string Name {get; set;}
    public string Phone { get; set;}
    public Contact(string name, string Phone)
    {
        Name =name;
        Phone = Phone;
    }
    public void Display()
    {
        Console.WriteLine($"Name: {Name}, Phone: {Phone}");
    }

}