﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DiamondBilliards.PoolTime.Business;

namespace DiamondBilliards.PoolTime.Presentation
{
    public partial class AddTableForm : Form
    {
        private int _tableNum;

        public AddTableForm(int tableNum)
        {
            InitializeComponent();

            this._tableNum = tableNum;

            this.Text = "Add Table " + this._tableNum;

            this.gridPlayers.Rows.Add();
            this.gridPlayers.Rows[0].Cells[0].Value = "Player 1";

            this.comboNumPlayers.SelectedIndex = 0;
            this.comboNumPlayers.Select();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNumPlayers_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int newIndex = 0;

            switch (button.Name)
            {
                case "btn1Player":
                    newIndex = 0;
                    break;
                case "btn2Player":
                    newIndex = 1;
                    break;
                case "btn3Player":
                    newIndex = 2;
                    break;
                case "btn4Player":
                    newIndex = 3;
                    break;
                case "btn5Player":
                    newIndex = 4;
                    break;
                case "btn6Player":
                    newIndex = 5;
                    break;
                case "btn7Player":
                    newIndex = 6;
                    break;
                case "btn8Player":
                    newIndex = 7;
                    break;
                case "btn9Player":
                    newIndex = 8;
                    break;
                default:
                    newIndex = 0;
                    break;
            }

            this.comboNumPlayers.SelectedIndex = newIndex;
        }

        private string GetDefaultPlayerName(int playerNumber)
        {
            bool gotName = false;
            string playerName = "Player " + playerNumber;

            while (!gotName)
            {
                if (CheckForDuplicateName(playerName))
                {
                    playerName = "Player " + ++playerNumber;
                } else {
                    gotName = true;
                }
            }

            return playerName;
        }

        private bool CheckForDuplicateName(string name)
        {
            int count = 0;

            for (int i = 0; i < this.gridPlayers.Rows.Count; i++)
            {
                if ((string)this.gridPlayers.Rows[i].Cells[0].Value == name)
                {
                    if (++count >= 2)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void comboNumPlayers_TextChanged(object sender, EventArgs e)
        {
            int currentRows = this.gridPlayers.Rows.Count;
            int newRows = 1;
            ComboBox comboBox = (ComboBox)sender;

            if (!int.TryParse(comboBox.Text, out newRows))
            {
                comboBox.SelectedIndex = this.gridPlayers.Rows.Count - 1;
                newRows = this.gridPlayers.Rows.Count;
            } else if (this.gridPlayers.Rows.Count == newRows)
            {
                return;
            }
           
            if (newRows > currentRows)
            {
                for (int i = currentRows; i < newRows; i++)
                {
                    this.gridPlayers.Rows.Add();
                    this.gridPlayers.Rows[i].Cells[0].Value = "Player " + (i + 1);
                    this.gridPlayers.Rows[i].Cells[0].Value = GetDefaultPlayerName(i + 1);
                }
            }
            else if (newRows < currentRows)
            {
                for (int i = currentRows - 1; i >= newRows; i--)
                {
                    this.gridPlayers.Rows.RemoveAt(i);
                }
            }

            this.gridPlayers.Select();
            this.gridPlayers.CurrentCell.Selected = false;
            this.gridPlayers.Rows[0].Cells[0].Selected = true;
            this.gridPlayers.CurrentCell = this.gridPlayers.SelectedCells[0];
            this.gridPlayers.CurrentCell.Selected = true;
        }

        private void comboNumPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnStartTable_Click(object sender, EventArgs e)
        {
            if (this._tableNum > 0 && this.gridPlayers.Rows.Count > 0 
                && this.dTStartTime.Value != null)
            {
                List<string> playerNames = new List<string>();

                for (int i = 0; i < this.gridPlayers.Rows.Count; i++)
                {
                    playerNames.Add((string)this.gridPlayers.Rows[i].Cells[0].Value);
                }

                TableDataService.StartTable(this._tableNum, this.dTStartTime.Value,
                        playerNames);

                this.Close();

                /*
                try
                {
                    TableDataService.StartTable(this._tableNum, this.dTStartTime.Value,
                        playerNames);

                    this.Close();
                }

                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }     
                */
            }
        }

        private void gridPlayers_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            string newPlayerName = GetPlayerName((string)dataGridView.Rows[e.RowIndex].Cells[0].Value);
            dataGridView.Rows[e.RowIndex].Cells[0].Value = newPlayerName;
        }

        private string GetPlayerName(string playerName)
        {
            bool gotName = false;
            int currentDuplicate = 1;
            string newPlayerName = playerName;

            if (playerName == null || playerName == string.Empty || playerName == "")
            {
                newPlayerName = GetDefaultPlayerName(this.gridPlayers.Rows.Count);
            } else {
                while (!gotName)
                {
                    if (CheckForDuplicateName(newPlayerName))
                    {
                        newPlayerName = playerName + "(" + ++currentDuplicate + ")";
                    } else {
                        gotName = true;
                    }
                }
            }

            return newPlayerName;
        }
    }
}
