using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using System.IO;
using System.Linq;

namespace SchetsEditor
{
    public class SchetsWin : Form
    {   
        MenuStrip menuStrip;
        SchetsControl schetscontrol;
        ISchetsTool huidigeTool;
        Panel paneel;
        bool vast;
        ResourceManager resourcemanager
            = new ResourceManager("SchetsEditor.Properties.Resources"
                                 , Assembly.GetExecutingAssembly()
                                 );

        public SchetsControl Schetscontrol
        {
            get
            {
                return schetscontrol;
            }

            set
            {
                schetscontrol = value;
            }
        }

        private void veranderAfmeting(object o, EventArgs ea)
        {
            schetscontrol.Size = new Size ( this.ClientSize.Width  - 70
                                          , this.ClientSize.Height - 50);
            paneel.Location = new Point(64, this.ClientSize.Height - 30);
            Graphics g = schetscontrol.MaakBitmapGraphics();

            foreach (SchetsItem i in schetscontrol.Itemlijst)
            {
                i.Tekenitem(g);
            }
            schetscontrol.Invalidate();
        }

        private void klikToolMenu(object obj, EventArgs ea)
        {
            this.huidigeTool = (ISchetsTool)((ToolStripMenuItem)obj).Tag;
        }

        private void klikToolButton(object obj, EventArgs ea)
        {
            this.huidigeTool = (ISchetsTool)((RadioButton)obj).Tag;
        }

        private void afsluiten(object obj, EventArgs ea)
        {
                this.Close();
        }

        public SchetsWin()
        {
            ISchetsTool[] deTools = { new PenTool()         
                                    , new LijnTool()
                                    , new RechthoekTool()
                                    , new VolRechthoekTool()
                                    , new TekstTool()
                                    , new GumTool()
                                    , new CirkelTool()
                                    , new BolTool()
                                    };
            String[] deKleuren = { "Black", "Red", "Green", "Blue"
                                 , "Yellow", "Magenta", "Cyan" 
                                 };

            this.ClientSize = new Size(700, 500);
            huidigeTool = deTools[0];

            schetscontrol = new SchetsControl();
            schetscontrol.Location = new Point(64, 10);
            schetscontrol.MouseDown += (object o, MouseEventArgs mea) =>
                                       {   vast=true;  
                                           huidigeTool.MuisVast(schetscontrol, mea.Location); 
                                       };
            schetscontrol.MouseMove += (object o, MouseEventArgs mea) =>
                                       {   if (vast)
                                           huidigeTool.MuisDrag(schetscontrol, mea.Location); 
                                       };
            schetscontrol.MouseUp   += (object o, MouseEventArgs mea) =>
                                       {   if (vast)
                                           huidigeTool.MuisLos (schetscontrol, mea.Location);
                                           vast = false; 
                                       };
            schetscontrol.KeyPress +=  (object o, KeyPressEventArgs kpea) => 
                                       {   huidigeTool.Letter  (schetscontrol, kpea.KeyChar); 
                                       };

            this.FormClosing += Handle_FormClosing;
            this.Controls.Add(schetscontrol);

            menuStrip = new MenuStrip();
            menuStrip.Visible = false;
            this.Controls.Add(menuStrip);
            this.maakFileMenu();
            this.maakToolMenu(deTools);
            this.maakAktieMenu(deKleuren);
            this.maakToolButtons(deTools);
            this.maakAktieButtons(deKleuren);
            this.Resize += this.veranderAfmeting;
            this.veranderAfmeting(null, null);
        }

        private void maakFileMenu()
        {   
            ToolStripMenuItem menu = new ToolStripMenuItem("File");
            menu.MergeAction = MergeAction.MatchOnly;  
            menu.DropDownItems.Add("Opslaan Als Afbeelding", null, this.OpslaanAls);
            menu.DropDownItems.Add("Opslaan Als Text", null, this.OpslaanAlsText);
            menu.DropDownItems.Add("Sluiten", null, this.afsluiten);
            menuStrip.Items.Add(menu);
        }

        private void maakToolMenu(ICollection<ISchetsTool> tools)
        {   
            ToolStripMenuItem menu = new ToolStripMenuItem("Tool");
            foreach (ISchetsTool tool in tools)
            {   ToolStripItem item = new ToolStripMenuItem();
                item.Tag = tool;
                item.Text = tool.ToString();
                item.Image = (Image)resourcemanager.GetObject(tool.ToString());
                item.Click += this.klikToolMenu;
                menu.DropDownItems.Add(item);
            }
            menuStrip.Items.Add(menu);
        }

