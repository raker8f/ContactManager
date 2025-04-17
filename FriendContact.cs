public class FriendContact : Contact
{
    public string Nickname { get; set; }

    public FriendContact(string name, string phone, string nickname)
        : base(name, phone)
    {
        Nickname = nickname;
    }

    public override void Display()
    {
        Console.WriteLine($"[Friend] {Nickname} ({Name}), Phone: {Phone}");
    }
}