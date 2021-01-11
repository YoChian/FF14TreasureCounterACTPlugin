using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using System.IO;
using System.Reflection;
using System.Xml;

namespace TreasureCounter
{
    public class PluginMain : UserControl,IActPluginV1
    {
        private CheckedListBox ItemList;
        private TextBox AddItemText;
        private Button AddItemButton;
        private Button RemoveButton;
        private TextBox BaseSalaryText;
        private Label label2;
        private Label MapLabel;
        private Label TPLabel;
        private Label SalaryLabel;
        private Button ResetButton;
        private Button StartButton;
        private GroupBox groupBox1;
        private Label label3;
        private TextBox PriceText;
        private Label label1;
        private ListBox PriceList;
        private ListBox ItemCount;
        bool sessionStart;

        public PluginMain()
        {
            InitializeComponent();
            LocalInit();
        }

        void LocalInit()
        {
            sessionStart = false;
            ResetButton.Enabled = false;
            RemoveButton.Enabled = false;
            ItemList.Items.Clear();
        }

        Label lblStatus;    // The status label that appears in ACT's Plugin tab
        string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\PluginSample.config.xml");
        SettingsSerializer xmlSettings;

        void IActPluginV1.InitPlugin(System.Windows.Forms.TabPage pluginScreenSpace, System.Windows.Forms.Label pluginStatusText)
        {
            lblStatus = pluginStatusText;   // Hand the status label's reference to our local var
            pluginScreenSpace.Controls.Add(this);   // Add this UserControl to the tab ACT provides
            this.Dock = DockStyle.Fill; // Expand the UserControl to fill the tab's client space
            pluginScreenSpace.Text = "挖宝计数";
            xmlSettings = new SettingsSerializer(this); // Create a new settings serializer and pass it this instance
            LoadSettings();
            ItemList.Items.Clear();
            // Create some sort of parsing event handler.  After the "+=" hit TAB twice and the code will be generated for you.
            ActGlobals.oFormActMain.AfterCombatAction += new CombatActionDelegate(oFormActMain_AfterCombatAction);
            ActGlobals.oFormActMain.OnLogLineRead += new LogLineEventDelegate(OFormActMain_OnLogLineRead);

            lblStatus.Text = "就绪";
        }

        void OFormActMain_OnLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {
            if (isImport&&!sessionStart)
                return;
            string tlogline = logInfo.logLine;
            Match match = Regex.Match(tlogline, "^.{14} 00:.{4}:(?<Player>.*?)获得了“(?<ItemName>.*?)(?”)×(?<ItemNum>.*?)。$");
            if (match.Success)
                for (int itemIndex = 0; itemIndex < ItemList.Items.Count; itemIndex++)
                {
                    if (ItemList.GetItemChecked(itemIndex) && match.Groups[1] == ItemList.Items[itemIndex])
                        ItemCount.Items[itemIndex] += match.Groups[2].Value;
                }
            match = Regex.Match(tlogline, "^.{14} 01:Changed Zone to 梦羽宝境.$");
            if (match.Success)
                for (int itemIndex = 0; itemIndex < ItemList.Items.Count; itemIndex++)
                {
                    if (ItemList.GetItemChecked(itemIndex) && match.Groups[1] == ItemList.Items[itemIndex])
                        ItemCount.Items[itemIndex] += match.Groups[2].Value;
                }
        }

        void IActPluginV1.DeInitPlugin()
        {
            // Unsubscribe from any events you listen to when exiting!
            ActGlobals.oFormActMain.AfterCombatAction -= oFormActMain_AfterCombatAction;
            ActGlobals.oFormActMain.OnLogLineRead -= OFormActMain_OnLogLineRead;

            SaveSettings();
            lblStatus.Text = "离线";
        }

        void oFormActMain_AfterCombatAction(bool isImport, CombatActionEventArgs actionInfo)
        {
            //throw new NotImplementedException();
        }

