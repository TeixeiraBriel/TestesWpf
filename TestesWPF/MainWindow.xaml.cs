using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestesWPF
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<char, string> _Dados;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CriarJson();
            if (!string.IsNullOrEmpty(txtBuscar.Text))
            {
                var x = _Dados[char.Parse(txtBuscar.Text)];
                lblResultado.Content = x;
            }
            else
            {
                AddJson();
            }
        }

        private void CriarJson()
        {
            var file = @"Dados\TesteJson.json";
            var data = JsonConvert.DeserializeObject<DadosTextos>(File.ReadAllText(file, Encoding.UTF8));

            _Dados = data.Items.SelectMany(i => i.id, (i, chave) => new { chave, valor = i.texto })
                                    .ToDictionary(i => i.chave, i => i.valor);
        }

        private void AddJson()
        {
            bool duplicidadeId = false;
            DadosTextos novo = new DadosTextos();
            novo.Items = new List<Textos>();


            //FEITO A MÂO
            Textos x = new Textos();
            x.id = "9";
            x.texto = "texto novo";
            //FEITO A MÂO


            var file = @"Dados\TesteJson.json";
            var data = JsonConvert.DeserializeObject<DadosTextos>(File.ReadAllText(file, Encoding.UTF8));

            foreach (var dado in data.Items)
            {
                novo.Items.Add(dado);
                if (dado.id == x.id)
                {
                    duplicidadeId = true;
                }
            }

            if (duplicidadeId)
            {
                MessageBox.Show("Id Duplicado");
            }
            else
            {
                novo.Items.Add(x);
            }

            string output = JsonConvert.SerializeObject(novo);

            File.WriteAllText(@"Dados\TesteJson.json", output.ToString());

            using (StreamWriter file2 = File.CreateText(@"Dados\TesteJson.json"))
            using (JsonTextWriter writer = new JsonTextWriter(file2))
            {
                JObject.Parse(output).WriteTo(writer);
            }
        }
    }
}
