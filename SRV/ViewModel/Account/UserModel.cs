
namespace FFLTask.SRV.ViewModel.Account
{
    public class UserModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string AuthCode { get; set; }
        public bool HasLogon { get; set; }
        public bool IsAdmin { get; set; }
        public bool HasJoinedProject { get; set; }
    }
}
