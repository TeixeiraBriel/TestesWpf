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
        }

        private void CriarJson()
        {
            var file = @"Dados\TesteJson.json";
            var data = JsonConvert.DeserializeObject<DadosTextos>(File.ReadAllText(file, Encoding.UTF8));

            _Dados = data.Items.SelectMany(i => i.id, (i, chave) => new { chave, valor = i.texto })
                                    .ToDictionary(i => i.chave, i => i.valor);
        }
    }
}
