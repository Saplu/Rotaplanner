using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RotaplannerApi.Models
{
    public class Group
    {
        public int Id { get; set; }
        public int OpenGroup { get; set; }

        public Group(int id, int openGroup)
        {
            Id = id;
            OpenGroup = openGroup;
        }

        public Group()
        {

        }
    }
}
