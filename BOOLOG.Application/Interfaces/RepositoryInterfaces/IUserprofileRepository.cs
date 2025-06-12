using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOLOG.Application.Interfaces.RepositoryInterfaces
{
    public interface IUserprofileRepository
    {
        Task ApproveKyc(bool IsApproved, Guid Id, Guid UserId);
        Task<bool> BlockUnblockUser(Guid UserId);
    }
}
