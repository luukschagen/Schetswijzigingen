using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SchetsEditor
{
    static class Opslaan
    {

        public static void opslaan(string filename, SchetsControl schetscontrol)
        {
            List<SchetsItem> itemlijst = schetscontrol.Itemlijst;

            // bitmapsetting
            using (StreamWriter s = new StreamWriter(filename))

                foreach (SchetsItem item in itemlijst)
            {
                    s.WriteLine(item.IteminTekst());
            }
         }
    }
}
