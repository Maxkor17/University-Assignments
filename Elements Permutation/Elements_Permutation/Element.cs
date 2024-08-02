using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elements_Permutation
{
    class Element
    {
        public int ID;
        public bool pinned = false;
        public int numPinned;
        public int numConnections;

        public Element(int ID)
        {
            this.ID = ID;
        }

        public Element(int ID, bool pinned)
        {
            this.ID = ID;
            this.pinned = pinned;
        }
    }
}