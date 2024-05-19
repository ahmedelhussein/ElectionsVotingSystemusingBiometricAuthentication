//AASTMT - Sheration Branch (Fall 2021-2022) Advanced Programming (CC319) Project
//Project Title: Elections Voting System with Biometric (Fingerprint) Authentication
//Project Members: Ahmed El-Hussein 19106798 - Menna Mohamed  - Moataz Asem  - Tasneem Hosaam 
//Lecturer: Dr. Ali Saudi
//Teaching Assistant: Eng. Karim Oussama
//Under the supervision of: Eng. Nour El-Din Samy
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.IO;
using System.IO.Ports;

namespace Project
{
    public partial class Form1 : Form
    {
        SerialPort sp = Global.sp;
        byte[] getImage = Global.ConvertToHex("EF01FFFFFFFF010003010005");
        byte[] storeInBuffer2 = Global.ConvertToHex("EF01FFFFFFFF01000402020009");
        byte[] sendToSensorBuffer1 = Global.ConvertToHex("EF01FFFFFFFF0100040901000F");
        byte[] match = Global.ConvertToHex("EF01FFFFFFFF010003030007");
        byte[] storeInBuffer1 = Global.ConvertToHex("EF01FFFFFFFF01000402010008");
        byte[] uploadFromCharBuffer1 = Global.ConvertToHex("EF01FFFFFFFF0100040801000E");
        int index = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.ShowDialog();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult m = MessageBox.Show("Are you sure you want to close the program?","Close Program",MessageBoxButtons.YesNo);
            if(m == DialogResult.No)
            {
                e.Cancel = true;
            }

            else //File Saving Data
            {
                int k = 0;
                string path = "Candidates.txt";
                List<string> x = File.ReadLines(path).ToList();
                for (int i=3;i<x.Count;i+=4)
                {
                    x[i] = (Candidate.c[k].CountVotes).ToString();
                    k++;
                }
                File.WriteAllLines(path, x);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            sp = new SerialPort(comPortTextBox.Text, 115200);
            try
            {
                sp.Open();
            }
            catch
            {
                MessageBox.Show("Connection Failed!");
            }
            if (!sp.IsOpen)
                MessageBox.Show("Input the correct COM port number!");
            else
            {
                MessageBox.Show("Connection OK!");
                Global.sp = sp;
                comPortTextBox.Visible = false;
                button2.Visible = false;
                button4.Visible = true;
                button3.Visible = true;
            }
               
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(ofd.FileName, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                Global.x = (test[])bf.Deserialize(fs);
                fs.Close();
                button3.Visible = false;
                button2.Visible = true;
                button1.Visible = true;
                button4.Visible = false;
                comPortTextBox.Visible = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Press Ok to Start Capturing the fingerprints of the voters");
            int co = 1;
            while (index < Global.x.Length)
            {
                Global.x[index] = new test();
                sp.Write(getImage, 0, getImage.Length);

                Thread.Sleep(1000);

                byte[] result = new byte[sp.BytesToRead];
                sp.Read(result, 0, sp.BytesToRead);

                if (result[9] == 0)
                {
                    MessageBox.Show($"Fingerprint {co} Read, Press Ok to read the next fingerprint");
                    co++;
                    sp.Write(storeInBuffer1, 0, storeInBuffer1.Length);
                    Thread.Sleep(1000);
                    result = new byte[sp.BytesToRead];
                    sp.Read(result, 0, sp.BytesToRead);
                    if (result[9] == 0)
                    {
                        //Send the upload request
                        sp.Write(uploadFromCharBuffer1, 0, uploadFromCharBuffer1.Length);
                        Thread.Sleep(1000);
                        Global.x[index].charBuffer = new byte[sp.BytesToRead];
                        sp.Read(Global.x[index].charBuffer, 0, sp.BytesToRead);
                        index++;
                    }
                }

                else
                    MessageBox.Show("Fingerprint error ! Make sure your finger is placed on the sensor");
            }

            //Save the fingerprint to a file
            SaveFileDialog svd = new SaveFileDialog();
            if (svd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(svd.FileName, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, Global.x);
                fs.Close();
            }
        }
    }
}
