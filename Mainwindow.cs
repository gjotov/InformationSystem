using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Info
{
    
    public partial class Mainwindow : Form
    {
        private DataGridView dataGridViewTopics;
        private Database dBase;

        private TabControl tabControl1;
        private TextBox textBoxVoltage;
        private TextBox textBoxCurrent;
        private TextBox textBoxResistance;
        private Button buttonCalculateOhm;
        private Label labelResultVoltage;
        private Label labelResultCurrent;
        private Label labelResultResistance;
        private ComboBox comboBoxUnits;
        private TextBox textBoxUnitValue;
        private Button buttonConvert;
        private Label labelResult;
        private Label label1;
        private Label label2;


        public Mainwindow()
        {
            InitializeComponent();
            InitializeOhmsLawCalculator();
            InitializeUnitConverter();
            InitializeDataGridView();
            LoadDataIntoDataGridView();

            dBase = new Database(@"C:\\Info\\Database\\PhysicsCont.db\");

            tabControl1 = new TabControl();
            tabControl1.Name = "tabControl1";
            tabControl1.Dock = DockStyle.Fill;
            this.Size = new Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;
            this.Controls.Add(tabControl1);
            this.Text = "Информационная система по Физике";


            TabPage tab1 = new TabPage("Тепловое движение. Температура");
            TabPage tab2 = new TabPage("Внутренняя энергия");
            TabPage tab3 = new TabPage("Способы изменения внутренней энергии тела");


            tab1.Controls.Add(CreateTopicControl("Тепловое движение. Температура"));
            tab2.Controls.Add(CreateTopicControl("Внутренняя энергия"));
            tab3.Controls.Add(CreateTopicControl("Способы изменения внутренней энергии тела"));

            tabControl1.TabPages.AddRange(new TabPage[] { tab1, tab2, tab3 });
        }
        private void InitializeDataGridView()
        {
            dataGridViewTopics = new DataGridView();
            dataGridViewTopics.Location = new Point(10, 30);
            dataGridViewTopics.Size = new Size(550, 180);
            Controls.Add(dataGridViewTopics);
        }
        private void LoadDataIntoDataGridView()
        {
            try
            {
                DataTable dt = dBase.GetTopicContent("Тепловое движение. Температура");
                dataGridViewTopics.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
            }
        }

        private Control CreateTopicControl(string name)
        {
            // Создание панели для содержимого темы
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;

            // Текстовое поле для отображения содержимого темы
            TextBox textBox = new TextBox();
            textBox.Multiline = true;
            textBox.Dock = DockStyle.Fill;

            // Добавление текстового поля на панель
            panel.Controls.Add(textBox);

            return panel;
        }

        public class OhmsLawCalculator
        {
            public static double CalculateCurrent(double voltage, double resistance)
            {
                if (resistance == 0)
                {
                    throw new ArgumentException("Сопротивление не может быть 0.");
                }
                return voltage / resistance;
            }

            public static double CalculateVoltage(double current, double resistance)
            {
                return current * resistance;
            }

            public static double CalculateResistance(double voltage, double current)
            {
                if (current == 0)
                {
                    throw new ArgumentException("Сила тока не может быть 0.");
                }
                return voltage / current;
            }
        }

        private void InitializeOhmsLawCalculator()
        {
            // Создание и настройка текстовых полей и кнопки для калькулятора закона Ома
            textBoxVoltage = new TextBox { Location = new Point(10, 230), Width = 110 };
            textBoxCurrent = new TextBox { Location = new Point(10, 250), Width = 110 };
            textBoxResistance = new TextBox { Location = new Point(10, 270), Width = 110 };
            buttonCalculateOhm = new Button { Text = "Рассчитать", Location = new Point(10, 290) };

            // Создание и настройка меток для отображения результатов
            label1 = new Label { Location = new Point(10, 210), Width = 220, Text="Калькулятор закона Ома", BackColor = Color.White };
            labelResultVoltage = new Label { Location = new Point(120, 240), Width = 110, BackColor = Color.White };
            labelResultCurrent = new Label { Location = new Point(120, 270), Width = 110, BackColor = Color.White };
            labelResultResistance = new Label { Location = new Point(120, 300), Width = 110, BackColor = Color.White };

            buttonCalculateOhm.Click += ButtonCalculateOhm_Click;

            Controls.Add(textBoxVoltage);
            Controls.Add(textBoxCurrent);
            Controls.Add(textBoxResistance);
            Controls.Add(buttonCalculateOhm);
            Controls.Add(label1);
            Controls.Add(labelResultVoltage);
            Controls.Add(labelResultCurrent);
            Controls.Add(labelResultResistance);
        }

        private void InitializeUnitConverter()
        {
            label2 = new Label { Location = new Point(250, 210), Width = 150, Text = "Конвертация значений", BackColor = Color.White };
            comboBoxUnits = new ComboBox { Location = new Point(250, 230), Width = 110 };
            textBoxUnitValue = new TextBox { Location = new Point(250, 250), Width = 110 };
            buttonConvert = new Button { Text = "Конвертировать", Location = new Point(250, 270), Width = 110, BackColor = Color.White };
            labelResult = new Label { Location = new Point(250, 300), Width = 110, BackColor = Color.White };
            comboBoxUnits.Items.AddRange(new string[] { "кА", "мА", "мкА" });
            buttonConvert.Click += ButtonConvert_Click;

            Controls.Add(comboBoxUnits);
            Controls.Add(textBoxUnitValue);
            Controls.Add(buttonConvert);
            Controls.Add(labelResult);
            Controls.Add(label2);
        }
        private void ButtonCalculateOhm_Click(object sender, EventArgs e)
        {
            try
            {
                double? voltage = string.IsNullOrWhiteSpace(textBoxVoltage.Text) ? (double?)null : double.Parse(textBoxVoltage.Text);
                double? current = string.IsNullOrWhiteSpace(textBoxCurrent.Text) ? (double?)null : double.Parse(textBoxCurrent.Text);
                double? resistance = string.IsNullOrWhiteSpace(textBoxResistance.Text) ? (double?)null : double.Parse(textBoxResistance.Text);

                // Расчет по закону Ома
                if (!voltage.HasValue)
                {
                    voltage = current.Value * resistance.Value;
                    labelResultVoltage.Text = $"U: {voltage.Value} В";
                }
                else if (!current.HasValue)
                {
                    current = voltage.Value / resistance.Value;
                    labelResultCurrent.Text = $"I: {current.Value} А";
                }
                else if (!resistance.HasValue)
                {
                    resistance = voltage.Value / current.Value;
                    labelResultResistance.Text = $"R: {resistance.Value} Ом";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void ButtonConvert_Click(object sender, EventArgs e)
        {
            try
            {
                string unit = comboBoxUnits.SelectedItem.ToString();
                double value = double.Parse(textBoxUnitValue.Text);
                double result = ConvertToAmps(unit, value);
                labelResult.Text = $"{result} А";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private double ConvertToAmps(string unit, double value)
        {
            switch (unit)
            {
                case "кА":
                    return value * 1000;
                case "мА":
                    return value * 0.001;
                case "мкА":
                    return value * 0.000001;
                default:
                    throw new ArgumentException("Неизвестная единица измерения.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
