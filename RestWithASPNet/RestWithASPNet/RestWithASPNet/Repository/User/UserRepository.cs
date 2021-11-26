using RestWithASPNet.Data.VO;
using RestWithASPNet.Model.Context;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RestWithASPNet.Repository.User
{
    public class UserRepository : IUserRepository
    {

        private readonly MySQLContext _context;

        public UserRepository(MySQLContext context)
        {
            _context = context;
        }

        public Model.User ValidateCredentials(UserVO userVO)
        {
            var pass = ComputeHash(userVO.Password, new SHA256CryptoServiceProvider());
            return _context.User.FirstOrDefault(u => u.UserName == userVO.UserName && u.Password == pass);
        }

        public Model.User ValidateCredentials(string username)
        {
            return _context.User.SingleOrDefault(u => u.UserName == username);
        }
        public bool RevokeToken(string username)
        {
            var user =_context.User.SingleOrDefault(u => u.UserName == username);

            if (user is null) return false;

            user.RefreshToken = null;

            _context.SaveChanges();

            return true;
        }

        public Model.User RefreshUserInfo(Model.User user)
        {
            if (!_context.User.Any(u => u.Id.Equals(user.Id))) return null;

            var result = _context.User.SingleOrDefault(p => p.Id.Equals(user.Id));

            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            return result;
        }

        private string ComputeHash(string input, SHA256CryptoServiceProvider algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }

    }
}
