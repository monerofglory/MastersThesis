namespace MastersThesis
{
    //Class for player arguments
    class Argument
    {
        public string statement;
        public Player sender;
        public Player receiver;

        public Argument(string stat, Player p1, Player p2)
        {
            statement = stat;
            sender = p1;
            receiver = p2;
        }
    }
}
