using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOG.Application.Interfaces.RepositoryInterfaces;
using BOOLOG.Domain.Model;
using BOOLOGAM.Domain.Model;
using BOOLOGAM.Infrastructure.Db_Context;
using Microsoft.EntityFrameworkCore;

namespace BOOLOG.Infrastructure.Repository
{
    public class UserprofileRepository (AppDbContext dbContext) : IUserprofileRepository
    {
        private readonly AppDbContext _appDbContext = dbContext;

        public async Task ApproveKyc (bool IsApproved, Guid Id, Guid UserId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(e => e.Id == UserId);
            var userProfile = await _appDbContext.UserProfiles.FirstOrDefaultAsync(up => up.Id == Id);
            if (IsApproved)
            {
                user.Role = Roles.Seller;
                userProfile.KycStatus = KycStatus.Approved;
                _appDbContext.SaveChanges();
            }
            else
            {
                userProfile.KycStatus = KycStatus.Rejected;
                _appDbContext.SaveChanges();
            }

        }
        public async Task<bool> BlockUnblockUser(Guid UserId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == UserId);
            user.IsBlocked = !user.IsBlocked;
            _appDbContext.SaveChanges();
            return user.IsBlocked;
        }
    }
}
