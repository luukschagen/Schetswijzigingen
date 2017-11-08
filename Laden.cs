using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace SchetsEditor
{
    static class Laden
    {
        static public void laadhet(string filenaam, SchetsControl schetscontrol)
        {
            int x = 0;
            List < SchetsItem > lijst = schetscontrol.Itemlijst;
            StreamReader sr = new StreamReader(filenaam);
            string regel;
            string[] woorden;
            while ((regel = sr.ReadLine()) != null)
            {
                x++;
                woorden = regel.Split('|');
                Console.WriteLine(woorden.Length);
                Point beginpunt = TekstnaarPoint(woorden[1], woorden[2]);
                Color kleur = Color.FromName(woorden[3]);
                Font font = new Font("Tahoma", 40);

                if (woorden[0] == "TI")
                    lijst.Add(new TextItem(beginpunt, kleur, Convert.ToChar(woorden[4]), font));

                else 
                {    Point eindpunt = TekstnaarPoint(woorden[4], woorden[5]);

                    if (woorden[0] == "LI")
                        lijst.Add(new LijnItem(beginpunt, eindpunt, kleur));
                    
                    else { 
                        bool gevuld = Convert.ToBoolean(woorden[6]);

                        if (woorden[0] == "CI")
                            lijst.Add(new CirkelItem(beginpunt, eindpunt, gevuld, kleur));
                        
                        else if (woorden[0] == "RI")
                            lijst.Add(new RechthoekItem(beginpunt, eindpunt, gevuld, kleur));
                        
                    }

                }


               

            }
           
        }

          

        static private Point TekstnaarPoint(string xpunt, string ypunt)
        { return new Point(int.Parse(xpunt), int.Parse(ypunt));

        }


    }
}
