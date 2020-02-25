using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Calculator
{
    public partial class CalculatorGridXAML : ContentPage
    {
        private readonly int ilMaxLen = 11;
        private readonly int dlMaxLen = 5;
        private bool blWaitForCalc = false;
        private double operand1 = 0.0d;
        private double operand2 = 0.0d;
        private double dRes = 0.0d;
        private string lastOpe = "";
        private string nextOpe = "";

        public CalculatorGridXAML()
        {
            InitializeComponent();
            OnClearClicked(null, null); 
        }
        private void PositioningCipher(string cfr)
        {
            try { 
                if (Cifre.Text.Equals("0")) // Elimio lo zero iniziale
                    Cifre.Text = "";

                int tLen;
                int iPart;
                int dPart;

                tLen = Cifre.Text.Length;

                // Verifico la lunghezza del numero, la dua parte intera e quella decimale
                if (Cifre.Text.LastIndexOf(",") > -1)
                {
                    // numero decimale
                    iPart = tLen - Cifre.Text.LastIndexOf(",");
                    dPart = tLen - (iPart + 1);
                    if (dPart > dlMaxLen)
                        return;
                    if (iPart > ilMaxLen)
                        return;
                }
                else
                    iPart = tLen; // numero intero


                if (Cifre.Text.Equals("0"))
                    Cifre.Text = "";

                if (ilMaxLen < iPart)
                    return;
                }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Cifre.Text += cfr;
        }
        public void OnCipherClicked(object sender, EventArgs args)
        {
            try
            {
                if (blWaitForCalc)
                {
                    // In attesa di eseguire una operazione ?
                    Cifre.Text = "0";       // Azzero momentaneamente il risultato
                    blWaitForCalc = false; 
                }
                
                string name = ((Button)sender).Text;

                if (name.Equals("0")) // Se digito 0
                {
                    /*
                     Nel caso la cifra già presente non sia 0 o sia un decimale ...
                    */
                    if (!Cifre.Text.Equals("0") || Cifre.Text.LastIndexOf("0,") > -1) 
                    {
                        PositioningCipher(name); // Aggiungi la cifra
                    }
                }
                else
                {

                    if (name.Equals(",")) // Se digito 0
                    {
                        /*
                         Nel caso la cifra già presente non contenga già la virgola ... 
                        */
                        if (Cifre.Text.LastIndexOf(",") == -1)
                        {
                            Cifre.Text += ",";
                        }
                    }
                    else 
                    {
                        // Se digito un qualsiasi altro numero
                        PositioningCipher(name);
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void OnClearClicked(object sender, EventArgs args)
        {
            // Ho digitato C (clear)
            Cifre.Text = "0";
            operand1 = 0.0d;
            operand2 = 0.0d;
            dRes = 0.0d;
            lastOpe = "";
            nextOpe = ""; 
        }

        private void EndOperationCycle() 
        {
            lastOpe = "";
            nextOpe = "";
            operand1 = dRes;
            Cifre.Text = string.Format("{0:N2}", dRes);
            blWaitForCalc = true;
        }
        private void OnEqualClicked(object sender, EventArgs args)
        {
            operand2 = Convert.ToDouble(Cifre.Text);
            if (!String.IsNullOrEmpty(lastOpe))
            {
                PerformCompute(lastOpe);
            }
            EndOperationCycle(); 
            lastOpe = "=";

        }
        private void OnPercentageClicked(object sender, EventArgs args)
        {
            operand2 = Convert.ToDouble(Cifre.Text);
            operand2 = (operand1 * operand2) / 100;
            if (lastOpe.Equals("+") || lastOpe.Equals("-") || lastOpe.Equals("div"))
            {
                PerformCompute(lastOpe);
            }
            else 
            {
                dRes = operand2; 
            }

            EndOperationCycle();

        }
        private void OnSignChangeClicked(object sender, EventArgs args)
        {
            if (!String.IsNullOrEmpty(lastOpe))
            {
                PerformCompute(lastOpe);
            }
            operand2 = Convert.ToDouble(Cifre.Text);
            dRes = operand2 * -1;
            operand1 = dRes;
            Cifre.Text = string.Format("{0:N2}", dRes);
        }
        private void PerformCompute(string name) 
        {
            if (name.Equals("X") || name.Equals("div"))
            {
                if (operand1 == 0.0d)
                    operand1 = 1.0d;

                if (operand2 == 0.0d)
                    operand2 = 1.0d;

                if (name.Equals("X")) 
                {
                    dRes = operand1 * operand2; // Eseguo la moltiplicazione
                    Console.WriteLine("Eseguo la moltiplicazione " + operand1 + " * " + operand2);
                }

                if (name.Equals("div")) 
                {
                    dRes = operand1 / operand2; // Eseguo la divisione
                    Console.WriteLine("Eseguo la divisione " + operand1 + " / " + operand2);
                }
            }
            else
            {
                if (name.Equals("+")) 
                {
                    dRes = operand1 + operand2; // Eseguo l'addizione
                    Console.WriteLine("Eseguo l'addizione " + operand1 + " + " + operand2);
                }

                if (name.Equals("-"))
                {
                    dRes = operand1 - operand2; // Eseguo la sottrazione
                    Console.WriteLine("Eseguo la sottrazione " + operand1 + " - " + operand2);
                }
            }
            Console.WriteLine("Risultato parziale " + dRes);
        }
        public void OnOperationClicked(object sender, EventArgs args)
        {
            string name = ((Button)sender).Text;
            operand2 = Convert.ToDouble(Cifre.Text);
            nextOpe = name;
            if (lastOpe.Equals("="))
                lastOpe = nextOpe;
            if (name.Equals(""))
            { 
            
            }
            if (blWaitForCalc)
            {
                if (operand2 == operand1)
                    operand2 = 0.0d;
            }
            if (!String.IsNullOrEmpty(lastOpe))
            {
                name = lastOpe;
                PerformCompute(name);
                name = nextOpe; 
            }
            else 
            {
                dRes = operand2;
            }
           
            operand1 = dRes;
            lastOpe = name;
            Cifre.Text = string.Format("{0:N2}", dRes); 
            blWaitForCalc = true; // Ho appena eseguito il calcolo 
        }
    }
}

