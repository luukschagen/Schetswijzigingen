﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace SchetsEditor
{   public class SchetsControl : UserControl
    {   private Schets schets;
        private List<SchetsItem> startlijst;
        private List<SchetsItem> itemlijst;
        private Color penkleur;

        public Color PenKleur
        { get { return penkleur; } }

        internal List<SchetsItem> Itemlijst
        { get { return itemlijst; } }

        internal List<SchetsItem> Startlijst 
        { get => startlijst; set => startlijst = value; }

        public Schets Schets
        { get { return schets;   }
        }
        public SchetsControl()
        {   this.BorderStyle = BorderStyle.Fixed3D;
            this.schets = new Schets();
            this.itemlijst = new List<SchetsItem>();
            this.Paint += this.teken;
            this.Resize += this.veranderAfmeting;
            this.veranderAfmeting(null, null);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
        private void teken(object o, PaintEventArgs pea)
        {   schets.Teken(pea.Graphics);
        }
        private void veranderAfmeting(object o, EventArgs ea)
        {   schets.VeranderAfmeting(this.ClientSize);
            this.Invalidate();
        }
        public Graphics MaakBitmapGraphics()
        {   Graphics g = schets.BitmapGraphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            return g;
        }
        public void Schoon(object o, EventArgs ea)
        {   schets.Schoon();
            itemlijst.Clear();
            this.Invalidate();
        }
        public void Roteer(object o, EventArgs ea)
        {   schets.VeranderAfmeting(new Size(this.ClientSize.Height, this.ClientSize.Width));
            schets.Roteer();
            this.Invalidate();
        }
        public void VeranderKleur(object obj, EventArgs ea)
        {   string kleurNaam = ((ComboBox)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }
        public void VeranderKleurViaMenu(object obj, EventArgs ea)
        {   string kleurNaam = ((ToolStripMenuItem)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }

        //nieuwe zooi

        public void nieuweBitmap(Bitmap b)
        {   schets.MaakBitmap(b, this.ClientSize);
            this.Invalidate();
        }

        public Bitmap GeefBitmap
        {
            get { return schets.GeefBitmap; }
        }

        // om te vergelijken of er wijzigingen zijn gedaan

        internal void MaakStartlijst()
        {
            startlijst = new List<SchetsItem>();
            foreach (SchetsItem i in Itemlijst)
                startlijst.Add(i);

        }

    }   
}
