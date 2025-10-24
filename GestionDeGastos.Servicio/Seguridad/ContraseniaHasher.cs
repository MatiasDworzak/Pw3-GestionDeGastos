
namespace GestionDeGastos.Servicio.Seguridad
{
   public interface IContraseniaHasher
   {
      string Hash(string password);
      bool Verificar(string input, string hash);
   }
   internal class ContraseniaHasher : IContraseniaHasher
   {
      public string Hash(string password)
      {
         return BCrypt.Net.BCrypt.HashPassword(password);
      }

      public bool Verificar(string input, string hash)
      {
         return BCrypt.Net.BCrypt.Verify(input, hash);
      }
   }
}
