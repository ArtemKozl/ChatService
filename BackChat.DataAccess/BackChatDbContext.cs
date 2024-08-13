using BackChat.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackChat.DataAccess
{
    public class BackChatDbContext : DbContext
    {
        public BackChatDbContext(DbContextOptions<BackChatDbContext> options)
            : base(options)
        {

        }
        public DbSet<ChatGroupsEntity> ChatGroups { get; set; }
        public DbSet<UserChatGroupsEntity> UserChatGroups { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
    }
}
