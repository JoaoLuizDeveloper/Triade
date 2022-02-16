using LubyTechAPI.Data;
using LubyTechAPI.Repository.IRepository;
using LubyTechModel.Models;
using LubyTechModel.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LubyTechAPI.Repository
{
    public class DeveloperRepository : Repository<Developer>, IDeveloperRepository
    {
        #region Construtor
        private readonly ApplicationDbContext _db;

        public DeveloperRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        #endregion

        public async Task<bool> AddHourToProject(Hour hour)
        {
            await _db.Hours.AddAsync(hour);
            return await _db.SaveChangesAsync() >= 0;
        }

        public async Task<ICollection<HourByDeveloper>> GetRankinfOfDevelopers()
        {
            var Contagem = new List<HourByDeveloper>();
            var devSevenDays = await _db.Developers.Include(d=> d.Hours).Where(x=> x.Hours.Where(h => h.Created > DateTime.Now.AddDays(-7)).Any()).ToListAsync();

            Parallel.ForEach(devSevenDays, (dev) =>
            {
                try
                {
                    if (dev.Hours != null && dev.Hours.Count > 0)
                    {
                        double GetWholeTime = 0;
                        var hoursDev = dev.Hours.Select(x => x.Time);

                        Parallel.ForEach(hoursDev, (h) =>
                        {
                            GetWholeTime += h;
                        });

                        Contagem.Add(new HourByDeveloper { IdDev = dev.Id, NameDev = dev.Name, AllTime = GetWholeTime });
                    }
                }
                catch (Exception)
                { }
            });

            var retorno = Contagem.OrderByDescending(o => o.AllTime).Take(5).ToList();
            return retorno;
        }

        public async Task<bool> CPFExists(long cpf)
        {
            var exist = await  _db.Developers.FirstOrDefaultAsync(x => x.CPF == cpf);

            if(exist == null)
            {
                return false;
            }
            else
            {
                return true;
            }             
        }

        public string GetToken()
        {
            // User found and generate Jwt Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY THING");
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, new Random().Next().ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
    }
}
