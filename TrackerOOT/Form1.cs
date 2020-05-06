﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackerOOT
{
    public partial class Form1 : Form
    {
        SortedSet<String> placesAlphabeticalOrder = new SortedSet<String>();
        SortedSet<String> sometimesHints = new SortedSet<string>();
        List<String> listDungeons = new List<string>();

        List<Image> strengthUpgrade = new List<Image>();
        List<Image> hookUpgrade = new List<Image>();
        List<Image> scaleUpgrade = new List<Image>();
        List<Image> magicUpgrade = new List<Image>();
        List<Image> walletUpgrade = new List<Image>();
        List<Image> guarenteedHintsUpgrade = new List<Image>();
        List<Image> sometimesHintsUpgrade = new List<Image>();
        List<Image> wothItems = new List<Image>();

        Dictionary<Label, int> wothPosition = new Dictionary<Label, int>();
        List<Label> barrenPosition = new List<Label>();
        


        bool isMouseDown = false;

        bool isColoredGreenMedaillon = false;
        bool isColoredRedMedaillon = false;
        bool isColoredBlueMedaillon = false;
        bool isColoredOrangeMedaillon = false;
        bool isColoredPurpleMedaillon = false;
        bool isColoredYellowMedaillon = false;
        bool isColoredKokiriStone = false;
        bool isColoredGoronStone = false;
        bool isColoredZoraStone = false;

        bool isColoredDinsFire = false;
        bool isColoredFaroresWind = false;
        String[] imageTagList_dins_farores =
        {
            "dins_farores_bw",
            "half_dins_fire",
            "half_farores_wind",
            "dins_farores"            
        };

        bool isColoredIronBoots = false;
        bool isColoredHoverBoots = false;
        String[] imageTagList_iron_hover_boots =
        {
            "iron_hover_boots_bw",
            "half_iron_boots",
            "half_hover_boots",
            "iron_hover_boots"
        };

        bool isColoredGoronTunic = false;
        bool isColoredZoraTunic = false;
        String[] imageTagList_goron_zora_tunic =
        {
            "goron_zora_tunic_bw",
            "half_goron_tunic",
            "half_zora_tunic",
            "goron_zora_tunic"
        };

        bool isColoredFireArrow = false;
        bool isColoredLightArrow = false;
        String[] imageTagList_fire_light_arrow =
        {
            "fire_light_arrow_bw",
            "half_fire_arrow",
            "half_light_arrow",
            "fire_light_arrow"
        };


        bool chronoRunning = false;

        Stopwatch chrono = new Stopwatch();

        public Form1()
        {
            InitializeComponent();
        }

        private void autocomplete()
        {
            //Textbox AutoComplete
            var source = new AutoCompleteStringCollection();
            source.AddRange(placesAlphabeticalOrder.ToArray());

            comboBox_places.AutoCompleteCustomSource = source;
            comboBox_places.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox_places.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var source_hints = new AutoCompleteStringCollection();
            source_hints.AddRange(sometimesHints.ToArray());

            textBox1.AutoCompleteCustomSource = source_hints;
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

            textBox2.AutoCompleteCustomSource = source_hints;
            textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

            textBox3.AutoCompleteCustomSource = source_hints;
            textBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;

            textBox4.AutoCompleteCustomSource = source_hints;
            textBox4.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox4.AutoCompleteSource = AutoCompleteSource.CustomSource;

            textBox5.AutoCompleteCustomSource = source_hints;
            textBox5.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox5.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();

            listDungeons = new List<string>
            {
                "????",
                "FREE",
                "DEKU",
                "DC",
                "JABU",
                "FOREST",
                "FIRE",
                "WATER",
                "SPIRIT",
                "SHADOW"
            };
            this.label_green_medaillon.Text = listDungeons[0];

            placesAlphabeticalOrder.Add("");
            JObject json_places = JObject.Parse(File.ReadAllText(@"oot_places.json"));
            foreach(var categorie in json_places)
            {
                foreach(var name in categorie.Value)
                {
                    placesAlphabeticalOrder.Add(name.ToString());
                }
            }
            loadComboBoxData(placesAlphabeticalOrder.ToArray());

            JObject json_hints = JObject.Parse(File.ReadAllText(@"sometimes_hints.json"));
            foreach (var categorie in json_hints)
            {
                foreach (var hint in categorie.Value)
                {
                    sometimesHints.Add(hint.ToString());
                }
            }

            autocomplete();

            pictureBox_oot_hint.AllowDrop = true;
            pictureBox_30skulls.AllowDrop = true;
            pictureBox_40skulls.AllowDrop = true;
            pictureBox_50skulls.AllowDrop = true;
            pictureBox_skullMask.AllowDrop = true;
            pictureBox_biggoron.AllowDrop = true;
            pictureBox_frogs.AllowDrop = true;

            pictureBox_sometimesHint1.AllowDrop = true;
            pictureBox_sometimesHint2.AllowDrop = true;
            pictureBox_sometimesHint3.AllowDrop = true;
            pictureBox_sometimesHint4.AllowDrop = true;
            pictureBox_sometimesHint5.AllowDrop = true;

            setListUpgrade();
            setPictureBoxTag();

            //Count Skulltulas
            Label nb_skulls = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Dock = DockStyle.None,
                Margin = new Padding(0,0,0,0),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "00",
                Font = new Font("Consolas", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Width = 42,
                Height = 32,
                AutoSize = false,
                Location = new Point(-3, -5),
                TextAlign = ContentAlignment.MiddleLeft
            };
            nb_skulls.MouseDown += new MouseEventHandler(label_collectedSkulls_MouseDown);
            
            pictureBox_collectedSkulls.Controls.Add(nb_skulls);

            comboBox_places.KeyDown += changeCollectedSkulls;
            textBox1.KeyDown += changeCollectedSkulls;
            textBox2.KeyDown += changeCollectedSkulls;
            textBox3.KeyDown += changeCollectedSkulls;
            textBox4.KeyDown += changeCollectedSkulls;
            textBox5.KeyDown += changeCollectedSkulls;
            button_woth.KeyDown += changeCollectedSkulls;
            button_barren.KeyDown += changeCollectedSkulls;
        }

        private void changeCollectedSkulls(object sender, KeyEventArgs k)
        {
            if(k.KeyCode == Keys.F11)
            {
                label_collectedSkulls_MouseDown(pictureBox_collectedSkulls.Controls[0], new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
            }
            if (k.KeyCode == Keys.F12)
                label_collectedSkulls_MouseDown(pictureBox_collectedSkulls.Controls[0], new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0));
        }

        private void setListUpgrade()
        {
            strengthUpgrade = new List<Image>
            {
                Properties.Resources.strength_bw,
                Properties.Resources.strength,
                Properties.Resources.strength2,
                Properties.Resources.strength3
            };
            this.pictureBox_strength.Image = strengthUpgrade[0];

            hookUpgrade = new List<Image>
            {
                Properties.Resources.hookshot_bw,
                Properties.Resources.hookshot_,
                Properties.Resources.longshot_,
            };
            this.pictureBox_hookshot.Image = hookUpgrade[0];

            scaleUpgrade = new List<Image>
            {
                Properties.Resources.scale_bw,
                Properties.Resources.scale,
                Properties.Resources.golden_scale
            };
            this.pictureBox_scale.Image = scaleUpgrade[0];

            magicUpgrade = new List<Image>
            { 
                Properties.Resources.magic_bw,
                Properties.Resources.magic,
                Properties.Resources.double_magic,
            };
            this.pictureBox_magic.Image = magicUpgrade[0];

            walletUpgrade = new List<Image>
            {
                 Properties.Resources.wallet,
                 Properties.Resources.wallet2,
                 Properties.Resources.wallet3,
            };
            this.pictureBox_wallet.Image = walletUpgrade[0];

            guarenteedHintsUpgrade = new List<Image>
            {
                Properties.Resources.gossip_stone_2,
                Properties.Resources.sold_out,
                Properties.Resources.bottle_empty,
                Properties.Resources.bottle_big_poe
                
            };
            this.pictureBox_30skulls.Image = guarenteedHintsUpgrade[0];
            this.pictureBox_40skulls.Image = guarenteedHintsUpgrade[0];
            this.pictureBox_50skulls.Image = guarenteedHintsUpgrade[0];
            this.pictureBox_skullMask.Image = guarenteedHintsUpgrade[0];
            this.pictureBox_biggoron.Image = guarenteedHintsUpgrade[0];
            this.pictureBox_frogs.Image = guarenteedHintsUpgrade[0];

            sometimesHintsUpgrade = new List<Image>
            {
                Properties.Resources.gossip_stone_2,
                Properties.Resources.sold_out,
                Properties.Resources.key,
                Properties.Resources.bk,
                Properties.Resources.bottle_empty,
                Properties.Resources.bottle_big_poe
            };
            this.pictureBox_sometimesHint1.Image = sometimesHintsUpgrade[0];
            this.pictureBox_sometimesHint2.Image = sometimesHintsUpgrade[0];
            this.pictureBox_sometimesHint3.Image = sometimesHintsUpgrade[0];
            this.pictureBox_sometimesHint4.Image = sometimesHintsUpgrade[0];
            this.pictureBox_sometimesHint5.Image = sometimesHintsUpgrade[0];

            wothItems = new List<Image>
            {
                Properties.Resources.gossip_stone_2,
                Properties.Resources.bottle_empty,
                Properties.Resources.bottle_big_poe
            };

        }
        private void setPictureBoxTag()
        {
            pictureBox_bombs.Image.Tag = "bombs";
            pictureBox_bow.Image.Tag = "bow";
            pictureBox_fire_light_arrow.Image.Tag = "fire_light_arrow";
            pictureBox_dins_farores.Image.Tag = "dins_farores";
            pictureBox_slingshot.Image.Tag = "slingshot";
            pictureBox_bchu.Image.Tag = "bomb_chu";
            pictureBox_hookshot.Image.Tag = "hookshot";
            pictureBox_boomerang.Image.Tag = "boomerang";
            pictureBox_hammer.Image.Tag = "hammer";
            pictureBox_bottle1.Image.Tag = "bottle_rutos_letter";
            pictureBox_lens.Image.Tag = "lens";
            pictureBox_mirror_shield.Image.Tag = "mirror_shield";
            pictureBox_goron_zora_tunic.Image.Tag = "goron_zora_tunic";
            pictureBox_iron_hover_boots.Image.Tag = "iron_hover_boots";
            pictureBox_magic.Image.Tag = "magic";
            pictureBox_scale.Image.Tag = "scale";
            pictureBox_strength.Image.Tag = "strength";
            pictureBox_kokiri_stone.Image.Tag = "kokiri_stone";
            pictureBox_goron_stone.Image.Tag = "goron_stone";
            pictureBox_zora_stone.Image.Tag = "zora_stone";
            pictureBox_wallet.Image.Tag = "wallet";

            //songs
            pictureBox_ZL.Image.Tag = "zeldas_lullaby";
            pictureBox_epona.Image.Tag = "epona";
            pictureBox_saria.Image.Tag = "saria";
            pictureBox_suns_song.Image.Tag = "suns_song";
            pictureBox_song_of_time.Image.Tag = "song_of_time";
            pictureBox_song_of_storms.Image.Tag = "song_of_storms";
            pictureBox_minuet.Image.Tag = "minuet";
            pictureBox_bolero.Image.Tag = "bolero";
            pictureBox_serenade.Image.Tag = "serenade";
            pictureBox_requiem.Image.Tag = "requiem";
            pictureBox_nocturne.Image.Tag = "nocturne";
            pictureBox_prelude.Image.Tag = "prelude";

            //medaillons
            pictureBox_green_medaillon.Image.Tag = "green_medaillon";
            pictureBox_red_medaillon.Image.Tag = "red_medaillon";
            pictureBox_blue_medaillon.Image.Tag = "blue_medaillon";
            pictureBox_orange_medaillon.Image.Tag = "orange_medaillon";
            pictureBox_purple_medaillon.Image.Tag = "purple_medaillon";
            pictureBox_yellow_medaillon.Image.Tag = "yellow_medaillon";

            pictureBox_go_mode.Image.Tag = "go_mode";

            //guarantedHints
            pictureBox_30skulls.Image.Tag = "gossip_stone_2";
            pictureBox_40skulls.Image.Tag = "gossip_stone_2";
            pictureBox_50skulls.Image.Tag = "gossip_stone_2";
            pictureBox_skullMask.Image.Tag = "gossip_stone_2";
            pictureBox_biggoron.Image.Tag = "gossip_stone_2";
            pictureBox_frogs.Image.Tag = "gossip_stone_2";

            //sometimesHints
            pictureBox_sometimesHint1.Image.Tag = "gossip_stone_2";
            pictureBox_sometimesHint2.Image.Tag = "gossip_stone_2";
            pictureBox_sometimesHint3.Image.Tag = "gossip_stone_2";
            pictureBox_sometimesHint4.Image.Tag = "gossip_stone_2";
            pictureBox_sometimesHint5.Image.Tag = "gossip_stone_2";
        }
        private void loadComboBoxData(String[] data)
        {
            comboBox_places.Items.Clear();
            comboBox_places.Items.AddRange(data);
        }

        #region combobox DropDownClosed
        void comboBox_woth1_DropDownClosed(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() => { comboBox_places.Select(0, 0); }));
        }
        #endregion

        #region chrono and timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan time = chrono.Elapsed;
            label1.Text = time.ToString(@"hh\:mm\:ss\.ff");
        }

        private void button_chrono_Click(object sender, EventArgs e)
        {
            if (chronoRunning)
            {
                chrono.Stop();
                button_chrono.Text = "Start";
            }
            else
            {
                chrono.Start();
                button_chrono.Text = "Stop";
            }

            chronoRunning = !chronoRunning;
        }

        private void button_chrono_reset_Click(object sender, EventArgs e)
        {
            chronoRunning = false;
            chrono.Reset();
            button_chrono.Text = "Start";
        }
        #endregion

        #region DragEnter
        private void object_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }
        #endregion

        #region DragDrop
        private void panel_DragDrop(object sender, DragEventArgs e)
        {
            var coord = new Point();
            var panel = (Panel)sender;

            for (int i = 0; i < panel.Controls.Count; i++)
            {
                coord = new Point(panel.Controls[i].Location.X + 32, panel.Controls[i].Location.Y);
            }

            var image = new PictureBox();
            var tag = ((Bitmap)e.Data.GetData(DataFormats.Bitmap)).Tag.ToString().Replace("_bw", "");
            image.Size = new Size(32, 32);
            image.Image = (Image)Properties.Resources.ResourceManager.GetObject(tag);
            image.Tag = tag;
            image.Location = coord;
            
            image.MouseClick += new MouseEventHandler(imageWoth_RightClick);

            panel.Controls.Add(image);
        }
        private void imageWoth_RightClick(object sender, MouseEventArgs e)
        {
            var image = ((PictureBox)sender);
            if (e.Button == MouseButtons.Right)
                image.Parent.Controls.Remove(image);
        }

        private void pictureBox_DragDrop(object sender, DragEventArgs e)
        {
            var imageTag = ((Bitmap)e.Data.GetData(DataFormats.Bitmap)).Tag.ToString().Replace("_bw", "");
            var imageColor = (Image)Properties.Resources.ResourceManager.GetObject(imageTag);
            ((PictureBox)sender).Image = imageColor;
            ((PictureBox)sender).Image.Tag = imageTag;
        }
        #endregion

        #region MouseDown
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Clicks != 1)
                isMouseDown = false;
            else isMouseDown = true;
        }
        #endregion

        #region MouseUp
        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            var pBox = (PictureBox)sender;
            var imageTag = pBox.Image.Tag.ToString();
            var imageColor = (Image)Properties.Resources.ResourceManager.GetObject(imageTag);
            var imageBlackWhite = (Image)Properties.Resources.ResourceManager.GetObject(imageTag + "_bw");
            isMouseDown = false;
            
            if (e.Button == MouseButtons.Left)
            {
                pBox.Image = imageColor;
            }
            if (e.Button == MouseButtons.Right)
                pBox.Image = imageBlackWhite;

            pBox.Image.Tag = imageTag;
        }

        private void pictureBox_MouseUp_Upgrade(PictureBox pBox, MouseButtons mouseButton, List<Image> listUpgrade)
        {
            var imageTag = pBox.Image.Tag.ToString();
            isMouseDown = false;

            if (mouseButton == MouseButtons.Left)
            {
                var index = listUpgrade.IndexOf(pBox.Image);
                if (index == listUpgrade.Count - 1)
                    pBox.Image = listUpgrade[0];
                else pBox.Image = listUpgrade[index + 1];
            }
            if (mouseButton == MouseButtons.Right)
            {
                var index = listUpgrade.IndexOf(pBox.Image);
                if(index < 0)
                    pBox.Image = listUpgrade[0];
                else if (index == 0)
                    pBox.Image = listUpgrade[listUpgrade.Count - 1];
                else pBox.Image = listUpgrade[index - 1];
            }
            pBox.Image.Tag = imageTag;
        }
        
        private void pictureBox_strength_MouseUp_Upgrade(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, strengthUpgrade);
        }

        private void pictureBox_scale_MouseUp_Upgrade(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, scaleUpgrade);
        }
             
        private void pictureBox_hookshot_MouseUp_Upgrade(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, hookUpgrade);
        }

        private void pictureBox_magic_MouseUp_Upgrade(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, magicUpgrade);
        }

        private void pictureBox_wallet_MouseUp_Upgrade(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, walletUpgrade);
        }
        
        private void medaillons_MouseUp(PictureBox pBox, MouseButtons mouseButton, ref bool isColored, Label label_medaillon)
        {
            var imageTag = pBox.Image.Tag.ToString();
            var imageColor = (Image)Properties.Resources.ResourceManager.GetObject(imageTag);
            var imageBlackWhite = (Image)Properties.Resources.ResourceManager.GetObject(imageTag + "_bw");
            isMouseDown = false;

            if (mouseButton == MouseButtons.Left)
            {
                if (isColored) pBox.Image = imageBlackWhite;
                else pBox.Image = imageColor;
                isColored = !isColored;
            }
            if (mouseButton == MouseButtons.Right)
            {
                var index = listDungeons.IndexOf(label_medaillon.Text);
                if (index == listDungeons.Count-1)
                    label_medaillon.Text = listDungeons[0];
                else label_medaillon.Text = listDungeons[index + 1];
            }

            pBox.Image.Tag = imageTag;
        }

        private void doubleItems_MouseUp(PictureBox pBox, MouseButtons mouseButton, ref bool item1, ref bool item2, String[] imageTagList)
        {
            String imageTag = string.Empty;          

            if (mouseButton == MouseButtons.Left)
            {
                if (!item1 && !item2)
                {
                    imageTag = imageTagList[1];
                    item1 = true;
                }
                else if (item1 && !item2)
                {
                    imageTag = imageTagList[0];
                    item1 = false;
                }
                else if (!item1 && item2)
                {
                    imageTag = imageTagList[3];
                    item1 = true;
                }
                else if (item1 && item2)
                {
                    imageTag = imageTagList[2];
                    item1 = false;
                }

            }
            if (mouseButton == MouseButtons.Right)
            {
                if (!item1 && !item2)
                {
                    imageTag = imageTagList[2];
                    item2 = true;
                }
                else if (item1 && !item2)
                {
                    imageTag = imageTagList[3];
                    item2 = true;
                }
                else if (!item1 && item2)
                {
                    imageTag = imageTagList[0];
                    item2 = false;
                }
                else if (item1 && item2)
                {
                    imageTag = imageTagList[1];
                    item2 = false;
                }
            }
            
            pBox.Image = (Image)Properties.Resources.ResourceManager.GetObject(imageTag);
            pBox.Image.Tag = imageTag;

        }

        private void pictureBox_fire_light_arrow_MouseUp(object sender, MouseEventArgs e)
        {
            doubleItems_MouseUp((PictureBox)sender, e.Button, ref isColoredFireArrow, ref isColoredLightArrow, imageTagList_fire_light_arrow);
        }

        private void pictureBox_goron_zora_tunic_MouseUp(object sender, MouseEventArgs e)
        {
            doubleItems_MouseUp((PictureBox)sender, e.Button, ref isColoredGoronTunic, ref isColoredZoraTunic, imageTagList_goron_zora_tunic);
        }

        private void pictureBox_iron_hover_boots_MouseUp(object sender, MouseEventArgs e)
        {
            doubleItems_MouseUp((PictureBox)sender, e.Button, ref isColoredIronBoots, ref isColoredHoverBoots, imageTagList_iron_hover_boots);
        }

        private void pictureBox_dins_farores_MouseUp(object sender, MouseEventArgs e)
        {
            doubleItems_MouseUp((PictureBox)sender, e.Button, ref isColoredDinsFire, ref isColoredFaroresWind, imageTagList_dins_farores);
        }

        private void pictureBox_30skulls_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, guarenteedHintsUpgrade);
        }

        private void pictureBox_40skulls_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, guarenteedHintsUpgrade);
        }

        private void pictureBox_sometimesHint1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, sometimesHintsUpgrade);
        }

        private void pictureBox_sometimesHint2_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, sometimesHintsUpgrade);
        }

        private void pictureBox_sometimesHint3_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, sometimesHintsUpgrade);
        }

        private void pictureBox_sometimesHint4_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, sometimesHintsUpgrade);
        }

        private void pictureBox_sometimesHint5_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, sometimesHintsUpgrade);
        }

        private void pictureBox_50skulls_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, guarenteedHintsUpgrade);
        }

        private void pictureBox_skullMask_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, guarenteedHintsUpgrade);
        }

        private void pictureBox_biggoron_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, guarenteedHintsUpgrade);
        }

        private void pictureBox_frogs_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, guarenteedHintsUpgrade);
        }
        #endregion

        #region MouseMove for DragAndDrop
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            var pBox = (PictureBox)sender;
            var image = pBox.Image;
            if (e.Button == MouseButtons.Left && isMouseDown)
            {
                if (pBox.Image.Tag.ToString() == "wallet")
                {
                    image = Properties.Resources.wallet2;
                    image.Tag = "wallet2";
                }
                pBox.DoDragDrop(image, DragDropEffects.Copy);
                isMouseDown = false;
            }
        }


        #endregion

        #region stones & medallions
        private void pictureBox_medaillons_green_MouseUp(object sender, MouseEventArgs e)
        {
            medaillons_MouseUp((PictureBox)sender, e.Button, ref isColoredGreenMedaillon, label_green_medaillon);
        }
        private void pictureBox_medaillons_red_MouseUp(object sender, MouseEventArgs e)
        {
            medaillons_MouseUp((PictureBox)sender, e.Button, ref isColoredRedMedaillon, label_red_medaillon);
        }
        private void pictureBox_medaillons_blue_MouseUp(object sender, MouseEventArgs e)
        {
            medaillons_MouseUp((PictureBox)sender, e.Button, ref isColoredBlueMedaillon, label_blue_medaillon);
        }
        private void pictureBox_medaillons_orange_MouseUp(object sender, MouseEventArgs e)
        {
            medaillons_MouseUp((PictureBox)sender, e.Button, ref isColoredOrangeMedaillon, label_orange_medaillon);
        }
        private void pictureBox_medaillons_purple_MouseUp(object sender, MouseEventArgs e)
        {
            medaillons_MouseUp((PictureBox)sender, e.Button, ref isColoredPurpleMedaillon, label_purple_medaillon);
        }
        private void pictureBox_medaillons_yellow_MouseUp(object sender, MouseEventArgs e)
        {
            medaillons_MouseUp((PictureBox)sender, e.Button, ref isColoredYellowMedaillon, label_yellow_medaillon);
        }
        private void pictureBox_stones_kokiri_MouseUp(object sender, MouseEventArgs e)
        {
            medaillons_MouseUp((PictureBox)sender, e.Button, ref isColoredKokiriStone, label_kokiri_stone);
        }
        private void pictureBox_stones_goron_MouseUp(object sender, MouseEventArgs e)
        {
            medaillons_MouseUp((PictureBox)sender, e.Button, ref isColoredGoronStone, label_goron_stone);
        }
        private void pictureBox_stones_zora_MouseUp(object sender, MouseEventArgs e)
        {
            medaillons_MouseUp((PictureBox)sender, e.Button, ref isColoredZoraStone, label_zora_stone);
        }
        #endregion

        private void button_woth_Click(object sender, EventArgs e)
        {
            if(wothPosition.Count < 5)
            {
                if (comboBox_places.Text != string.Empty)
                {
                    var selectedPlace = comboBox_places.Text.ToUpper().Trim();
                    var existingWoth = wothPosition.Where(x => x.Key.Text == selectedPlace).ToList();
                    if (existingWoth.Count > 0 && existingWoth[0].Value == 1)
                    {
                        existingWoth[0].Key.Font = new Font("Corbel", 10, FontStyle.Bold | FontStyle.Underline);
                        wothPosition[existingWoth[0].Key]++;
                    }
                    else if (existingWoth.Count > 0 && existingWoth[0].Value == 2)
                    {
                        existingWoth[0].Key.ForeColor = Color.MidnightBlue;
                        wothPosition[existingWoth[0].Key]++;
                    }
                    else if (existingWoth.Count == 0)
                    {
                        Label newLabel = new Label
                        {
                            Name = Guid.NewGuid().ToString(),
                            Text = selectedPlace,
                            ForeColor = Color.White,
                            BackColor = Color.CadetBlue,
                            Font = new Font("Corbel", 10, FontStyle.Bold),
                            Width = 200,
                            Height = 32,
                            TextAlign = ContentAlignment.MiddleLeft
                        };
                        newLabel.Location = new Point(2, (wothPosition.Count * newLabel.Height));
                        newLabel.MouseClick += new MouseEventHandler(label_woth_MouseClick);

                        for (int i = 0; i < 4; i++)
                        {
                            PictureBox newPictureBox = new PictureBox
                            {
                                Image = wothItems[0],
                                Name = newLabel.Name + i,
                                Size = new Size(32, 32),
                                TabStop = false,
                                AllowDrop = true
                            };

                            newPictureBox.Image.Tag = "gossip_stone_2";
                            newPictureBox.DragDrop += new DragEventHandler(pictureBox_DragDrop);
                            newPictureBox.DragEnter += new DragEventHandler(object_DragEnter);
                            newPictureBox.MouseClick += new MouseEventHandler(woth_pictureBox_MouseClick);

                            switch (i)
                            {
                                case 0: newPictureBox.Location = new Point(newLabel.Width, newLabel.Location.Y); break;
                                case 1: newPictureBox.Location = new Point(newLabel.Width + 32, newLabel.Location.Y); break;
                                case 2: newPictureBox.Location = new Point(newLabel.Width + 64, newLabel.Location.Y); break;
                                case 3: newPictureBox.Location = new Point(newLabel.Width + 96, newLabel.Location.Y); break;
                                default: break;
                            }

                            panel_woth.Controls.Add(newPictureBox);
                        }
                        wothPosition.Add(newLabel, 1);
                        panel_woth.Controls.Add(newLabel);
                    }
                }
            }
        }

        private void woth_pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBox_MouseUp_Upgrade((PictureBox)sender, e.Button, wothItems);
        }

        private void label_woth_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                var label = (Label)sender;
                wothPosition.Remove(label);
                label.Dispose();
                panel_woth.Controls.Find(label.Name + 0, false)[0].Dispose();
                panel_woth.Controls.Find(label.Name + 1, false)[0].Dispose();
                panel_woth.Controls.Find(label.Name + 2, false)[0].Dispose();
                panel_woth.Controls.Find(label.Name + 3, false)[0].Dispose();

                panel_woth.Controls.Remove(label);
                panel_woth.Controls.RemoveByKey(label.Name + 0);
                panel_woth.Controls.RemoveByKey(label.Name + 1);
                panel_woth.Controls.RemoveByKey(label.Name + 2);
                panel_woth.Controls.RemoveByKey(label.Name + 3); 
                
                for(int i = 0; i < wothPosition.Count; i++)
                {
                    var labelName = wothPosition.Keys.ElementAt(i).Name;
                    var wothLabel = (Label)panel_woth.Controls.Find(labelName, false)[0];
                    wothLabel.Location = new Point(2, (i * label.Height));

                    var pictureBox1 = (PictureBox)panel_woth.Controls.Find(labelName + 0, false)[0];
                    pictureBox1.Location = new Point(wothLabel.Width, wothLabel.Location.Y);

                    var pictureBox2 = (PictureBox)panel_woth.Controls.Find(labelName + 1, false)[0];
                    pictureBox2.Location = new Point(wothLabel.Width + 32, wothLabel.Location.Y);

                    var pictureBox3 = (PictureBox)panel_woth.Controls.Find(labelName + 2, false)[0];
                    pictureBox3.Location = new Point(wothLabel.Width + 64, wothLabel.Location.Y);

                    var pictureBox4 = (PictureBox)panel_woth.Controls.Find(labelName + 3, false)[0];
                    pictureBox4.Location = new Point(wothLabel.Width + 96, wothLabel.Location.Y);
                }
            }
        }

        private void label_barren_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                var label = (Label)sender;
                barrenPosition.Remove(label);
                label.Dispose();
                panel_barren.Controls.Remove(label);

                for (int i = 0; i < barrenPosition.Count; i++)
                {
                    var labelName = barrenPosition[i].Name;
                    var barrenLabel = (Label)panel_barren.Controls.Find(labelName, false)[0];
                    barrenLabel.Location = new Point(2, (i * label.Height));
                }
            }
        }

        private void button_barren_Click(object sender, EventArgs e)
        {
            if (barrenPosition.Count < 3)
            {
                if (comboBox_places.Text != string.Empty)
                {
                    var selectedPlace = comboBox_places.Text.ToUpper().Trim();
                    Label newLabel = new Label
                    {
                        Name = Guid.NewGuid().ToString(),
                        Text = selectedPlace,
                        ForeColor = Color.White,
                        BackColor = Color.IndianRed,
                        Font = new Font("Corbel", 9, FontStyle.Bold),
                        Width = 192,
                        TextAlign = ContentAlignment.MiddleLeft
                    };
                    newLabel.Location = new Point(2, (barrenPosition.Count * newLabel.Height));
                    newLabel.MouseClick += new MouseEventHandler(label_barren_MouseClick);
                    barrenPosition.Add(newLabel);
                    panel_barren.Controls.Add(newLabel);
                }
            }
        }

        private void label_collectedSkulls_MouseDown(object sender, MouseEventArgs e)
        {
            var label_collectedSkulls = (Label)sender;
            var intLabelText = Convert.ToInt32(label_collectedSkulls.Text);
            if(e.Button == MouseButtons.Left)
                intLabelText++;
            if (e.Button == MouseButtons.Right && intLabelText > 0)
                intLabelText--;
            
            if(intLabelText < 10) 
                label_collectedSkulls.Text = "0" + intLabelText.ToString();
            else 
                label_collectedSkulls.Text = intLabelText.ToString();
        }
    }
}
