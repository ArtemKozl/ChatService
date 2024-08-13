using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackChat.Core.Models
{
    public class ChatGroups
    {
        public ChatGroups( string name) 
        {

            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;


    }
}