        private void SaveSettings()
        {
            FileStream fs = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            XmlTextWriter xWriter = new XmlTextWriter(fs, Encoding.UTF8);
            xWriter.Formatting = Formatting.Indented;
            xWriter.Indentation = 1;
            xWriter.IndentChar = '\t';
            xWriter.WriteStartDocument(true);
            xWriter.WriteStartElement("Config");    // <Config>
            xWriter.WriteStartElement("SettingsSerializer");    // <Config><SettingsSerializer>
            xmlSettings.ExportToXml(xWriter);   // Fill the SettingsSerializer XML
            xWriter.WriteEndElement();  // </SettingsSerializer>
            xWriter.WriteEndElement();  // </Config>
            xWriter.WriteEndDocument(); // Tie up loose ends (shouldn't be any)
            xWriter.Flush();    // Flush the file buffer to disk
            xWriter.Close();
        }

        private void LoadSettings()
        {
            // Add any controls you want to save the state of
            /*
			xmlSettings.AddControlSetting(textBox1.Name, textBox1);

			if (File.Exists(settingsFile))
			{
				FileStream fs = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				XmlTextReader xReader = new XmlTextReader(fs);

				try
				{
					while (xReader.Read())
					{
						if (xReader.NodeType == XmlNodeType.Element)
						{
							if (xReader.LocalName == "SettingsSerializer")
							{
								xmlSettings.ImportFromXml(xReader);
							}
						}
					}
				}
				catch (Exception ex)
				{
					lblStatus.Text = "Error loading settings: " + ex.Message;
				}
				xReader.Close();
			}
            */
        }

