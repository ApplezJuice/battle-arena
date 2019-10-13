using System.Collections;
using System.Collections.Generic;

namespace ArenaGame
{
    public class Buff : IBuff
    {
        public Buff(int spellID, string spellName)
        {
            this.spellID = spellID;
            this.spellName = spellName;
        }

        public int spellID { get; set; }
        public string spellName { get; set; }
    }

    public interface IBuff
    {
        int spellID { get; set; }
        string spellName { get; set; }
    }
}