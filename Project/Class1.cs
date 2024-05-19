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
using System.IO;
using System.IO.Ports;

namespace Project
{
    static class Global
    {
        public static SerialPort sp;
        public static test[] x = new test[5];

        public static byte[] ConvertToHex(String hex)
        {
            byte[] result = Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
            return result;
        }
    }
}
