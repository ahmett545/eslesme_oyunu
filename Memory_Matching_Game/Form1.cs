using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace Memory_Matching_Game
{
    public partial class mainWindow : Form
    {
        //Variables
        int attempts; //Player attempts
        int gameDuration; //Counts the duration of the game in seconds, after the cards are hidden.
        static int dataset = 1; //The dataset will be selected through the radiobuttons. The default is the Mario dataset.
        int puan = 0;
        PictureBox firstCardOpen; //The first card the player has selected
        PictureBox secondCardOpen; //The second card the player has selected
        Random rndIndex = new Random();
        List<PictureBox> unPairedPictureBoxes = new List<PictureBox>(); //A List that contains all the PictureBoxes that are not paired. 

        

        public mainWindow()
        {
            InitializeComponent();
        }

        private PictureBox[] NumOfPictureBoxes  
        {
            get {return Controls.OfType<PictureBox>().ToArray(); }
            
        }
      

        #region IEnumerable<Image>        
        private static IEnumerable<Image> DatasetImages 
        {
            get
            {
                if (dataset == 1) //If the Mario radiobutton is selected
                {
                    return new Image[]
                    {
                                Properties.Resources.Mario0,
                                Properties.Resources.Mario1,
                                Properties.Resources.Mario2,
                                Properties.Resources.Mario3,
                                Properties.Resources.Mario4,
                                           
                    };
                }
                else if (dataset == 2) //If the Crash radiobutton is selected
                {
                    return new Image[]
                    {
                                Properties.Resources.Crash0,
                                Properties.Resources.Crash1,
                                Properties.Resources.Crash2,
                                Properties.Resources.Crash3,
                                Properties.Resources.Crash4,
                                Properties.Resources.Crash5,
                                Properties.Resources.Crash6,
                                Properties.Resources.Crash7,
                    };
                }

                else
                {
                    return new Image[] //If the Pokemon radiobutton is selected
                    {
                                Properties.Resources.Pokemon0,
                                Properties.Resources.Pokemon1,
                                Properties.Resources.Pokemon2,
                                Properties.Resources.Pokemon3,
                                Properties.Resources.Pokemon4,
                                Properties.Resources.Pokemon5,
                                Properties.Resources.Pokemon6,
                                Properties.Resources.Pokemon7,
                                Properties.Resources.Pokemon8,
                                Properties.Resources.Pokemon9
                    };
                }
            }
        }        
        #endregion

        #region RadioButtons
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dataset = 1;
           

        }

        private void crashButton_CheckedChanged(object sender, EventArgs e)
        {
            dataset = 2;
        }

        private void pokemonButton_CheckedChanged(object sender, EventArgs e)
        {
            dataset = 3;
        }
        #endregion

        private PictureBox FindUntagged()  //Returns a PictureBox that has not Image.Tag.
        {
            int num;
            
            do
            {

                if (dataset == 1) { num = rndIndex.Next(0, 10); }
                if (dataset == 2) { num = rndIndex.Next(0, 16); }
                else
                { num = rndIndex.Next(0, NumOfPictureBoxes.Count());}
            }
            
            while (NumOfPictureBoxes[num].Tag != null);
            return NumOfPictureBoxes[num];
        }

        private void ShuffleCards()  //Imports each dataset image on two randomly selected PictureBoxes.
        {
            if (dataset == 1) {
                this.Controls.Remove(this.Controls["PictureBox11"]);
                this.Controls.Remove(this.Controls["PictureBox12"]);
                this.Controls.Remove(this.Controls["PictureBox13"]);
                this.Controls.Remove(this.Controls["PictureBox14"]);
                this.Controls.Remove(this.Controls["PictureBox15"]);
                this.Controls.Remove(this.Controls["PictureBox16"]);
                this.Controls.Remove(this.Controls["PictureBox17"]);
                this.Controls.Remove(this.Controls["PictureBox18"]);
                this.Controls.Remove(this.Controls["PictureBox19"]);
                this.Controls.Remove(this.Controls["PictureBox20"]);
            }
            else if (dataset == 2)
            {
                this.Controls.Remove(this.Controls["PictureBox11"]);
                this.Controls.Remove(this.Controls["PictureBox12"]);
                this.Controls.Remove(this.Controls["PictureBox13"]);
                this.Controls.Remove(this.Controls["PictureBox14"]);
                //this.Controls.Remove(this.Controls["PictureBox15"]);
            }
           

            foreach (var image in DatasetImages)
                {
                    FindUntagged().Tag = image;  //Import each image of the dataset twice, in order to create pairs.
                    FindUntagged().Tag = image;  //Calls the FindUnTagged() again and imports the same image to a another PictureBox.
                }
                DisplayCards();
            }

        private void DisplayCards()
        {
            foreach (var pic in NumOfPictureBoxes)
            {
                pic.Image = (Image)pic.Tag;
                pic.Enabled = false;
                unPairedPictureBoxes.Add(pic);  
            }
        }

        private void ResetCards() //Remove (if exist) all the image tags.
        {
            foreach (var pic in NumOfPictureBoxes)
            {
                pic.Tag = null; 
                pic.Visible = true;
            }
        }

        private void reverseTimer_Tick(object sender, EventArgs e)
        {            
            int labelTimer = Convert.ToInt32(reverseTimeLabel.Text) - 1;
            reverseTimeLabel.Text = labelTimer.ToString();             

            if (labelTimer == 0)  
            {
                reverseTimeLabel.Text = "BAŞLA!";
                gameTimer.Start();
                HideCards();
                reverseTimer.Stop();
                
            }
        }
               
        private void HideCards()
        {
            foreach (var pic in NumOfPictureBoxes)
            {
                pic.Image = Properties.Resources.Cover;
                pic.Enabled = true;
                pic.Cursor = Cursors.Hand;
            }
        }

        private void StartButton(object sender, EventArgs e)
        {
            ResetCards();
            ShuffleCards();
            attempts = 0;
            puan = 0;
            gameDuration = 182;
            reverseTimeLabel.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label3.Visible = false;
            reverseTimeLabel.Text = "5";
            reverseTimer.Start();
            startButton.Enabled = false;
            marioButton.Enabled = false;
            crashButton.Enabled = false;
            pokemonButton.Enabled = false;
            
            

        }

        

        private void ClickOnImage(object sender, EventArgs e)
        {
            var pic = (PictureBox)sender;

            SoundPlayer flip = new SoundPlayer();
            string path = "card_flip.wav";// Müzik adresi
            flip.SoundLocation = path;
            flip.Play(); //Oynat

            pic.Image = (Image)pic.Tag;
            if (firstCardOpen == null)
            {
                firstCardOpen = pic;
            }
            else
            {
                attempts = attempts + 1;
                secondCardOpen = pic;
                if ((pic.Tag == firstCardOpen.Tag) && (pic != firstCardOpen)) // && pic != firstCardOpen
                {
                    SoundPlayer player = new SoundPlayer();
                    string path1 = "dogru_cevap.wav";        // Müzik adresi
                    player.SoundLocation = path1;
                    player.Play(); //Oynat
                    MessageBox.Show("Doğru Eşleşme !!");
                    puan += 100;
                    label5.Text = Convert.ToString(puan);
                    unPairedPictureBoxes.Remove(firstCardOpen); //Remove from the unpaired List.
                    unPairedPictureBoxes.Remove(secondCardOpen);
                    firstCardOpen.Visible = false;
                    secondCardOpen.Visible = false;
                    

                    if (unPairedPictureBoxes.Count == 0) //All pairs are found
                    {
                        Winning();
                    }

                    firstCardOpen.Enabled = false; //Will not be able to click on these two paired PictureBoxes anymore in this game.
                    secondCardOpen.Enabled = false;
                    firstCardOpen = null;
                    secondCardOpen = null;
                }
                else
                { 
                    foreach (var pic2 in unPairedPictureBoxes)
                    {
                        pic2.Enabled = false; //Prevent to click on any other cards while the two cards are open.
                        SoundPlayer y_cevap = new SoundPlayer();
                        string path2 = "yanlis_cevap.wav";        // Müzik adresi
                        y_cevap.SoundLocation = path2;
                        y_cevap.Play(); //Oynat
                        
                        
                    }
                    MessageBox.Show("Yanlış Eşleşme !!");
                    puan -= 100;
                    label5.Text = Convert.ToString(puan);
                    waitTimer.Start(); //Wait 0.750 seconds till you hide the two unpaired cards.
                }
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            label2.Text = "Kalan Süre:";
            gameDuration = gameDuration - 1;
            int label = Convert.ToInt32(gameDuration) - 1;
            label1.Text = label.ToString();
            label4.Visible = true; label5.Visible = true;

            if (label == 0)  //Time to hide the cards and start the game.
            {
                reverseTimeLabel.Text = "Süreniz Doldu!";
                HideCards();
                Losing();
 
            }
            reverseTimeLabel.Hide();
        }

        private void waitTimer_Tick(object sender, EventArgs e)
        {
            waitTimer.Stop();
            firstCardOpen.Image = Properties.Resources.Cover; //Hide the two unpaired cards.
            secondCardOpen.Image = Properties.Resources.Cover;
            foreach (var pic2 in unPairedPictureBoxes)
            {
                pic2.Enabled = true; //Allow to click on hidden cards again.
            }
            firstCardOpen = null;
            secondCardOpen = null;
        }

        private void Winning()
        {
            gameTimer.Stop();
            SoundPlayer win = new SoundPlayer();
            win.SoundLocation = "win.wav";
            win.Play(); //Oynat
            MessageBox.Show("Oyun Bitti! Süre Bitmeden Başardınız." + Environment.NewLine+ Environment.NewLine + "Oyun Seviyesi: " + dataset.ToString() + Environment.NewLine + "Girişimler: " + attempts.ToString() + Environment.NewLine + "Kalan Süre: " + gameDuration.ToString() + "s" + Environment.NewLine + "Puan: " + puan.ToString());
            startButton.Enabled = true;
            marioButton.Enabled = true;
            crashButton.Enabled = true;
            pokemonButton.Enabled = true;
            label3.Visible = true;
            label1.Visible = false;
            label2.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            reverseTimeLabel.Visible = false;

        }
        private void Losing()
        {
            gameTimer.Stop();
            SoundPlayer lose = new SoundPlayer();
            lose.SoundLocation = "lose.wav";
            lose.Play(); //Oynat
            MessageBox.Show("Malesef Süreniz Doldu! Tekrar Deneyiniz." + Environment.NewLine + Environment.NewLine + "Girişimler: " + attempts.ToString() + Environment.NewLine );
            DisplayCards();
            startButton.Enabled = true;
            marioButton.Enabled = true;
            crashButton.Enabled = true;
            pokemonButton.Enabled = true;
            label3.Visible = true;
            label1.Visible = false;
            label2.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            reverseTimeLabel.Visible = false;


        }
        public void result()
        {
            TextWriter dosya = new StreamWriter(@"C:\Users\SAMSUNG\Desktop\YazGel2-2\17307029_Ahmet_Binekoğlu\Memory_Matching_Game\sonuclar.txt");
            try
            {
                dosya.WriteLine("Ali Duran ");
                dosya.WriteLine("Sema Şahin");
                dosya.WriteLine("Kemal Kaya");
            }

            finally
            {
                dosya.Flush();
                dosya.Close();

            }
        }


    }
}