        private void maakAktieMenu(String[] kleuren)
        {   
            ToolStripMenuItem menu = new ToolStripMenuItem("Aktie");
            menu.DropDownItems.Add("Clear", null, schetscontrol.Schoon );
            menu.DropDownItems.Add("Roteer", null, schetscontrol.Roteer );
            ToolStripMenuItem submenu = new ToolStripMenuItem("Kies kleur");
            foreach (string k in kleuren)
                submenu.DropDownItems.Add(k, null, schetscontrol.VeranderKleurViaMenu);
            menu.DropDownItems.Add(submenu);
            menuStrip.Items.Add(menu);
        }

        private void maakToolButtons(ICollection<ISchetsTool> tools)
        {
            int t = 0;
            foreach (ISchetsTool tool in tools)
            {
                RadioButton b = new RadioButton();
                b.Appearance = Appearance.Button;
                b.Size = new Size(45, 62);
                b.Location = new Point(10, 10 + t * 62);
                b.Tag = tool;
                b.Text = tool.ToString();
                b.Image = (Image)resourcemanager.GetObject(tool.ToString());
                b.TextAlign = ContentAlignment.TopCenter;
                b.ImageAlign = ContentAlignment.BottomCenter;
                b.Click += this.klikToolButton;
                this.Controls.Add(b);
                if (t == 0) b.Select();
                t++;
            }
        }

        private void maakAktieButtons(String[] kleuren)
        {   
            paneel = new Panel();
            paneel.Size = new Size(600, 24);
            this.Controls.Add(paneel);
            
            Button b; Label l; ComboBox cbb;
            b = new Button(); 
            b.Text = "Clear";  
            b.Location = new Point(  0, 0); 
            b.Click += schetscontrol.Schoon; 
            paneel.Controls.Add(b);
            
            b = new Button(); 
            b.Text = "Rotate"; 
            b.Location = new Point( 80, 0); 
            b.Click += schetscontrol.Roteer; 
            paneel.Controls.Add(b);
            
            l = new Label();  
            l.Text = "Penkleur:"; 
            l.Location = new Point(180, 3); 
            l.AutoSize = true;               
            paneel.Controls.Add(l);
            
            cbb = new ComboBox(); cbb.Location = new Point(240, 0); 
            cbb.DropDownStyle = ComboBoxStyle.DropDownList; 
            cbb.SelectedValueChanged += schetscontrol.VeranderKleur;
            foreach (string k in kleuren)
                cbb.Items.Add(k);
            cbb.SelectedIndex = 0;
            paneel.Controls.Add(cbb);
        }

        // NIEUW

        private void OpslaanAls(object o, EventArgs ea)
        {
            SaveFileDialog dialoog = new SaveFileDialog();
            dialoog.Filter = "Image Files(*.jpg; *.jpeg; *.bmp)|*.jpg; *.jpeg; *.bmp";
            dialoog.Title = "Schets opslaan als";

            DialogResult result = dialoog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.Text = dialoog.FileName;
                this.SchrijfNaarFile();
            }
            schetscontrol.MaakStartlijst();

        }

        private void OpslaanAlsText(object o, EventArgs ea)
        {
            SaveFileDialog dialoog = new SaveFileDialog();
            dialoog.Filter = "Tekstfiles|*.txt|Alle files|*.*";
            dialoog.Title = "Tekst Opslaan";

            if (dialoog.ShowDialog() == DialogResult.OK)
            {
                this.Text = dialoog.FileName;
                Opslaan.opslaan(dialoog.FileName, schetscontrol);
            }
            schetscontrol.MaakStartlijst();

        }

        private void OpslaanBitmap(object o, EventArgs ea)
        {
            if( this.Text == "")
            OpslaanAls(o, ea);

            else SchrijfNaarFile();

        }

        private void SchrijfNaarFile()
        {
            if (this.Text.EndsWith(".txt", StringComparison.Ordinal))
                OpslaanAlsText(null, null);
            else
            {
                Bitmap bitmap = schetscontrol.GeefBitmap;
                bitmap.Save(this.Text);
            }
            schetscontrol.MaakStartlijst();

        }
        public void MaakbitmapvanFile(Bitmap b)

        {
            schetscontrol.nieuweBitmap(b);

        }

        // waarschuwing voor sluitend scherm
        void Handle_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!schetscontrol.Itemlijst.SequenceEqual(schetscontrol.Startlijst))
            {
                DialogResult result = MessageBox.Show("Er zijn niet opgeslagen wijzigingen. " +
                                    "Wilt u deze alsnog opslaan?", "Opslaan",
                                                  MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel)
                    e.Cancel = true;

                else if (result == DialogResult.Yes)
                    OpslaanBitmap(null, new EventArgs());
                    
                
                    
            }
        }
    }
}
