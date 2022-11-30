namespace APIMyMyStore.Entites
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public DateTime? updatedate { get; set; }
        public DateTime createdate { get; set; }
        public int status { get; set; }
        public string token { get; set; }

    }
}