        private void InitializeComponent()
        {
            this.ItemList = new System.Windows.Forms.CheckedListBox();
            this.AddItemText = new System.Windows.Forms.TextBox();
            this.AddItemButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.BaseSalaryText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.MapLabel = new System.Windows.Forms.Label();
            this.TPLabel = new System.Windows.Forms.Label();
            this.SalaryLabel = new System.Windows.Forms.Label();
            this.ResetButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PriceText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PriceList = new System.Windows.Forms.ListBox();
            this.ItemCount = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ItemList
            // 
            this.ItemList.FormattingEnabled = true;
            this.ItemList.Location = new System.Drawing.Point(4, 4);
            this.ItemList.Name = "ItemList";
            this.ItemList.Size = new System.Drawing.Size(106, 196);
            this.ItemList.TabIndex = 0;
            this.ItemList.SelectedIndexChanged += new System.EventHandler(this.ItemList_SelectedIndexChanged);
            // 
            // AddItemText
            // 
            this.AddItemText.Location = new System.Drawing.Point(6, 46);
            this.AddItemText.Name = "AddItemText";
            this.AddItemText.Size = new System.Drawing.Size(100, 21);
            this.AddItemText.TabIndex = 2;
            // 
            // AddItemButton
            // 
            this.AddItemButton.Location = new System.Drawing.Point(186, 45);
            this.AddItemButton.Name = "AddItemButton";
            this.AddItemButton.Size = new System.Drawing.Size(59, 21);
            this.AddItemButton.TabIndex = 3;
            this.AddItemButton.Text = "添加";
            this.AddItemButton.UseVisualStyleBackColor = true;
            this.AddItemButton.Click += new System.EventHandler(this.AddItemButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(266, 16);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveButton.TabIndex = 4;
            this.RemoveButton.Text = "移除";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // BaseSalaryText
            // 
            this.BaseSalaryText.Location = new System.Drawing.Point(266, 62);
            this.BaseSalaryText.Name = "BaseSalaryText";
            this.BaseSalaryText.Size = new System.Drawing.Size(100, 21);
            this.BaseSalaryText.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(264, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "底薪";
            // 
            // MapLabel
            // 
            this.MapLabel.AutoSize = true;
            this.MapLabel.Location = new System.Drawing.Point(264, 86);
            this.MapLabel.Name = "MapLabel";
            this.MapLabel.Size = new System.Drawing.Size(65, 12);
            this.MapLabel.TabIndex = 7;
            this.MapLabel.Text = "已挖图数：";
            // 
            // TPLabel
            // 
            this.TPLabel.AutoSize = true;
            this.TPLabel.Location = new System.Drawing.Point(264, 98);
            this.TPLabel.Name = "TPLabel";
            this.TPLabel.Size = new System.Drawing.Size(65, 12);
            this.TPLabel.TabIndex = 8;
            this.TPLabel.Text = "下洞次数：";
            // 
            // SalaryLabel
            // 
            this.SalaryLabel.AutoSize = true;
            this.SalaryLabel.Location = new System.Drawing.Point(264, 110);
            this.SalaryLabel.Name = "SalaryLabel";
            this.SalaryLabel.Size = new System.Drawing.Size(65, 12);
            this.SalaryLabel.TabIndex = 9;
            this.SalaryLabel.Text = "当前工资：";
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(266, 176);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 10;
            this.ResetButton.Text = "重置";
            this.ResetButton.UseVisualStyleBackColor = true;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(266, 147);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 11;
            this.StartButton.Text = "开始";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.PriceText);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.AddItemText);
            this.groupBox1.Controls.Add(this.AddItemButton);
            this.groupBox1.Location = new System.Drawing.Point(4, 206);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 88);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "添加物品";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "物品名";
            // 
            // PriceText
            // 
            this.PriceText.Location = new System.Drawing.Point(112, 46);
            this.PriceText.Name = "PriceText";
            this.PriceText.Size = new System.Drawing.Size(63, 21);
            this.PriceText.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(112, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "单价";
            // 
            // PriceList
            // 
            this.PriceList.FormattingEnabled = true;
            this.PriceList.ItemHeight = 12;
            this.PriceList.Location = new System.Drawing.Point(117, 4);
            this.PriceList.Name = "PriceList";
            this.PriceList.Size = new System.Drawing.Size(62, 196);
            this.PriceList.TabIndex = 13;
            // 
            // ItemCount
            // 
            this.ItemCount.FormattingEnabled = true;
            this.ItemCount.ItemHeight = 12;
            this.ItemCount.Location = new System.Drawing.Point(190, 4);
            this.ItemCount.Name = "ItemCount";
            this.ItemCount.Size = new System.Drawing.Size(59, 196);
            this.ItemCount.TabIndex = 14;
            // 
            // PluginMain
            // 
            this.Controls.Add(this.ItemCount);
            this.Controls.Add(this.PriceList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.SalaryLabel);
            this.Controls.Add(this.TPLabel);
            this.Controls.Add(this.MapLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BaseSalaryText);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.ItemList);
            this.Name = "PluginMain";
            this.Size = new System.Drawing.Size(370, 297);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void AddItemButton_Click(object sender, EventArgs e)
        {
            string ItemName = AddItemText.Text, ItemPrice = PriceText.Text;
            int ItemPriceN=0;
            if (string.IsNullOrWhiteSpace(ItemName)) 
                MessageBox.Show("输入的物品名非法","挖宝计数",MessageBoxButtons.OK,MessageBoxIcon.Error);
            else if(string.IsNullOrWhiteSpace(ItemPrice))
                MessageBox.Show("输入的物品价格非法", "挖宝计数", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (int.TryParse(ItemPrice, out ItemPriceN))
            {
                ItemList.Items.Add(ItemName, true);
                PriceList.Items.Add(ItemPriceN);
                ItemCount.Items.Add(0);
            }
            else
                MessageBox.Show("输入的物品价格必须为整数", "挖宝计数", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }
        
        private void StartButton_Click(object sender, EventArgs e)
        {
            if (sessionStart)
            {
                sessionStart = false;
                StartButton.Text = "开始";
            }
            else
            {
                sessionStart = true;
                StartButton.Text = "结束";
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            int ItemSelected = ItemList.SelectedIndex;
            if (ItemSelected!=-1)
            {
                ItemList.Items.RemoveAt(ItemSelected);
                PriceList.Items.RemoveAt(ItemSelected);
                ItemCount.Items.RemoveAt(ItemSelected);
            }
        }

        private void ItemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ItemSelected = ItemList.SelectedIndex;
            if (ItemSelected == -1)
                RemoveButton.Enabled = false;
            else
                RemoveButton.Enabled = true;
        }
    }
}
