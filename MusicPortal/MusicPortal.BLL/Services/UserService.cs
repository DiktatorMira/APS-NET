using MusicPortal.BLL.DTO;
using MusicPortal.DAL.Repositories;
using MusicPortal.DAL.Entities;
using AutoMapper;

namespace MusicPortal.BLL.Services {
    public interface IUserService {
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO> GetUserById(int userId);
        Task<UserDTO> GetUserByLogin(string login);
        Task<bool> IsLoginTaken(string login);
        Task AddUser(UserDTO model);
        void UpdateUser(UserDTO model);
        Task DeleteUser(int userId);
        Task Save();
    }
    public class UserService : IUserService {
        ISaveUnit db { get; set; }
        public UserService(ISaveUnit su) => db = su;
        public async Task<IEnumerable<UserDTO>> GetUsers() {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await db.Users.GetAll());
        }
        public async Task<UserDTO> GetUserById(int userId) {
            var user = await db.Users.GetById(userId);
            if (user == null) throw new ValidationException("Неверный пользователь!", "");
            return new UserDTO {
                Id = user.Id,
                Login = user.Login,
                FullName = user.FullName,
                Password = user.Password,
                Salt = user.Salt,
                IsAuthorized = user.IsAuthorized
            };
        }
        public async Task<UserDTO> GetUserByLogin(string login)  {
            var user = await db.Users.GetByStr(login);
            if (user == null) throw new ValidationException("Неверный пользователь!", "");
            return new UserDTO {
                Id = user.Id,
                Login = user.Login,
                FullName = user.FullName,
                Password = user.Password,
                Salt = user.Salt,
                IsAuthorized = user.IsAuthorized
            };
        }
        public async Task<bool> IsLoginTaken(string login) => await db.Users.IsStr(login);
        public async Task AddUser(UserDTO model) {
            await db.Users.Add(new User {
                Id = model.Id,
                Login = model.Login,
                FullName = model.FullName,
                Password = model.Password,
                Salt = model.Salt,
                IsAuthorized = model.IsAuthorized
            });
        }
        public void UpdateUser(UserDTO model) {
            db.Users.Update(new User {
                Id = model.Id,
                Login = model.Login,
                FullName = model.FullName,
                Password = model.Password,
                Salt = model.Salt,
                IsAuthorized = model.IsAuthorized
            });
        }
        public async Task DeleteUser(int userId) => await db.Users.Delete(userId);
        public async Task Save() => await db.Save();
    }
}