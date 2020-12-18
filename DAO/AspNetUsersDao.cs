using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class AspNetUsersDao : DaoBase
    {
        #region Action
        public string Insert(AspNetUser item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.AspNetUsers.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(AspNetUser item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.AspNetUsers.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    //dbItem.Email = item.Email;
                    dbItem.Name = item.Name;
                    dbItem.Avatar = item.Avatar;
                    dbItem.Locataion = item.Locataion;
                    dbItem.EmailConfirmed = item.EmailConfirmed;
                    //dbItem.PasswordHash = item.PasswordHash;
                    dbItem.SecurityStamp = item.SecurityStamp;
                    dbItem.PhoneNumber = item.PhoneNumber;
                    dbItem.PhoneNumberConfirmed = item.PhoneNumberConfirmed;
                    dbItem.TwoFactorEnabled = item.TwoFactorEnabled;
                    dbItem.LockoutEndDateUtc = item.LockoutEndDateUtc;
                    dbItem.LockoutEnabled = item.LockoutEnabled;
                    dbItem.AccessFailedCount = item.AccessFailedCount;
                    //dbItem.UserName = item.UserName;
                    dbItem.Modified = item.Modified;
                    dbItem.ModifiedBy = item.ModifiedBy;
                    dbItem.Created = item.Created;
                    dbItem.CreatedBy = item.CreatedBy;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(AspNetUser item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.AspNetUsers.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.AspNetUsers.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public void ChangeStatus(AspNetUser item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.AspNetUsers.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.LockoutEndDateUtc = item.LockoutEndDateUtc;
                    dbItem.LockoutEnabled = item.LockoutEnabled;
                    dbContext.SubmitChanges();
                }
            }
        }

        #endregion
        #region Query
        public void Search(AspNetUsersParam param)
        {
            var filter = param.AspNetUsersFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUsers
                            where (string.IsNullOrEmpty(filter.Id) || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            && (string.IsNullOrEmpty(filter.UserName) || n.UserName.ToLower().Equals(filter.UserName.ToLower()))
                            && (filter.LockoutEnabled.HasValue == false || filter.LockoutEnabled == n.LockoutEnabled)
                            select new AspNetUsersEntity
                            {
                                Id = n.Id,
                                Email = n.Email,
                                Name = n.Name,
                                Avatar = n.Avatar,
                                Locataion = n.Locataion,
                                EmailConfirmed = n.EmailConfirmed,
                                PasswordHash = n.PasswordHash,
                                SecurityStamp = n.SecurityStamp,
                                PhoneNumber = n.PhoneNumber,
                                PhoneNumberConfirmed = n.PhoneNumberConfirmed,
                                TwoFactorEnabled = n.TwoFactorEnabled,
                                LockoutEndDateUtc = n.LockoutEndDateUtc,
                                LockoutEnabled = n.LockoutEnabled,
                                AccessFailedCount = n.AccessFailedCount,
                                UserName = n.UserName,
                                Modified = n.Modified,
                                ModifiedBy = n.ModifiedBy,
                                Created = n.Created,
                                CreatedBy = n.CreatedBy,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetUsersEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetUsersEntitys = query.ToList();
                }
            }
        }
        public void GetById(AspNetUsersParam param)
        {
            Search(param);
            if (param.AspNetUsersEntitys != null && param.AspNetUsersEntitys.Any())
            {
                param.AspNetUser = param.AspNetUsersEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
