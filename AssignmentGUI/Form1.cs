﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssignmentGUI
{
    public partial class Form1 : Form
    {
        static string[] headers = new string[6];
        BindingList<Countries> countries = new BindingList<Countries>();

        public Form1()
        {
            InitializeComponent();

            const int MAX_LINES_FILE = 50000;
            string[] AllLines = new string[MAX_LINES_FILE];

            AllLines = File.ReadAllLines("countries.csv");

            foreach (string line in AllLines)
            {
                if (line.StartsWith("Country"))
                {
                    headers = line.Split(',');
                }
                else
                {
                    string[] columns = line.Split(',');
                    countries.Add(new Countries
                    {
                        Name = columns[0],
                        GdpGrowth = Convert.ToDouble(columns[1]),
                        Inflation = Convert.ToDouble(columns[2]),
                        TradeBalance = Convert.ToDouble(columns[3]),
                        HdiRank = Int32.Parse(columns[4]),
                        TradePartners = columns[5]
                    });
                }
            }
            resortList();
        }

        private void resortList()
        {
            List<Countries> sortedList = countries.OrderBy(x => x.Name).ToList();
            countries = new BindingList<Countries>(sortedList);

            listBox1.DataSource = countries;
            listBox1.DisplayMember = "Name";
        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            countries.RemoveAt(listBox1.SelectedIndex);
            updateView();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateView();
        }

        private void updateView()
        {
            gdpBox.Text = countries[listBox1.SelectedIndex].GdpGrowth.ToString();
            inflationBox.Text = countries[listBox1.SelectedIndex].Inflation.ToString();
            tradeBalanceBox.Text = countries[listBox1.SelectedIndex].TradeBalance.ToString();
            hdiBox.Text = countries[listBox1.SelectedIndex].HdiRank.ToString();
            listBox2.DataSource = countries[listBox1.SelectedIndex].TradingPartners;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            countries[listBox1.SelectedIndex].GdpGrowth = Convert.ToDouble(gdpBox.Text);
            countries[listBox1.SelectedIndex].Inflation = Convert.ToDouble(inflationBox.Text);
            countries[listBox1.SelectedIndex].TradeBalance = Convert.ToDouble(tradeBalanceBox.Text);
            countries[listBox1.SelectedIndex].HdiRank = Convert.ToInt32(hdiBox.Text);
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            AddCountry cf = new AddCountry();
            cf.FormClosed += new FormClosedEventHandler(cfClosed);
            cf.Show();
        }

        void cfClosed(object sender, FormClosedEventArgs e)
        {
            if(Program.newCountryName != "")
            {
                countries.Add(new Countries
                {
                    Name = Program.newCountryName,
                    GdpGrowth = 0,
                    Inflation = 0,
                    TradeBalance = 0,
                    HdiRank = 0,
                    TradePartners = "[none]"
                });

                Program.newCountryName = null;
                resortList();
            }
        }
    }
}
