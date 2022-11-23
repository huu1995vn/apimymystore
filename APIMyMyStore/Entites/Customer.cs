
namespace APIMyMyStore.Entites
{
    public class Customer
    {
        public int id { get; set; }

        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public DateTime? updatedate { get; set; }
        public DateTime createdate { get; set; }

    }
}
