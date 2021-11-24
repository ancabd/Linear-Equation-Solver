using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rezolvare_sisteme_liniare_determinanti
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int nrvariables = 0, n = 1;
        int[] variables, result;
        int[,] matrix;
        char[] charvariables;

        bool ifDigit(char x)
        {
            return x <= '9' && x >= '0';
        }
        bool ifLetter(char x)
        {
            return (x <= 'z' && x >= 'a') || (x <= 'Z' && x >= 'A');
        }

        int letterCode(char x)
        {
            if (x <= 'z' && x >= 'a') return x - 'a' + 1;
            else return x - 'A' + 30;
        }

        void CreateMatrix()
        {
            try
            {
                variables = new int[60];
                result = new int[60];
                charvariables = new char[60];
                matrix = new int[60, 60];
                nrvariables = 0; n = 1;

                string arr;
                arr = textBox1.Text;
                for (int i = 0; i < arr.Length; i++)
                    if (ifLetter(arr[i]) && variables[letterCode(arr[i])] == 0)
                    {
                        variables[letterCode(arr[i])] = ++nrvariables;
                        charvariables[nrvariables] = arr[i];
                    }

                /// calulam matricea
                bool beforeEq = true;
                int nr = 0, sign = 1;
                for (int i = 0; i < arr.Length; i++, n++)
                {
                    beforeEq = true;
                    sign = 1;
                    nr = 0;
                    while (i < arr.Length && arr[i] != '\n')
                    {
                        if (ifDigit(arr[i]))
                            nr = nr * 10 + arr[i] - '0';
                        else if (ifLetter(arr[i]))
                        {
                            if (nr == 0) nr = 1;
                            if (beforeEq) matrix[n, variables[letterCode(arr[i])]] += nr * sign;
                            else matrix[n, variables[letterCode(arr[i])]] -= nr * sign;
                            nr = 0;
                        }
                        else if (arr[i] == '+')
                        {
                            if (nr != 0)
                            {
                                if (beforeEq) result[n] += nr * sign;
                                else result[n] -= nr * sign;
                                nr = 0;
                            }
                            sign = 1;
                        }
                        else if (arr[i] == '-')
                        {
                            if (nr != 0)
                            {
                                if (beforeEq) result[n] += nr * sign;
                                else result[n] -= nr * sign;
                                nr = 0;
                            }
                            sign = -1;
                        }
                        else if (arr[i] == '=')
                        {
                            if (nr != 0)
                            {
                                if (beforeEq) result[n] += nr * sign;
                                else result[n] -= nr * sign;
                                nr = 0;
                            }
                            beforeEq = false;
                            sign = 1;
                        }
                        i++;
                    }
                    if (nr != 0)
                    {
                        if (beforeEq) result[n] += nr * sign;
                        else result[n] -= nr * sign;
                        nr = 0;
                    }
                }
                for (int i = 1; i <= n; ++i)
                    result[i] *= -1;
                n--;
            }
            catch
            {
                MessageBox.Show("This is not a vald equation");
            };
        }
        
        int AlgebricComplementary(int[,] x, int i, int j, int m)
        {
            int[,] newx = new int[m, m];
            int i2 = 0, j2 = 0;
            for (int i1 = 1; i1 <= m; i1++)
            {
                j2 = 0;
                if (i1 != i) i2++;
                for (int j1 = 1; j1 <= m; j1++)
                {
                    if (j1 != j) j2++;
                    if (i1 != i && j1 != j)
                        newx[i2, j2] = x[i1, j1];
                }
            }
            if ((i + j) % 2 == 0) return FindDeterminant(newx, m - 1);
            else return FindDeterminant(newx, m - 1) * -1;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Default");
            comboBox1.Items.Add("Dark Mode (Red)");
            comboBox1.Items.Add("Dark Mode (Blue)");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = comboBox1.SelectedItem.ToString();
            switch(s)
            {
                case "Default":
                    this.ForeColor = SystemColors.ControlText;
                    this.BackColor = SystemColors.ActiveCaption;
                    button1.BackColor = Color.DarkKhaki;
                    comboBox1.BackColor = textBox2.BackColor = textBox1.BackColor = SystemColors.Window;
                    comboBox1.ForeColor = textBox1.ForeColor = textBox2.ForeColor = SystemColors.WindowText;
                    break;
                case "Dark Mode (Red)":
                    this.ForeColor = SystemColors.Window;
                    this.BackColor = Color.FromArgb(23, 21, 21);
                    button1.BackColor = Color.FromArgb(125, 30, 42);
                    comboBox1.BackColor = textBox1.BackColor = textBox2.BackColor = Color.FromArgb(52, 48, 47);
                    comboBox1.ForeColor = textBox1.ForeColor = textBox2.ForeColor = Color.RosyBrown;
                    break;
                case "Dark Mode (Blue)":
                    this.ForeColor = Color.CornflowerBlue;
                    this.BackColor = Color.FromArgb(23, 21, 21);
                    button1.BackColor = Color.MidnightBlue;
                    comboBox1.BackColor = textBox1.BackColor = textBox2.BackColor = Color.FromArgb(52, 48, 47);
                    comboBox1.ForeColor = textBox1.ForeColor = textBox2.ForeColor = Color.White;
                    break;
            }
            
        }

        int FindDeterminant(int[,] x, int m)
        {
            int det = 0;
            if (m == 2) // determinant de ordin 2
                det = x[1, 1] * x[2, 2] - x[1, 2] * x[2, 1];
            else if (m == 1) det = x[1, 1];
            else
            {
                //dezvolt dupa linia 1
                for (int i = 1; i <= m; i++)
                    det += x[1, i] * AlgebricComplementary(x, 1, i, m);
            }
            
            return det;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CreateMatrix();
            try
            {
                int det = FindDeterminant(matrix, n);
                if (det == 0) MessageBox.Show("This is not a vald equation");
                else
                {
                    int[,] newmatrix = new int[60, 60];

                    for (int i = 1; i <= n; i++)
                        for (int j = 1; j <= n; j++)
                            newmatrix[i, j] = matrix[i, j];

                    /// folosesc relatiile lui Cramer
                    int deti;
                    double xvalue;
                    string s = "";
                    for (int j = 1; j <= n; j++)
                    {
                        for (int i = 1; i <= n; i++)
                        {
                            newmatrix[i, j - 1] = matrix[i, j - 1];
                            newmatrix[i, j] = result[i];
                        }
                        deti = FindDeterminant(newmatrix, n);
                        xvalue = 1.00 * deti / det;
                        s += charvariables[j] + "=" + xvalue + "\r\n";
                    }
                    textBox2.Text = s;
                }
            }
            catch
            {
                MessageBox.Show("This is not a vald equation");
            }
            /*string s = "";
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                    s += matrix[i, j] + " ";
                s += "\r\n";
            }
            textBox2.Text = s;*/
        }
    }
}
