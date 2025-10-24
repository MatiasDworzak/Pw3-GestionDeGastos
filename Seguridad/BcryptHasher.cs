namespace Seguridad
{
   public interface IPasswordHasher
   {
      string Hash(string password);
      bool Verificar(string input, string hash);
   }
   public class BcryptHasher : IPasswordHasher
   {
      public string Hash(string password)
         => BCrypt.Net.BCrypt.HashPassword(password);
      

      public bool Verificar(string input, string hash)
         => BCrypt.Net.BCrypt.Verify(input, hash);
      
   }
}
